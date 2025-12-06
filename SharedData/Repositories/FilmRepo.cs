using Microsoft.Data.Sqlite;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Repositories
{
    public class FilmRepo
    {
        
        // Lấy toàn bộ phim
        public List<Film> GetAllFilms()
        {
            List<Film> films = new List<Film>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM movie";
                using (var cmd = new SqliteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        films.Add(MapFilm(reader));
                    }
                }
            }
            return films;
        }

        // Lấy phim theo thể loại
        public List<Film> GetFilmByType(string genre)
        {
            List<Film> films = new List<Film>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM movie WHERE genre = @genre";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@genre", genre);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            films.Add(MapFilm(reader));
                        }
                    }
                }
            }
            return films;
        }

        // Tìm kiếm phim theo tên, không phân biệt chữ hoa, chữ thường
        public List<Film> SearchFilmByName1(string keyword)
        {
            // Lấy toàn bộ phim
            var allFilms = GetAllFilms();

            if (string.IsNullOrEmpty(keyword))
                return allFilms;
            var results = allFilms
                .Where(f => f.title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            return results;
        }


        // Tìm kiếm phim theo tên
        public List<Film> SearchFilmByName(string keyword)
        {
            List<Film> films = new List<Film>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM movie WHERE title LIKE @kw";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            films.Add(MapFilm(reader));
                        }
                    }
                }
            }
            return films;
        }

        // Lấy suất chiếu theo phim
        public List<Film> GetCurrentlyShowingFilms()
        {
            var allFilms = GetAllFilms();
            DateTime today = DateTime.Today;

            var showingFilms = allFilms
                .Where(film =>
                {
                    // Chỉ lấy phim active (nếu muốn)
                    if (!string.Equals(film.status, "active", StringComparison.OrdinalIgnoreCase))
                        return false;

                    // Parse release_date
                    DateTime release;
                    string[] formats = { "yyyy-MM-dd", "dd/MM/yyyy" }; // hỗ trợ 2 format phổ biến
                    bool parsed = DateTime.TryParseExact(
                        film.release_date,
                        formats,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out release
                    );

                    return parsed && release <= today; // chỉ lấy phim đã chiếu
                })
                .ToList();

            return showingFilms;
        }

        // Lấy phim theo ID
        public Film GetById(string id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT movie_id, title, duration FROM movie WHERE movie_id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                        return new Film { movie_id = r["movie_id"].ToString(), title = r["title"].ToString(), duration = Convert.ToInt32(r["duration"]) };
                }
            }
            return null;
        }

        // Lấy toàn bộ thông tin phim theo ID
        public Film GetById2(string id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = @"SELECT movie_id, title, description, genre, director, actor, 
                              release_date, language, age_restriction, duration, 
                              film_purchase_price, status
                       FROM movie
                       WHERE movie_id = @id";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Film
                            {
                                movie_id = reader["movie_id"].ToString(),
                                title = reader["title"].ToString(),
                                description = reader["description"].ToString(),
                                genre = reader["genre"].ToString(),
                                director = reader["director"].ToString(),
                                actor = reader["actor"].ToString(),
                                release_date = reader["release_date"].ToString(),
                                language = reader["language"].ToString(),
                                age_restriction = reader["age_restriction"].ToString(),
                                duration = int.Parse(reader["duration"].ToString()),
                                film_purchase_price = reader["film_purchase_price"] == DBNull.Value ? (int?)null : int.Parse(reader["film_purchase_price"].ToString()),
                                status = reader["status"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Hàm lấy tất cả phim đã chiếu
        public List<Film> GetCurrentlyShowingFilms1()
        {
            var allFilms = GetAllFilms();
            DateTime today = DateTime.Today;
            List<Film> showingFilms = new List<Film>();

            foreach (var film in allFilms)
            {
                DateTime release;
                bool parsed = DateTime.TryParseExact(
                    film.release_date,
                    new string[] { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out release
                );

                if (!parsed)
                {
                    parsed = DateTime.TryParse(film.release_date, out release);
                }

                if (!parsed)
                    continue; 

                if (release <= today)
                    showingFilms.Add(film);
            }

            return showingFilms;
        }

        // Cập nhật phim
        public void UpdateFilm(Film film)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            string sql = @"UPDATE movie
                   SET title = @title, genre = @genre, language = @language, 
                       director = @director, actor = @actor, description = @description, 
                       status = @status, film_purchase_price = @price, duration = @duration, 
                       age_restriction = @age, release_date = @releaseDate
                   WHERE movie_id = @id";

            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@title", film.title);
            cmd.Parameters.AddWithValue("@genre", film.genre);
            cmd.Parameters.AddWithValue("@language", film.language);
            cmd.Parameters.AddWithValue("@director", film.director);
            cmd.Parameters.AddWithValue("@actor", film.actor);
            cmd.Parameters.AddWithValue("@description", film.description);
            cmd.Parameters.AddWithValue("@status", film.status);
            cmd.Parameters.AddWithValue("@price", film.film_purchase_price);
            cmd.Parameters.AddWithValue("@duration", film.duration);
            cmd.Parameters.AddWithValue("@age", film.age_restriction);
            cmd.Parameters.AddWithValue("@releaseDate", film.release_date);
            cmd.Parameters.AddWithValue("@id", film.movie_id);

            cmd.ExecuteNonQuery();
        }

        // Hàm Map dữ liệu
        private Film MapFilm(SqliteDataReader r)
        {
            return new Film
            {
                movie_id = r["movie_id"].ToString(),
                title = r["title"].ToString(),
                description = r["description"].ToString(),
                genre = r["genre"].ToString(),
                director = r["director"].ToString(),
                actor = r["actor"].ToString(),
                release_date = r["release_date"].ToString(),
                language = r["language"].ToString(),
                age_restriction = r["age_restriction"].ToString(),
                duration = r["duration"] != DBNull.Value ? Convert.ToInt32(r["duration"]) : 0,
                film_purchase_price = r["film_purchase_price"] == DBNull.Value ? null : (int?)Convert.ToInt32(r["film_purchase_price"]),
                status = r["status"].ToString()
            };
        }
    }
}
