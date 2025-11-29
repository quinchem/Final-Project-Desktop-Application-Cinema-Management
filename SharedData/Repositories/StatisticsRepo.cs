using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Repositories
{

        public class StatisticRepo
        {
            public (decimal totalRevenue, int totalTickets, decimal avgRevenuePerMovie) GetRevenueKPI(DateTime from, DateTime to, string movieTitle = null)
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();

                string sql = @"
                SELECT 
                    SUM(b.total) AS TotalRevenue,
                    COUNT(b.bill_id) AS TotalTickets,
                    COUNT(DISTINCT m.movie_id) AS MovieCount
                FROM bill b
                JOIN showtime s ON b.showtime_id = s.showtime_id
                JOIN movie m ON s.movie_id = m.movie_id
                WHERE substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2) BETWEEN @from AND @to
            ";
                if (!string.IsNullOrEmpty(movieTitle))
                    sql += " AND m.title = @movieTitle";

                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@from", from.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@to", to.ToString("yyyy-MM-dd"));
                if (!string.IsNullOrEmpty(movieTitle))
                    cmd.Parameters.AddWithValue("@movieTitle", movieTitle);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    decimal totalRevenue = reader["TotalRevenue"] != DBNull.Value ? Convert.ToDecimal(reader["TotalRevenue"]) : 0;
                    int totalTickets = reader["TotalTickets"] != DBNull.Value ? Convert.ToInt32(reader["TotalTickets"]) : 0;
                    int movieCount = reader["MovieCount"] != DBNull.Value ? Convert.ToInt32(reader["MovieCount"]) : 1;
                    decimal avgRevenue = movieCount > 0 ? totalRevenue / movieCount : 0;
                    return (totalRevenue, totalTickets, avgRevenue);
                }
                return (0, 0, 0);
            }

            public List<(string date, decimal totalRevenue, decimal topMovieRevenue, string topMovieName)>
                GetRevenueWithTopMovieByDay(DateTime fromDate, DateTime toDate, string filterMovie = null)
            {
                var result = new List<(string, decimal, decimal, string)>();
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();

                string sql = @"
                SELECT 
                    substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2) AS BillDate,
                    m.title AS MovieTitle,
                    SUM(b.total) AS Revenue
                FROM bill b
                JOIN showtime s ON b.showtime_id = s.showtime_id
                JOIN movie m ON s.movie_id = m.movie_id
                WHERE substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2) BETWEEN @from AND @to
            ";
                if (!string.IsNullOrEmpty(filterMovie))
                    sql += " AND m.title = @filterMovie";

                sql += " GROUP BY BillDate, m.title ORDER BY BillDate, Revenue DESC";

                using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@from", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@to", toDate.ToString("yyyy-MM-dd"));
                if (!string.IsNullOrEmpty(filterMovie))
                    cmd.Parameters.AddWithValue("@filterMovie", filterMovie);

                var dataByDate = new Dictionary<string, List<(string movie, decimal revenue)>>();

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string date = reader.GetString(0);
                    string movie = reader.GetString(1);
                    decimal revenue = reader.GetDecimal(2);

                    if (!dataByDate.ContainsKey(date))
                        dataByDate[date] = new List<(string, decimal)>();

                    dataByDate[date].Add((movie, revenue));
                }

                foreach (var kvp in dataByDate.OrderBy(x => x.Key))
                {
                    string date = kvp.Key;
                    var movies = kvp.Value;
                    decimal totalRevenue = movies.Sum(x => x.revenue);
                    var topMovie = movies.OrderByDescending(x => x.revenue).FirstOrDefault();
                    result.Add((date, totalRevenue, topMovie.revenue, topMovie.movie ?? "N/A"));
                }

                return result;
            }

            public (string movieTitle, decimal totalRevenue, int totalTickets) GetTopMovie(DateTime fromDate, DateTime toDate, string filterMovie = null)
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();

                string sql = @"
                SELECT m.title, SUM(b.total) AS TotalRevenue, COUNT(b.bill_id) AS TotalTickets
                FROM bill b
                JOIN showtime s ON b.showtime_id = s.showtime_id
                JOIN movie m ON s.movie_id = m.movie_id
                WHERE substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2) BETWEEN @from AND @to
            ";
                if (!string.IsNullOrEmpty(filterMovie))
                    sql += " AND m.title = @filterMovie";
                sql += " GROUP BY m.movie_id ORDER BY TotalRevenue DESC LIMIT 1";

                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@from", fromDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@to", toDate.ToString("yyyy-MM-dd"));
                if (!string.IsNullOrEmpty(filterMovie))
                    cmd.Parameters.AddWithValue("@filterMovie", filterMovie);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return (reader.GetString(0), reader.GetDecimal(1), reader.GetInt32(2));
                }
                return ("N/A", 0, 0);
            }

            public List<string> GetMoviesCurrentlyShowing()
            {
                var list = new List<string>();
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT title FROM movie WHERE status='Đang chiếu'";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(reader.GetString(0));
                return list;
            }

            public List<(string movieTitle, decimal total)> GetRevenueForPie(DateTime from, DateTime to, string selectedMovie = null)
            {
                var list = new List<(string, decimal)>();
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();

                string sql = @"
                SELECT m.title, SUM(b.total) AS Total
                FROM bill b
                JOIN showtime s ON b.showtime_id = s.showtime_id
                JOIN movie m ON s.movie_id = m.movie_id
                WHERE substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2) BETWEEN @from AND @to
            ";
                if (!string.IsNullOrEmpty(selectedMovie))
                    sql += " AND m.title = @selectedMovie";
                sql += " GROUP BY m.movie_id ORDER BY Total DESC";

                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@from", from.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@to", to.ToString("yyyy-MM-dd"));
                if (!string.IsNullOrEmpty(selectedMovie))
                    cmd.Parameters.AddWithValue("@selectedMovie", selectedMovie);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add((reader.GetString(0), reader.GetDecimal(1)));

                return list;
            }

            public List<(string movieTitle, decimal total)> GetRevenueBar(DateTime from, DateTime to, string movieTitle = null)
            {
                var list = new List<(string, decimal)>();
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var cmd = conn.CreateCommand();

                string sql = @"
                SELECT m.title, SUM(b.total) AS Total
                FROM bill b
                JOIN showtime s ON b.showtime_id = s.showtime_id
                JOIN movie m ON s.movie_id = m.movie_id
                WHERE substr(b.bill_date,7,4)||'-'||substr(b.bill_date,4,2)||'-'||substr(b.bill_date,1,2) BETWEEN @from AND @to
            ";
                if (!string.IsNullOrEmpty(movieTitle))
                    sql += " AND m.title = @movieTitle";
                sql += " GROUP BY m.movie_id ORDER BY Total DESC";

                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@from", from.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@to", to.ToString("yyyy-MM-dd"));
                if (!string.IsNullOrEmpty(movieTitle))
                    cmd.Parameters.AddWithValue("@movieTitle", movieTitle);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add((reader.GetString(0), reader.GetDecimal(1)));

                return list;
            }
        }
    }
