**Ứng dụng Desktop đặt vé xem phim có tích hợp Chatbot Gemini AI và MoMo API dành cho khách hàng và quản lý rạp phim dành cho quản trị viên trên nền tảng WinForms ngôn ngữ C#**

**1. Clone dự án từ Github về máy tính cá nhân**

Mở Visual Studio, chọn Clone a repository 

<img width="1916" height="1052" alt="image" src="https://github.com/user-attachments/assets/c9589027-194d-4508-be1e-cb14b2674df5" />


Sau đó, dán câu lệnh "https://github.com/quinchem/Final-Project-Desktop-Application-Cinema-Management.git" vào Repository location và nhấn chọn Clone

<img width="1915" height="1136" alt="image" src="https://github.com/user-attachments/assets/3792e1ed-38e6-4be6-befb-e48f562c3e94" />

Sau khi chạy lệnh này thành công, thư mục dự án sẽ được tải máy cá nhân.

<img width="1918" height="823" alt="image" src="https://github.com/user-attachments/assets/bed0f90b-1070-4a64-a482-6db4fca75c44" />


**2. Cách kết nối Chatbot với Gemini**

Mở Visual Studio, sau đó mở Solution Explorer và nhấp chuột phải vào file FormChatbot.cs trong folder Forms của project UserApp chọn View Code. 

<img width="1913" height="1100" alt="image" src="https://github.com/user-attachments/assets/5a26990a-ba99-4907-9db7-7647f1803306" />


Sau đó, tìm câu lệnh khai báo API key "private const string API_KEY = "Your_key"" 

<img width="1542" height="756" alt="image" src="https://github.com/user-attachments/assets/43aaf8d1-e500-4fad-980f-e68dfdfab45c" />


Thay "Your_key" bằng API key /*AIzaSyDijpUre8M///fuP65B73iw///9GYypxl///IqrdQyUtest*/.

Lưu ý: Bỏ dấu "/" và bỏ từ "test" ở cuối API Key khi nhập vào code

<img width="1536" height="762" alt="image" src="https://github.com/user-attachments/assets/88d61205-afdd-496c-95f5-2765088b6242" />

**3. Cách chạy giao diện**

Đầu tiên trên thanh công cụ, chọn UserApp nếu muốn chạy giao diện của người dùng, chọn AdminApp nếu muốn chạy giao diện của quản lý

<img width="1919" height="151" alt="image" src="https://github.com/user-attachments/assets/50747b93-68a9-46d3-84f6-44ed25cac9f2" />


Sau đó nhấn F5 để chạy được ứng dụng

Nếu chọn UserApp, khi đăng nhập điền thông tin như sau:

    Email: quynhtram18@gmail.com

    Password:Qtram185@
    
<img width="1916" height="649" alt="image" src="https://github.com/user-attachments/assets/8c51a6b4-9861-4484-8c0a-566987e2ec55" />


Sau khi đăng nhập thành công vào ứng dụng, chọn các chức năng trên thanh menu để trải nghiệm ứng dụng ở vai trò là người dùng

<img width="1917" height="1017" alt="image" src="https://github.com/user-attachments/assets/9ea2d738-d36f-4b70-932f-d9b335ba1b08" />


Nếu chọn AdminApp, khi đăng nhập điền thông tin như sau:

    Username: admin

    Password: admin12345

<img width="1919" height="774" alt="image" src="https://github.com/user-attachments/assets/fbe7cb68-12ac-4a77-8b84-a1229749ba32" />


Sau khi đăng nhập thành công vào ứng dụng, chọn các chức năng trên thanh menu để trải nghiệm ứng dụng ở vai trò là quản trị viên

<img width="1918" height="977" alt="image" src="https://github.com/user-attachments/assets/8a6cbe6a-bec6-40b4-abbd-6d6f6c1758db" />


**4. Cách trải nghiệm tính năng thanh toán bằng Momo**

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
  
  <img width="1919" height="1108" alt="image" src="https://github.com/user-attachments/assets/61c4120f-7398-4031-9d71-f7cfb043bc9c" />




