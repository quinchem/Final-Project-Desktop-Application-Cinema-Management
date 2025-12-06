using Newtonsoft.Json.Linq;
using SharedData.Models;
using SharedData.MoMo;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing; 
using System.Linq;
using System.Net; 
using System.Windows.Forms;
using System.Media; 

namespace UserApp
{
    public partial class FormPayment2 : Form
    {
        // Các thông tin cấu hình kết nối MoMo (dùng bản test)
        // PARTNER_CODE: mã định danh của doanh nghiệp trên MoMo
        // ACCESS_KEY + SECRET_KEY: dùng để ký SHA256 đảm bảo không bị giả mạo request
        private const string PARTNER_CODE = "MOMOFZTI20251130_TEST";
        private const string ACCESS_KEY = "HTYX5Dl2Hao3j7Zk";
        private const string SECRET_KEY = "7qHvdJbaJVbDlj5rGDXMecdpmzyEwYKg";

        // Dữ liệu truyền từ FormPayment1 – gồm suất chiếu, ghế, khách hàng, tổng tiền
        private ShowtimeInfo _showtime;
        private List<SeatUser> _seats;
        private Customer _customer;
        private double _total;

        // orderId và requestId là hai tham số MoMo yêu cầu
        // orderId: định danh cho đơn hàng
        // requestId: định danh cho request gửi MoMo
        private string _orderId; 
        private string _requestId;

        // Timer 3 giây/lần dùng để gọi API Query kiểm tra trạng thái thanh toán
        private System.Windows.Forms.Timer _checkStatusTimer;

        // Thời gian hiệu lực của mã QR: 10 phút (600 giây)
        private int _qrCountdown = 600;

        public UserMainForm parentForm;

        public FormPayment2(ShowtimeInfo showtime, List<SeatUser> seats, Customer customer, double total)
        {
            InitializeComponent();
            
            // Gán dữ liệu từ FormPayment1
            _showtime = showtime;
            _seats = seats;
            _customer = customer;
            _total = total;

            // Tạo timer kiểm tra trạng thái giao dịch mỗi 3 giây
            _checkStatusTimer = new System.Windows.Forms.Timer();
            _checkStatusTimer.Interval = 3000;          // 3000ms = 3 giây
            _checkStatusTimer.Tick += CheckStatusTimer_Tick;

            // Load thông tin hiển thị cho người dùng
            LoadPaymentInfo();

            // Bắt đầu tạo QR thanh toán MoMo
            CreateMomoPayment();
        }

         // Timer gọi hàm kiểm tra giao dịch liên tục
        private void CheckStatusTimer_Tick(object sender, EventArgs e)
        {
            // Gọi hàm kiểm tra trạng thái
            CheckTransactionStatus();
        }

        // Hàm kiểm tra trạng thái thanh toán qua API Query của MoMo
        private void CheckTransactionStatus()
        {
            try
            {
                string endpoint = "https://test-payment.momo.vn/v2/gateway/api/query";

                // MoMo yêu cầu mỗi lần query phải tạo 1 requestId mới
                string queryRequestId = Guid.NewGuid().ToString();
                
                // rawHash là chuỗi MoMo bắt buộc phải tạo để ký SHA256
                // Ký dữ liệu bằng secretKey cho Query Request
                string rawHash = "accessKey=" + ACCESS_KEY +
                                 "&orderId=" + _orderId +
                                 "&partnerCode=" + PARTNER_CODE +
                                 "&requestId=" + queryRequestId;

                MoMoSecurity crypto = new MoMoSecurity();
                string signature = crypto.signSHA256(rawHash, SECRET_KEY);
                
                // Tạo JSON body đúng format yêu cầu của MoMo Query API
                JObject message = new JObject
                {
                    { "partnerCode", PARTNER_CODE },
                    { "requestId", queryRequestId },
                    { "orderId", _orderId },
                    { "signature", signature },
                    { "lang", "vi" }
                };

                // Gửi API Query
                string response = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                JObject json = JObject.Parse(response);

                // Nếu MoMo trả về resultCode = 0 nghĩa là thanh toán thành công
                if (json["resultCode"] != null && json["resultCode"].ToString() == "0")
                {
                    _checkStatusTimer.Stop(); // Dừng kiểm tra
                    HandlePaymentSuccess();   // Chuyển màn hình
                }
            }
            catch (Exception)
            {
                // Có lỗi mạng nhưng không ảnh hưởng → tiếp tục đợi
            }
        }

        // Hàm xử lý sau khi MoMo báo thanh toán thành công
        private void HandlePaymentSuccess()
        {
            this.Invoke(new Action(() =>
            {
                try
                {
                    // Lưu hóa đơn vào database
                    SaveBillToDatabase();

                    lblTrangThai.Text = "Thanh toán thành công!";
                    SoundPlayer player = new SoundPlayer(Properties.Resources.purchase_sound);
                    player.Play();
                    MessageBox.Show("Thanh toán MOMO thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Điều hướng về danh sách suất chiếu
                    var parent = this.ParentForm as UserMainForm;
                    if (parent != null)
                    {
                        parent.OpenChildForm(new FormShowtimeList(parent));
                    }
                    else
                    {
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Lỗi sau thanh toán: " + ex.Message);
                }
            }));
        }
        
        // Hiển thị thông tin vé và khách hàng lên giao diện
        private void LoadPaymentInfo()
        {
            try
            {
                var filmRepo = new FilmRepo();
                var film = filmRepo.GetById(_showtime.movie_id);

                // Nếu tìm được phim trong database thì lấy tên chuẩn còn không lấy theo suất chiếu
                lblPhim.Text = film != null ? $"{film.title}" : _showtime.title;

                // Hiển thị phòng chiếu  định dạng phòng
                lblLoaiRap.Text = $"{_showtime.auditorium_type} - {_showtime.name}";
                lblNgay.Text = _showtime.show_date;
                lblGio.Text = $"{_showtime.start_time} - {_showtime.end_time}";
                
                // Danh sách ghế theo thứ tự hàng + cột
                lblGhe.Text = string.Join(", ", 
                                _seats.OrderBy(s => s.Row)
                                      .ThenBy(s => s.Col)
                                      .Select(s => $"{s.Row}{s.Col:00}"));

                // Tổng tiền vé
                lblTien.Text = _total.ToString("N0") + " VND";

                // Tên khách hàng
                lblKhachHang.Text = _customer.full_name;

                lblTrangThai.Text = "Đang tạo mã thanh toán...";
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Lỗi hiển thị mã thanh toán: " + ex.Message);
            }
        }

        // Gửi yêu cầu tạo QR MoMo
        private void CreateMomoPayment()
        {
            picQR.SizeMode = PictureBoxSizeMode.Zoom; // Cho QR hiển thị vừa khung
            picQR.Cursor = Cursors.Hand;              // Cho phép click để mở MoMo
            picQR.Click -= PicQR_Click;
            picQR.Click += PicQR_Click;
            
             // Bật các giao thức bảo mật TLS để kết nối HTTPS an toàn
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            try
            {
                string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";

                string partnerCode = PARTNER_CODE.Trim();
                string accessKey = ACCESS_KEY.Trim();
                string secretKey = SECRET_KEY.Trim();

                string orderInfo = "Thanh toan ve xem phim";
                string amount = Convert.ToInt64(_total).ToString();
                
                // orderId: ID của đơn hàng dùng để truy xuất
                // requestId: ID của request tạo QR
                _orderId = Guid.NewGuid().ToString("N");
                _requestId = Guid.NewGuid().ToString("N"); 

                // redirectUrl và ipnUrl là 2 URL MoMo trả hướng dẫn
                // redirectUrl dùng cho web – không quan trọng trong app desktop
                // ipnUrl: URL MoMo gọi lại báo trạng thái (bản test dùng webhook)
                string redirectUrl = "https://momo.vn";
                string ipnUrl = "https://webhook.site/8095cf34-d952-448d-b231-550802c23eb5";

                string extraData = "";
                string requestType = "captureWallet";

                // rawHash là chuỗi dùng để ký SHA256 để tránh bị giả mạo
                string rawHash =
                    "accessKey=" + accessKey +
                    "&amount=" + amount +
                    "&extraData=" + extraData +
                    "&ipnUrl=" + ipnUrl +
                    "&orderId=" + _orderId +
                    "&orderInfo=" + orderInfo +
                    "&partnerCode=" + partnerCode +
                    "&redirectUrl=" + redirectUrl +
                    "&requestId=" + _requestId +
                    "&requestType=" + requestType;

                MoMoSecurity crypto = new MoMoSecurity();
                string signature = crypto.signSHA256(rawHash, secretKey);

                // Tạo JSON  gửi MoMo để tạo QR Code
                JObject message = new JObject
                {
                    { "partnerCode", partnerCode },
                    { "partnerName", "Hamster Cinema" },
                    { "storeId", "HC01" },
                    { "requestId", _requestId },
                    { "amount", Convert.ToInt64(amount) },
                    { "orderId", _orderId },
                    { "orderInfo", orderInfo },
                    { "redirectUrl", redirectUrl },
                    { "ipnUrl", ipnUrl },
                    { "lang", "vi" },
                    { "extraData", extraData },
                    { "requestType", requestType },
                    { "signature", signature }
                };
                
                // Gửi request tạo QR đến MoMo
                string response = PaymentRequest.sendPaymentRequest(endpoint, message.ToString(Newtonsoft.Json.Formatting.None));
                
                // Kiểm tra xem server có trả đúng JSON không
                if (string.IsNullOrEmpty(response) || !response.TrimStart().StartsWith("{"))
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Phản hồi không hợp lệ:\n" + response);
                    lblTrangThai.Text = "Lỗi kết nối";
                    return;
                }

                JObject json = JObject.Parse(response);
                
                // Nếu MoMo trả về resultCode khác 0 thì lỗi tạo QR
                if (json["resultCode"]?.ToString() != "0")
                {
                    string errorCode = json["resultCode"]?.ToString();
                    // Các lỗi đặc biệt liên quan tới chữ ký
                    if (errorCode == "11007" || errorCode == "1001")
                    {
                        SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                        player.Play();
                        Clipboard.SetText(rawHash);
                        MessageBox.Show($"Lỗi chữ ký (11007). RawHash đã copy vào Clipboard.", "Lỗi Key", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                        player.Play();
                        MessageBox.Show($"Lỗi MoMo: {json["message"]} (Mã: {errorCode})", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    lblTrangThai.Text = "Tạo QR thất bại";
                    return;
                }
                
                // URL chứa mã QR
                string qrUrl = json["qrCodeUrl"]?.ToString();
                string payUrl = json["payUrl"]?.ToString();

               // Nếu MoMo không trả về qrCodeUrl thì tự tạo QR bằng payUrl
                if (string.IsNullOrEmpty(qrUrl) && !string.IsNullOrEmpty(payUrl))
                {
                    string encodedPayUrl = System.Net.WebUtility.UrlEncode(payUrl);
                    qrUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=250x250&data={encodedPayUrl}";
                }

                // Tải ảnh QR về và hiển thị trên PictureBox
                if (string.IsNullOrEmpty(qrUrl))
                {
                    SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                    player.Play();
                    MessageBox.Show("Không tạo được mã QR.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblTrangThai.Text = "Lỗi tạo QR";
                    return;
                }
                
                using (WebClient client = new WebClient()) // Tạo WebClient để tải dữ liệu từ Internet
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0"); 
                    
                    byte[] imageBytes = client.DownloadData(qrUrl); 
                    // Tải ảnh QR từ đường dẫn MoMo trả về dưới dạng mảng byte (chưa phải ảnh hiển thị)
                
                    using (var ms = new System.IO.MemoryStream(imageBytes)) 
                    // Tạo MemoryStream để đọc dữ liệu byte ngay trong RAM (không cần lưu file ra ổ cứng)
                    {
                        picQR.Image = Image.FromStream(ms); 
                        // Chuyển dữ liệu byte thành đối tượng Image rồi hiển thị lên PictureBox
                    }
                }

                lblTrangThai.Text = "Đang chờ thanh toán (Tự động kiểm tra)...";
                
               // Lưu payUrl để mở trình duyệt khi user click QR
               picQR.Tag = payUrl;

                // Bắt đầu kiểm tra trạng thái giao dịch
                _checkStatusTimer.Start();

                // Bắt đầu đếm ngược hạn QR
                timer1.Start();
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }
        
        // Người dùng click vào QR thì mở MoMo theo payUrl
        private void PicQR_Click(object sender, EventArgs e)
        {
            if (picQR.Tag != null)
            {
                OpenBrowser(picQR.Tag.ToString());
            }
        }

        private void OpenBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,          // URL cần mở (thường là payUrl của MoMo)
                    UseShellExecute = true   // Cho phép Windows tự chọn ứng dụng phù hợp để mở URL (trình duyệt)
                    // Khi UseShellExecute = true → Windows sẽ dùng Chrome/Edge mặc định để mở đường link
                });
            }
            catch (Exception ex)
            {
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
                MessageBox.Show("Không thể mở trình duyệt: " + ex.Message);
            }
        }

        // Lưu hóa đơn và ghế đã đặt xuống database
        private void SaveBillToDatabase()
        {
            BillRepo repo = new BillRepo();  
            // Khởi tạo repository để thao tác với bảng hóa đơn
        
            repo.CreateBill(
                _customer.customer_id,                // ID khách hàng mua vé
                _showtime.showtime_id,                // ID suất chiếu
                _total,                               // Tổng tiền thanh toán
                _seats.Select(s => s.SeatId).ToList() // Danh sách ghế (seat_id) cần lưu vào CSDL
            );
        
            // CreateBill sẽ:
            // 1. Tạo 1 record hóa đơn (bill)
            // 2. Ghi từng ghế đã chọn vào bảng bill_detail
            // 3. Đánh dấu ghế là FULL ở suất chiếu tương ứng
        }
        

        // Đếm ngược hạn dùng QR
        private void timer1_Tick(object sender, EventArgs e)
        {
            _qrCountdown--;  
            // Giảm thời gian mỗi giây
        
            lblTimer.Text = $"{_qrCountdown / 60:00}:{_qrCountdown % 60:00}";
            // Hiển thị thời gian còn lại theo định dạng mm:ss
        
            // Hết thời gian → QR hết hạn
            if (_qrCountdown <= 0)
            {
                timer1.Stop();           // Ngừng đếm ngược
                _checkStatusTimer.Stop(); // Dừng kiểm tra trạng thái thanh toán
        
                SoundPlayer player = new SoundPlayer(Properties.Resources.fail_sound);
                player.Play();
        
                MessageBox.Show("Hết thời gian thanh toán (10 phút).\nVui lòng chọn ghế lại!",
                    "Mã QR hết hạn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        
                var parent = this.ParentForm as UserMainForm;
                if (parent != null)
                    parent.OpenChildForm(new FormSeatSelection(parent, _showtime));
                else
                    this.Close();
        
                // Hành động khi QR hết hạn:
                // 1. Hủy tiến trình thanh toán
                // 2. Không lưu ghế
                // 3. Quay người dùng về màn hình chọn ghế để đặt lại
            }
        }
        }
    }
}
