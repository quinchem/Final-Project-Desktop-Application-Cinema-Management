using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserApp
{
    public partial class ProfileChangePassword : UserControl
    {
        public ProfileChangePassword()
        {
            InitializeComponent();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int radius = 20;
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();

            path.AddArc(0, 0, d, d, 180, 90);
            path.AddArc(Width - d, 0, d, d, 270, 90);
            path.AddArc(Width - d, Height - d, d, d, 0, 90);
            path.AddArc(0, Height - d, d, d, 90, 90);

            path.CloseFigure();
            this.Region = new Region(path);

            // Vẽ viền
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(pen, path);
            }
        }
        private void MyUserControl_Load(object sender, EventArgs e)
        {
            this.Invalidate(); // vẽ lại => tự chạy OnPaint
        }
    }
}
