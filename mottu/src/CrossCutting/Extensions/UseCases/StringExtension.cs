namespace CrossCutting.Extensions.UseCases;

public static class StringExtension
{
    public static string NormalizeString(
        this string input)
    {
        return input?.ToUpper().Trim();
    }
}