// See https://aka.ms/new-console-template for more information

using Mathtone.Sdk.PackageUtilities.Services;
using Mathtone.Sdk.Secrets;
using Mathtone.Secrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;

Console.WriteLine("Hello, World!");
var json = await File.ReadAllTextAsync(args[0]);
var cfg = JsonSerializer.Deserialize<SolutionAnalysisConfiguration>(json)!;
await new ServiceCollection()
	.AddLogging(bld => bld.AddConsole())
	.AddSingleton(cfg)
	.AddSingleton<SolutionAnalysisService>()
#if DEBUG
	.AddSingleton<ISecrets, MathtoneUserSecrets>()
#else
	.AddSingleton<ISecrets, EnvironmentSecrets>()
#endif
	.BuildServiceProvider()
	.GetRequiredService<SolutionAnalysisService>()
	.Analyze();
Console.WriteLine("Goodbye, World!");