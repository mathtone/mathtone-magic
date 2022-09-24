dotnet nuget add source packages --name Local

dotnet pack src/sdk/Mathtone.Sdk.Common/Mathtone.Sdk.Common.csproj --configuration Release -o packages
dotnet pack src/sdk/Mathtone.Sdk.Utilities/Mathtone.Sdk.Utilities.csproj --configuration Release -o packages
dotnet pack src/sdk/Mathtone.Sdk.Logging/Mathtone.Sdk.Logging.csproj --configuration Release -o packages

#Mathtone.Sdk.Testing.Xunit
dotnet remove src/sdk/Mathtone.Sdk.Testing.Xunit/Mathtone.Sdk.Testing.Xunit.csproj reference "..\Mathtone.Sdk.Common\Mathtone.Sdk.Common.csproj"
dotnet remove src/sdk/Mathtone.Sdk.Testing.Xunit/Mathtone.Sdk.Testing.Xunit.csproj reference "..\Mathtone.Sdk.Logging\Mathtone.Sdk.Logging.csproj"
dotnet remove src/sdk/Mathtone.Sdk.Testing.Xunit/Mathtone.Sdk.Testing.Xunit.csproj reference "..\Mathtone.Sdk.Utilities\Mathtone.Sdk.Utilities.csproj"

dotnet add src/sdk/Mathtone.Sdk.Testing.Xunit/Mathtone.Sdk.Testing.Xunit.csproj package Mathtone.Sdk.Common --prerelease --source Local
dotnet add src/sdk/Mathtone.Sdk.Testing.Xunit/Mathtone.Sdk.Testing.Xunit.csproj package Mathtone.Sdk.Logging --prerelease --source Local
dotnet add src/sdk/Mathtone.Sdk.Testing.Xunit/Mathtone.Sdk.Testing.Xunit.csproj package Mathtone.Sdk.Utilities --prerelease --source Local

dotnet pack src/sdk/Mathtone.Sdk.Testing.Xunit/Mathtone.Sdk.Testing.Xunit.csproj --configuration Release -o packages


#dotnet add src/sdk/Mathtone.Sdk.Testing.Xunit.Tests/Mathtone.Sdk.Testing.Xunit.Tests.csproj package Mathtone.Sdk.Testing.Xunit --source ./packages --prerelease


#dotnet pack src/sdk/Mathtone.Sdk.Testing.Xunit/Mathtone.Sdk.Testing.Xunit.csproj --configuration Release -o packages

#dotnet remove src/tests/Mathtone.Sdk.Testing.Xunit.Tests/Mathtone.Sdk.Testing.Xunit.Tests.csproj reference "..\..\sdk\Mathtone.Sdk.Testing.Xunit\Mathtone.Sdk.Testing.Xunit.csproj"
#dotnet add src/tests/Mathtone.Sdk.Testing.Xunit.Tests/Mathtone.Sdk.Testing.Xunit.Tests.csproj package Mathtone.Sdk.Testing.Xunit --source ./packages --prerelease

dotnet nuget remove source Local   
  ls packages