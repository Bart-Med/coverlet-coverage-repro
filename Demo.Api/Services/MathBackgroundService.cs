namespace Demo.Api.Services;

public interface IMathBackgroundService
{
    Task EnqueueFactorialCalculation(int n);
    Task EnqueueAddition(int a, int b);
}

public class MathBackgroundService : IMathBackgroundService
{
    private readonly IMathService _mathService;

    public MathBackgroundService(IMathService mathService)
    {
        _mathService = mathService;
    }

    public async Task EnqueueFactorialCalculation(int n)
    {
        await Task.Run(() =>
        {
            var result = _mathService.Factorial(n);
            Console.WriteLine($"Background: Factorial {n} = {result}");
        });
    }

    public async Task EnqueueAddition(int a, int b)
    {
        await Task.Run(() =>
        {
            var result = _mathService.Add(a, b);
            Console.WriteLine($"Background: Addition {a} + {b} = {result}");
        });
    }
}
