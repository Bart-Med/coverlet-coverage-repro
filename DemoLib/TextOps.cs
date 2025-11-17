namespace DemoLib;

public static class TextOps
{
    public static string Reverse(string? input)
    {
        if (input is null)
        {
            return string.Empty;
        }

        var arr = input.ToCharArray();
        Array.Reverse(arr);
        return new string(arr);
    }

    public static int CountVowels(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return 0;
        }

        var vowels = new HashSet<char>("aeiouAEIOU");
        var count = 0;
        foreach (var c in input)
        {
            if (vowels.Contains(c))
            {
                count++;
            }
        }

        return count;
    }
}
