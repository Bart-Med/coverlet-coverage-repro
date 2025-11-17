using System.Net.Http.Json;

namespace Demo.Api.IntegrationTests.tests;

/// <summary>
/// Combined tests for Math endpoints - run in parallel.
/// </summary>
public class MathCombinedTests
{
    private readonly HttpClient _client;

    public MathCombinedTests(DemoApiIntegrationTestsWebApplicationFactories factories)
    {
        _client = factories.Default.CreateClient();
    }

    [Fact]
    public async Task MathEndpoints_AddAndFactorial_BothWork()
    {

        var addResponse = await _client.GetAsync("/api/math/add?a=2&b=3", TestContext.Current.CancellationToken);
        addResponse.EnsureSuccessStatusCode();
        var addResult = await addResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(5, addResult);

        var factResponse = await _client.GetAsync("/api/math/factorial?n=5", TestContext.Current.CancellationToken);
        factResponse.EnsureSuccessStatusCode();
        var factResult = await factResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(120, factResult);
    }

    [Fact]
    public async Task MathEndpoints_MultipleAddCalls_AllSucceed()
    {
        var tasks = new List<Task<HttpResponseMessage>>();
        for (int i = 1; i <= 10; i++)
        {
            tasks.Add(_client.GetAsync($"/api/math/add?a={i}&b={i}", TestContext.Current.CancellationToken));
        }

        var responses = await Task.WhenAll(tasks);
        foreach (var response in responses)
        {
            response.EnsureSuccessStatusCode();
        }
    }

    [Fact]
    public async Task MathEndpoints_ParallelRequests_AllSucceed()
    {
        var addTask = _client.GetAsync("/api/math/add?a=10&b=20", TestContext.Current.CancellationToken);
        var factTask = _client.GetAsync("/api/math/factorial?n=6", TestContext.Current.CancellationToken);

        await Task.WhenAll(addTask, factTask);

        var addResult = await (await addTask).Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        var factResult = await (await factTask).Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);

        Assert.Equal(30, addResult);
        Assert.Equal(720, factResult);
    }

    [Fact]
    public async Task MathEndpoints_AddResultAsFactorialInput_Works()
    {
        var addResponse = await _client.GetAsync("/api/math/add?a=2&b=3", TestContext.Current.CancellationToken);
        var addResult = await addResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);

        var factResponse = await _client.GetAsync($"/api/math/factorial?n={addResult}", TestContext.Current.CancellationToken);
        factResponse.EnsureSuccessStatusCode();
        var factResult = await factResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
        Assert.Equal(120, factResult);
    }

    [Fact]
    public async Task MathEndpoints_SequentialCalls_MaintainState()
    {
        for (int i = 0; i < 3; i++)
        {
            var addResponse = await _client.GetAsync($"/api/math/add?a={i}&b={i + 1}", TestContext.Current.CancellationToken);
            addResponse.EnsureSuccessStatusCode();
            var result = await addResponse.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);
            Assert.Equal(2 * i + 1, result);
        }
    }
}

