using System;
using System.Data.SQLite;
using System.Web;
using System.Web.UI;
using System.Security.Cryptography;
using System.Text;

namespace ProjectManagementSystem.Views.Account
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserProfile();
            }
        }

        private void LoadUserProfile()
        {
            string userId = Session["UserId"]?.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT UserName, Email FROM User WHERE UserId = @UserId";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtName.Text = reader["UserName"].ToString();
                            txtEmail.Text = reader["Email"].ToString();
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string userId = Session["UserId"]?.ToString();
            if (string.IsNullOrEmpty(userId))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            string newName = txtName.Text.Trim();
            string newEmail = txtEmail.Text.Trim();
            string oldPassword = txtOldPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            bool changePassword = chkChangePassword.Checked;

            if (string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(newEmail))
            {
                ShowMessage("All fields are required.", "alert-danger");
                return;
            }

            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                // Check if password change is requested
                if (changePassword)
                {
                    if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                    {
                        ShowMessage("All password fields are required.", "alert-danger");
                        return;
                    }

                    if (newPassword != confirmPassword)
                    {
                        ShowMessage("New passwords do not match.", "alert-danger");
                        return;
                    }

                    // Verify old password
                    string storedPasswordHash = GetStoredPasswordHash(userId, conn);
                    if (string.IsNullOrEmpty(storedPasswordHash) || storedPasswordHash != HashPassword(oldPassword))
                    {
                        ShowMessage("Incorrect old password.", "alert-danger");
                        return;
                    }

                    // Update password
                    string newPasswordHash = HashPassword(newPassword);
                    string updatePasswordQuery = "UPDATE User SET PasswordHash = @PasswordHash WHERE UserId = @UserId";
                    using (var updateCmd = new SQLiteCommand(updatePasswordQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@PasswordHash", newPasswordHash);
                        updateCmd.Parameters.AddWithValue("@UserId", userId);
                        updateCmd.ExecuteNonQuery();
                    }
                }

                // Update user details
                string updateQuery = "UPDATE User SET UserName = @UserName, Email = @Email WHERE UserId = @UserId";
                using (var cmd = new SQLiteCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", newName);
                    cmd.Parameters.AddWithValue("@Email", newEmail);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }

            ShowMessage("Profile updated successfully!", "alert-success");
        }

        private string GetStoredPasswordHash(string userId, SQLiteConnection conn)
        {
            string query = "SELECT Password FROM User WHERE UserId = @UserId";
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read() ? reader["Password"].ToString() : null;
                }
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void ShowMessage(string message, string alertClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "alert " + alertClass + " mt-3";
            lblMessage.Visible = true;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/views/shared/Dashboard/welcome.aspx");
        }
    }
}
