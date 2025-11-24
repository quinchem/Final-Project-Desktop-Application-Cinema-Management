namespace AdminApp
{
    partial class FormRoomLayoutManagement
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnRoom1;
        private System.Windows.Forms.Button btnRoom2;
        private System.Windows.Forms.Button btnRoom3;
        private System.Windows.Forms.Button btnRoom4;
        private System.Windows.Forms.Button btnRoom5;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblScreen;
        private System.Windows.Forms.Panel panelSeats;
        private System.Windows.Forms.Panel panelLegend;
        private System.Windows.Forms.Label lblLegendNormal;
        private System.Windows.Forms.Label lblLegendVIP;
        private System.Windows.Forms.Label lblLegendMaintenance;
        private System.Windows.Forms.Panel panelSeatTypes;
        private System.Windows.Forms.Label lblSeatTypeTitle;
        private System.Windows.Forms.RadioButton rbNormalSeat;
        private System.Windows.Forms.RadioButton rbVIPSeat;
        private System.Windows.Forms.Panel panelSeatStatus;
        private System.Windows.Forms.Label lblStatusTitle;
        private System.Windows.Forms.RadioButton rbNormalStatus;
        private System.Windows.Forms.RadioButton rbMaintenanceStatus;

        // Dictionary để lưu các button ghế
        private System.Collections.Generic.Dictionary<string, System.Windows.Forms.Button> seatButtons =
            new System.Collections.Generic.Dictionary<string, System.Windows.Forms.Button>();

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnRoom1 = new System.Windows.Forms.Button();
            this.btnRoom2 = new System.Windows.Forms.Button();
            this.btnRoom3 = new System.Windows.Forms.Button();
            this.btnRoom4 = new System.Windows.Forms.Button();
            this.btnRoom5 = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblScreen = new System.Windows.Forms.Label();
            this.panelSeats = new System.Windows.Forms.Panel();
            this.panelLegend = new System.Windows.Forms.Panel();
            this.lblLegendNormal = new System.Windows.Forms.Label();
            this.lblLegendVIP = new System.Windows.Forms.Label();
            this.lblLegendMaintenance = new System.Windows.Forms.Label();
            this.panelSeatTypes = new System.Windows.Forms.Panel();
            this.lblSeatTypeTitle = new System.Windows.Forms.Label();
            this.rbNormalSeat = new System.Windows.Forms.RadioButton();
            this.rbVIPSeat = new System.Windows.Forms.RadioButton();
            this.panelSeatStatus = new System.Windows.Forms.Panel();
            this.lblStatusTitle = new System.Windows.Forms.Label();
            this.rbNormalStatus = new System.Windows.Forms.RadioButton();
            this.rbMaintenanceStatus = new System.Windows.Forms.RadioButton();

            this.panelTop.SuspendLayout();
            this.panelSeats.SuspendLayout();
            this.panelLegend.SuspendLayout();
            this.panelSeatTypes.SuspendLayout();
            this.panelSeatStatus.SuspendLayout();
            this.SuspendLayout();

            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.BackColor = System.Drawing.Color.FromArgb(88, 115, 140);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblScreen);
            this.Controls.Add(this.panelSeats);
            this.Controls.Add(this.panelLegend);
            this.Controls.Add(this.panelSeatTypes);
            this.Controls.Add(this.panelSeatStatus);
            this.Name = "SeatManagementForm";
            this.Text = "Quản Lý Ghế Rạp Phim";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            // 
            // panelTop - Panel chứa các nút phòng
            // 
            this.panelTop.Location = new System.Drawing.Point(30, 20);
            this.panelTop.Size = new System.Drawing.Size(750, 50);
            this.panelTop.Controls.Add(this.btnRoom1);
            this.panelTop.Controls.Add(this.btnRoom2);
            this.panelTop.Controls.Add(this.btnRoom3);
            this.panelTop.Controls.Add(this.btnRoom4);
            this.panelTop.Controls.Add(this.btnRoom5);

            // Buttons cho các phòng
            int roomBtnWidth = 130;
            int roomBtnHeight = 35;
            int roomBtnSpacing = 10;

            this.btnRoom1.Location = new System.Drawing.Point(0, 7);
            this.btnRoom1.Size = new System.Drawing.Size(roomBtnWidth, roomBtnHeight);
            this.btnRoom1.Text = "Phòng 1";
            this.btnRoom1.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            this.btnRoom1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoom1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRoom1.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnRoom2.Location = new System.Drawing.Point(roomBtnWidth + roomBtnSpacing, 7);
            this.btnRoom2.Size = new System.Drawing.Size(roomBtnWidth, roomBtnHeight);
            this.btnRoom2.Text = "Phòng 2";
            this.btnRoom2.BackColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.btnRoom2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoom2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRoom2.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnRoom3.Location = new System.Drawing.Point((roomBtnWidth + roomBtnSpacing) * 2, 7);
            this.btnRoom3.Size = new System.Drawing.Size(roomBtnWidth, roomBtnHeight);
            this.btnRoom3.Text = "Phòng 3";
            this.btnRoom3.BackColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.btnRoom3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoom3.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRoom3.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnRoom4.Location = new System.Drawing.Point((roomBtnWidth + roomBtnSpacing) * 3, 7);
            this.btnRoom4.Size = new System.Drawing.Size(roomBtnWidth, roomBtnHeight);
            this.btnRoom4.Text = "Phòng 4";
            this.btnRoom4.BackColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.btnRoom4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoom4.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRoom4.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnRoom5.Location = new System.Drawing.Point((roomBtnWidth + roomBtnSpacing) * 4, 7);
            this.btnRoom5.Size = new System.Drawing.Size(roomBtnWidth, roomBtnHeight);
            this.btnRoom5.Text = "Phòng 5";
            this.btnRoom5.BackColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.btnRoom5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRoom5.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRoom5.Cursor = System.Windows.Forms.Cursors.Hand;

            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(30, 90);
            this.lblTitle.Size = new System.Drawing.Size(150, 30);
            this.lblTitle.Text = "Sơ đồ ghế";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;

            // 
            // lblScreen - Màn hình
            // 
            this.lblScreen.Location = new System.Drawing.Point(140, 140);
            this.lblScreen.Size = new System.Drawing.Size(900, 60);
            this.lblScreen.Text = "Màn hình";
            this.lblScreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblScreen.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
            this.lblScreen.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblScreen.ForeColor = System.Drawing.Color.Black;

            // 
            // panelSeats - Panel chứa các ghế
            // 
            this.panelSeats.Location = new System.Drawing.Point(30, 220);
            this.panelSeats.Size = new System.Drawing.Size(1050, 450);
            this.panelSeats.BackColor = System.Drawing.Color.FromArgb(88, 115, 140);

            // Tạo labels cho các hàng (A, B, C...)
            this.CreateRowLabels();

            // Tạo ghế cho mỗi hàng
            this.CreateSeatButtons();

            // 
            // panelLegend - Chú thích
            // 
            this.panelLegend.Location = new System.Drawing.Point(340, 685);
            this.panelLegend.Size = new System.Drawing.Size(520, 60);
            this.panelLegend.BackColor = System.Drawing.Color.FromArgb(88, 115, 140);
            this.panelLegend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Ghế thường
            Label boxNormal = new Label();
            boxNormal.Location = new System.Drawing.Point(30, 18);
            boxNormal.Size = new System.Drawing.Size(35, 25);
            boxNormal.BackColor = System.Drawing.Color.FromArgb(220, 220, 220);
            boxNormal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLegend.Controls.Add(boxNormal);

            this.lblLegendNormal.Location = new System.Drawing.Point(75, 18);
            this.lblLegendNormal.Size = new System.Drawing.Size(100, 25);
            this.lblLegendNormal.Text = "Ghế thường";
            this.lblLegendNormal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLegendNormal.ForeColor = System.Drawing.Color.White;
            this.panelLegend.Controls.Add(this.lblLegendNormal);

            // Ghế VIP
            Label boxVIP = new Label();
            boxVIP.Location = new System.Drawing.Point(195, 18);
            boxVIP.Size = new System.Drawing.Size(35, 25);
            boxVIP.BackColor = System.Drawing.Color.FromArgb(255, 193, 7);
            boxVIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLegend.Controls.Add(boxVIP);

            this.lblLegendVIP.Location = new System.Drawing.Point(240, 18);
            this.lblLegendVIP.Size = new System.Drawing.Size(80, 25);
            this.lblLegendVIP.Text = "Ghế VIP";
            this.lblLegendVIP.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLegendVIP.ForeColor = System.Drawing.Color.White;
            this.panelLegend.Controls.Add(this.lblLegendVIP);

            // Ghế bảo trì
            Label boxMaintenance = new Label();
            boxMaintenance.Location = new System.Drawing.Point(340, 18);
            boxMaintenance.Size = new System.Drawing.Size(35, 25);
            boxMaintenance.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
            boxMaintenance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLegend.Controls.Add(boxMaintenance);

            this.lblLegendMaintenance.Location = new System.Drawing.Point(385, 18);
            this.lblLegendMaintenance.Size = new System.Drawing.Size(100, 25);
            this.lblLegendMaintenance.Text = "Ghế bảo trì";
            this.lblLegendMaintenance.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLegendMaintenance.ForeColor = System.Drawing.Color.White;
            this.panelLegend.Controls.Add(this.lblLegendMaintenance);

            // 
            // panelSeatTypes - Loại ghế
            // 
            this.panelSeatTypes.Location = new System.Drawing.Point(1100, 220);
            this.panelSeatTypes.Size = new System.Drawing.Size(250, 150);
            this.panelSeatTypes.BackColor = System.Drawing.Color.FromArgb(150, 120, 120);
            this.panelSeatTypes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.lblSeatTypeTitle.Location = new System.Drawing.Point(10, 10);
            this.lblSeatTypeTitle.Size = new System.Drawing.Size(230, 30);
            this.lblSeatTypeTitle.Text = "Loại ghế";
            this.lblSeatTypeTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSeatTypeTitle.ForeColor = System.Drawing.Color.White;
            this.panelSeatTypes.Controls.Add(this.lblSeatTypeTitle);

            // Radio button với icon tròn cho Ghế thường
            this.rbNormalSeat.Location = new System.Drawing.Point(20, 60);
            this.rbNormalSeat.Size = new System.Drawing.Size(200, 30);
            this.rbNormalSeat.Text = "   Ghế thường";
            this.rbNormalSeat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbNormalSeat.ForeColor = System.Drawing.Color.White;
            this.rbNormalSeat.Checked = true;
            this.rbNormalSeat.CheckedChanged += new System.EventHandler(this.SeatType_CheckedChanged);
            this.panelSeatTypes.Controls.Add(this.rbNormalSeat);

            // Radio button với icon tròn cho Ghế VIP
            this.rbVIPSeat.Location = new System.Drawing.Point(20, 100);
            this.rbVIPSeat.Size = new System.Drawing.Size(200, 30);
            this.rbVIPSeat.Text = "   Ghế VIP";
            this.rbVIPSeat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbVIPSeat.ForeColor = System.Drawing.Color.White;
            this.rbVIPSeat.CheckedChanged += new System.EventHandler(this.SeatType_CheckedChanged);
            this.panelSeatTypes.Controls.Add(this.rbVIPSeat);

            // 
            // panelSeatStatus - Tình trạng
            // 
            this.panelSeatStatus.Location = new System.Drawing.Point(1100, 390);
            this.panelSeatStatus.Size = new System.Drawing.Size(250, 150);
            this.panelSeatStatus.BackColor = System.Drawing.Color.FromArgb(150, 120, 120);
            this.panelSeatStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.lblStatusTitle.Location = new System.Drawing.Point(10, 10);
            this.lblStatusTitle.Size = new System.Drawing.Size(230, 30);
            this.lblStatusTitle.Text = "Tình trạng";
            this.lblStatusTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatusTitle.ForeColor = System.Drawing.Color.White;
            this.panelSeatStatus.Controls.Add(this.lblStatusTitle);

            this.rbNormalStatus.Location = new System.Drawing.Point(20, 60);
            this.rbNormalStatus.Size = new System.Drawing.Size(200, 30);
            this.rbNormalStatus.Text = "   Bình thường";
            this.rbNormalStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbNormalStatus.ForeColor = System.Drawing.Color.White;
            this.rbNormalStatus.Checked = true;
            this.rbNormalStatus.CheckedChanged += new System.EventHandler(this.SeatStatus_CheckedChanged);
            this.panelSeatStatus.Controls.Add(this.rbNormalStatus);

            this.rbMaintenanceStatus.Location = new System.Drawing.Point(20, 100);
            this.rbMaintenanceStatus.Size = new System.Drawing.Size(200, 30);
            this.rbMaintenanceStatus.Text = "   Bảo trì";
            this.rbMaintenanceStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbMaintenanceStatus.ForeColor = System.Drawing.Color.White;
            this.rbMaintenanceStatus.CheckedChanged += new System.EventHandler(this.SeatStatus_CheckedChanged);
            this.panelSeatStatus.Controls.Add(this.rbMaintenanceStatus);

            this.panelTop.ResumeLayout(false);
            this.panelSeats.ResumeLayout(false);
            this.panelLegend.ResumeLayout(false);
            this.panelSeatTypes.ResumeLayout(false);
            this.panelSeatStatus.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void CreateRowLabels()
        {
            Label lblRowA = new Label();
            lblRowA.Location = new System.Drawing.Point(30, 30);
            lblRowA.Size = new System.Drawing.Size(40, 30);
            lblRowA.Text = "A";
            lblRowA.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblRowA.ForeColor = System.Drawing.Color.White;
            lblRowA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelSeats.Controls.Add(lblRowA);

            Label lblRowB = new Label();
            lblRowB.Location = new System.Drawing.Point(30, 83);
            lblRowB.Size = new System.Drawing.Size(40, 30);
            lblRowB.Text = "B";
            lblRowB.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblRowB.ForeColor = System.Drawing.Color.White;
            lblRowB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelSeats.Controls.Add(lblRowB);

            Label lblRowC = new Label();
            lblRowC.Location = new System.Drawing.Point(30, 136);
            lblRowC.Size = new System.Drawing.Size(40, 30);
            lblRowC.Text = "C";
            lblRowC.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblRowC.ForeColor = System.Drawing.Color.White;
            lblRowC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelSeats.Controls.Add(lblRowC);

            Label lblRowD = new Label();
            lblRowD.Location = new System.Drawing.Point(30, 189);
            lblRowD.Size = new System.Drawing.Size(40, 30);
            lblRowD.Text = "D";
            lblRowD.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblRowD.ForeColor = System.Drawing.Color.White;
            lblRowD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelSeats.Controls.Add(lblRowD);

            Label lblRowE = new Label();
            lblRowE.Location = new System.Drawing.Point(30, 242);
            lblRowE.Size = new System.Drawing.Size(40, 30);
            lblRowE.Text = "E";
            lblRowE.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblRowE.ForeColor = System.Drawing.Color.White;
            lblRowE.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelSeats.Controls.Add(lblRowE);

            Label lblRowF = new Label();
            lblRowF.Location = new System.Drawing.Point(30, 295);
            lblRowF.Size = new System.Drawing.Size(40, 30);
            lblRowF.Text = "F";
            lblRowF.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            lblRowF.ForeColor = System.Drawing.Color.White;
            lblRowF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panelSeats.Controls.Add(lblRowF);
        }

        private void CreateSeatButtons()
        {
            // Hàng A - Ghế thường
            this.CreateSeatRow("A", 0, System.Drawing.Color.FromArgb(220, 220, 220), System.Drawing.Color.Black);

            // Hàng B - Ghế thường
            this.CreateSeatRow("B", 1, System.Drawing.Color.FromArgb(220, 220, 220), System.Drawing.Color.Black);

            // Hàng C - Ghế thường (C10-C15 là bảo trì)
            this.CreateSeatRow("C", 2, System.Drawing.Color.FromArgb(220, 220, 220), System.Drawing.Color.Black);
            this.SetMaintenanceSeats("C", 10, 15);

            // Hàng D - Ghế VIP
            this.CreateSeatRow("D", 3, System.Drawing.Color.FromArgb(255, 193, 7), System.Drawing.Color.Black);

            // Hàng E - Ghế VIP (E7 màu xanh)
            this.CreateSeatRow("E", 4, System.Drawing.Color.FromArgb(255, 193, 7), System.Drawing.Color.Black);
            this.SetSelectedSeat("E7");

            // Hàng F - Ghế VIP
            this.CreateSeatRow("F", 5, System.Drawing.Color.FromArgb(255, 193, 7), System.Drawing.Color.Black);
        }

        private void CreateSeatRow(string rowLetter, int rowIndex, System.Drawing.Color backColor, System.Drawing.Color foreColor)
        {
            int seatSize = 45;
            int seatSpacing = 8;
            int startX = 80;
            int startY = 20;

            // A1
            Button btn1 = this.CreateSingleSeat(rowLetter + "1", startX, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn1);
            seatButtons.Add(rowLetter + "1", btn1);

            // A2
            Button btn2 = this.CreateSingleSeat(rowLetter + "2", startX + 53, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn2);
            seatButtons.Add(rowLetter + "2", btn2);

            // A3
            Button btn3 = this.CreateSingleSeat(rowLetter + "3", startX + 106, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn3);
            seatButtons.Add(rowLetter + "3", btn3);

            // A4
            Button btn4 = this.CreateSingleSeat(rowLetter + "4", startX + 159, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn4);
            seatButtons.Add(rowLetter + "4", btn4);

            // A5
            Button btn5 = this.CreateSingleSeat(rowLetter + "5", startX + 212, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn5);
            seatButtons.Add(rowLetter + "5", btn5);

            // A6
            Button btn6 = this.CreateSingleSeat(rowLetter + "6", startX + 265, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn6);
            seatButtons.Add(rowLetter + "6", btn6);

            // A7
            Button btn7 = this.CreateSingleSeat(rowLetter + "7", startX + 318, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn7);
            seatButtons.Add(rowLetter + "7", btn7);

            // A8
            Button btn8 = this.CreateSingleSeat(rowLetter + "8", startX + 371, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn8);
            seatButtons.Add(rowLetter + "8", btn8);

            // A9
            Button btn9 = this.CreateSingleSeat(rowLetter + "9", startX + 424, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn9);
            seatButtons.Add(rowLetter + "9", btn9);

            // A10
            Button btn10 = this.CreateSingleSeat(rowLetter + "10", startX + 477, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn10);
            seatButtons.Add(rowLetter + "10", btn10);

            // A11
            Button btn11 = this.CreateSingleSeat(rowLetter + "11", startX + 530, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn11);
            seatButtons.Add(rowLetter + "11", btn11);

            // A12
            Button btn12 = this.CreateSingleSeat(rowLetter + "12", startX + 583, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn12);
            seatButtons.Add(rowLetter + "12", btn12);

            // A13
            Button btn13 = this.CreateSingleSeat(rowLetter + "13", startX + 636, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn13);
            seatButtons.Add(rowLetter + "13", btn13);

            // A14
            Button btn14 = this.CreateSingleSeat(rowLetter + "14", startX + 689, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn14);
            seatButtons.Add(rowLetter + "14", btn14);

            // A15
            Button btn15 = this.CreateSingleSeat(rowLetter + "15", startX + 742, startY + rowIndex * 53, seatSize, backColor, foreColor);
            this.panelSeats.Controls.Add(btn15);
            seatButtons.Add(rowLetter + "15", btn15);
        }

        private Button CreateSingleSeat(string seatId, int x, int y, int size, System.Drawing.Color backColor, System.Drawing.Color foreColor)
        {
            Button btnSeat = new Button();
            btnSeat.Name = "btn" + seatId;
            btnSeat.Location = new System.Drawing.Point(x, y);
            btnSeat.Size = new System.Drawing.Size(size, size);
            btnSeat.Text = seatId;
            btnSeat.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            btnSeat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSeat.Cursor = System.Windows.Forms.Cursors.Hand;
            btnSeat.Tag = seatId;
            btnSeat.BackColor = backColor;
            btnSeat.ForeColor = foreColor;
            btnSeat.FlatAppearance.BorderSize = 1;
            btnSeat.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(100, 100, 100);
            btnSeat.Click += new System.EventHandler(this.SeatButton_Click);
            return btnSeat;
        }

        private void SetMaintenanceSeats(string row, int fromCol, int toCol)
        {
            Button btn10 = seatButtons[row + "10"];
            btn10.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
            btn10.ForeColor = System.Drawing.Color.White;

            Button btn11 = seatButtons[row + "11"];
            btn11.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
            btn11.ForeColor = System.Drawing.Color.White;

            Button btn12 = seatButtons[row + "12"];
            btn12.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
            btn12.ForeColor = System.Drawing.Color.White;

            Button btn13 = seatButtons[row + "13"];
            btn13.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
            btn13.ForeColor = System.Drawing.Color.White;

            Button btn14 = seatButtons[row + "14"];
            btn14.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
            btn14.ForeColor = System.Drawing.Color.White;

            Button btn15 = seatButtons[row + "15"];
            btn15.BackColor = System.Drawing.Color.FromArgb(150, 150, 150);
            btn15.ForeColor = System.Drawing.Color.White;
        }

        private void SetSelectedSeat(string seatId)
        {
            Button btn = seatButtons[seatId];
            btn.BackColor = System.Drawing.Color.FromArgb(76, 175, 80);
        }

        // Event handlers - Bạn cần implement trong file .cs chính
        private void SeatButton_Click(object sender, System.EventArgs e)
        {
            Button clickedSeat = sender as Button;
            string seatId = clickedSeat.Tag.ToString();
            System.Windows.Forms.MessageBox.Show("Đã click vào ghế: " + seatId);
        }

        private void SeatType_CheckedChanged(object sender, System.EventArgs e)
        {
            // Xử lý khi thay đổi loại ghế
        }

        private void SeatStatus_CheckedChanged(object sender, System.EventArgs e)
        {
            // Xử lý khi thay đổi tình trạng ghế
        }
    }
}

