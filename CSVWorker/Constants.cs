namespace CSVWorker;

/// <summary>
/// Constant values (messages).
/// </summary>
internal struct Constants
{
    public const ConsoleKey ExitKeyboardKey = ConsoleKey.Q;
    public const string ProgramFinishedMessage = "CSV-Worker program finished.";
    
    public const string FileNameInputMessage = "Enter the file name:";
    public const string FileExistMessage = "Something was found. Trying to open it.";

    public const string PanelMessage = 
     "Press Q to exit.\n" +
     "This is a panel for working with a csv file.\n" +
     "Select an action using the arrow keys to select and Enter to confirm.\n" +
     "The console must be fully open, otherwise the characters will fit on top of each other.";

    public const string SearchMessage = "> What are we going to look for?";
    
    public static readonly string AgainMessage = $"Press any key to restart or {ExitKeyboardKey} to exit.";
    public const string ProgramStartedMessage = @"
  ____   ____   __     __   __        __                 _                  
 / ___| / ___|  \ \   / /   \ \      / /   ___    _ __  | | __   ___   _ __ 
| |     \___ \   \ \ / /     \ \ /\ / /   / _ \  | '__| | |/ /  / _ \ | '__|
| |___   ___) |   \ V /       \ V  V /   | (_) | | |    |   <  |  __/ | |   
 \____| |____/     \_/         \_/\_/     \___/  |_|    |_|\_\  \___| |_|                     
CSV Worker program started.";
}