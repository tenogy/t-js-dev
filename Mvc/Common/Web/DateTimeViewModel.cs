using System;

namespace Tjs.Web
{
	public sealed class DateTimeViewModel : IComparable<DateTimeViewModel>
	{
		public int Year { get; set; }
		public int Month { get; set; }
		public int Day { get; set; }
		public int Hour { get; set; }
		public int Minute { get; set; }
		public int Second { get; set; }
		public int Millisecond { get; set; }

		public DateTimeViewModel()
		{
		}

		public DateTimeViewModel(int year, int month, int day) : this(new DateTime(year, month, day))
		{
		}

		public DateTimeViewModel(DateTime dateTime)
		{
			Year = dateTime.Year;
			Month = dateTime.Month;
			Day = dateTime.Day;
			Hour = dateTime.Hour;
			Minute = dateTime.Minute;
			Second = dateTime.Second;
			Millisecond = dateTime.Millisecond;
		}

		public DateTime ToDateTime()
		{
			DateTime result = new DateTime(Year, Month, Day, Hour, Minute, Second);
			return result.AddMilliseconds(Millisecond);
		}

		public DateTime ToUtcDateTime()
		{
			DateTime result = new DateTime(Year, Month, Day, Hour, Minute, Second, DateTimeKind.Utc);
			return result.AddMilliseconds(Millisecond);
		}

		public int CompareTo(DateTimeViewModel other)
		{
			return ToDateTime().CompareTo(other.ToDateTime());
		}
	}
}