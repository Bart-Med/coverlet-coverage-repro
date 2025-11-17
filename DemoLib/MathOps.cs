namespace DemoLib;

public static class MathOps
{
    public static int Add(int a, int b)
    {
        return a + b;
    }

    public static int Factorial(int n)
    {
        if (n is < 0 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Supported range is 0..12.");
        }

        var result = 1;
        for (var i = 2; i <= n; i++)
        {
            result *= i;
        }

        return result;
    }
}
