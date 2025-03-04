using System;
using System.Web.UI;
using ProjectManagementSystem.Helpers;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    public partial class UserController : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                Response.Redirect("~/Views/Shared/Login.aspx");
            }
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = ddlRole.SelectedValue;

            // No need to hash the password here, SQLiteHelper.CreateUser does it using BCrypt
            if (SQLiteHelper.CreateUser(username, password, role))
            {
                lblMessage.Text = "User created successfully!";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblMessage.Text = "Failed to create user.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        private User AuthenticateUser(string username, string password)
        {
            User user = SQLiteHelper.GetUserByUsernameAndPassword(username, password);

            if (user == null)
            {
                return null; // User does not exist or password is incorrect
            }

            return user; // Authenticated successfully
        }
    }
}
