using System;
using System.Collections.Generic;

namespace Blog.Extensions
{
    public interface IDateTimeResolver
    {
        DateTime NowOfTimeZone { get; }

        DateTime ToLocalTime(DateTime dateTime);

        DateTime ToTimeZone(DateTime utcDateTime);
        DateTime ToUtc(DateTime userDateTime);
        IEnumerable<TimeZoneInfo> ListTimeZones();
        TimeSpan GetTimeSpanByZoneId(string timeZoneId);
    }
}
