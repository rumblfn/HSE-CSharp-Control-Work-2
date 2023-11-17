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
    /// Simple readKey method to avoid errors.
    /// </summary>
    /// <param name="intercept">Not display input?</param>
    /// <returns>Read key.</returns>
    public static ConsoleKey ReadKey(bool intercept = true)
    {
        try
        {
            return Console.ReadKey(intercept).Key;
        }
        catch
        {
            return ConsoleKey.Spacebar;
        }
    }

    /// <summary>
    /// Simple readLine method to avoid errors.
    /// </summary>
    /// <returns>Read or empty string.</returns>
    public static string ReadLine()
    {
        try
        {
            return Console.ReadLine() ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

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

    /// <summary>
    /// Outputs fields to the console with the specified columns count.
    /// Where each field follows each other in array.
    /// Adjusts the length of the column relative to the data required for output
    /// and the width of the console.
    /// </summary>
    /// <param name="fields">Array of parsed fields.</param>
    /// <param name="columnsCount">Count of columns.</param>
    public static void PrintFieldsAsTable(string[] fields, int columnsCount)
    {
        int windowWidth = Console.WindowWidth - 1;
        int defaultColumnWidth = windowWidth / columnsCount - 3;
        int tableWidth = 1;
        int rowsCount = fields.Length / columnsCount;
        int[] columnWidths = new int[columnsCount];
        for (int i = 0; i < columnsCount; i++)
        {
            for (int j = 0; j < rowsCount; j++)
            {
                string field = fields[j * columnsCount + i];
                columnWidths[i] = Math.Max(columnWidths[i], field.Length);
            }
            columnWidths[i] = defaultColumnWidth > columnWidths[i] 
                ? columnWidths[i] : defaultColumnWidth;
            tableWidth += columnWidths[i] + 3;
            windowWidth -= columnWidths[i];
            if (i + 1 < columnsCount)
            {
                defaultColumnWidth = windowWidth / (columnsCount - (i + 1)) - 3;
            }
        }

        string rowsSeparator = new('-', tableWidth);
        NicePrint($"Record(s): {rowsCount}", CustomColor.Primary);
        
        for (int i = 0; i < rowsCount * 2; i++)
        {
            if (i % 2 == 0)
            {
                NicePrint(rowsSeparator, CustomColor.Secondary);
            }
            else
            {
                for (int j = 0; j < columnsCount; j++)
                {
                    NicePrint("|", CustomColor.Secondary, " ");
                    string field = fields[columnsCount * (i - 1) / 2 + j];
                    if (field.Length > columnWidths[j])
                    {
                        field = field[..Math.Abs(columnWidths[j] - 2)] + "..";
                    }
                    int whiteSpacesCount = columnWidths[j] - field.Length + 1;
                    NicePrint(field, CustomColor.Primary, 
                        new string(' ', whiteSpacesCount >= 0 ? whiteSpacesCount : 1));
                }

                if (columnsCount > 0)
                {
                    NicePrint("|", CustomColor.Secondary);
                }
            }
        }

        if (rowsCount > 0)
        {
            NicePrint(rowsSeparator, CustomColor.Secondary);
        }
    }
}