using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SAPLink.Core.Utilities;

/// <summary>
/// Extension methods for various data types.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Determines whether all characters in a string are digits.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <returns>True if all characters are digits; otherwise, false.</returns>
    public static bool IsDigit(this string input) => input.All(char.IsDigit);

    /// <summary>
    /// Determines whether the string has any content.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <returns>True if the string is neither null nor empty; otherwise, false.</returns>
    public static bool IsHasValue(this string input) => !string.IsNullOrEmpty(input);

    /// <summary>
    /// Determines whether the string is null or empty.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <returns>True if the string is null or empty; otherwise, false.</returns>
    public static bool IsNullOrEmpty(this string input) => string.IsNullOrEmpty(input);

    /// <summary>
    /// Converts a DateTime to SAP date format.
    /// </summary>
    /// <param name="dateTime">The DateTime to format.</param>
    /// <returns>The formatted date as a string.</returns>
    public static string ToSAPDateFormat(this DateTime dateTime) => dateTime.ToString("yyyyMMdd");

    /// <summary>
    /// Converts a DateTime to Prism To-Date format, ensuring it's in UTC.
    /// </summary>
    /// <param name="dateTime">The DateTime to format.</param>
    /// <returns>The formatted date-time as a string.</returns>
    public static string ToPrismToDateFormat(this DateTime dateTime)
    {
        DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 00, DateTimeKind.Utc);

        if (dateTime.Kind != DateTimeKind.Utc)
        {
            dateTime = dateTime2.ToUniversalTime();
        }

        return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
    }

    /// <summary>
    /// Beautifies a JSON string using indents.
    /// </summary>
    /// <param name="unPrettyJson">The raw JSON string.</param>
    /// <returns>The beautified JSON string.</returns>
    public static string PrettyJson(this string unPrettyJson)
    {
        if (unPrettyJson.IsHasValue())
        {
            var options = new JsonSerializerOptions() { WriteIndented = true };
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(unPrettyJson);

            return JsonSerializer.Serialize(jsonElement, options);
        }

        return unPrettyJson;
    }

    /// <summary>
    /// Converts a DateTime to Prism From-Date format, ensuring it's in UTC.
    /// </summary>
    /// <param name="dateTime">The DateTime to format.</param>
    /// <returns>The formatted date-time as a string.</returns>
    public static string ToPrismFromDateFormat(this DateTime dateTime)
    {
        DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 00, 00, 00, 00, DateTimeKind.Utc);

        if (dateTime.Kind != DateTimeKind.Utc)
        {
            dateTime = dateTime2.ToUniversalTime();
        }

        return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
    }

    /// <summary>
    /// Retrieves a substring from the current string. If the requested length exceeds the string length, it returns the full string.
    /// </summary>
    /// <param name="value">The source string.</param>
    /// <param name="maxLength">The desired substring length.</param>
    /// <returns>The substring.</returns>
    public static string SubString(this string value, int maxLength) => value.Substring(0, Math.Min(value.Length, maxLength));

    /// <summary>
    /// Ensures the directory for the provided file path exists.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    public static void EnsureDirectoryExists(this string filePath)
    {
        var directoryName = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
    }

    /// <summary>
    /// Convert a string to a title-friendly format (e.g., "Title Case String").
    /// </summary>
    /// <param name="input">String to convert</param>
    /// <returns>Title-friendly string</returns>
    public static string ToTitleFormat(this string input)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
    }

    public class ComboBoxItem
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return Value + " - " + Text;
        }
    }
}
