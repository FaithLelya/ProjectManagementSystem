using System;
using System.Data.SQLite;
using BCrypt.Net;

namespace ProjectManagementSystem.Helpers
{
    public static class PasswordMigrationHelp
    {
        //private static string ConnectionString = "Data Source=App_Data/project_tracking.db;Version=3;";
        private static string ConnectionString = "Data Source=C:\\Projects\\ProjectManagementSystem\\App_Data\\project_tracking.db;Version=3;";

        public static void MigratePasswords()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                // Step 1: Fetch all users with plaintext passwords
                string selectQuery = "SELECT UserID, Password FROM USER";
                using (var selectCommand = new SQLiteCommand(selectQuery, connection))
                using (var reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int userId = reader.GetInt32(0);
                        string plaintextPassword = reader.GetString(1);

                        // Step 2: Hash the plaintext password
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plaintextPassword);

                        // Step 3: Update the user's password with the hashed version
                        string updateQuery = "UPDATE USER SET Password = @Password WHERE UserID = @UserID";
                        using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@Password", hashedPassword);
                            updateCommand.Parameters.AddWithValue("@UserID", userId);
                            updateCommand.ExecuteNonQuery();
                        }

                        Console.WriteLine($"Updated password for UserId: {userId}");
                    }
                }
            }
        }
    }
}