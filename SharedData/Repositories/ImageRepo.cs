using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedData.Repositories
{
    public class ImageRepo
    {
        /// <summary>
        /// Lưu ảnh cho staff (ghi đè nếu đã tồn tại)
        /// </summary>
        public bool SaveStaffImage(string staffId, byte[] imageBytes)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // 1️⃣ Xóa ảnh cũ
                string deleteSql = @"
                    DELETE FROM ImageStore
                    WHERE related_id = @id AND image_type = 'staff';
                ";

                using (var deleteCmd = new SqliteCommand(deleteSql, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@id", staffId);
                    deleteCmd.ExecuteNonQuery();
                }

                // 2️⃣ Insert ảnh mới
                string insertSql = @"
                    INSERT INTO ImageStore (related_id, image_type, image_data)
                    VALUES (@id, 'staff', @img);
                ";

                using (var insertCmd = new SqliteCommand(insertSql, conn))
                {
                    insertCmd.Parameters.Add("@id", SqliteType.Text).Value = staffId;
                    insertCmd.Parameters.Add("@img", SqliteType.Blob).Value = imageBytes;

                    return insertCmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Lấy ảnh staff
        /// </summary>
        public byte[] GetStaffImage(string staffId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT image_data
                    FROM ImageStore
                    WHERE related_id = @id AND image_type = 'staff'
                    LIMIT 1;
                ";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", staffId);

                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                        return null;

                    return (byte[])result;
                }
            }
        }

        /// Lưu Avatar Customer
        /// </summary>
        public bool SaveCustomerAvatar(string customerId, byte[] imageBytes)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // 1. Xóa ảnh cũ (nếu có)
                string deleteSql = "DELETE FROM ImageStore WHERE related_id = @id AND image_type = 'customer';";
                using (var cmd = new SqliteCommand(deleteSql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", customerId);
                    cmd.ExecuteNonQuery();
                }

                // 2. Thêm ảnh mới
                string insertSql = "INSERT INTO ImageStore (related_id, image_type, image_data) VALUES (@id, 'customer', @img);";
                using (var cmd = new SqliteCommand(insertSql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", customerId);
                    cmd.Parameters.Add("@img", SqliteType.Blob).Value = imageBytes;
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Lấy Avatar Customer
        /// </summary>
        public byte[] GetCustomerAvatar(string customerId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "SELECT image_data FROM ImageStore WHERE related_id = @id AND image_type = 'customer' LIMIT 1;";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", customerId);
                    object result = cmd.ExecuteScalar();

                    if (result == null || result == DBNull.Value) return null;
                    return (byte[])result;
                }
            }
        }

        /// <summary>
        /// Xóa ảnh staff
        /// </summary>
        public bool DeleteStaffImage(string staffId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"
                    DELETE FROM ImageStore
                    WHERE related_id = @id AND image_type = 'staff';
                ";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", staffId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Lưu poster phim (ghi đè nếu đã tồn tại)
        /// </summary>
        public bool SaveMoviePoster(string movieId, byte[] imageBytes)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string deleteSql = @"
                    DELETE FROM ImageStore
                    WHERE related_id = @id AND image_type = 'poster';
                ";

                using (var deleteCmd = new SqliteCommand(deleteSql, conn))
                {
                    deleteCmd.Parameters.AddWithValue("@id", movieId);
                    deleteCmd.ExecuteNonQuery();
                }

                string insertSql = @"
                    INSERT INTO ImageStore (related_id, image_type, image_data)
                    VALUES (@id, 'poster', @img);
                ";

                using (var insertCmd = new SqliteCommand(insertSql, conn))
                {
                    insertCmd.Parameters.AddWithValue("@id", movieId);
                    insertCmd.Parameters.AddWithValue("@img", imageBytes);
                    return insertCmd.ExecuteNonQuery() > 0;
                }
            }
        }

        /// <summary>
        /// Lấy poster phim
        /// </summary>
        public byte[] GetMoviePoster(string movieId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT image_data
                    FROM ImageStore
                    WHERE related_id = @id AND image_type = 'poster'
                    LIMIT 1;
                ";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", movieId);
                    object result = cmd.ExecuteScalar();
                    return (result == null || result == DBNull.Value) ? null : (byte[])result;
                }
            }
        }

        public bool DeleteMoviePoster(string movieId)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"
                    DELETE FROM ImageStore
                    WHERE related_id = @id AND image_type = 'poster';
                ";

                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", movieId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}


