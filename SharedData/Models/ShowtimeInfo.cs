using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.Models
{
    public class ShowtimeInfo
    {
        public int showtime_id { get; set; }
        public int movie_id { get; set; }
        public string title { get; set; }
        public int auditorium_id { get; set; }
        public string name { get; set; }
        public string auditorium_type { get; set; }
        public string show_date { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }


        // Computed Properties
        //public string Month => show_date.ToString("MM");
        //public string MonthName => $"THÁNG {show_date.Month}";
        //public DayOfWeek DayOfWeek => show_date.DayOfWeek;
        //public int Day => show_date.Day;

        // Format thời gian chiếu: "15:15 - 17:14"
        public string TimeRange => $"{start_time:hh\\:mm} - {end_time:hh\\:mm}";

        // Tên thứ tiếng Việt
        //public string DayName
        //{
        //    get
        //    {
        //        switch (DayOfWeek)
        //        {
        //            case DayOfWeek.Monday: return "Thứ 2";
        //            case DayOfWeek.Tuesday: return "Thứ 3";
        //            case DayOfWeek.Wednesday: return "Thứ 4";
        //            case DayOfWeek.Thursday: return "Thứ 5";
        //            case DayOfWeek.Friday: return "Thứ 6";
        //            case DayOfWeek.Saturday: return "Thứ 7";
        //            case DayOfWeek.Sunday: return "Chủ nhật";
        //            default: return "";
        //        }
        //    }
        //}
    }
}
