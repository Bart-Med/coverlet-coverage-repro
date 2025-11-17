using System.Net;
using System.Net.Http.Json;

namespace Demo.Api.IntegrationTests.tests;

/// <summary>
/// Tests for /api/math/factorial endpoint - run in parallel.
/// </summary>
public class MathFactorialTests
{
    private readonly HttpClient _client;

    public MathFactorialTests(DemoApiIntegrationTestsWebApplicationFactories factories)
    {
        _client = factories.Default.CreateClient();
    }

    [Fact]
    public async Task Factorial_WithZero_ReturnsOne()
    {
        var response = await _client.GetAsync("/api/math/factorial?n=0", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task Factorial_WithOne_ReturnsOne()
    {
        var response = await _client.GetAsync("/api/math/factorial?n=1", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task Factorial_WithFive_Returns120()
    {
        var response = await _client.GetAsync("/api/math/factorial?n=5", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(120, result);
    }

    [Fact]
    public async Task Factorial_WithTen_ReturnsCorrectValue()
    {
        var response = await _client.GetAsync("/api/math/factorial?n=10", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(3628800, result);
    }

    [Fact]
    public async Task Factorial_WithNegativeNumber_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/math/factorial?n=-1", TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Factorial_WithNumberAbove12_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/math/factorial?n=13", TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}

