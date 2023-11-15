namespace Lib;

/// <summary>
/// The constant values used by both projects can be changed to any values.
/// </summary>
public struct Constants
{
    // Errors.
    public const string EmptyArrayMessage = "Empty array.";
    public const string DefaultErrorMessage = "Something went wrong.";
    public const string NotAbsolutePathErrorMessage =
        "The specified path does not contain a root, enter an absolute path.";
    public const string FileNotExistErrorMessage = "Nothing was found for the specified path.";

    public const string FileRowsLengthErrorMessage = "File contains less than 2 lines. Where is headers?";
    public const string RowEndErrorMessage = "Row template is not correct.";
    public const string ColumnCountErrorMessage = "Error with count of columns.";
    public const string HeadersErrorMessage = "Error in headers.";
    
    // Default template headers.
    public static readonly Dictionary<string, string> Headers = new()
    {
        {"\"Name\"", "\"ФИО почетного реставратора\""},
        {"\"RankYear\"", "\"Год присуждения звания\""},
        {"\"MainObjects\"", "\"Основные объекты реставрации\""},
        {"\"Workplace\"", "\"Место работы\""},
        {"\"Photo\"", "\"Фотография\""},
        {"\"global_id\"", "\"global_id\""}
    };
    public static readonly int ColumnCount = Headers.Count;
    public static readonly int HeaderRowsCount = 2;
}