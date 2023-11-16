using System.Text;
using Lib;

namespace CSVLib;

/// <summary>
/// Contains methods for working with raw csv data.
/// And converts them to a normal form or vice versa.
/// </summary>
public class CsvParser
{
    private readonly char _separator;
    private const char Quote = '"';

    private readonly string _fPath;
    private string[] _pendingFieldLine;

    private bool _multiline;
    private string _pendingField = "";

    /// <summary>
    /// Initialization.
    /// </summary>
    /// <param name="path">Path to file.</param>
    /// <param name="sep">Fields separator.</param>
    public CsvParser(string path, char sep = Constants.FieldsSeparator)
    {
        _fPath = path;
        _separator = sep;
        _pendingFieldLine = Array.Empty<string>();
    }

    /// <summary>
    /// Merges all arrays into the first one.
    /// </summary>
    /// <param name="baseArr">Target array.</param>
    /// <param name="arrays">Other arrays.</param>
    private static void ConcatArrays(ref string[] baseArr, params string[][] arrays)
    {
        baseArr = arrays
            .Aggregate(baseArr, (current, arr) => current.Concat(arr).ToArray());
    }

    /// <summary>
    /// It is used for static parsing of a ready-made record.
    /// </summary>
    /// <param name="line">Record in string.</param>
    /// <param name="sep">Fields separator.</param>
    /// <returns>Fields of record.</returns>
    public static string[] ParseCorrectCsvRecord(string line, char sep)
    {
        string[] result = Array.Empty<string>();
        bool quoted = false;
        bool withQuotes = false;

        StringBuilder field = new ();

        foreach (char ch in line)
        {
            if (ch == Quote && withQuotes)
            {
                if (field.Length > 0)
                {
                    field.Append(ch);
                    withQuotes = false;
                }
            }
            else
            {
                withQuotes = false;
            }

            if (ch == Quote)
            {
                quoted = !quoted;
            }
            else
            {
                if (ch == sep && !quoted)
                {
                    ConcatArrays(ref result, new[] { field.ToString() });
                    field.Length = 0;
                }
                else
                {
                    field.Append(ch);
                }
            }
        }

        if (!quoted)
        {
            ConcatArrays(ref result, new[] { field.ToString() });
        }

        return result;
    }

    /// <summary>
    /// Converts each row into an array of fields,
    /// and then merges them into a single array.
    /// </summary>
    /// <param name="lines">Data records.</param>
    /// <param name="columnCount">Count of columns.</param>
    /// <param name="sep">Fields separator.</param>
    /// <returns>Array of fields.</returns>
    public static string[] LinesToFields(string[] lines, int columnCount, char sep)
    {
        int fieldsCount = columnCount * lines.Length;
        int fieldsIndex = 0;
        
        string[] fields = new string[fieldsCount];
        foreach (string line in lines)
        {
            string[] record = ParseCorrectCsvRecord(line, sep);
            foreach (string field in record)
            {
                fields[fieldsIndex] = field;
                fieldsIndex++;
            }
        }

        return fields;
    }

    /// <summary>
    /// Converts fields to a string.
    /// </summary>
    /// <param name="fields">Fields of record.</param>
    /// <param name="sep">Fields separator.</param>
    /// <returns>Record.</returns>
    public static string FieldsToLine(string[] fields, char sep)
    {
        for (int i = 0; i < fields.Length; i++)
        {
            fields[i] = '\"' + fields[i] + '\"';
        }

        return string.Join(sep, fields);
    }

    /// <summary>
    /// Converts an array of fields to an array of strings.
    /// </summary>
    /// <param name="fields">Array of fields.</param>
    /// <param name="columnsCount">Count of columns.</param>
    /// <param name="sep">Fields separator.</param>
    /// <returns>Array of records in string.</returns>
    public static string[] FieldsToLines(string[] fields, int columnsCount, char sep)
    {
        string[] lines = new string[fields.Length / columnsCount];
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = FieldsToLine(fields[(i * columnsCount)..((i + 1) * columnsCount)], sep);
        }

        return lines;
    }

    /// <summary>
    /// All fields to the text ready to be written to the file.
    /// </summary>
    /// <param name="fields">Array of fields.</param>
    /// <param name="columnCount">Count of columns.</param>
    /// <param name="sep">Fields separator.</param>
    /// <returns>Joined lines.</returns>
    public static string FieldsToText(string[] fields, int columnCount, char sep)
    {
        return string.Join(Environment.NewLine, FieldsToLines(fields, columnCount, sep));
    }

    /// <summary>
    /// Parses strings using the rules of RFC 4180.
    /// </summary>
    /// <param name="line">One line of file.</param>
    /// <returns>Parsed fields.</returns>
    private string[] ParseLine(string line)
    {
        string[] result = Array.Empty<string>();
        bool quoted = false;
        bool withQuotes = false;

        StringBuilder field = new ();

        foreach (char ch in line)
        {
            if (ch == Quote && withQuotes)
            {
                if (field.Length > 0)
                {
                    field.Append(ch);
                    withQuotes = false;
                }
            }
            else
            {
                withQuotes = false;
            }

            if (_multiline)
            {
                field.Append(_pendingField + Environment.NewLine);
                _pendingField = "";
                quoted = true;
                _multiline = false;
            }

            if (ch == Quote)
            {
                quoted = !quoted;
            }
            else
            {
                if (ch == _separator && !quoted)
                {
                    ConcatArrays(ref result, new[] { field.ToString() });
                    field.Length = 0;
                }
                else
                {
                    field.Append(ch);
                }
            }
        }

        if (quoted)
        {
            _pendingField = field.ToString();
            _multiline = true;
        }
        else
        {
            ConcatArrays(ref result, new[] { field.ToString() });
        }

        return result;
    }
    
    /// <summary>
    /// Csv main parse function.
    /// </summary>
    /// <returns>Array of records.</returns>
    public string[] Parse()
    {
        string[] lines = File.ReadAllLines(_fPath);
        string[][] matrix = new string[lines.Length][];
        int currentLineIndex = 0;

        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            
            string[] csvLineInArray = ParseLine(line);
            if (_multiline)
            {
                ConcatArrays(ref _pendingFieldLine, csvLineInArray);
            }
            else
            {
                matrix[currentLineIndex] = Array.Empty<string>();
                if (_pendingFieldLine is { Length: > 0 })
                {
                    ConcatArrays(ref matrix[currentLineIndex], _pendingFieldLine, csvLineInArray);
                    _pendingFieldLine = Array.Empty<string>();
                }
                else
                {
                    matrix[currentLineIndex] = csvLineInArray;
                }

                currentLineIndex++;
            }
        }
            
        matrix = matrix[..currentLineIndex];

        string[] correctLines = new string[currentLineIndex];
        for (int i = 0; i < currentLineIndex; i++)
        {
            correctLines[i] = string.Join(';', matrix[i].Select(field => '"' + field + '"'));
        }
        return correctLines;
    }
}