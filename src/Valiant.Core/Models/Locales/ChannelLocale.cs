using LiteDB;
using System.Globalization;

namespace Valiant.Models;

public class ChannelLocale
{
    [BsonId]
    public ulong Id { get; set; }
    public CultureInfo Culture { get; set; }
}
