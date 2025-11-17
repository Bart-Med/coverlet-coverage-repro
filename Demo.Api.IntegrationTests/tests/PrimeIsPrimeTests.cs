using System.Net.Http.Json;

namespace Demo.Api.IntegrationTests.tests;

/// <summary>
/// Tests for /api/prime/isprime endpoint - run in parallel.
/// </summary>
public class PrimeIsPrimeTests
{
    private readonly HttpClient _client;

    public PrimeIsPrimeTests(DemoApiIntegrationTestsWebApplicationFactories factories)
    {
        _client = factories.Default.CreateClient();
    }

    [Fact]
    public async Task IsPrime_WithTwo_ReturnsTrue()
    {
        var response = await _client.GetAsync("/api/prime/isprime?n=2", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<bool>(TestContext.Current.CancellationToken);
        Assert.True(result);
    }

    [Fact]
    public async Task IsPrime_WithThree_ReturnsTrue()
    {
        var response = await _client.GetAsync("/api/prime/isprime?n=3", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<bool>(TestContext.Current.CancellationToken);
        Assert.True(result);
    }

    [Fact]
    public async Task IsPrime_WithFour_ReturnsFalse()
    {
        var response = await _client.GetAsync("/api/prime/isprime?n=4", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<bool>(TestContext.Current.CancellationToken);
        Assert.False(result);
    }

    [Fact]
    public async Task IsPrime_WithSeventeen_ReturnsTrue()
    {
        var response = await _client.GetAsync("/api/prime/isprime?n=17", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<bool>(TestContext.Current.CancellationToken);
        Assert.True(result);
    }

    [Fact]
    public async Task IsPrime_WithOne_ReturnsFalse()
    {
        var response = await _client.GetAsync("/api/prime/isprime?n=1", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<bool>(TestContext.Current.CancellationToken);
        Assert.False(result);
    }

    [Fact]
    public async Task IsPrime_WithNegativeNumber_ReturnsFalse()
    {
        var response = await _client.GetAsync("/api/prime/isprime?n=-5", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<bool>(TestContext.Current.CancellationToken);
        Assert.False(result);
    }
}

