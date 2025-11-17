using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.DependencyInjection.Logging;

//[assembly: CollectionBehavior(MaxParallelThreads = 1)]

namespace Demo.Api.IntegrationTests;

public class DemoApiIntegrationTestsStartup
{
    public void ConfigureHost(IHostBuilder hostBuilder)
    {
        hostBuilder.UseEnvironment(Environments.Development);
    }

    public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
    {
        services.AddLogging(lb => lb.AddXunitOutput());

        services.AddSingleton<DemoApiIntegrationTestsWebApplicationFactory>();
        services.AddSingleton<DemoApiIntegrationTestsWebApplicationFactories>();
    }

    public void Configure(IServiceProvider serviceProvider)
    {
    }
}

public class DemoApiIntegrationTestsWebApplicationFactories
{
    private readonly DemoApiIntegrationTestsWebApplicationFactory factory;
    private WebApplicationFactory<Program>? @default;
    private WebApplicationFactory<Program>? withHangfire;

    public DemoApiIntegrationTestsWebApplicationFactories(DemoApiIntegrationTestsWebApplicationFactory factory)
    {
        this.factory = factory;
    }

    public WebApplicationFactory<Program> CreateDefault()
        => factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
            });
        });

    public WebApplicationFactory<Program> CreateWithHangfireEnabled()
        => factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                if (!services.Any(s => s.ServiceType == typeof(IBackgroundProcessingServer)))
                {
                    services.AddHangfireServer(options =>
                    {
                        options.SchedulePollingInterval = TimeSpan.FromSeconds(0.1);
                    });
                }
            });
        });

    public WebApplicationFactory<Program> Default
        => @default ??= CreateDefault();

    public WebApplicationFactory<Program> WithHangfire
        => withHangfire ??= CreateWithHangfireEnabled();
}
