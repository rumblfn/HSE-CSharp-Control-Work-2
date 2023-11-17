using Lib;

namespace CSVLib;

/// <summary>
/// Contains methods to work with data.
/// </summary>
public static class DataProcessing
{
    private static string[] _headersEn = Array.Empty<string>();
    private static string[] _headersRu = Array.Empty<string>();
    private static string[] _data = Array.Empty<string>();
    private static readonly int ColumnsCount = Constants.ColumnCount;
    
    /// <summary>
    /// Dp Initialization.
    /// </summary>
    /// <param name="csvData">Array of fields with correct format.</param>
    private static void Init(string[] csvData)
    {
        _headersEn = csvData[..ColumnsCount];
        _headersRu = csvData[ColumnsCount..(ColumnsCount * 2)];
        _data = csvData[(ColumnsCount * 2)..];
    }

    /// <summary>
    /// It is used to prepare the initial data (headers).
    /// </summary>
    /// <returns>Fields of headers.</returns>
    private static string[] GetInitialData()
    {
        return _headersEn.Concat(_headersRu).ToArray();
    }

    /// <summary>
    /// Gets the index of the column with the desired heading.
    /// </summary>
    /// <param name="column">Column name.</param>
    /// <returns>Index of column.</returns>
    private static int GetColumnIndex(string column)
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
    /// <param name="fields">Array of fields.</param>
    /// <param name="columnName">Column for search.</param>
    /// <param name="sub">Search query.</param>
    /// <returns>Suitable entries, including headers.</returns>
    public static string[] SamplingByColumn(string[] fields, string columnName, string sub)
    {
        Init(fields);
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
                int rowIndex = index / ColumnsCount;
                int firstRowElIdx = rowIndex * ColumnsCount;
                
                resultData = resultData
                    .Concat(_data[firstRowElIdx..(firstRowElIdx + ColumnsCount)])
                    .ToArray();
            }
            index += ColumnsCount;
        }

        return resultData;
    }

    /// <summary>
    /// Sorting record by the specified column and type.
    /// </summary>
    /// <param name="fields"></param>
    /// <param name="columnName">Column name.</param>
    /// <param name="type">Default sorting types.</param>
    /// <returns>Array of fields.</returns>
    public static string[] SortingByColumn(string[] fields, string columnName, string type)
    {
        Init(fields);
        string[] resultData = GetInitialData();
        int index = GetColumnIndex(columnName);
        if (index < 0)
        {
            return resultData;
        }

        int offsetIndex = resultData.Length;
        int rowsCount = _data.Length / ColumnsCount;
        string[] emptyData = new string[_data.Length];
        resultData = resultData.Concat(emptyData).ToArray();
        
        string[][] matrix = new string[rowsCount][];
        for (int i = 0; i < rowsCount; i++)
        {
            string[] row = new string[ColumnsCount];
            for (int j = 0; j < ColumnsCount; j++)
            {
                row[j] = _data[i * ColumnsCount + j];
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
            resultData[i + offsetIndex] = matrix[i / ColumnsCount][i % ColumnsCount];
        }

        return resultData;
    }
}