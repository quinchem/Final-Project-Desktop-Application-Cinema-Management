using Microsoft.Data.Sqlite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
public static class DatabaseHelper
{
    public static string GetConnectionString()
    {
        string dir = AppDomain.CurrentDomain.BaseDirectory;
        while (dir != null && !Directory.GetFiles(dir, "*.sln").Any())
        {
            dir = Directory.GetParent(dir)?.FullName;
        }
        if (dir == null)
            throw new Exception("Không tìm thấy thư mục solution (.sln)!");
        string dbPath = Path.Combine(dir, "SharedDatabase", "Cinema.db");

        return $"Data Source={dbPath}";
    }
    
    public static SqliteConnection GetConnection()
    {
        return new SqliteConnection(GetConnectionString());
    }
}