namespace Lib;

public class CsvTemplate
{
    // Each field in single string and the data must be template-based.
    private readonly string[] _csvData;
    
    public CsvTemplate(string[] csvData)
    {
        _csvData = csvData;
    }
    
    private readonly int _minRowsCount = Constants.HeaderRowsCount;
    
    /// <summary>
    /// Row count greater than or equal to <see cref="_minRowsCount"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">Returns if row count is less than need.</exception>
    private void ValidateRowCount()
    {
        if (_csvData.Length < Constants.ColumnCount * _minRowsCount)
        {
            throw new ArgumentNullException(Constants.FileRowsLengthErrorMessage);
        }
    }

    /// <summary>
    /// Validate by template headers column count.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Returns if any row contains another column count.
    /// </exception>
    private void ValidateColumnCount()
    {
        Console.WriteLine(string.Join(';', _csvData));
        Console.WriteLine(_csvData.Length);
        Console.WriteLine(Constants.ColumnCount);
        if (_csvData.Length % Constants.ColumnCount != 0)
        {
            throw new ArgumentNullException(Constants.ColumnCountErrorMessage);
        }
    }

    /// <summary>
    /// Checks headers for compliance.
    /// Columns can go in different order,
    /// but Russian and English headings must match the template.
    /// </summary>
    /// <exception cref="ArgumentNullException">Error in headers.</exception>
    private void ValidateHeaders()
    {
        foreach (KeyValuePair<string, string> headerPair in Constants.Headers)
        {
            int index = Array.IndexOf(_csvData, headerPair.Key);
            if (
                index < 0 || 
                index >= Constants.ColumnCount || 
                _csvData[index + Constants.ColumnCount] != headerPair.Value
                )
            {
                throw new ArgumentNullException(Constants.HeadersErrorMessage);
            }
        }
    }

    /// <summary>Validates according to the calling <b>methods</b>.</summary>
    public void ValidateTemplate()
    {
        // Order of calling methods is important.
        ValidateRowCount();
        ValidateColumnCount();
        ValidateHeaders();
    }
}