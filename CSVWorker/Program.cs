using Lib;
namespace CSVWorker;

/// <summary>
/// Main class of the file reader program.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Checks for exit from the program.
    /// </summary>
    /// <returns>Key is not <see cref="Constants.ExitKeyboardKey"/>.</returns>
    private static bool HandleAgain()
    {
        ConsoleMethod.NicePrint(Constants.AgainMessage, CustomColor.SystemColor);
        return ConsoleMethod.ReadKey() != Constants.ExitKeyboardKey;
    }
    
    /// <summary>
    /// Entry point of the program.
    /// </summary>
    private static void Main()
    {
        ConsoleMethod.NicePrint(Constants.ProgramStartedMessage, CustomColor.SystemColor);
        
        do
        {
            try
            {
                CsvHandler.Run();
            }
            catch (Exception ex)
            {
                ConsoleMethod.NicePrint(Lib.Constants.DefaultErrorMessage, CustomColor.ErrorColor);
                ConsoleMethod.NicePrint(ex.Message, CustomColor.ErrorColor);
            }
        } while (HandleAgain());
        
        ConsoleMethod.NicePrint(Constants.ProgramFinishedMessage, CustomColor.ProgressColor);
    }
}