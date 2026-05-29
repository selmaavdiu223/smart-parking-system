using System.Text;

namespace SmartParkingSystem.Utils
{
    public static class CsvUtils
    {
        public static string ToLine(params object?[] values)
        {
            return string.Join(",", values.Select(value => Escape(Convert.ToString(value) ?? string.Empty)));
        }

        public static List<string> ParseLine(string line)
        {
            var values = new List<string>();
            var current = new StringBuilder();
            var insideQuotes = false;

            for (var i = 0; i < line.Length; i++)
            {
                var character = line[i];

                if (character == '"')
                {
                    if (insideQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        insideQuotes = !insideQuotes;
                    }
                }
                else if (character == ',' && !insideQuotes)
                {
                    values.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(character);
                }
            }

            values.Add(current.ToString());
            return values;
        }

        private static string Escape(string value)
        {
            if (!value.Contains(',') && !value.Contains('"') && !value.Contains('\n') && !value.Contains('\r'))
                return value;

            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
    }
}
