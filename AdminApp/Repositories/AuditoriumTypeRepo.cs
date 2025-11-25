using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using AdminApp.Models;

namespace AdminApp.Repositories
{
    public class AuditoriumTypeRepo
    {
        public List<AuditoriumType> GetAll()
        {
            var list = new List<AuditoriumType>();

            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = "SELECT auditorium_type_id, auditorium_type FROM auditorium_type ORDER BY auditorium_type_id";

                using (var cmd = new SqliteCommand(sql, conn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new AuditoriumType
                        {
                            auditorium_type_id = rd["auditorium_type_id"].ToString(),
                            auditorium_type = rd["auditorium_type"].ToString()
                        });
                    }
                }
            }

            return list;
        }

        public void Insert(AuditoriumType t)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"INSERT INTO auditorium_type (auditorium_type_id, auditorium_type)
                           VALUES (@id, @name)";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", t.auditorium_type_id);
                    cmd.Parameters.AddWithValue("@name", t.auditorium_type);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(AuditoriumType t)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"UPDATE auditorium_type 
                           SET auditorium_type = @name
                           WHERE auditorium_type_id = @id";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", t.auditorium_type_id);
                    cmd.Parameters.AddWithValue("@name", t.auditorium_type);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(string typeId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"DELETE FROM auditorium_type 
                           WHERE auditorium_type_id = @id";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", typeId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
