namespace Valiant;

public struct StringTime(TimeSpan Value)
{
    public static StringTime Parse(string value)
    {
        if (TryParse(value, out var result))
            return result;
        throw new ArgumentException("Invalid string time format.", paramName: nameof(value));
    }
    public static bool TryParse(string value, out StringTime time)
    {
        // Assuming 2y1w30d24h60m60s

        var result = TimeSpan.Zero;
        string numpart = "";
        foreach (var c in value)
        {
            if (!char.IsLetter(c))
            {
                // Final value did not have a time indicator
                if (value.Last() == c)
                    return false;

                numpart += c;
                continue;
            }
            // Time indicators cannot have multiple characters
            if (string.IsNullOrWhiteSpace(numpart)) 
                return false;

            // Value of numpart is not a valid duration number
            if (!double.TryParse(numpart.Trim(), out double timeValue))
                return false;

            result = c switch
            {
                'y' => result.Add(TimeSpan.FromDays(timeValue * 365)),
                'w' => result.Add(TimeSpan.FromDays(timeValue * 7)),
                'd' => result.Add(TimeSpan.FromDays(timeValue)),
                'h' => result.Add(TimeSpan.FromHours(timeValue)),
                'm' => result.Add(TimeSpan.FromMinutes(timeValue)),
                's' => result.Add(TimeSpan.FromSeconds(timeValue)),
                _ => TimeSpan.Zero
            };
            numpart = "";
        }

        time = new StringTime(result);
        return true;
    }
}
