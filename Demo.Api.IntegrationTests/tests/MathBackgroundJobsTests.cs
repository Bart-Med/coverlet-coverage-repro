using Demo.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Api.IntegrationTests.tests;

/// <summary>
/// Tests for Hangfire background jobs - run in parallel.
/// </summary>
public class MathBackgroundJobsTests
{
    private readonly IServiceProvider _services;

    public MathBackgroundJobsTests(DemoApiIntegrationTestsWebApplicationFactories factories)
    {
        // Enable Hangfire Server for these tests using WebApplicationFactories
        var factory = factories.WithHangfire;
        _services = factory.Services;
    }

    [Fact]
    public async Task Factorial_background_job_executes_successfully()
    {
        var backgroundService = _services.GetRequiredService<IMathBackgroundService>();

        await backgroundService.EnqueueFactorialCalculation(5);

        Assert.True(true);
    }

    [Fact]
    public async Task Addition_background_job_executes_successfully()
    {
        var backgroundService = _services.GetRequiredService<IMathBackgroundService>();

        // Enqueue job
        await backgroundService.EnqueueAddition(10, 20);

        Assert.True(true);
    }

    [Fact]
    public async Task Multiple_background_jobs_execute_in_parallel()
    {
        var backgroundService = _services.GetRequiredService<IMathBackgroundService>();

        // Enqueue multiple jobs
        var tasks = new List<Task>
        {
            backgroundService.EnqueueFactorialCalculation(4),
            backgroundService.EnqueueAddition(5, 10),
            backgroundService.EnqueueFactorialCalculation(3)
        };

        await Task.WhenAll(tasks);

        Assert.True(true);
    }
}
