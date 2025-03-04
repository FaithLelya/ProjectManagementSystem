using System;
using ProjectManagementSystem.Helpers;

namespace ProjectManagementSystem.Views.Shared
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                Response.Redirect("~/Views/Shared/Dashboard/Welcome.aspx");
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
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text.Trim();
                // Debug: Print input credentials
                System.Diagnostics.Debug.WriteLine($"Email: {email}, Password: {password}");

                // Fetch the user from the database
                var user = SQLiteHelper.GetUserByEmailAndPassword(email, password);

                //Debug: Print user object
                System.Diagnostics.Debug.WriteLine($"User: {user}");

                if (user != null)
                {
                    if (!user.IsActive)
                    {
                        lblError.Text = "Your account is inactive. Please contact the administrator.";
                        lblError.Visible = true;
                        return;
                    }

                    //Debug: print session details
                    Console.WriteLine($"UserID: {user.UserId}, Role: {user.Role}, Email:{user.Username}");


                    Session["UserID"] = user.UserId;
                    Session["UserRole"] = user.Role; 
                    Session["Username"] = user.Username;

                    // Debug: Print redirect message
                    System.Diagnostics.Debug.WriteLine("Redirecting to Welcome.aspx");

                    // Redirect to the dashboard
                    Response.Redirect("~/Views/Shared/Dashboard/Welcome.aspx");
                }
                else
                {
                    // Debug: Print invalid credentials message
                    System.Diagnostics.Debug.WriteLine("Invalid email or password");

                    lblError.Text = "Invalid email or password";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {

                // Debug: Print exception details
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");

                lblError.Text = "An error occurred during login. Please try again.";
                lblError.Visible = true;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Views/Shared/Login.aspx");
        }

    }
}