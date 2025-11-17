using DemoLib;

namespace Demo.Api.Services;

public interface IPrimeService
{
    bool IsPrime(int n);

    int NextPrime(int n);
}

public class PrimeService : IPrimeService
{
    public bool IsPrime(int n)
    {
        return PrimeOps.IsPrime(n);
    }

    public int NextPrime(int n)
    {
        return PrimeOps.NextPrime(n);
    }
}

