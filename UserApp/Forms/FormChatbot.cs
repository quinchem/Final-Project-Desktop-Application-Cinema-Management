using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedData.Repositories;

namespace UserApp.Forms
{
    public partial class FormChatbot : Form
    {
        private readonly UserMainForm _parentForm;
        private readonly ChatbotRepo _repo;
        private readonly List<string> _userHistoryGenres = new();

        private const string API_KEY = "AIzaSyA3o7LHNHl2_xJyBUjrBHaIURDFJ_r0W3A";

        public FormChatbot(UserMainForm parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;

            _repo = new ChatbotRepo();

            btnSend.Click += btnSend_Click;

            this.Shown += (s, e) => txtChat.Focus();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string userMessage = txtChat.Text.Trim();
            if (string.IsNullOrEmpty(userMessage)) return;

            rctChat.AppendText("Bạn: " + userMessage + "\n");
            txtChat.Clear();

            // Lấy dữ liệu từ DB
            string dbData = ProcessUserQuestion(userMessage);

            // Gửi thông tin đến AI
            string aiResponse = await SendMessageToGeminiAPI(userMessage, dbData);

            rctChat.AppendText("Bot: " + aiResponse + "\n\n");
            rctChat.ScrollToCaret();
        }

        private string ProcessUserQuestion(string msg)
        {
            msg = msg.ToLower().Trim();
            string movieName = MovieHelper.ExtractMovieTitle(msg);
            //Lịch chiếu phim
            if ((msg.Contains("lịch chiếu") || msg.Contains("suất chiếu") ||
                 msg.Contains("mấy giờ") || msg.Contains("đặt vé") || msg.Contains("mua vé"))
                && !string.IsNullOrEmpty(movieName))
            {
                return _repo.GetShowtimesByMovie(movieName);
            }

            //Thông tin chi tiết phim
            if (!string.IsNullOrEmpty(movieName))
            {
                if (msg.Contains("đạo diễn") || msg.Contains("ai làm phim") || msg.Contains("ai quay"))
                    return _repo.GetMovieDetails(movieName, "director");

                if (msg.Contains("diễn viên") || msg.Contains("ai đóng") || msg.Contains("nhân vật"))
                    return _repo.GetMovieDetails(movieName, "actor");

                if (msg.Contains("thời lượng") || msg.Contains("bao lâu") || msg.Contains("bao nhiêu phút"))
                    return _repo.GetMovieDetails(movieName, "duration");

                if (msg.Contains("thể loại") || msg.Contains("phim gì") || msg.Contains("kinh dị không"))
                    return _repo.GetMovieDetails(movieName, "genre");

                if (msg.Contains("ngày chiếu") || msg.Contains("khi nào chiếu") || msg.Contains("khởi chiếu"))
                    return _repo.GetMovieDetails(movieName, "release_date");

                if (msg.Contains("tiếng gì") || msg.Contains("nước nào") || msg.Contains("xuất xứ") || msg.Contains("phụ đề"))
                    return _repo.GetMovieDetails(movieName, "language");

                if (msg.Contains("thông tin") || msg.Contains("nội dung") || msg.Contains("mô tả"))
                    return _repo.GetMovieDetails(movieName, "all");
            }
            //Ngôn ngữ phim
            if (msg.Contains("tiếng") || msg.Contains("phim") || msg.Contains("nước"))
            {
                if (msg.Contains("tiếng anh") || msg.Contains("phim mỹ") || msg.Contains("âu mỹ"))
                    return _repo.GetMoviesByLanguage("anh");

                if (msg.Contains("tiếng việt") || msg.Contains("phim việt"))
                    return _repo.GetMoviesByLanguage("việt");

                if (msg.Contains("tiếng hàn") || msg.Contains("phim hàn"))
                    return _repo.GetMoviesByLanguage("hàn");

                if (msg.Contains("tiếng thái") || msg.Contains("phim thái"))
                    return _repo.GetMoviesByLanguage("thái");

                if (msg.Contains("tiếng nhật") || msg.Contains("phim nhật") ||
                    msg.Contains("anime") || msg.Contains("hoạt hình nhật"))
                    return _repo.GetMoviesByLanguage("nhật");

                if (msg.Contains("tiếng trung") || msg.Contains("quan thoại") || msg.Contains("đài loan"))
                    return _repo.GetMoviesByLanguage("quan thoại");
            }

            //Thể loại phim
            string genre = ExtractGenre(msg);
            if (!string.IsNullOrEmpty(genre))
            {
                _userHistoryGenres.Add(genre);
                return _repo.SuggestNowOrSoonByGenre(genre);
            }
            //Gợi ý phim
            if (msg.Contains("gợi ý") || msg.Contains("phim hay") || msg.Contains("xem gì"))
            {
                if (string.IsNullOrEmpty(genre) && _userHistoryGenres.Count > 0)
                    genre = _userHistoryGenres.Last();

                string result = string.IsNullOrEmpty(genre)
                                ? _repo.GetMoviesInTheaters()  // Không có thể loại → gợi ý phim đang chiếu
                                : _repo.SuggestNowOrSoonByGenre(genre);

                if (!string.IsNullOrEmpty(genre) && !result.Contains("chưa có"))
                    _userHistoryGenres.Add(genre);

                return result;
            }

            //Giá vé
            if (msg.Contains("giá vé") || msg.Contains("bao nhiêu tiền"))
                return _repo.GetSeatPricesSummary();

            //Phim đang chiếu / sắp chiếu
            if (msg.Contains("đang chiếu") || msg.Contains("phim mới"))
                return _repo.GetMoviesInTheaters();

            if (msg.Contains("sắp chiếu"))
                return _repo.GetComingSoonMovies();

            return "";
        }


        // Gửi thông tin lên Gemini API
        private async Task<string> SendMessageToGeminiAPI(string userMessage, string dbData)
        {
            string apiUrl =
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={API_KEY}";

            string prompt;

            // Nếu DB không trả dữ liệu
            if (string.IsNullOrEmpty(dbData))
            {
                prompt = $@"
Bạn là trợ lý ảo của rạp chiếu phim.
Người dùng: ""{userMessage}""

Quy tắc:
- Nếu người dùng chào → chào lại thân thiện.
- Nếu họ hỏi phim nhưng DB chưa có → xin lỗi và nói chưa cập nhật.
- Tuyệt đối không bịa thông tin.
";
            }
            else
            {
                // Nếu có dữ liệu DB
                prompt = $@"
Bạn là nhân viên tư vấn rạp phim.
Dùng CHÍNH XÁC dữ liệu sau để trả lời:

======================
{dbData}
======================

Người dùng hỏi: ""{userMessage}""

Quy tắc:
- Không dùng kiến thức internet ngoài DB.
- Nếu thiếu thông tin → trả lời 'Chưa cập nhật'.
- Trả lời tự nhiên, thân thiện, mời khách đặt vé.
";
            }

            var payload = new
            {
                contents = new object[]
                {
                    new
                    {
                        role = "user",
                        parts = new object[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            try
            {
                string json = JsonSerializer.Serialize(payload);

                using var client = new HttpClient();
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                string resultJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return "Lỗi kết nối AI server.";

                var doc = JsonDocument.Parse(resultJson);
                string text =
                    doc.RootElement.GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return text.Trim();
            }
            catch (Exception ex)
            {
                return "Lỗi xử lý: " + ex.Message;
            }
        }

        // Tách các thê rloại phim
        private string ExtractGenre(string msg)
        {
            string[] genres =
            {
                "hành động", "tình cảm", "hài", "kinh dị", "tâm lý",
                "hoạt hình", "phiêu lưu", "khoa học viễn tưởng",
                "gia đình", "hồi hộp", "thần thoại", "bí ẩn", "kịch tính"
            };

            foreach (var g in genres)
                if (msg.Contains(g)) return g;

            return "";
        }


        //Tách tên phim
        public static class MovieHelper
        {
            public static string ExtractMovieTitle(string msg)
            {
                string lower = msg.ToLower();

                string[] prefixes =
                {
                    "phim", "thông tin phim", "chi tiết phim", "mô tả phim",
                    "review phim", "lịch chiếu phim", "suất chiếu phim",
                    "cho mình hỏi", "về", "là gì", "ai đóng", "của"
                };

                foreach (var pre in prefixes)
                    lower = lower.Replace(pre, "");

                char[] trim = { '*', '?', '!', '.', ' ', ',', '-', ':', '"' };

                return lower.Trim(trim);
            }
        }


        private void txtChat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                e.SuppressKeyPress = true;
                btnSend.PerformClick();
            }
        }
    }
}



