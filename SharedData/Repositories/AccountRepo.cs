using Microsoft.Data.Sqlite;

namespace AdminApp.Repositories
{
    public class AccountRepository
    {
        public bool CheckOldPassword(string staffId, string oldPassword)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT COUNT(*) 
                    FROM Account
                    WHERE staff_id = @staffId
                      AND password = @oldPass";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@staffId", staffId);
                    cmd.Parameters.AddWithValue("@oldPass", oldPassword);

                    long count = (long)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool UpdatePassword(string staffId, string newPassword)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string query = @"
                    UPDATE Account
                    SET password = @newPass
                    WHERE staff_id = @staffId";

                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@newPass", newPassword);
                    cmd.Parameters.AddWithValue("@staffId", staffId);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
