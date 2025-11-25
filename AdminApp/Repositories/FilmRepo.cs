using AdminApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace AdminApp.Repositories
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
                duration = Convert.ToInt32(r["duration"]),
                film_purchase_price = r["film_purchase_price"] == DBNull.Value ? null : (int?)Convert.ToInt32(r["film_purchase_price"]),
                status = r["status"].ToString()
            };
        }
    }

}
