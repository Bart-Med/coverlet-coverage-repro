using System.Net.Http.Json;

namespace Demo.Api.IntegrationTests.tests;

/// <summary>
/// Tests for /api/prime/next endpoint - run in parallel.
/// </summary>
public class PrimeNextTests
{
    private readonly HttpClient _client;

    public PrimeNextTests(DemoApiIntegrationTestsWebApplicationFactories factories)
    {
        _client = factories.Default.CreateClient();
    }

    [Fact]
    public async Task NextPrime_WithOne_ReturnsTwo()
    {
        var response = await _client.GetAsync("/api/prime/next?n=1", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task NextPrime_WithPrimeNumber_ReturnsSamePrime()
    {
        
        var response = await _client.GetAsync("/api/prime/next?n=13", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(13, result);
    }

    [Fact]
    public async Task NextPrime_WithFour_ReturnsFive()
    {
        
        var response = await _client.GetAsync("/api/prime/next?n=4", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(5, result);
    }

    [Fact]
    public async Task NextPrime_WithTen_ReturnsEleven()
    {
        
        var response = await _client.GetAsync("/api/prime/next?n=10", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(11, result);
    }

    [Fact]
    public async Task NextPrime_WithTwenty_ReturnsTwentyThree()
    {
        
        var response = await _client.GetAsync("/api/prime/next?n=20", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(23, result);
    }

    [Fact]
    public async Task NextPrime_WithZero_ReturnsTwo()
    {
        
        var response = await _client.GetAsync("/api/prime/next?n=0", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(2, result);
    }
}

