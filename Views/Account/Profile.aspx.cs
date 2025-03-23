using System;
using System.Data.SQLite;
using System.Web;
using System.Web.UI;

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

            if (string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(newEmail))
            {
                lblMessage.Text = "All fields are required.";
                lblMessage.CssClass = "alert alert-danger mt-3";
                return;
            }

            string connectionString = "Data Source=C:\\ProjectsDb\\ProjectTracking\\project_tracking.db;Version=3;";
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE User SET UserName = @UserName, Email = @Email WHERE UserId = @UserId";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", newName);
                    cmd.Parameters.AddWithValue("@Email", newEmail);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Profile updated successfully!";
                        lblMessage.CssClass = "alert alert-success mt-3";
                    }
                    else
                    {
                        lblMessage.Text = "Update failed. Try again.";
                        lblMessage.CssClass = "alert alert-danger mt-3";
                    }
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Shared/Dashboard/Welcome");
        }
    }
}
