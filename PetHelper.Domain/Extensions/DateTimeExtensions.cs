namespace PetHelper.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime MinusTime(this DateTime bigger, DateTime smaller)
        {
            return new DateTime(
                bigger.Year - smaller.Year, 
                bigger.Month - smaller.Month, 
                bigger.Day - smaller.Day, 
                bigger.Hour - smaller.Hour, 
                bigger.Minute - smaller.Minute, 
                bigger.Second - smaller.Second
            );
        }

        public static decimal ToMinutes(this DateTime source)
        {
            var months = source.Year * 12 + source.Month;
            var days = months * 30 + source.Day;
            var hours = days * 24 + source.Hour;
            var minutes = hours * 60 + source.Minute;

            return minutes;
        }
    }
}
