using Lib;

namespace CSVLib;

/// <summary>
/// Class for validation csv data by template.
/// </summary>
public class CsvTemplate
{
    // Each field in single string and the data must be template-based.
    private readonly string[] _csvData;
    
    public CsvTemplate(string[] csvData)
    {
        _csvData = csvData;
    }

    private const int MinRowsCount = Constants.HeaderRowsCount;

    /// <summary>
    /// Row count greater than or equal to <see cref="MinRowsCount"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">Returns if row count is less than need.</exception>
    private void ValidateRowCount()
    {
        if (_csvData.Length < MinRowsCount)
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
        foreach (string line in _csvData)
        {
            string[] record = CsvParser.ParseCorrectCsvRecord(line, Constants.FieldsSeparator);
            if (record.Length != Constants.ColumnCount)
            {
                throw new ArgumentNullException(Constants.ColumnCountErrorMessage);
            }
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
        string[] headersEn = CsvParser.ParseCorrectCsvRecord(_csvData[0], Constants.FieldsSeparator);
        string[] headersRu = CsvParser.ParseCorrectCsvRecord(_csvData[1], Constants.FieldsSeparator);
        
        foreach (KeyValuePair<string, string> headerPair in Constants.Headers)
        {
            int index = Array.IndexOf(headersEn, headerPair.Key);
            if (index < 0 || headersRu[index] != headerPair.Value)
            {
                throw new ArgumentNullException(Constants.HeadersErrorMessage);
            }
        }
    }

    /// <summary>
    /// Validates according to the calling <b>methods</b>.
    /// </summary>
    public void ValidateTemplate()
    {
        // Order of calling methods is important.
        ValidateRowCount();
        ValidateColumnCount();
        ValidateHeaders();
    }
}