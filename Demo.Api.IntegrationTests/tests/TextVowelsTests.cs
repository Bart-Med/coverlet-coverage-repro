using System.Net.Http.Json;

namespace Demo.Api.IntegrationTests.tests;

/// <summary>
/// Tests for /api/text/vowels endpoint - run in parallel.
/// </summary>
public class TextVowelsTests
{
    private readonly HttpClient _client;

    public TextVowelsTests(DemoApiIntegrationTestsWebApplicationFactories factories)
    {
        _client = factories.Default.CreateClient();
    }

    [Fact]
    public async Task CountVowels_WithSimpleString_ReturnsCorrectCount()
    {
        var response = await _client.GetAsync("/api/text/vowels?value=Hello%20World", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(3, result);
    }

    [Fact]
    public async Task CountVowels_WithOnlyVowels_ReturnsLength()
    {
        var response = await _client.GetAsync("/api/text/vowels?value=aeiou", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(5, result);
    }

    [Fact]
    public async Task CountVowels_WithNoVowels_ReturnsZero()
    {
        var response = await _client.GetAsync("/api/text/vowels?value=bcdfg", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task CountVowels_WithEmptyString_ReturnsZero()
    {
        var response = await _client.GetAsync("/api/text/vowels?value=", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task CountVowels_WithMixedCase_CountsBothCases()
    {
        var response = await _client.GetAsync("/api/text/vowels?value=AEIOUaeiou", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(10, result);
    }

    [Fact]
    public async Task CountVowels_WithNumbers_IgnoresNumbers()
    {
        var response = await _client.GetAsync("/api/text/vowels?value=a1e2i3o4u5", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(5, result);
    }
}

