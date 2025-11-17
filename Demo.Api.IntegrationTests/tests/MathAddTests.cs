using System.Net.Http.Json;

namespace Demo.Api.IntegrationTests.tests;

/// <summary>
/// Math endpoint tests.
/// </summary>
public class MathAddTests
{
    private readonly HttpClient _client;

    public MathAddTests(DemoApiIntegrationTestsWebApplicationFactories factories)
    {
        _client = factories.Default.CreateClient();
    }

    [Fact]
    public async Task Add_WithPositiveNumbers_ReturnsSum()
    {
        var response = await _client.GetAsync("/api/math/add?a=2&b=3", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(5, result);
    }

    [Fact]
    public async Task Add_WithNegativeNumbers_ReturnsSum()
    {
        
        var response = await _client.GetAsync("/api/math/add?a=-5&b=-3", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(-8, result);
    }

    [Fact]
    public async Task Add_WithZero_ReturnsOtherNumber()
    {
        
        var response = await _client.GetAsync("/api/math/add?a=0&b=10", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(10, result);
    }

    [Fact]
    public async Task Add_WithMixedNumbers_ReturnsSum()
    {
        
        var response = await _client.GetAsync("/api/math/add?a=-5&b=8", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(3, result);
    }

    [Fact]
    public async Task Add_WithLargeNumbers_ReturnsSum()
    {
        
        var response = await _client.GetAsync("/api/math/add?a=1000000&b=2000000", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(3000000, result);
    }

    [Fact]
    public async Task Add_MultipleSequentialCalls_AllSucceed()
    {
        
        for (int i = 0; i < 5; i++)
        {
            var response = await _client.GetAsync($"/api/math/add?a={i}&b={i}", TestContext.Current.CancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
            Assert.Equal(i * 2, result);
        }
    }
}

