namespace AdminApp
{
    public partial class AdminMainForm : Form
    {
        public AdminMainForm()
        {
            InitializeComponent();
        }

        private void btnSuatChieu_Click(object sender, EventArgs e)
        {
            var f = new FormShowManagement();
            f.Show();
        }
    }
}
