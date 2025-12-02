using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserApp.Forms
{
    public partial class FormChatbot : Form
    {
        private UserMainForm _parentForm;
        private const string API_KEY = "Your_Key";

        private readonly List<string> _userHistoryGenres = new();

        public FormChatbot(UserMainForm parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
            btnSend.Click += btnSend_Click;
            this.Shown += (s, e) =>
            {
                txtChat.Focus();
            };
        }
        private async void btnSend_Click(object sender, EventArgs e)
        {
            string userMessage = txtChat.Text.Trim();
            if (string.IsNullOrEmpty(userMessage)) return;

            // Hiển thị tin nhắn người dùng
            rctChat.AppendText("Bạn: " + userMessage + "\n");
            txtChat.Clear();

            // 1. Lấy dữ liệu từ DB dựa trên ý định câu hỏi
            string dbData = ProcessUserQuestion(userMessage);

            // 2. Gửi dữ liệu + câu hỏi lên Gemini
            string aiResponse = await SendMessageToGeminiAPI(userMessage, dbData);

            // Hiển thị phản hồi AI
            rctChat.AppendText("Bot: " + aiResponse + "\n\n");
            rctChat.ScrollToCaret();
        }

        // Hàm này xác định người dùng hỏi gì để lấy đúng dữ liệu DB
        private string ProcessUserQuestion(string msg)
        {
            msg = msg.ToLower();
            string movieName = MovieHelper.ExtractMovieTitle(msg);

            // 1. Hỏi về nội dung/mô tả phim (Ưu tiên cao)
            if ((msg.Contains("lịch chiếu") || msg.Contains("suất chiếu") || msg.Contains("đặt vé")) && !string.IsNullOrEmpty(movieName))
            {
                return DatabaseHelper.GetShowtimesByMovie(movieName);
            }

            // 2. Nhóm hỏi CHI TIẾT CỤ THỂ (Phân loại ý định)
            if (!string.IsNullOrEmpty(movieName))
            {
                // Hỏi Đạo diễn
                if (msg.Contains("đạo diễn") || msg.Contains("ai làm phim") || msg.Contains("ai quay"))
                    return DatabaseHelper.GetMovieDetails(movieName, "director");

                // Hỏi Diễn viên
                if (msg.Contains("diễn viên") || msg.Contains("ai đóng") || msg.Contains("nhân vật"))
                    return DatabaseHelper.GetMovieDetails(movieName, "actor");

                // Hỏi Thời lượng
                if (msg.Contains("thời lượng") || msg.Contains("bao lâu") || msg.Contains("bao nhiêu phút") || msg.Contains("dài không"))
                    return DatabaseHelper.GetMovieDetails(movieName, "duration");

                // Hỏi Thể loại
                if (msg.Contains("thể loại") || msg.Contains("phim gì") || msg.Contains("kinh dị không"))
                    return DatabaseHelper.GetMovieDetails(movieName, "genre");

                // Hỏi Ngày chiếu
                if (msg.Contains("ngày chiếu") || msg.Contains("khi nào chiếu") || msg.Contains("khởi chiếu"))
                    return DatabaseHelper.GetMovieDetails(movieName, "release_date");

                // Hỏi Ngôn ngữ
                if (msg.Contains("nước nào") || msg.Contains("xuất xứ") || msg.Contains("tiếng gì") || msg.Contains("phụ đề"))
                    return DatabaseHelper.GetMovieDetails(movieName, "language");

                // Hỏi Nội dung/Mặc định (Lấy hết)
                if (msg.Contains("thông tin") || msg.Contains("nội dung") || msg.Contains("mô tả") || msg.Contains("chi tiết"))
                    return DatabaseHelper.GetMovieDetails(movieName, "all");
            }
            if (msg.Contains("tiếng") || msg.Contains("phim") || msg.Contains("nước"))
            {
                // --- Tiếng Anh / Mỹ ---
                if (msg.Contains("tiếng anh") || msg.Contains("phim mỹ") || msg.Contains("âu mỹ") || msg.Contains("nước ngoài"))
                    return DatabaseHelper.GetMoviesByLanguage("anh"); // Tìm chữ "anh" trong DB

                // --- Tiếng Việt ---
                if (msg.Contains("tiếng việt") || msg.Contains("phim việt") || msg.Contains("việt nam"))
                    return DatabaseHelper.GetMoviesByLanguage("việt");

                // --- Tiếng Hàn ---
                if (msg.Contains("tiếng hàn") || msg.Contains("phim hàn") || msg.Contains("korea"))
                    return DatabaseHelper.GetMoviesByLanguage("hàn");

                // --- Tiếng Thái ---
                if (msg.Contains("tiếng thái") || msg.Contains("phim thái"))
                    return DatabaseHelper.GetMoviesByLanguage("thái");

                // --- Tiếng Nhật / Anime ---
                if (msg.Contains("tiếng nhật") || msg.Contains("phim nhật") || msg.Contains("anime") || msg.Contains("hoạt hình nhật"))
                    return DatabaseHelper.GetMoviesByLanguage("nhật");

                // --- Tiếng Trung / Quan Thoại ---
                if (msg.Contains("tiếng trung") || msg.Contains("quan thoại") || msg.Contains("đài loan"))
                    return DatabaseHelper.GetMoviesByLanguage("quan thoại");
            }
            //string movieName = MovieHelper.ExtractMovieTitle(msg);
            // 2. Hỏi lịch chiếu
            if ((msg.Contains("lịch chiếu") || msg.Contains("suất chiếu") || msg.Contains("mấy giờ") ||
         msg.Contains("đặt vé") || msg.Contains("mua vé")) && !string.IsNullOrEmpty(movieName))
            {
                // Gọi hàm lấy lịch chiếu (Đã sửa ở DatabaseHelper2)
                string data = DatabaseHelper.GetShowtimesByMovie(movieName);

                // Nếu tìm thấy lịch chiếu, trả về ngay
                if (!string.IsNullOrEmpty(data)) return data;

                // Nếu không thấy lịch, có thể phim đó chưa có lịch hoặc user gõ sai tên
                // Code sẽ chạy tiếp xuống dưới hoặc trả về rỗng
            }
            string genre = ExtractGenre(msg);
            if (!string.IsNullOrEmpty(genre))
            {
                _userHistoryGenres.Add(genre);
                return DatabaseHelper.SuggestNowOrSoonByGenre(genre);
            }
            if (msg.Contains("gợi ý") || msg.Contains("phim hay") || msg.Contains("xem gì") ||
        msg == "gợi ý phim" || msg == "gợi ý phim cho mình")
            {
                // B1: Thử lấy thể loại từ câu nói (VD: "Gợi ý phim tình cảm")
                //string genre = ExtractGenre(msg);

                // B2: Nếu khách không nói thể loại -> Lấy từ lịch sử cũ (nếu có)
                if (string.IsNullOrEmpty(genre) && _userHistoryGenres.Count > 0)
                {
                    genre = _userHistoryGenres[_userHistoryGenres.Count - 1]; // Lấy cái mới nhất
                }

                string result;
                if (string.IsNullOrEmpty(genre))
                {
                    // B3.1: Không có thể loại + Không có lịch sử -> Gợi ý phim ĐANG CHIẾU (Hot nhất)
                    result = DatabaseHelper.GetMoviesInTheaters();
                }
                else
                {
                    // B3.2: Có thể loại -> Tìm phim theo thể loại
                    result = DatabaseHelper.SuggestNowOrSoonByGenre(genre);

                    // Lưu lại thể loại này vào lịch sử để lần sau dùng tiếp
                    if (!string.IsNullOrEmpty(result) && !result.Contains("chưa có phim"))
                    {
                        _userHistoryGenres.Add(genre);
                    }
                }

                if (string.IsNullOrEmpty(result) || result.Contains("chưa có phim"))
                    return $"Xin lỗi, hiện tại rạp chưa có phim nào phù hợp với yêu cầu '{genre}' của bạn.";

                return result;
            }

            // 3. Hỏi giá vé
            if (msg.Contains("giá vé") || msg.Contains("bao nhiêu tiền"))
            {
                return DatabaseHelper.GetSeatPricesSummary();
            }

            // 4. Hỏi phim đang chiếu
            if (msg.Contains("đang chiếu") || msg.Contains("phim mới"))
            {
                return DatabaseHelper.GetMoviesInTheaters();
            }

            // 5. Hỏi phim sắp chiếu
            if (msg.Contains("sắp chiếu"))
            {
                return DatabaseHelper.GetComingSoonMovies();
            }

            // 6. Gợi ý phim theo thể loại
            //string genre = ExtractGenre(msg);
            if (!string.IsNullOrEmpty(genre))
            {
                _userHistoryGenres.Add(genre);
                return DatabaseHelper.SuggestNowOrSoonByGenre(genre);
            }

            // Mặc định: Trả về rỗng (để AI tự xử lý câu chào hỏi xã giao)
            return "";
        }

        private async Task<string> SendMessageToGeminiAPI(string userMessage, string dbData)
        {
            string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={API_KEY}";

            // 🔥 TẠO PROMPT (KỊCH BẢN) NGHIÊM NGẶT 🔥
            // Đây là phần quan trọng nhất để chặn kiến thức ngoài
            string prompt;

            if (string.IsNullOrEmpty(dbData))
            {
                // Trường hợp 1: Không có dữ liệu từ DB (Hoặc câu hỏi xã giao)
                prompt = $@"
                Bạn là trợ lý ảo của rạp chiếu phim.
                Người dùng nói: ""{userMessage}""
                
                Hướng dẫn xử lý:
                1. Nếu người dùng chào hỏi, hãy chào lại thân thiện.
                2. Nếu người dùng hỏi về phim mà hệ thống không tìm thấy dữ liệu (Dữ liệu rỗng), hãy xin lỗi và nói rằng bạn không có thông tin về phim đó trong hệ thống.
                3. TUYỆT ĐỐI KHÔNG tự bịa ra thông tin phim hoặc lấy từ kiến thức bên ngoài.
                ";
            }
            else
            {
                // Trường hợp 2: Có dữ liệu từ DB
                prompt = $@"
                Đóng vai: Nhân viên rạp chiếu phim chuyên nghiệp.
                
                DỮ LIỆU CƠ SỞ (Đây là thông tin duy nhất bạn biết):
                ----------------
                {dbData}
                ----------------

                Yêu cầu của người dùng: ""{userMessage}""

                LUẬT BẮT BUỘC:
                1. CHỈ sử dụng thông tin trong phần [DỮ LIỆU CƠ SỞ] để trả lời.
                2. Nếu trong dữ liệu có phần 'MÔ TẢ NỘI DUNG', hãy dùng nó để tóm tắt hấp dẫn cho khách.
                3. KHÔNG được sử dụng kiến thức bên ngoài (Google, Internet) để bổ sung. Nếu dữ liệu thiếu, hãy nói 'Thông tin này chưa được cập nhật'.
                4. Giọng điệu: Thân thiện, ngắn gọn, mời khách đặt vé.
                ";
            }

            var payload = new
            {
                contents = new object[]
                {
                    new
                    {
                        role = "user",
                        parts = new object[] { new { text = prompt } }
                    }
                }
            };

            try
            {
                string jsonPayload = JsonSerializer.Serialize(payload);
                using var client = new HttpClient();
                using var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string resultJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return "Lỗi kết nối AI server.";

                // Parse JSON kết quả
                using var doc = JsonDocument.Parse(resultJson);
                var root = doc.RootElement;
                if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
                {
                    var text = candidates[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();
                    return text?.Trim();
                }
                return "AI không phản hồi.";
            }
            catch (Exception ex)
            {
                return "Lỗi xử lý: " + ex.Message;
            }
        }

        // Các hàm phụ trợ
        private string ExtractGenre(string msg)
        {
            // Danh sách thể loại khớp với Database của bạn
            string[] genres = {
        "hành động", "tình cảm", "hài", "kinh dị", "tâm lý",
        "hoạt hình", "phiêu lưu", "khoa học viễn tưởng",
        "gia đình", "hồi hộp", "thần thoại", "bí ẩn", "kịch tính", "hòa nhạc"
    };

            foreach (var g in genres)
            {
                if (msg.Contains(g)) return g;
            }
            return "";
        }

        public static class MovieHelper
        {
            public static string ExtractMovieTitle(string msg)
            {
                string lowerMsg = msg.ToLower();

                // 🔥 Danh sách các từ thừa cần loại bỏ để lòi ra tên phim
                // Càng liệt kê nhiều, khả năng bắt trúng tên phim càng cao
                string[] prefixes = {
                    "thông tin phim", "chi tiết phim", "nội dung phim", "mô tả phim", "review phim",
                    "đạo diễn phim", "diễn viên phim", "thể loại phim", "lịch chiếu phim", "suất chiếu phim",
                    "đạo diễn", "diễn viên", "thể loại", "thời lượng", "bao nhiêu phút", "ngày chiếu",
                    "phim", "về", "cho mình hỏi", "là gì", "như thế nào", "là ai", "ai đóng", "của", "nước nào"
                };

                foreach (var pre in prefixes)
                {
                    if (lowerMsg.Contains(pre))
                    {
                        lowerMsg = lowerMsg.Replace(pre, "");
                    }
                }

                // Loại bỏ các ký tự đặc biệt (*, ?, !)
                char[] charsToTrim = { '*', '?', '!', '.', ' ', ',', '-', ':', '"', '\'' };
                return lowerMsg.Trim(charsToTrim);
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
