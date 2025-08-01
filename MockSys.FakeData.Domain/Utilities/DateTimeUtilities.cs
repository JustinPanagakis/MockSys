namespace MockSys.FakeData.Domain.Utilities;

internal static class DateTimeUtilities
{
    private static readonly Random _rng = new();

    public static DateTime ConvertToUtcWithRandomTime(DateTime input)
    {
        DateTime utcDate = input.ToUniversalTime();

        int year = utcDate.Year;
        int month = utcDate.Month;
        int day = utcDate.Day;

        int hour = _rng.Next(9, 19);
        int minute = _rng.Next(0, 60);
        int second = _rng.Next(0, 60);

        return new DateTime(
            year, month, day,
            hour, minute, second,
            DateTimeKind.Utc);
    }
}