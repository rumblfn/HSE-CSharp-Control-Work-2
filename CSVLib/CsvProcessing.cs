using Lib;

namespace CSVLib;

/// <summary>
/// Csv methods.
/// </summary>
public abstract class CsvProcessing
{
    // Resources.
    private static string _fPath = string.Empty;

    // Setup.
    private const char FieldsSeparator = ';';
    
    /// <summary>
    /// Checks the necessary conditions for the path.
    /// </summary>
    /// <exception cref="ArgumentException">Filepath error.</exception>
    /// <exception cref="ArgumentNullException">File exists error.</exception>
    private static void ValidateFilePath()
    {
        if (!File.Exists(_fPath))
        {
            throw new ArgumentNullException(Constants.FileNotExistErrorMessage);
        }
        if (!Path.IsPathRooted(_fPath))
        {
            throw new ArgumentException(Constants.NotAbsolutePathErrorMessage);
        }
    }

    /// <summary>
    /// Writes a csv file to a jagged array by filepath.
    /// </summary>
    /// <param name="csvFilePath">Path to file.</param>
    /// <returns>Csv data in array.</returns>
    public static string[] Read(string csvFilePath)
    {
        _fPath = csvFilePath;
        ValidateFilePath();
        
        CsvParser parser = new CsvParser(csvFilePath);
        string[] correctLines = parser.Parse();

        CsvTemplate template = new (correctLines);
        template.ValidateTemplate();
        
        return correctLines;
    }
    /// <summary>
    /// Overwrites file with specified data by saved path <see cref="_fPath"/>.
    /// </summary>
    /// <param name="data">Strings to write.</param>
    public static void Write(string[] data)
    {
        try
        {
            string text = string.Join(Environment.NewLine, data);
            File.WriteAllText(_fPath, text + Environment.NewLine);
        }
        catch (Exception ex)
        {
            ConsoleMethod.NicePrint(ex.Message);
        }
    }
    
    /// <summary>
    /// Appends text to file or creates file with specified text.
    /// </summary>
    /// <param name="text">Text to write.</param>
    /// <param name="nPath">Path to write.</param>
    public static void Write(string text, string nPath)
    {
        try
        {
            File.AppendAllText(nPath, text);
        }
        catch (Exception ex)
        {
            ConsoleMethod.NicePrint(ex.Message);
        }
    }
}