**1. Clone dự án từ github về máy tính cá nhân**

Mở Visual Studio, chọn Clone a repository và dán câu lệnh " https://github.com/quinchem/Final-Project-Desktop-Application-Cinema-Management.git
"sau vào Repository location

Sau khi chạy lệnh này, thư mục dự án sẽ được tải xuống hệ thống.

**2. Cách kết nối Chatbot với Gemini**

Mở Visual Studio và tìm câu lệnh khai báo API key "private const string API_KEY = "Your_key"" trong file FormChatbot.cs

Thay "Your_key" bằng API key /*AIzaSyA3o7LHNHl2_xJyBUjrBHaIURDFJ_r0W3Atest*/.

Lưu ý: Bỏ dấu "/" và bỏ từ "test" ở cuối API Key khi nhập vào code

**3. Cách chạy giao diện**

Đầu tiên trên thanh công cụ, chọn UserApp nếu muốn chạy giao diện của người dùng, chọn AdminApp nếu muốn chạy giao diện của quản lý
Nếu chọn UserApp, khi đăng nhập điền thông tin như sau:
Email: quynhtram18@gmail.com
Password:Qtram185@

Nếu chọn AdminApp, khi đăng nhập điền thông tin như sau:
Username: admin
Password: admin12345

<img width="1919" height="151" alt="image" src="https://github.com/user-attachments/assets/50747b93-68a9-46d3-84f6-44ed25cac9f2" />


Sau đó nhấn F5 để chạy được ứng dụng


**4. Tích hợp thanh toán Momo và ứng dụng**

Đầu tiên cần xoá ứng dụng MoMo chính thức nếu đang cài trên điện thoại. Sau đó, tải và cài đặt ứng dụng MoMo Test (UAT) theo đường link: https://developers.momo.vn/v3/download

Tiếp theo, Tạo tài khoản ví test theo các bước sau:

  Bước 1. Nhập số điện thoại của bạn
  
  Bước 2. Nhập OTP mặc định là là 0000 hoặc 000000 trên App MoMo test
  
  Bước 3. Nhập mật khẩu theo mật khẩu mặc định 000000
  
  Bước 4. Điền thông tin cá nhân.

Tiếp đến, cần liên kết ngân hàng & và thêm số dư ví theo các bước

  Bước 1. Ở góc phải phía bên dưới chọn "Ví của tôi", chọn "Liên kết tài khoản" hoặc chọn "Nạp tiền" ngay tại góc trái màn hình chính.
  
  Bước 2. Chọn ngân hàng (Agribank) rồi chọn "Liên kết bằng số thẻ ATM"
  
  Bước 3. Nhập thông tin thẻ ATM.

    Số thẻ: 9704 05XX XXXX XXXX (16 chữ số, X là số bất kỳ từ 0-9)
    
    Họ và tên chủ thẻ
    
    Ngày phát hành

  Bước 4. Nạp tiền

  Màn hình chính chọn Nạp tiền. Sau đó Nhập số tiền cần nạp là 5 000 000 VND. Tiếp đến nhập mật khẩu là 000000 và nhập OTP (nếu có)

  Dùng ứng dụng trên để thanh toán khi đặt vé xem phim 
  
  <img width="1919" height="1137" alt="image" src="https://github.com/user-attachments/assets/fd2eb27d-0dfe-4dbc-ba56-dbe26da54e1e" />



