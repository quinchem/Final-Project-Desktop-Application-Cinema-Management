using System;
using System.Collections.Generic;
using System.Globalization; // Cần thêm cái này để xử lý ngày tháng chuẩn
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

        // --- THÊM DÒNG NÀY ĐỂ CHỨA ĐƯỜNG DẪN ẢNH ---
        //public string poster_path { get; set; }

        public int auditorium_id { get; set; }
        public string name { get; set; }
        public string auditorium_type { get; set; }
        public string show_date { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }

        // Logic xử lý ngày tháng an toàn hơn (tránh crash nếu sai format)
        public DateTime ParsedDate
        {
            get
            {
                // Thử parse theo chuẩn Việt Nam (dd-MM-yyyy)
                if (DateTime.TryParseExact(show_date, "dd-MM-yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                {
                    return dt;
                }

                // Nếu không được thì thử parse theo chuẩn Database (yyyy-MM-dd)
                if (DateTime.TryParseExact(show_date, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    return dt;
                }

                // Nếu vẫn lỗi thì trả về ngày mặc định (để không crash app)
                return DateTime.MinValue;
            }
        }

        public TimeSpan ParsedStartTime
        {
            get
            {
                if (TimeSpan.TryParse(start_time, out TimeSpan ts)) return ts;
                return TimeSpan.Zero;
            }
        }

        public TimeSpan ParsedEndTime
        {
            get
            {
                if (TimeSpan.TryParse(end_time, out TimeSpan ts)) return ts;
                return TimeSpan.Zero;
            }
        }

        // Các thuộc tính hiển thị (Chỉ đọc)
        public string Month => ParsedDate.ToString("MM");

        public string MonthName => $"THÁNG {ParsedDate.Month}";

        public DayOfWeek DayOfWeek => ParsedDate.DayOfWeek;

        public int Day => ParsedDate.Day;

        // Format thời gian chiếu: "15:15 - 17:14"
        public string TimeRange =>
            $"{ParsedStartTime:hh\\:mm} - {ParsedEndTime:hh\\:mm}";

        // Tên thứ tiếng Việt
        public string DayName
        {
            get
            {
                // Mẹo: Nếu ParsedDate bị lỗi (MinValue) thì trả về chuỗi rỗng
                if (ParsedDate == DateTime.MinValue) return "";

                return DayOfWeek switch
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
    }
}