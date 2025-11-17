using System.Net.Http.Json;

namespace Demo.Api.IntegrationTests.tests;

/// <summary>
/// Combined tests for Text endpoints - run in parallel.
/// </summary>
public class TextCombinedTests
{
    private readonly HttpClient _client;

    public TextCombinedTests(DemoApiIntegrationTestsWebApplicationFactories factories)
    {
        _client = factories.Default.CreateClient();
    }

    [Fact]
    public async Task TextEndpoints_ReverseAndVowels_BothWork()
    {
        var reverseResponse = await _client.GetAsync("/api/text/reverse?value=Hello", TestContext.Current.CancellationToken);
        reverseResponse.EnsureSuccessStatusCode();
        var reverseResult = await reverseResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Equal("olleH", reverseResult);

        var vowelsResponse = await _client.GetAsync("/api/text/vowels?value=Hello", TestContext.Current.CancellationToken);
        vowelsResponse.EnsureSuccessStatusCode();
        var vowelsResult = await vowelsResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(2, vowelsResult);
    }

    [Fact]
    public async Task TextEndpoints_ParallelRequests_AllSucceed()
    {
        var reverseTask = _client.GetAsync("/api/text/reverse?value=test", TestContext.Current.CancellationToken);
        var vowelsTask = _client.GetAsync("/api/text/vowels?value=test", TestContext.Current.CancellationToken);

        await Task.WhenAll(reverseTask, vowelsTask);

        var reverseResult = await (await reverseTask).Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        var vowelsResult = await (await vowelsTask).Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);

        Assert.Equal("tset", reverseResult);
        Assert.Equal(1, vowelsResult);
    }

    [Fact]
    public async Task TextEndpoints_ReverseOfReverse_ReturnsOriginal()
    {
        var firstResponse = await _client.GetAsync("/api/text/reverse?value=demo", TestContext.Current.CancellationToken);
        var firstResult = await firstResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Equal("omed", firstResult);

        var secondResponse = await _client.GetAsync($"/api/text/reverse?value={firstResult}", TestContext.Current.CancellationToken);
        var secondResult = await secondResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Equal("demo", secondResult);
    }

    [Fact]
    public async Task TextEndpoints_MultipleVowelCounts_AllCorrect()
    {
        var words = new[] { "apple", "orange", "banana", "grape" };
        var expectedCounts = new[] { 2, 3, 3, 2 };

        for (int i = 0; i < words.Length; i++)
        {
            var response = await _client.GetAsync($"/api/text/vowels?value={words[i]}", TestContext.Current.CancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
            Assert.Equal(expectedCounts[i], result);
        }
    }

    [Fact]
    public async Task TextEndpoints_VowelsInReversedString_SameCount()
    {
        var word = "programming";

        var vowelsResponse = await _client.GetAsync($"/api/text/vowels?value={word}", TestContext.Current.CancellationToken);
        var vowelsCount = await vowelsResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);

        var reverseResponse = await _client.GetAsync($"/api/text/reverse?value={word}", TestContext.Current.CancellationToken);
        var reversedWord = await reverseResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var reversedVowelsResponse = await _client.GetAsync($"/api/text/vowels?value={reversedWord}", TestContext.Current.CancellationToken);
        var reversedVowelsCount = await reversedVowelsResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);

        Assert.Equal(vowelsCount, reversedVowelsCount);
    }

    [Fact]
    public async Task TextEndpoints_EmptyStrings_HandleCorrectly()
    {
        var reverseResponse = await _client.GetAsync("/api/text/reverse?value=", TestContext.Current.CancellationToken);
        reverseResponse.EnsureSuccessStatusCode();
        var reverseResult = await reverseResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Equal("", reverseResult);

        var vowelsResponse = await _client.GetAsync("/api/text/vowels?value=", TestContext.Current.CancellationToken);
        vowelsResponse.EnsureSuccessStatusCode();
        var vowelsResult = await vowelsResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(0, vowelsResult);
    }
}

