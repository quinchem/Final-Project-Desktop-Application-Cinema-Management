using Microsoft.Data.Sqlite;
using SharedData.Models;
using System;
using System.Collections.Generic;

namespace SharedData.Repositories
{
    public class SearchRepo
    {
        private static string ConnStr => DatabaseHelper.GetConnectionString();

        // Hàm tìm phim theo từ khóa (theo title hoặc mô tả)
        public List<Film> SearchFilms(string keyword)
        {
            List<Film> list = new List<Film>();

            if (string.IsNullOrEmpty(keyword))
                return list;

            string likeKey = "%" + keyword + "%";

            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT movie_id, title, duration, age_restriction, release_date
                FROM movie
                WHERE title LIKE $k OR description LIKE $k
            ";

            cmd.Parameters.AddWithValue("$k", likeKey);

            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new Film
                {
                    movie_id = rd.GetString(0),
                    title = rd.GetString(1),
                    duration = rd.GetInt32(2),
                    age_restriction = rd.GetString(3),
                    release_date = rd.GetString(4)
                });
            }

            return list;
        }
    }
}
