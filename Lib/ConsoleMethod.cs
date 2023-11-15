namespace Lib;
/// <summary>
/// Class for common console methods.
/// </summary>
public abstract class ConsoleMethod
{
    // Separators.
    private const string DefaultElementsSeparator = ", ";
    private static readonly string DefaultLinesSeparator = Environment.NewLine;

    /// <summary>
    /// Prints a message with a specified color and a specified line end.
    /// </summary>
    /// <param name="message">Message content.</param>
    /// <param name="color">Message color.</param>
    /// <param name="end">End of line.</param>
    public static void NicePrint(string message, ConsoleColor color = CustomColor.DefaultColor, string? end = null)
    {
        Console.ForegroundColor = color;
        Console.Write(message + (end ?? Environment.NewLine));
        Console.ResetColor();
    }

    /// <summary>
    /// Outputs an array to the console with the specified delimiters.
    /// </summary>
    /// <param name="arr">Common array.</param>
    /// <param name="elementsSeparator">Sep for elements.</param>
    /// <param name="linesSeparator">End for line.</param>
    public static void PrintArray(in string [][] arr, string? linesSeparator = null, string? elementsSeparator = DefaultElementsSeparator)
    {
        string[] arrayRows = new string[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            arrayRows[i] = string.Join(elementsSeparator, arr[i]);
        }

        string arrayString = string.Join(linesSeparator ?? DefaultLinesSeparator, arrayRows);

        NicePrint(
            arrayString.Length > 0 ? arrayString : Constants.EmptyArrayMessage, 
            CustomColor.ProgressColor);
    }
}