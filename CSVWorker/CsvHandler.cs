using Lib;
namespace CSVWorker;

/// <summary>
/// Main CSV handler.
/// </summary>
internal static class CsvHandler
{
    private const string DefaultPath = 
        "/Users/samilvaliahmetov/Projects/ControlHomework-2/assets/recreators.csv";
    
    /// <summary>
    /// File handler entry point.
    /// </summary>
    public static void Run()
    {
        ConsoleMethod.NicePrint(Constants.FileNameInputMessage);
        string userPathInput = Console.ReadLine() ?? "";

        try
        {
            string[] csvData = CsvProcessing.Read(userPathInput);
            
            DataPanel panel = new(csvData);
            panel.Run();
        }
        catch (Exception ex)
        {
            ConsoleMethod.NicePrint(Lib.Constants.DefaultErrorMessage, CustomColor.ErrorColor);
            ConsoleMethod.NicePrint(ex.Message, CustomColor.ErrorColor);
        }
    }
}