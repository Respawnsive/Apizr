using Microsoft.Extensions.Logging;
using Xunit.Runners.Maui;

namespace Apizr.Tests.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .ConfigureTests(new TestOptions()
                {
                    Assemblies =
                    {
                        typeof(MauiProgram).Assembly,
                        typeof(ApizrTests).Assembly
                    }
                })
                .UseVisualRunner();

#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}