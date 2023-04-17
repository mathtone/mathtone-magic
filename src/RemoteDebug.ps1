# Define your variables
param (
    [string]$projectName = "<YourProjectName>",
    [string]$containerName = "<YourContainerName>",
    [string]$remoteIp = "<EC2-Instance-IP>",
    [int]$remotePort = 2222
)

# Build and publish the project
dotnet publish -c Release -o ./publish

# Build the Docker image
docker build -t $containerName -f ./Dockerfile .

# Save the Docker image to a tar file
$tarFileName = "$containerName.tar"
docker save -o $tarFileName $containerName

# Copy the tar file to the EC2 instance using scp
scp -i "<path-to-your-private-key>.pem" $tarFileName ec2-user@$remoteIp:~

# SSH into the EC2 instance, load the Docker image, and run the container with remote debugging enabled
ssh -i "<path-to-your-private-key>.pem" ec2-user@$remoteIp `
    "docker load -i $tarFileName; docker stop $containerName; docker rm $containerName; docker run -d -p $remotePort:$remotePort --name $containerName $containerName"

# Wait for the container to start
Start-Sleep -s 10

# Attach the remote debugger to the current Visual Studio instance
$debugArguments = "/Command `"`"Debug.AttachtoProcess /DockerLinuxTarget $($remoteIp):$($remotePort)`"`""
& 'devenv' $debugArguments


{
  "iisSettings": {
    // ...
  },
  "profiles": {
    "IIS Express": {
      // ...
    },
    "YourProjectName": {
      // ...
    },
    "Remote Debug": {
      "commandName": "Executable",
      "executablePath": "cmd.exe",
      "commandLineArgs": "/C powershell -File \"${ProjectDir}RemoteDebug.ps1\"",
      "workingDirectory": "${ProjectDir}",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}

# Use the official .NET image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:5.0

# Install the remote debugger (vsdbg) and other required packages
RUN apt-get update \
    && apt-get install -y --no-install-recommends unzip procps \
    && rm -rf /var/lib/apt/lists/* \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

# Set environment variables for the remote debugger
ENV VSDBG_PORT 2222
ENV ASPNETCORE_URLS http://+:80

# Expose the remote debugger port
EXPOSE $VSDBG_PORT

# Copy the published output of the application
COPY ./publish /app
WORKDIR /app

# Start the application and the remote debugger
ENTRYPOINT ["sh", "-c", "dotnet /app/YourAppName.dll & /vsdbg/vsdbg --interpreter=vscode --connection=/tmp/pipe --pipeTransport 'docker exec -i <container_id> \"$@\"'"]
