using Xunit.Runner.InProc.SystemConsole;

namespace VetClinic.Tests;

internal static class TestEntryPoint
{
    public static int Main(string[] args)
    {
        // SDK 10 передаёт --nologo, а Microsoft.Testing.Platform 2.4 считает параметр неизвестным
        // и завершает запуск с кодом 5, не выполнив тесты.
        args = args.Where(static arg => arg != "--nologo").ToArray();

        if (args.Any(static arg => arg is "--server" or "--internal-msbuild-node"))
        {
            return Xunit.MicrosoftTestingPlatform.TestPlatformTestFramework
                .RunAsync(args, SelfRegisteredExtensions.AddSelfRegisteredExtensions)
                .GetAwaiter()
                .GetResult();
        }

        return ConsoleRunner.Run(args).GetAwaiter().GetResult();
    }
}
