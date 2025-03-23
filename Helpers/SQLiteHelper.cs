using System;
using System.Collections.Generic;
using ProjectManagementSystem.Models;
using System.Data.SQLite;
using ProjectManagementSystem.Controllers;

namespace ProjectManagementSystem.Helpers
{
    public static class SQLiteHelper
    {
        private static string ConnectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";

        public static User GetUserByEmailAndPassword(string email, string password)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT UserID, Username, Password, Role, IsActive, CreatedDate, Email FROM User WHERE Email = @Email";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //get stored hash from database
                            string storedHash = reader["Password"].ToString();
                            // Debug: Print stored hash
                            System.Diagnostics.Debug.WriteLine($"Stored Hash: {storedHash}");

                            // Check if the stored password is a valid bcrypt hash
                            if (!storedHash.StartsWith("$2a$") && !storedHash.StartsWith("$2b$") && !storedHash.StartsWith("$2y$"))
                            {
                                System.Diagnostics.Debug.WriteLine("Error: Password in DB is not a valid bcrypt hash!");

                                // If using plain text passwords for testing, uncomment this
                                // if (password == storedHash)
                                // {
                                //     // Plain text match for testing only
                                // }
                                // else
                                // {
                                //     return null;
                                // }

                                return null;
                            }

                            // Verify the password hash
                            try
                            {
                                if (!BCrypt.Net.BCrypt.Verify(password, storedHash))
                                {
                                    System.Diagnostics.Debug.WriteLine("Password does not match");
                                    return null; // Password does not match
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"BCrypt error: {ex.Message}");
                                return null;
                            }

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
                                    throw new Exception($"Unknown role detected: {role}");
                            }

                            // Populate common properties
                            user.UserId = Convert.ToInt32(reader["UserID"]);
                            user.Username = reader["Username"].ToString();
                            user.PasswordHash = storedHash; // Store the hashed password
                            user.IsActive = Convert.ToBoolean(reader["IsActive"]);
                            user.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                            user.Email = reader["Email"].ToString();
                            user.Role = role; // Set the role property explicitly

                            // Debug: Print user details
                            System.Diagnostics.Debug.WriteLine($"User found: {user.Email}, Role: {user.Role}, ID: {user.UserId}");

                            return user;
                        }
                        else
                        {
                            // Debug: Print no user found
                            System.Diagnostics.Debug.WriteLine("No user found with the provided email.");
                            return null; // No user found
                        }
                    }
                }
            }
        }
        public static void InsertProject(string ProjectName, string Description, string Location, DateTime StartDate, DateTime EndDate, decimal TechnicianPayment, decimal MaterialsCost, decimal Budget, int ProjectManagerId, string Resources)
        {
            using (SQLiteConnection conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Projects (ProjectName, Description, Location, StartDate, EndDate, TechnicianPayment, MaterialsCost, Budget, ProjectManagerId, Resources) " +
                             "VALUES (@ProjectName, @Description, @Location, @StartDate, @EndDate, @TechnicianPayment, @MaterialsCost, @Budget, @ProjectManagerId, @Resources)";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProjectName", ProjectName);
                    cmd.Parameters.AddWithValue("@Description", Description);
                    cmd.Parameters.AddWithValue("@Location", Location);
                    cmd.Parameters.AddWithValue("@StartDate", StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", EndDate);
                    cmd.Parameters.AddWithValue("@TechnicianPayment", TechnicianPayment);
                    cmd.Parameters.AddWithValue("@MaterialsCost", MaterialsCost);
                    cmd.Parameters.AddWithValue("@Budget", Budget);
                    cmd.Parameters.AddWithValue("@ProjectManagerId", ProjectManagerId);
                    cmd.Parameters.AddWithValue("@Resources", Resources);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static List<ProjectManager> GetProjectManagers()
        {
            List<ProjectManager> projectManagers = new List<ProjectManager>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT UserID, Username FROM User WHERE Role = @Role";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Role", "ProjectManager");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProjectManager user = new ProjectManager
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserID")), // Use GetOrdinal for safety
                                Username = reader.GetString(reader.GetOrdinal("Username")) // Use GetOrdinal for safety
                            };
                            projectManagers.Add(user);
                        }
                    }
                }
            }

            return projectManagers;
        }
        public static bool CreateUser(string email, string password, string role, string username, string technicianLevel = null)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO USER (Email, Password, Role, Username, CreatedDate, IsActive) VALUES (@Email, @Password, @Role, @Username, @CreatedDate, 1)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", hashedPassword); // Use hashed password
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@Username", username); // Add username parameter
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    command.Parameters.AddWithValue("@IsActive", 1);

                    bool userCreated = command.ExecuteNonQuery() > 0;

                    // If the user is a technician, insert into the Technicians table
                    if (userCreated)
                    {
                        int userId = (int)connection.LastInsertRowId; // Get the last inserted UserId
                        switch (role)
                        {
                            case "Technician":
                                CreateTechnician(userId, technicianLevel);
                                break;
                            case "ProjectManager":
                                CreateProjectManager(userId);
                                break;
                            case "Admin":
                                CreateAdmin(userId);
                                break;
                        }
                    }
                    return userCreated;
                }
            }
        }
        public static void CreateTechnician(int userId, string technicianLevel)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Technician (TechnicianID, TechnicianLevel) VALUES (@TechnicianID, @TechnicianLevel)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("TechnicianID", userId);
                    command.Parameters.AddWithValue("@TechnicianLevel", technicianLevel);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void CreateProjectManager(int userId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO ProjectManager (ProjectManagerID) VALUES (@ProjectManagerID)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectManagerID", userId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void CreateAdmin(int userId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Admin (User Id) VALUES (@User Id)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@User Id", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static bool UserExists(string email)
        {
            using (var connection = new SQLiteConnection("Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;"))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM User WHERE Email = @Email";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    return Convert.ToInt32(command.ExecuteScalar()) > 0; // Return true if user exists
                }
            }
        }
        public static void UpdateUserPassword(string username, string newPassword)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword); // Hash only if not already hashed

            using (var connection = new SQLiteConnection(ConnectionString))
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

        // Add this to your SQLiteHelper.cs file
        public static List<ProjectResource> GetProjectResources(int projectId)
        {
            List<ProjectResource> resources = new List<ProjectResource>();

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT ResourceId, ResourceName, Quantity, ProjectId FROM ProjectResources WHERE ProjectId = @ProjectId";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProjectId", projectId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProjectResource resource = new ProjectResource
                            {
                                ResourceId = Convert.ToInt32(reader["ResourceId"]),
                                ResourceName = reader["ResourceName"].ToString(),
                                QuantityUsed = Convert.ToInt32(reader["QuantityUsed"]),
                                ProjectId = projectId
                            };
                            resources.Add(resource);
                        }
                    }
                }
            }

            return resources;
        }

    }
}
