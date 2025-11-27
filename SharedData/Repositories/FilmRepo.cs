using Microsoft.Data.Sqlite;
using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Repositories
{
    public class FilmRepo
    {
        // ---- Lấy toàn bộ phim ----
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

        // ---- Lấy phim theo thể loại ----
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

        // ---- Tìm kiếm phim theo tên (C# LINQ) ----
        public List<Film> SearchFilmByName1(string keyword)
        {
            // Lấy toàn bộ phim
            var allFilms = GetAllFilms();

            if (string.IsNullOrEmpty(keyword))
                return allFilms;

            // Lọc trong C#, không phân biệt chữ hoa/chữ thường
            var results = allFilms
                .Where(f => f.title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            return results;
        }


        // ---- Tìm kiếm phim theo tên ----
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

        // ---- Lấy suất chiếu theo phim ----
        public List<Showtime> GetShowByFilm(string filmId)
        {
            List<Showtime> shows = new List<Showtime>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM showtime WHERE movie_id = @id";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", filmId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            shows.Add(new Showtime
                            {
                                showtime_id = reader["showtime_id"].ToString(),
                                movie_id = reader["movie_id"].ToString(),
                                auditorium_id = reader["auditorium_id"].ToString(),
                                show_date = reader["show_date"].ToString(),
                                start_time = reader["start_time"].ToString(),
                                end_time = reader["end_time"].ToString()
                            });
                        }
                    }
                }
            }
            return shows;
        }

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

        public void UpdateFilm(Film film)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();

            string sql = @"UPDATE movie
                   SET title = @title, genre = @genre, language = @language, 
                       director = @director, actor = @actor, description = @description, 
                       status = @status, film_purchase_price = @price, duration = @duration, 
                       age_restrictione = @age, release_date = @releaseDate
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


        // ----------- HÀM MAP DỮ LIỆU -----------

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
