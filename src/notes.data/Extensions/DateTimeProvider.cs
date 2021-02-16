using System;

namespace notes.data.Extensions
{
    public static class DateTimeProvider
    {
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
