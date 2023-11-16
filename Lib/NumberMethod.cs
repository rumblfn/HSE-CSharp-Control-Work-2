namespace Lib;

/// <summary>
/// Class for common number methods.
/// </summary>
public abstract class NumberMethod
{
    /// <summary>
    /// Parses strings without errors.
    /// </summary>
    /// <param name="value">Value to parse.</param>
    /// <returns>Parsed value or zero.</returns>
    public static int Parse(string value)
    {
        return int.TryParse(value, out int x) ? x : 0;
    }
}