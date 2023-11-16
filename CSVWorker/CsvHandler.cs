using CSVLib;
using Lib;
namespace CSVWorker;

/// <summary>
/// Main CSV handler.
/// </summary>
internal static class CsvHandler
{
    /// <summary>
    /// File handler entry point.
    /// </summary>
    public static void Run()
    {
        ConsoleMethod.NicePrint(Constants.FileNameInputMessage);
        string userPathInput = Console.ReadLine() ?? "";

        string[] lines = CsvProcessing.Read(userPathInput);

        int columns = Lib.Constants.ColumnCount;
        const char sep = Lib.Constants.FieldsSeparator;
        string[] fields = CsvParser.LinesToFields(lines, columns, sep);
            
        DataPanel panel = new(fields);
        panel.Run();
    }
}