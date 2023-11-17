using CSVLib;
using CSVWorker.Components;
using Lib;

namespace CSVWorker;

/// <summary>
/// Panel menu (task manager) for working with data.
/// </summary>
internal class DataPanel
{
    private bool _toExit;
    private int _currentRowIndex = Console.CursorTop;
    private int _currentColumnIndex = Console.CursorLeft;

    private int _selectedRowIndex;
    private int _selectedColumnIndex;
    private MenuItem _selectedItem = new("Default", () => {});
    private MenuGroup _selectedGroup = new("Default", Array.Empty<MenuItem>());
    
    private string[] _data;
    private readonly MenuGroup[] _menuGroups;
    
    /// <summary>
    /// Initialization.
    /// </summary>
    /// <param name="csvData">
    /// Each field is on a separated string.
    /// The fields follow each other.
    /// </param>
    public DataPanel(string[] csvData)
    {
        _data = csvData;
        _menuGroups = new[]
        {
            new MenuGroup("Selecting by field values in specified column", new MenuItem[]
            {
                new("MainObjects", () => HandleSelecting("MainObjects")),
                new("Workplace", () => HandleSelecting("Workplace")),
                new("RankYear", () => HandleSelecting("RankYear")),
                new("Photo", () => HandleSelecting("Photo")),
            }),
            new MenuGroup("Sorting by field values", new MenuItem[]
            {
                new("Alphabetical order by Name field", 
                    () => HandleSorting("Name", "Alphabetical")),
                new("RankYear descending", 
                    () => HandleSorting("RankYear", "Descending")),
            }),
            new MenuGroup("File", new MenuItem[]
            {
                new("Show", HandleShow),
                new("Save", HandleSave),
                new("Exit", () => { _toExit = true; }),
            }),
        };
        UpdateSelectedItem();
    }

    /// <summary>
    /// It is used to prepare the canvas for work.
    /// </summary>
    /// <param name="message">Starting message.</param>
    private void Restore(string message)
    {
        Console.Clear();
        ConsoleMethod.NicePrint(message);
        UpdateCursorPosition();
    }

    /// <summary>
    /// Used to save the current data to a file.
    /// </summary>
    private void HandleSave()
    {
        ConsoleMethod.NicePrint("> Specify absolute path or default will be used:", CustomColor.Primary);
        string nPath = ConsoleMethod.ReadLine();
        if (nPath.Length > 0)
        {
            CsvProcessing.Write(
                CsvParser.FieldsToText(_data, Lib.Constants.ColumnCount, 
                    Lib.Constants.FieldsSeparator) + Environment.NewLine, nPath);
            Restore("Data added to the specified path.");
        }
        else
        {
            CsvProcessing.Write(CsvParser.FieldsToLines(_data, Lib.Constants.ColumnCount, 
                Lib.Constants.FieldsSeparator));
            Restore("Data saved.");
        }
    }

    /// <summary>
    /// Displaying data in the console.
    /// </summary>
    private void HandleShow()
    {
        ConsoleMethod.PrintFieldsAsTable(_data, Lib.Constants.ColumnCount);
        ConsoleMethod.NicePrint("Press any key to continue.", CustomColor.ErrorColor);
        ConsoleMethod.ReadKey();
        
        Restore("Data showed.");
    }

    /// <summary>
    /// Selecting data by columns and user search.
    /// </summary>
    /// <param name="column">Column to check.</param>
    private void HandleSelecting(string column)
    {
        ConsoleMethod.NicePrint(Constants.SearchMessage, CustomColor.Primary);
        string sub = ConsoleMethod.ReadLine();
        _data = DataProcessing.SamplingByColumn(_data, column, sub);
        int rowsCount = _data.Length / Lib.Constants.ColumnCount;
        rowsCount -= Lib.Constants.HeaderRowsCount;
        Restore($"The new selection contains {rowsCount} record(s).");
    }

    /// <summary>
    /// Sorting data by specified column and type.
    /// </summary>
    /// <param name="column">Column from headers.</param>
    /// <param name="sortType">Alphabetic or Descending.</param>
    private void HandleSorting(string column, string sortType)
    {
        _data = DataProcessing.SortingByColumn(_data, column, sortType);
        Restore($"{sortType} sorting by column {column} completed.");
    }

    /// <summary>
    /// Updates the currently selected item.
    /// </summary>
    private void UpdateSelectedItem()
    {
        _selectedGroup.Selected = false;
        _selectedGroup = _menuGroups[_selectedRowIndex];
        _selectedGroup.Selected = true;

        _selectedItem.Selected = false;
        _selectedItem = _selectedGroup.Items[_selectedColumnIndex];
        _selectedItem.Selected = true;
    }

    /// <summary>
    /// Updates the indexes of the selected group and item.
    /// </summary>
    /// <param name="key">Pressed key.</param>
    private void HandleArrowKeys(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.DownArrow:
                if (_selectedRowIndex < _menuGroups.Length - 1)
                {
                    _selectedRowIndex++;
                    _selectedColumnIndex = 0;
                }
                break;
            case ConsoleKey.UpArrow:
                if (_selectedRowIndex > 0)
                {
                    _selectedRowIndex--;
                    _selectedColumnIndex = 0;
                }
                break;
            case ConsoleKey.LeftArrow:
                if (_selectedColumnIndex > 0)
                {
                    _selectedColumnIndex--;
                }
                break;
            case ConsoleKey.RightArrow:
                if (_selectedColumnIndex < _selectedGroup.Items.Length - 1)
                {
                    _selectedColumnIndex++;
                }
                break;
            default:
                return;
        }
        UpdateSelectedItem();
    }

    /// <summary>
    /// Panel shutdown.
    /// </summary>
    /// <param name="key">User pressed key.</param>
    private void HandleExitKey(ConsoleKey key)
    {
        if (key == ConsoleKey.Q)
        {
            _toExit = true;
        }
    }

    /// <summary>
    /// Processes the selected item.
    /// </summary>
    /// <param name="key">User pressed button key.</param>
    private void HandleEnterKey(ConsoleKey key)
    {
        if (key != ConsoleKey.Enter)
        {
            return;
        }

        _selectedItem.SelectAction.Invoke();
    }

    /// <summary>
    /// Updates cursor position of the console.
    /// </summary>
    private void UpdateCursorPosition()
    {
        _currentRowIndex = Console.CursorTop;
        _currentColumnIndex = Console.CursorLeft;
    }
    
    /// <summary>
    /// Panel runner.
    /// </summary>
    public void Run()
    {
        Restore(Constants.PanelMessage);
        
        while (!_toExit)
        {
            DrawPanel();
            ConsoleKey pressedButtonKey = ConsoleMethod.ReadKey();
            HandleArrowKeys(pressedButtonKey);
            HandleEnterKey(pressedButtonKey);
            HandleExitKey(pressedButtonKey);
        }
    }
    
    /// <summary>
    /// Displays the panel on the screen.
    /// </summary>
    private void DrawPanel()
    {
        Console.SetCursorPosition(_currentColumnIndex, _currentRowIndex);
        foreach (MenuGroup group in _menuGroups)
        {
            group.Write();
        }
    }
}