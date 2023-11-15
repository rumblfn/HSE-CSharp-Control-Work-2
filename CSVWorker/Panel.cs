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
    private readonly Dictionary<string, List<MenuItem>> _menuItems;
    
    public DataPanel(string[] csvData)
    {
        CsvTemplate template = new (csvData);
        template.ValidateTemplate();
        
        _data = csvData;
        _menuItems = new ()
        {
            {
                "Selecting by field values in specified column",
                new List<MenuItem>()
                {
                    new("MainObjects", () => HandleSelecting("MainObjects")),
                    new("Workplace", () => HandleSelecting("Workplace")),
                    new("RankYear", () => HandleSelecting("RankYear")),
                    new("Photo", () => HandleSelecting("Photo")),
                }
            },
            {
                "Sorting by field values",
                new List<MenuItem>()
                {
                    new("Alphabetical order by Name field", () => {}),
                    new("RankYear descending", () => {}),
                }
            },
            {
                "File",
                new List<MenuItem>()
                {
                    new("Show", () => {}),
                    new("Save", () => {}),
                    new("Exit", () => { _toExit = true; }),
                }
            }
        };
        UpdateSelectedItem();
    }

    private void HandleSelecting(string column)
    {
        ConsoleMethod.NicePrint(Constants.SearchMessage, CustomColor.Primary);
        string sub = Console.ReadLine() ?? "";
        DataProcessing dp = new(_data);
        _data = dp.SamplingByColumn(column, sub);
        int rowsCount = _data.Length / Lib.Constants.ColumnCount;
        rowsCount -= Lib.Constants.HeaderRowsCount;
        ConsoleMethod.NicePrint($"The new selection contains {rowsCount} record(s).");
        UpdateCursorPosition();
    }

    private void UpdateSelectedItem()
    {
        _selectedItem.Selected = false;
        KeyValuePair<string, List<MenuItem>> selectedRow = _menuItems.ElementAt(_selectedRowIndex);
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
                if (_selectedColumnIndex < _menuItems.ElementAt(_selectedRowIndex).Value.Count - 1)
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
            (string? key, List<MenuItem> items) = _menuItems.ElementAt(i);

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