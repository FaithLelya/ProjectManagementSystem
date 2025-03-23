using System;
using ProjectManagementSystem.Helpers;
using System.Web;

namespace ProjectManagementSystem.Views.Shared
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Disable browser caching
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));

            if (Session["UserID"] != null)
            {
                // Debug: Log redirect from Page_Load
                System.Diagnostics.Debug.WriteLine($"User already logged in with ID: {Session["UserID"]}, redirecting to dashboard");
                Response.Redirect("~/Views/Shared/Dashboard/Welcome.aspx", false);
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
                System.Diagnostics.Debug.WriteLine($"Login attempt - Email: {email}, Password length: {password.Length}");

                // Validate input
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    lblError.Text = "Email and password are required.";
                    lblError.Visible = true;
                    return;
                }

                // Fetch the user from the database
                var user = SQLiteHelper.GetUserByEmailAndPassword(email, password);

                // Debug: Print user object
                System.Diagnostics.Debug.WriteLine($"User object retrieved: {(user != null ? "Yes" : "No")}");

                if (user != null)
                {
                    if (!user.IsActive)
                    {
                        lblError.Text = "Your account is inactive. Please contact the administrator.";
                        lblError.Visible = true;
                        return;
                    }

                    // Debug: Log session details
                    System.Diagnostics.Debug.WriteLine($"Setting session - UserID: {user.UserId}, Role: {user.Role}, Email: {user.Email}");

                    // Store user information in session
                    Session["UserID"] = user.UserId;
                    Session["UserRole"] = user.Role;
                    Session["Username"] = user.Username;

                    // Debug: Print redirect message
                    System.Diagnostics.Debug.WriteLine("Preparing to redirect to Welcome.aspx");

                    // Redirect to the dashboard with absolute URL
                    string redirectUrl = ResolveUrl("~/Views/Shared/Dashboard/Welcome.aspx");
                    System.Diagnostics.Debug.WriteLine($"Redirecting to: {redirectUrl}");

                    Response.Redirect(redirectUrl, true);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    // Debug: Print invalid credentials message
                    System.Diagnostics.Debug.WriteLine("Authentication failed: Invalid email or password");
                    lblError.Text = "Invalid email or password";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // Debug: Print exception details
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                lblError.Text = "An error occurred during login: " + ex.Message;
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