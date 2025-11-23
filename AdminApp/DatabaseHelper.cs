using Microsoft.Data.Sqlite;
using System.IO;
using System.Linq;

public static class DatabaseHelper
{
    public static string GetConnectionString()
    {
        // Lấy thư mục bắt đầu = thư mục .exe
        string dir = AppDomain.CurrentDomain.BaseDirectory;

        // Lùi dần đến khi gặp file .sln => thư mục gốc của solution
        while (dir != null && !Directory.GetFiles(dir, "*.sln").Any())
        {
            dir = Directory.GetParent(dir)?.FullName;
        }

        // Nếu không tìm thấy .sln => báo lỗi
        if (dir == null)
            throw new Exception("Không tìm thấy thư mục solution (.sln)!");

        // Ghép đường dẫn DB thực sự
        string dbPath = Path.Combine(dir, "SharedDatabase", "Cinema.db");

        return $"Data Source={dbPath}";
    }

    public static SqliteConnection GetConnection()
    {
        return new SqliteConnection(GetConnectionString());
    }
}
