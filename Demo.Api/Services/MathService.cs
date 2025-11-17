using DemoLib;

namespace Demo.Api.Services;

public interface IMathService
{
    int Add(int a, int b);

    int Factorial(int n);
}

public class MathService : IMathService
{
    public int Add(int a, int b)
    {
        return MathOps.Add(a, b);
    }

    public int Factorial(int n)
    {
        return MathOps.Factorial(n);
    }
}

