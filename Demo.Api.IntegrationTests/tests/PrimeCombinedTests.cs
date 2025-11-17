using System.Net.Http.Json;

namespace Demo.Api.IntegrationTests.tests;

/// <summary>
/// Combined tests for Prime endpoints - run in parallel.
/// </summary>
public class PrimeCombinedTests
{
    private readonly HttpClient _client;

    public PrimeCombinedTests(DemoApiIntegrationTestsWebApplicationFactories factories)
    {
        _client = factories.Default.CreateClient();
    }

    [Fact]
    public async Task PrimeEndpoints_IsPrimeAndNext_BothWork()
    {
        var isPrimeResponse = await _client.GetAsync("/api/prime/isprime?n=7", TestContext.Current.CancellationToken);
        isPrimeResponse.EnsureSuccessStatusCode();
        var isPrimeResult = await isPrimeResponse.Content.ReadFromJsonAsync<bool>(TestContext.Current.CancellationToken);
        Assert.True(isPrimeResult);

        var nextResponse = await _client.GetAsync("/api/prime/next?n=7", TestContext.Current.CancellationToken);
        nextResponse.EnsureSuccessStatusCode();
        var nextResult = await nextResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(7, nextResult);
    }

    [Fact]
    public async Task PrimeEndpoints_NextPrimeIsPrime_Always()
    {
        var nextResponse = await _client.GetAsync("/api/prime/next?n=8", TestContext.Current.CancellationToken);
        var nextPrime = await nextResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);

        var isPrimeResponse = await _client.GetAsync($"/api/prime/isprime?n={nextPrime}", TestContext.Current.CancellationToken);
        var isPrime = await isPrimeResponse.Content.ReadFromJsonAsync<bool>(TestContext.Current.CancellationToken);

        Assert.True(isPrime);
    }

    [Fact]
    public async Task PrimeEndpoints_ParallelRequests_AllSucceed()
    {
        var isPrimeTask = _client.GetAsync("/api/prime/isprime?n=11", TestContext.Current.CancellationToken);
        var nextTask = _client.GetAsync("/api/prime/next?n=11", TestContext.Current.CancellationToken);

        await Task.WhenAll(isPrimeTask, nextTask);

        var isPrimeResult = await (await isPrimeTask).Content.ReadFromJsonAsync<bool>(TestContext.Current.CancellationToken);
        var nextResult = await (await nextTask).Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);

        Assert.True(isPrimeResult);
        Assert.Equal(11, nextResult);
    }

    [Fact]
    public async Task PrimeEndpoints_SequenceOfPrimes_AllCorrect()
    {
        var primes = new[] { 2, 3, 5, 7, 11, 13 };

        foreach (var prime in primes)
        {
            var response = await _client.GetAsync($"/api/prime/isprime?n={prime}", TestContext.Current.CancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<bool>(TestContext.Current.CancellationToken);
            Assert.True(result);
        }
    }

    [Fact]
    public async Task PrimeEndpoints_NonPrimeSequence_AllCorrect()
    {
        var nonPrimes = new[] { 4, 6, 8, 9, 10, 12 };

        foreach (var nonPrime in nonPrimes)
        {
            var response = await _client.GetAsync($"/api/prime/isprime?n={nonPrime}", TestContext.Current.CancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<bool>(TestContext.Current.CancellationToken);
            Assert.False(result);
        }
    }

    [Fact]
    public async Task PrimeEndpoints_NextPrimeChain_Works()
    {
        var current = 2;
        for (int i = 0; i < 5; i++)
        {
            var response = await _client.GetAsync($"/api/prime/next?n={current + 1}", TestContext.Current.CancellationToken);
            response.EnsureSuccessStatusCode();
            var next = await response.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
            Assert.True(next > current);
            current = next;
        }
    }
}

