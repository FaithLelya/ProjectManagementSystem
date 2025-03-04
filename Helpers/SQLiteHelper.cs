using System;
using BCrypt.Net;
using System.Web.UI;
using System.Collections.Generic;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Controllers;
using System.Data.SQLite;

namespace ProjectManagementSystem.Helpers
{
    public static class SQLiteHelper
    {
        //private static string ConnectionString => System.Configuration.ConfigurationManager.ConnectionStrings["SQLiteDB"].ConnectionString;
        //private static string ConnectionString = "Data Source=App_Data/project_tracking.db;Version=3;";

        //private static string ConnectionString = "Data Source=C:\\Projects\\ProjectManagementSystem\\App_Data\\project_tracking.db;Version=3;";
        private static string ConnectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";

        public static User GetUserByUsernameAndPassword(string username, string password)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT UserID, Username, Password, Role, IsActive, CreatedDate FROM USER WHERE Username = @Username";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //get stored hash from database
                            string storedHash = reader["Password"].ToString();
                            // Debug: Print stored hash
                            System.Diagnostics.Debug.WriteLine($"Stored Hash: {storedHash}");

                            //verify the password hash
                            if (!BCrypt.Net.BCrypt.Verify(password, storedHash))
                            {
                                System.Diagnostics.Debug.WriteLine("Password does not match");
                                return null; //password does not match
                            };
                            User user = null;
                            string role = reader["Role"].ToString();

                            switch (role)
                            {
                                case "Admin":
                                    user = new Admin();
                                    break;
                                case "ProjectManager":
                                    user = new ProjectManager();
                                    break;
                                case "Technician":
                                    user = new Technician();
                                    break;
                                default:
                                    throw new Exception("Unknown role detected.");
                            }
                            // Populate common properties
                            user.UserId = Convert.ToInt32(reader["UserID"]);
                            user.Username = reader["Username"].ToString();
                            user.PasswordHash = storedHash; // Store the hashed password
                            user.IsActive = Convert.ToBoolean(reader["IsActive"]);
                            user.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);

                            // Debug: Print user details
                            System.Diagnostics.Debug.WriteLine($"User found: {user.Username}, Role: {user.Role}");

                            return user;
                        }
                        else {
                            // Debug: Print no user found
                            System.Diagnostics.Debug.WriteLine("No user found with the provided username");

                            return null; // No user found
                        }
                    }
                }
            }
        }
        public static bool CreateUser(string username, string password, string role)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password); 

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO USER (Username, Password, Role, CreatedDate, IsActive) VALUES (@Username, @Password, @Role, @CreatedDate, 1)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", hashedPassword); // Use hashed password
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public static void UpdateUserPassword(string username, string newPassword)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword); // Hash only if not already hashed

            using (var connection = new SQLiteConnection("Data Source=App_Data/project_tracking.db;Version=3;"))
            {
                connection.Open();
                using (var command = new SQLiteCommand("UPDATE User SET Password = @Password WHERE Username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.Parameters.AddWithValue("@Username", username);
                    command.ExecuteNonQuery();
                }
            }
        }


    }
}

