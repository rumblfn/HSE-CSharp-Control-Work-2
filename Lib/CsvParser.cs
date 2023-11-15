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

    public CsvParser(string path, char sep)
    {
        _fPath = path;
        _separator = sep;
        _pendingFieldLine = Array.Empty<string>();
    }

    private void ConcatArrays(params string[][] arrays)
    {
        for (int i = 1; i < arrays.Length; i++)
        {
            arrays[0] = arrays[0].Concat(arrays[i]).ToArray();
        }
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
                    ConcatArrays(result, new[] { field.ToString() });
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
            ConcatArrays(result, new[] { field.ToString() });
        }

        return result;
    }
    
    public string[][] Parse()
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
                ConcatArrays(_pendingFieldLine, csvLineInArray);
            }
            else
            {
                if (_pendingFieldLine is { Length: > 0 })
                {
                    ConcatArrays(matrix[currentLineIndex], _pendingFieldLine, csvLineInArray);
                    _pendingFieldLine = Array.Empty<string>();
                }
                else
                {
                    matrix[currentLineIndex] = csvLineInArray;
                }

                currentLineIndex++;
            }
        }
            
        return matrix[..currentLineIndex];
    }
}