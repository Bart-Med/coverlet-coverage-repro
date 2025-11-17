using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace Demo.Api.IntegrationTests;

public class DemoApiIntegrationTestsWebApplicationFactory : WebApplicationFactory<Program>
{
    public static string ApiVersion { get; } = "v1";

    public DemoApiIntegrationTestsWebApplicationFactory(ITestOutputHelperAccessor outputHelperAccessor)
    {
        OutputHelperAccessor = outputHelperAccessor;
    }

    protected ITestOutputHelperAccessor OutputHelperAccessor { get; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureLogging(logging =>
        {
            logging.AddXunitOutput();
        });

        builder.ConfigureServices(services =>
        {
            services.AddSingleton(OutputHelperAccessor);

            services.RemoveAll<IBackgroundProcessingServer>();

            services.Configure<BackgroundJobServerOptions>(options =>
            {
                options.SchedulePollingInterval = TimeSpan.FromSeconds(0.1);
            });
        });
    }
}

