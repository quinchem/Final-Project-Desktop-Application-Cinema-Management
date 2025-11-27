using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SharedData.Models;

namespace SharedData.Repositories
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
                           VALUES (@auditorium_type_id, @auditorium_type)";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@auditorium_type_id", t.auditorium_type_id);
                    cmd.Parameters.AddWithValue("@auditorium_type", t.auditorium_type);
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
                           SET auditorium_type = @auditorium_type
                           WHERE auditorium_type_id = @auditorium_type_id";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@auditorium_type_id", t.auditorium_type_id);
                    cmd.Parameters.AddWithValue("@auditorium_type", t.auditorium_type);
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
                           WHERE auditorium_type_id = @auditorium_type_id";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@auditorium_type_id", typeId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public AuditoriumType GetById(string id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT auditorium_type_id, auditorium_type FROM auditorium_type WHERE auditorium_type_id = @auditorium_type_id";
                cmd.Parameters.AddWithValue("@auditorium_type_id", id);
                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                        return new AuditoriumType
                        {
                            auditorium_type_id = r["auditorium_type_id"].ToString(),
                            auditorium_type = r["auditorium_type"].ToString(),
                        };
                }
            }
            return null;
        }
    }
}
