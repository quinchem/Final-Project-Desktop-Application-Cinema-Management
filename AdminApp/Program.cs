namespace AdminApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // demo – sau này bạn lấy từ form login
            string staff_id = "S01";

            //Application.Run(new AdminMainForm(staff_id));
            Application.Run(new FormStatistics2());

        }
    }
}