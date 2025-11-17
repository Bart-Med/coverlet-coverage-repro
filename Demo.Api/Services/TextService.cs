using DemoLib;

namespace Demo.Api.Services;

public interface ITextService
{
    string Reverse(string? input);

    int CountVowels(string? input);
}

public class TextService : ITextService
{
    public string Reverse(string? input)
    {
        return TextOps.Reverse(input);
    }

    public int CountVowels(string? input)
    {
        return TextOps.CountVowels(input);
    }
}

