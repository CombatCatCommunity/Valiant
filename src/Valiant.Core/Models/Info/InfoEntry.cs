using System.Globalization;

namespace Valiant.Models;

public record class InfoEntry(
    string Value,
    string Language)
{
    public DateTime EditedAt { get; set; }

    public override string ToString() => Value;
}