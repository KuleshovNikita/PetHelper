using PetHelper.Domain.Wrappers;

namespace PetHelper.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTimeWrapper MinusTime(this DateTime bigger, DateTime smaller)
        {
            return new DateTimeWrapper
            {
                Year = bigger.Year - smaller.Year,
                Month = bigger.Month - smaller.Month,
                Day = bigger.Day - smaller.Day,
                Hour = bigger.Hour - smaller.Hour,
                Minute = bigger.Minute - smaller.Minute,
                Second = bigger.Second - smaller.Second
            };
        }

        public static decimal ToMinutes(this DateTimeWrapper source)
        {
            var months = source.Year * 12 + source.Month;
            var days = months * 30 + source.Day;
            var hours = days * 24 + source.Hour;
            var minutes = hours * 60 + source.Minute;

            return minutes;
        }
    }
}
