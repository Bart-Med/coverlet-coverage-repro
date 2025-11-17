using System.Net.Http.Json;

namespace Demo.Api.IntegrationTests.tests;

/// <summary>
/// Basic integration tests for all API endpoints
/// </summary>
public class ApiIntegrationTests
{
    private readonly HttpClient _client;

    public ApiIntegrationTests(DemoApiIntegrationTestsWebApplicationFactories factories)
    {
        _client = factories.Default.CreateClient();
    }

    [Fact]
    public async Task AllEndpoints_Respond200_WhenValid()
    {
        var endpoints = new[]
        {
            "/api/math/add?a=1&b=2",
            "/api/math/factorial?n=5",
            "/api/text/reverse?value=test",
            "/api/text/vowels?value=test",
            "/api/prime/isprime?n=7",
            "/api/prime/next?n=7"
        };

        foreach (var endpoint in endpoints)
        {
            var response = await _client.GetAsync(endpoint, TestContext.Current.CancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }

    [Fact]
    public async Task AllEndpoints_ParallelCalls_AllSucceed()
    {
        var tasks = new[]
        {
            _client.GetAsync("/api/math/add?a=5&b=10", TestContext.Current.CancellationToken),
            _client.GetAsync("/api/math/factorial?n=4", TestContext.Current.CancellationToken),
            _client.GetAsync("/api/text/reverse?value=parallel", TestContext.Current.CancellationToken),
            _client.GetAsync("/api/text/vowels?value=parallel", TestContext.Current.CancellationToken),
            _client.GetAsync("/api/prime/isprime?n=13", TestContext.Current.CancellationToken),
            _client.GetAsync("/api/prime/next?n=14", TestContext.Current.CancellationToken)
        };

        var responses = await Task.WhenAll(tasks);

        foreach (var response in responses)
        {
            response.EnsureSuccessStatusCode();
        }
    }

    [Fact]
    public async Task MathAndText_CombinedWorkflow_Works()
    {
        var addResponse = await _client.GetAsync("/api/math/add?a=2&b=3", TestContext.Current.CancellationToken);
        var sum = await addResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);

        var reverseResponse = await _client.GetAsync($"/api/text/reverse?value={sum}", TestContext.Current.CancellationToken);
        var reversed = await reverseResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal("5", reversed);
    }

    [Fact]
    public async Task PrimeAndMath_CombinedWorkflow_Works()
    {
        var nextPrimeResponse = await _client.GetAsync("/api/prime/next?n=10", TestContext.Current.CancellationToken);
        var nextPrime = await nextPrimeResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(11, nextPrime);

        var addResponse = await _client.GetAsync($"/api/math/add?a={nextPrime}&b=2", TestContext.Current.CancellationToken);
        var sum = await addResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(13, sum);

        var isPrimeResponse = await _client.GetAsync($"/api/prime/isprime?n={sum}", TestContext.Current.CancellationToken);
        var isPrime = await isPrimeResponse.Content.ReadFromJsonAsync<bool>(TestContext.Current.CancellationToken);
        Assert.True(isPrime);
    }

    [Fact]
    public async Task MultipleClients_IndependentRequests_AllSucceed()
    {
        var response1 = await _client.GetAsync("/api/math/add?a=1&b=1", TestContext.Current.CancellationToken);
        var response2 = await _client.GetAsync("/api/text/reverse?value=abc", TestContext.Current.CancellationToken);
        var response3 = await _client.GetAsync("/api/prime/isprime?n=5", TestContext.Current.CancellationToken);

        response1.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();
        response3.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task StressTest_100Requests_AllSucceed()
    {
        var tasks = new List<Task<HttpResponseMessage>>();

        for (int i = 0; i < 100; i++)
        {
            var endpointIndex = i % 3;
            var endpoint = endpointIndex switch
            {
                0 => $"/api/math/add?a={i}&b={i}",
                1 => $"/api/text/vowels?value=test{i}",
                _ => $"/api/prime/isprime?n={i + 2}"
            };
            tasks.Add(_client.GetAsync(endpoint, TestContext.Current.CancellationToken));
        }

        var responses = await Task.WhenAll(tasks);
        var successCount = responses.Count(r => r.IsSuccessStatusCode);

        Assert.Equal(100, successCount);
    }
}

