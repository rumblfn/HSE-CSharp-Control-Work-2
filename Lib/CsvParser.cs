using System.Text;

namespace Lib;

/// <summary>
/// 
/// </summary>
public class CsvParser
{
    private readonly char _separator;
    private const char Quote = '"';

    private readonly string _fPath;
    private string[] _pendingFieldLine;

    private bool _multiline;
    private string _pendingField = "";

    public CsvParser(string path, char sep = Constants.FieldsSeparator)
    {
        _fPath = path;
        _separator = sep;
        _pendingFieldLine = Array.Empty<string>();
    }

    private static void ConcatArrays(ref string[] baseArr, params string[][] arrays)
    {
        baseArr = arrays
            .Aggregate(baseArr, (current, arr) => current.Concat(arr).ToArray());
    }

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

    public static string FieldsToLine(string[] fields, char sep)
    {
        for (int i = 0; i < fields.Length; i++)
        {
            fields[i] = '\"' + fields[i] + '\"';
        }

        return string.Join(sep, fields);
    }

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