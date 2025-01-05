using System;
using System.Web.UI;
using System.Collections.Generic;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Controllers;

namespace ProjectManagementSystem.Views.Shared
{
    public partial class Login : System.Web.UI.Page
    {
       
        // This list simulates our database of users for now
        private static List<User> _users = new List<User>
        {
            new Admin {
                UserId = 1,
                Username = "admin",
                Password = "admin123",
                Role = "Admin",
                IsActive = true,
                CreatedDate = DateTime.Now,
            },
            new ProjectManager {
                UserId = 2,
                Username = "projectmanager",
                Password = "pm123",
                Role = "ProjectManager",
                IsActive = true,
                CreatedDate = DateTime.Now,
            },
            new Technician {
                UserId = 3,
                Username = "technician",
                Password = "tech123",
                Role = "Technician",
                IsActive = true,
                CreatedDate = DateTime.Now,
                HourlyRate = 120.00m,
                TotalPayment = 0
            }
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                //Response.Redirect("~/Views/Shared/Dashboard/Index.aspx");
                Response.Redirect("~/Views/Shared/Login.aspx");
            }

            if (!IsPostBack)
            {
                lblError.Visible = false;
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                // Find the user by username and check if the password matches
                var user = _users.Find(u =>
                    u.Username == username &&
                    u.Password == password &&
                    u.IsActive);

                if (user != null)
                {
                    Session["UserId"] = user.UserId;
                    Session["UserRole"] = user.Role; //Use the method to get the role
                    Session["Username"] = user.Username;

                    // Redirect to the single dashboard
                    Response.Redirect("~/Views/Shared/Dashboard/Welcome.aspx");
                }
                else
                {
                    lblError.Text = "Invalid username or password";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "An error occurred during login. Please try again.";
                lblError.Visible = true;
            }
        }
    }
}