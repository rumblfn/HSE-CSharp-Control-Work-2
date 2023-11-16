using Lib;

namespace CSVLib;

/// <summary>
/// Contains methods to work with data.
/// </summary>
public class DataProcessing
{
    private readonly string[] _headersEn;
    private readonly string[] _headersRu;
    private readonly string[] _data;
    private readonly int _columnsCount = Constants.ColumnCount;
    
    /// <summary>
    /// Dp Initialization.
    /// </summary>
    /// <param name="csvData">Array of fields with correct format.</param>
    public DataProcessing(string[] csvData)
    {
        _headersEn = csvData[.._columnsCount];
        _headersRu = csvData[_columnsCount..(_columnsCount * 2)];
        _data = csvData[(_columnsCount * 2)..];
    }

    /// <summary>
    /// It is used to prepare the initial data (headers).
    /// </summary>
    /// <returns>Fields of headers.</returns>
    private string[] GetInitialData()
    {
        return _headersEn.Concat(_headersRu).ToArray();
    }

    /// <summary>
    /// Gets the index of the column with the desired heading.
    /// </summary>
    /// <param name="column">Column name.</param>
    /// <returns>Index of column.</returns>
    private int GetColumnIndex(string column)
    {
        int index = Array.IndexOf(_headersEn, column);
        if (index < 0)
        {
            index = Array.IndexOf(_headersRu, column);
        }
        
        return index;
    }

    /// <summary>
    /// Selection of records for the specified column and search query.
    /// </summary>
    /// <param name="columnName">Column for search.</param>
    /// <param name="sub">Search query.</param>
    /// <returns>Suitable entries, including headers.</returns>
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

    /// <summary>
    /// Sorting record by the specified column and type.
    /// </summary>
    /// <param name="columnName">Column name.</param>
    /// <param name="type">Default sorting types.</param>
    /// <returns>Array of fields.</returns>
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
                comparer.Compare(NumberMethod.Parse(y[index]), NumberMethod.Parse(x[index])));
        }

        for (int i = 0; i < _data.Length; i++)
        {
            resultData[i + offsetIndex] = matrix[i / _columnsCount][i % _columnsCount];
        }

        return resultData;
    }
}