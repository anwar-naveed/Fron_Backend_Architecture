namespace Fron.Application.Utility;

public static class Extensions
{
    public static string? ToSqlFormat(this DateTime? dateTime)
    {
        if (dateTime is null)
            return null;
        else 
            return dateTime.Value.ToString("dd.MM.yyyy");
    }
}
