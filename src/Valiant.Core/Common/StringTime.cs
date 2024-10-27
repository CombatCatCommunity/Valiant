namespace Valiant;

public struct StringTime(TimeSpan Value)
{
    public static bool TryParse(string value, out StringTime time)
    {
        time = default;
        try
        {
            time = Parse(value);
            return true;
        } catch
        {
            return false;
        }
    }
    public static StringTime Parse(string value)
    {
        // Assuming 30d24h60m60s

        var result = TimeSpan.Zero;
        string numpart = "";
        foreach (var c in value)
        {
            if (!char.IsLetter(c))
            {
                if (value.Last() == c)
                    throw new ArgumentException($"Final value did not have a time indicator");

                numpart += c;
                continue;
            }

            if (string.IsNullOrWhiteSpace(numpart))
                throw new ArgumentException("Time indicators cannot have multiple characters");
            if (!double.TryParse(numpart.Trim(), out double timeValue))
                throw new ArgumentException($"Value of `{numpart}` is not a valid duration number");

            result = c switch
            {
                'y' => result.Add(TimeSpan.FromDays(timeValue * 365)),
                'w' => result.Add(TimeSpan.FromDays(timeValue * 7)),
                'd' => result.Add(TimeSpan.FromDays(timeValue)),
                'h' => result.Add(TimeSpan.FromHours(timeValue)),
                'm' => result.Add(TimeSpan.FromMinutes(timeValue)),
                's' => result.Add(TimeSpan.FromSeconds(timeValue)),
                _ => throw new ArgumentException($"Value of `{c}` is not a valid time indicator"),
            };
            numpart = "";
        }

        return new StringTime(result);
    }
}
