namespace DemoLib;

public static class PrimeOps
{
    public static bool IsPrime(int n)
    {
        if (n is < 2)
        {
            return false;
        }

        if (n % 2 == 0)
        {
            return n == 2;
        }

        var limit = (int)Math.Sqrt(n);
        for (var d = 3; d <= limit; d += 2)
        {
            if (n % d == 0)
            {
                return false;
            }
        }

        return true;
    }

    public static int NextPrime(int n)
    {
        var candidate = Math.Max(2, n);
        if (candidate % 2 == 0 && candidate != 2)
        {
            candidate++;
        }

        while (!IsPrime(candidate))
        {
            candidate += 2;
        }

        return candidate;
    }
}
