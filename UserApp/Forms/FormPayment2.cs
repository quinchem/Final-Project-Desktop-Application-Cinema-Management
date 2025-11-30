using Newtonsoft.Json.Linq;
using SharedData.MoMo;
using SharedData.Models;
using SharedData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net; // Dùng cho WebClient và SecurityProtocol
using System.Drawing; // Dùng cho Image và PictureBox
using System.Diagnostics; // Dùng cho Process

namespace UserApp
{
    public partial class FormPayment2 : Form
    {
        // =================================================================================
        // KEY ĐÃ ĐÚNG (GIỮ NGUYÊN)
        // =================================================================================
        private const string PARTNER_CODE = "MOMOFZTI20251130_TEST";
        private const string ACCESS_KEY = "HTYX5Dl2Hao3j7Zk";
        private const string SECRET_KEY = "7qHvdJbaJVbDlj5rGDXMecdpmzyEwYKg";
        // =================================================================================

        private ShowtimeInfo _showtime;
        private List<SeatUser> _seats;
        private Customer _customer;
        private double _total;

        private string _orderId; // Lưu lại OrderId để kiểm tra trạng thái
        private string _requestId; // Lưu RequestId ban đầu

        // Timer để tự động kiểm tra trạng thái giao dịch
        private System.Windows.Forms.Timer _checkStatusTimer;

        // Vẫn giữ callback server nếu muốn (nhưng logic chính sẽ dùng Timer)
        private readonly MomoCallbackServer _callbackServer = new MomoCallbackServer();

        public FormPayment2(ShowtimeInfo showtime, List<SeatUser> seats, Customer customer, double total)
        {
            InitializeComponent();

            _showtime = showtime;
            _seats = seats;
            _customer = customer;
            _total = total;

            // Khởi tạo Timer kiểm tra trạng thái (3 giây check 1 lần)
            _checkStatusTimer = new System.Windows.Forms.Timer();
            _checkStatusTimer.Interval = 3000;
            _checkStatusTimer.Tick += CheckStatusTimer_Tick;

            LoadPaymentInfo();
            // StartCallbackServer(); // Có thể tạm tắt callback server local nếu dùng Timer
            CreateMomoPayment();
        }

        // Sự kiện Timer chạy mỗi 3 giây
        private void CheckStatusTimer_Tick(object sender, EventArgs e)
        {
            // Gọi hàm kiểm tra trạng thái
            CheckTransactionStatus();
        }

        // Hàm chủ động hỏi MoMo xem đơn hàng xong chưa
        private void CheckTransactionStatus()
        {
            try
            {
                string endpoint = "https://test-payment.momo.vn/v2/gateway/api/query";

                // Tạo requestId mới cho mỗi lần query
                string queryRequestId = Guid.NewGuid().ToString();

                // Tạo chữ ký cho Query Request
                // Chuẩn chữ ký Query: accessKey=$accessKey&orderId=$orderId&partnerCode=$partnerCode&requestId=$requestId
                string rawHash = "accessKey=" + ACCESS_KEY +
                                 "&orderId=" + _orderId +
                                 "&partnerCode=" + PARTNER_CODE +
                                 "&requestId=" + queryRequestId;

                MoMoSecurity crypto = new MoMoSecurity();
                string signature = crypto.signSHA256(rawHash, SECRET_KEY);

                JObject message = new JObject
                {
                    { "partnerCode", PARTNER_CODE },
                    { "requestId", queryRequestId },
                    { "orderId", _orderId },
                    { "signature", signature },
                    { "lang", "vi" }
                };

                string response = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                JObject json = JObject.Parse(response);

                // Nếu MoMo trả về resultCode = 0 nghĩa là Đã Thanh Toán Thành Công
                if (json["resultCode"] != null && json["resultCode"].ToString() == "0")
                {
                    _checkStatusTimer.Stop(); // Dừng kiểm tra
                    HandlePaymentSuccess();   // Chuyển màn hình
                }
            }
            catch (Exception)
            {
                // Lỗi mạng hoặc lỗi API thì bỏ qua, chờ lần tick tiếp theo
            }
        }

        private void HandlePaymentSuccess()
        {
            this.Invoke(new Action(() =>
            {
                try
                {
                    SaveBillToDatabase();

                    lblTrangThai.Text = "Thanh toán thành công!";
                    MessageBox.Show("Thanh toán MOMO thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    MessageBox.Show("Lỗi sau thanh toán: " + ex.Message);
                }
            }));
        }

        private void LoadPaymentInfo()
        {
            try
            {
                var filmRepo = new FilmRepo();
                var film = filmRepo.GetById(_showtime.movie_id);

                lblPhim.Text = film != null ? $"{film.title} ({film.age_restriction})" : _showtime.title;

                lblLoaiRap.Text = $"{_showtime.auditorium_type} - Phòng {_showtime.name}";
                lblNgay.Text = _showtime.show_date;
                lblGio.Text = $"{_showtime.start_time} - {_showtime.end_time}";

                lblGhe.Text = string.Join(", ", _seats.OrderBy(s => s.Row)
                                                     .ThenBy(s => s.Col)
                                                     .Select(s => $"{s.Row}{s.Col:00}"));

                lblTien.Text = _total.ToString("N0") + " VND";
                lblKhachHang.Text = _customer.full_name;

                lblTrangThai.Text = "Đang tạo mã thanh toán...";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị Payment2: " + ex.Message);
            }
        }

        // Tạm thời không cần CallbackServer vì đã dùng Timer
        private void StartCallbackServer()
        {
            try
            {
                _callbackServer.Start();
                _callbackServer.OnPaymentSuccess += CallbackSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khởi động Callback Server: " + ex.Message);
            }
        }

        private void CallbackSuccess(MomoCallbackData data)
        {
            // Logic cũ (giữ lại để tham khảo)
            if (!string.Equals(data.orderId, _orderId, StringComparison.OrdinalIgnoreCase))
                return;

            if (data.resultCode == 0)
            {
                _checkStatusTimer.Stop();
                HandlePaymentSuccess();
            }
        }

        private void CreateMomoPayment()
        {
            picQR.SizeMode = PictureBoxSizeMode.Zoom;
            picQR.Cursor = Cursors.Hand;
            picQR.Click -= PicQR_Click;
            picQR.Click += PicQR_Click;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            try
            {
                string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";

                string partnerCode = PARTNER_CODE.Trim();
                string accessKey = ACCESS_KEY.Trim();
                string secretKey = SECRET_KEY.Trim();

                string orderInfo = "Thanh toan ve xem phim";
                string amount = Convert.ToInt64(_total).ToString();

                _orderId = Guid.NewGuid().ToString("N");
                _requestId = Guid.NewGuid().ToString("N"); // Lưu lại requestId

                string redirectUrl = "https://momo.vn";
                string ipnUrl = "https://webhook.site/8095cf34-d952-448d-b231-550802c23eb5";

                string extraData = "";
                string requestType = "captureWallet";

                // Tạo chuỗi Hash
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

                string response = PaymentRequest.sendPaymentRequest(endpoint, message.ToString(Newtonsoft.Json.Formatting.None));

                if (string.IsNullOrEmpty(response) || !response.TrimStart().StartsWith("{"))
                {
                    MessageBox.Show("Phản hồi không hợp lệ:\n" + response);
                    lblTrangThai.Text = "Lỗi kết nối";
                    return;
                }

                JObject json = JObject.Parse(response);

                if (json["resultCode"]?.ToString() != "0")
                {
                    string errorCode = json["resultCode"]?.ToString();
                    if (errorCode == "11007" || errorCode == "1001")
                    {
                        Clipboard.SetText(rawHash);
                        MessageBox.Show($"Lỗi chữ ký (11007). RawHash đã copy vào Clipboard.", "Lỗi Key", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show($"Lỗi MoMo: {json["message"]} (Mã: {errorCode})", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    lblTrangThai.Text = "Tạo QR thất bại";
                    return;
                }

                string qrUrl = json["qrCodeUrl"]?.ToString();
                string payUrl = json["payUrl"]?.ToString();

                // NẾU MOMO KHÔNG TRẢ VỀ QR -> TỰ TẠO TỪ PAYURL
                if (string.IsNullOrEmpty(qrUrl) && !string.IsNullOrEmpty(payUrl))
                {
                    string encodedPayUrl = System.Net.WebUtility.UrlEncode(payUrl);
                    qrUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=250x250&data={encodedPayUrl}";
                }

                if (string.IsNullOrEmpty(qrUrl))
                {
                    MessageBox.Show("Không tạo được mã QR.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblTrangThai.Text = "Lỗi tạo QR";
                    return;
                }

                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "Mozilla/5.0");
                    byte[] imageBytes = client.DownloadData(qrUrl);
                    using (var ms = new System.IO.MemoryStream(imageBytes))
                    {
                        picQR.Image = Image.FromStream(ms);
                    }
                }

                lblTrangThai.Text = "Đang chờ thanh toán (Tự động kiểm tra)...";
                picQR.Tag = payUrl;

                // BẮT ĐẦU TIMER KIỂM TRA TRẠNG THÁI
                _checkStatusTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

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
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở trình duyệt: " + ex.Message);
            }
        }

        private void SaveBillToDatabase()
        {
            BillRepo repo = new BillRepo();
            repo.CreateBill(
                _customer.customer_id,
                _showtime.showtime_id,
                _total,
                _seats.Select(s => s.SeatId).ToList()
            );
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // Dừng timer khi thoát form
            _checkStatusTimer.Stop();

            var parent = this.ParentForm as UserMainForm;
            if (parent != null)
            {
                parent.OpenChildForm(new FormPayment1(_showtime, _seats, _customer));
            }
            else
            {
                this.Close();
            }
        }
    }
}