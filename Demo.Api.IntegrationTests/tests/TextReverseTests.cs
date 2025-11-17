namespace Demo.Api.IntegrationTests.tests;

/// <summary>
/// Tests for /api/text/reverse endpoint - run in parallel.
/// </summary>
public class TextReverseTests
{
    private readonly HttpClient _client;

    public TextReverseTests(DemoApiIntegrationTestsWebApplicationFactories factories)
    {
        _client = factories.Default.CreateClient();
    }

    [Fact]
    public async Task Reverse_WithSimpleString_ReturnsReversed()
    {
        var response = await _client.GetAsync("/api/text/reverse?value=abc", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Equal("cba", result);
    }

    [Fact]
    public async Task Reverse_WithEmptyString_ReturnsEmpty()
    {
        var response = await _client.GetAsync("/api/text/reverse?value=", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Equal("", result);
    }

    [Fact]
    public async Task Reverse_WithSingleCharacter_ReturnsSameCharacter()
    {
        var response = await _client.GetAsync("/api/text/reverse?value=a", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Equal("a", result);
    }

    [Fact]
    public async Task Reverse_WithPalindrome_ReturnsSameString()
    {
        var response = await _client.GetAsync("/api/text/reverse?value=racecar", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Equal("racecar", result);
    }

    [Fact]
    public async Task Reverse_WithSpecialCharacters_ReversesCorrectly()
    {
        var response = await _client.GetAsync("/api/text/reverse?value=Hello%21", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Equal("!olleH", result);
    }

    [Fact]
    public async Task Reverse_WithSpaces_ReversesWithSpaces()
    {
        var response = await _client.GetAsync("/api/text/reverse?value=Hello%20World", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Equal("dlroW olleH", result);
    }
}

