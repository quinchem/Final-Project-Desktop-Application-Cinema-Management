using System;
using System.Collections.Generic;
using System.Globalization; // Cần thêm cái này để xử lý ngày tháng chuẩn
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Models
{
    public class ShowtimeInfo
    {
        public string showtime_id { get; set; }
        public string movie_id { get; set; }
        public string title { get; set; }
        public string auditorium_id { get; set; }
        public string name { get; set; }
        public string auditorium_type { get; set; }
        public string show_date { get; set; }
        public string start_time { get; set; } 
        public string end_time { get; set; }
        public int duration { get; set; }

        // Chuẩn hóa ngày – giờ
        public DateTime ParsedDate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(show_date)) return DateTime.MinValue;
                DateTime dt;
                string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "MM/dd/yyyy" };

                if (DateTime.TryParseExact(show_date, formats,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out dt))
                {
                    return dt;
                }
                if (DateTime.TryParse(show_date, out dt)) return dt;

                return DateTime.MinValue;
            }
        }
        public TimeSpan StartTime
        {
            get
            {
                if (string.IsNullOrEmpty(start_time)) return TimeSpan.Zero;
                if (TimeSpan.TryParse(start_time, out TimeSpan time))
                    return time;

                return TimeSpan.Zero;
            }
        }

        public TimeSpan EndTime
        {
            get
            {
                TimeSpan result = TimeSpan.Zero;
                if (!string.IsNullOrEmpty(end_time))
                {
                    TimeSpan.TryParse(end_time, out result);
                }
                if (result == TimeSpan.Zero && duration > 0)
                {
                    return StartTime.Add(TimeSpan.FromMinutes(duration));
                }

                return result;
            }
        }
        public string Month => ParsedDate.ToString("MM");
        public string MonthName => $"THÁNG {ParsedDate.Month}";
        public DayOfWeek DayOfWeek => ParsedDate.DayOfWeek;
        public int Day => ParsedDate.Day;
        public string TimeRange => $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}";
        public string DayName =>
            DayOfWeek switch
            {
                DayOfWeek.Monday => "Thứ 2",
                DayOfWeek.Tuesday => "Thứ 3",
                DayOfWeek.Wednesday => "Thứ 4",
                DayOfWeek.Thursday => "Thứ 5",
                DayOfWeek.Friday => "Thứ 6",
                DayOfWeek.Saturday => "Thứ 7",
                DayOfWeek.Sunday => "Chủ nhật",
                _ => ""
            };
    }
}
