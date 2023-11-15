namespace Lib;

/// <summary>
/// Contains methods to work with data.
/// </summary>
public class DataProcessing
{
    private readonly string[] _headersEn;
    private readonly string[] _headersRu;
    private readonly string[] _data;
    private readonly int _rowLength = Constants.ColumnCount;
    
    public DataProcessing(string[] csvData)
    {
        CsvTemplate template = new (csvData);
        template.ValidateTemplate();

        _headersEn = csvData[.._rowLength];
        _headersRu = csvData[_rowLength..(_rowLength * 2)];
        _data = csvData[(_rowLength * 2)..];
    }

    private string[] GetInitialData()
    {
        return _headersEn.Concat(_headersRu).ToArray();
    }

    private int GetColumnIndex(string column, bool recursive = false)
    {
        string[] aliases = { column, "\"" + column + "\"" };
        int index = -1;

        foreach (string alias in aliases)
        {
            index = Array.IndexOf(_headersEn, alias);
            if (index < 0)
            {
                index = Array.IndexOf(_headersRu, alias);
            }
            if (index >= 0)
            {
                break;
            }
        }
        
        return index;
    }

    public string[] SamplingByColumn(string columnName, string subString)
    {
        string sub = subString.ToLower();
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
                int rowIndex = index / _rowLength;
                int firstRowElIdx = rowIndex * _rowLength;
                
                resultData = resultData
                    .Concat(_data[firstRowElIdx..(firstRowElIdx + _rowLength)])
                    .ToArray();
            }
            index += _rowLength;
        }

        return resultData;
    }

    public string[] SortingByColumn(string columnName)
    {
        return GetInitialData();
    }
}