using AdminApp.Models;
using AdminApp.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminApp.Repositories
{
    public class MovieRepository
    {
        public List<Movie> GetAllMovies()
        {
            var list = new List<Movie>();

            using (var conn = new SqliteConnection(DatabaseHelper.GetConnectionString()))
            {
                conn.Open();

                string query = @"
                    SELECT movie_id, title, description, genre,
                           director, actor, release_date, language, age_restriction, duration, film_purchase_price, status
                    FROM movie";

                using (var cmd = new SqliteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Movie
                        {
                            MovieId = reader["movie_id"].ToString(),
                            Title = reader["title"].ToString(),
                            Description = reader["description"].ToString(),
                            Genre = reader["genre"].ToString(),
                            Director = reader["director"].ToString(),
                            Actor = reader["actor"].ToString(),
                            ReleaseDate = reader["release_date"].ToString(),
                            Language = reader["language"].ToString(),
                            AgeRestriction = reader["age_restriction"].ToString(),
                            Duration = int.Parse(reader["duration"].ToString()),
                            FilmPurchasePrice = reader["film_purchase_price"] == DBNull.Value ? 0 : int.Parse(reader["film_purchase_price"].ToString()),
                            Status = reader["status"].ToString()
                        });
                    }
                }
            }

            return list;
        }

    }
}
