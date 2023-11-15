namespace Lib;

/// <summary>
/// Contains methods to work with data.
/// </summary>
public class DataProcessing
{
    private readonly string[] _headersEn;
    private readonly string[] _headersRu;
    private readonly string[] _data;
    private readonly int _columnsCount = Constants.ColumnCount;
    
    public DataProcessing(string[] csvData)
    {
        _headersEn = csvData[.._columnsCount];
        _headersRu = csvData[_columnsCount..(_columnsCount * 2)];
        _data = csvData[(_columnsCount * 2)..];
    }

    private string[] GetInitialData()
    {
        return _headersEn.Concat(_headersRu).ToArray();
    }

    private int GetColumnIndex(string column, bool recursive = false)
    {
        int index = Array.IndexOf(_headersEn, column);
        if (index < 0)
        {
            index = Array.IndexOf(_headersRu, column);
        }
        
        return index;
    }

    public string[] SamplingByColumn(string columnName, string sub)
    {
        sub = sub.ToLower();
        string[] resultData = GetInitialData();
        
        int index = GetColumnIndex(columnName);
        if (index < 0)
        {
            return resultData;
        }

        while (index < _data.Length)
        {
            if (_data[index].ToLower().Contains(sub))
            {
                int rowIndex = index / _columnsCount;
                int firstRowElIdx = rowIndex * _columnsCount;
                
                resultData = resultData
                    .Concat(_data[firstRowElIdx..(firstRowElIdx + _columnsCount)])
                    .ToArray();
            }
            index += _columnsCount;
        }

        return resultData;
    }

    public string[] SortingByColumn(string columnName, string type)
    {
        string[] resultData = GetInitialData();
        int index = GetColumnIndex(columnName);
        if (index < 0)
        {
            return resultData;
        }

        int offsetIndex = resultData.Length;
        int rowsCount = _data.Length / _columnsCount;
        string[] emptyData = new string[_data.Length];
        resultData = resultData.Concat(emptyData).ToArray();
        
        string[][] matrix = new string[rowsCount][];
        for (int i = 0; i < rowsCount; i++)
        {
            string[] row = new string[_columnsCount];
            for (int j = 0; j < _columnsCount; j++)
            {
                row[j] = _data[i * _columnsCount + j];
            }
            matrix[i] = row;
        }
        
        if (type == "Alphabetical")
        {
            var comparer = Comparer<string>.Default;
            Array.Sort(matrix, (x, y) => 
                comparer.Compare(x[index], y[index]));
        }
        else
        {
            var comparer = Comparer<int>.Default;
            Array.Sort(matrix, (x, y) => 
                comparer.Compare(int.Parse(y[index]), int.Parse(x[index])));
        }

        for (int i = 0; i < _data.Length; i++)
        {
            resultData[i + offsetIndex] = matrix[i / _columnsCount][i % _columnsCount];
        }

        return resultData;
    }

    public void Write()
    {
        
    }
}