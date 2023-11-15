using CSVWorker.Components;
using Lib;

namespace CSVWorker;

internal class DataPanel
{
    private bool _toExit = false;
    private int _currentRowIndex = Console.CursorTop;
    private int _currentColumnIndex = Console.CursorLeft;

    private int _selectedRowIndex;
    private int _selectedColumnIndex;
    private MenuItem _selectedItem = new MenuItem("Default", () => {});
    
    private string[] _data;
    private readonly Dictionary<string, MenuItem[]> _menuItems;
    
    public DataPanel(string[] csvData)
    {
        _data = csvData;
        _menuItems = new Dictionary<string, MenuItem[]>
        {
            {
                "Selecting by field values in specified column",
                new MenuItem[]
                {
                    new("MainObjects", () => HandleSelecting("MainObjects")),
                    new("Workplace", () => HandleSelecting("Workplace")),
                    new("RankYear", () => HandleSelecting("RankYear")),
                    new("Photo", () => HandleSelecting("Photo")),
                }
            },
            {
                "Sorting by field values",
                new MenuItem[]
                {
                    new("Alphabetical order by Name field", 
                        () => HandleSorting("Name", "Alphabetical")),
                    new("RankYear descending", 
                        () => HandleSorting("RankYear", "Descending")),
                }
            },
            {
                "File",
                new MenuItem[]
                {
                    new("Show", HandleShow),
                    new("Save", HandleSave),
                    new("Exit", () => { _toExit = true; }),
                }
            }
        };
        UpdateSelectedItem();
    }

    private void HandleSave()
    {
        int columnsCount = Lib.Constants.ColumnCount;
        string[] lines = new string[_data.Length / columnsCount];
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = CsvParser.FieldsToLine(_data[(i * columnsCount)..((i + 1) * columnsCount)], Lib.Constants.FieldsSeparator);
        }
        CsvProcessing.Write(lines);
        Console.Clear();
        ConsoleMethod.NicePrint("Data saved.");
        UpdateCursorPosition();
    }

    private void HandleShow()
    {
        DataProcessing dp = new(_data);
        dp.Write();
        ConsoleMethod.NicePrint("Press any key to continue.", CustomColor.ErrorColor);
        Console.ReadKey(true);
        Console.Clear();
        UpdateCursorPosition();
    }

    private void HandleSelecting(string column)
    {
        ConsoleMethod.NicePrint(Constants.SearchMessage, CustomColor.Primary);
        string sub = Console.ReadLine() ?? "";
        DataProcessing dp = new(_data);
        _data = dp.SamplingByColumn(column, sub);
        int rowsCount = _data.Length / Lib.Constants.ColumnCount;
        rowsCount -= Lib.Constants.HeaderRowsCount;
        Console.Clear();
        ConsoleMethod.NicePrint($"The new selection contains {rowsCount} record(s).");
        UpdateCursorPosition();
    }

    private void HandleSorting(string column, string sortType)
    {
        DataProcessing dp = new(_data);
        _data = dp.SortingByColumn(column, sortType);
        Console.Clear();
        ConsoleMethod.NicePrint($"{sortType} sorting by column {column} completed.");
        UpdateCursorPosition();
    }

    private void UpdateSelectedItem()
    {
        _selectedItem.Selected = false;
        KeyValuePair<string, MenuItem[]> selectedRow = _menuItems.ElementAt(_selectedRowIndex);
        _selectedItem = selectedRow.Value[_selectedColumnIndex];
        _selectedItem.Selected = true;
    }

    private void HandleArrowKeys(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.DownArrow:
                if (_selectedRowIndex < _menuItems.Count - 1)
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
                if (_selectedColumnIndex < _menuItems.ElementAt(_selectedRowIndex).Value.Length - 1)
                {
                    _selectedColumnIndex++;
                }
                break;
            default:
                return;
        }
        UpdateSelectedItem();
    }

    private void HandleExitKey(ConsoleKey key)
    {
        if (key == ConsoleKey.Q)
        {
            _toExit = true;
        }
    }

    private void HandleEnterKey(ConsoleKey key)
    {
        if (key != ConsoleKey.Enter)
        {
            return;
        }

        _selectedItem.SelectAction.Invoke();
    }

    private void UpdateCursorPosition()
    {
        _currentRowIndex = Console.CursorTop;
        _currentColumnIndex = Console.CursorLeft;
    }
    
    public void Run()
    {
        Console.Clear();
        ConsoleMethod.NicePrint(Constants.PanelMessage);
        UpdateCursorPosition();
        
        while (!_toExit)
        {
            DrawPanel();
            ConsoleKey pressedButtonKey = Console.ReadKey(true).Key;
            HandleArrowKeys(pressedButtonKey);
            HandleEnterKey(pressedButtonKey);
            HandleExitKey(pressedButtonKey);
        }
    }
    
    private void DrawPanel()
    {
        Console.SetCursorPosition(_currentColumnIndex, _currentRowIndex);
        for (int i = 0; i < _menuItems.Count; i++)
        {
            (string? key, MenuItem[] items) = _menuItems.ElementAt(i);

            if (i == _selectedRowIndex)
            {
                ConsoleMethod.NicePrint("?", CustomColor.Primary, " ");
            }

            Console.Write(key + ":");
            foreach (MenuItem item in items)
            {
                item.Write();
            }
            Console.WriteLine("  ");
        }
    }
}