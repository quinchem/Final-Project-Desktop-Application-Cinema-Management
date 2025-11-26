using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace UserApp
{
    public partial class FormSeatSelection : Form
    {
        public FormSeatSelection()
        {
            InitializeComponent();
        }
        int timeLeft = 600;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timeLeft--;

            // Hiển thị dạng mm:ss
            lblTime.Text = TimeSpan.FromSeconds(timeLeft).ToString(@"mm\:ss");
            lblTime.Refresh();

            if (timeLeft <= 0)
            {
                timer1.Stop();
                lblTime.Text = "00:00";
                MessageBox.Show("Hết giờ rồi!");
            }
        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {
            timeLeft = 600;       // reset 10 phút
            lblTime.Text = "10:00";
            timer1.Start();
        }
    }
}
