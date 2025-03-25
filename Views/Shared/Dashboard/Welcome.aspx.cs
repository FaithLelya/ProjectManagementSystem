using System;
using System.Web.UI;

namespace ProjectManagementSystem.Views.Shared.Dashboard
{
    public partial class Welcome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/Views/Shared/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                SetupDashboard();
            }
        }

        private void SetupDashboard()
        {
            string userRole = Session["UserRole"]?.ToString() ?? "User";
            string username = Session["Username"]?.ToString() ?? "User";

            lblUsername.Text = username;
            lblRole.Text = userRole;

            // Set user initial for profile circle
            if (!string.IsNullOrEmpty(username))
            {
                ltUserInitial.Text = username.Substring(0, 1).ToUpper();
            }
            else
            {
                ltUserInitial.Text = "U";
            }

            // Set role-specific welcome message
            switch (userRole.ToLower())
            {
                case "technician":
                    lblRoleSpecificMessage.Text = "Welcome! Access your work logs and payment information";
                    break;
                case "projectmanager":
                    lblRoleSpecificMessage.Text = "Welcome! Manage projects and team resources";
                    break;
                case "admin":
                    lblRoleSpecificMessage.Text = "Welcome! Monitor system performance and manage budgets";
                    break;
                default:
                    lblRoleSpecificMessage.Text = "Welcome to the Project Management System";
                    break;
            }

            // Configure sidebar item visibility based on role
            ConfigureSidebarVisibility(userRole);
        }

        private void ConfigureSidebarVisibility(string role)
        {
            // Technician-specific items
            btnAttendance.Visible = role.Equals("Technician", StringComparison.OrdinalIgnoreCase);
            btnPaymentInfo.Visible = role.Equals("Technician", StringComparison.OrdinalIgnoreCase);

            // Project Manager-specific items
            btnAssignTechnicians.Visible = role.Equals("ProjectManager", StringComparison.OrdinalIgnoreCase);

            // Admin-specific items
            btnReports.Visible = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            btnCreateProject.Visible = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            btnCreateUser.Visible = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        protected bool IsUserTechnician() => Session["UserRole"]?.ToString().Equals("Technician", StringComparison.OrdinalIgnoreCase) ?? false;
        protected bool IsProjectManager() => Session["UserRole"]?.ToString().Equals("ProjectManager", StringComparison.OrdinalIgnoreCase) ?? false;
        protected bool IsAdmin() => Session["UserRole"]?.ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase) ?? false;

        // Button click handlers
        protected void btnDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Shared/Dashboard/Welcome.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Views/Shared/Login.aspx");
        }

        protected void btnAttendance_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Technicians/RecordAttendance.aspx");
        }

        protected void btnPaymentInfo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Technicians/PaymentInfo.aspx");
        }

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Projects/CreateProject.aspx");
        }

        protected void btnAssignTechnicians_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/ProjectManagers/ViewTechnicians.aspx");
        }

        protected void btnReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Admin/Reports.aspx");
        }

        protected void btnProjects_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Projects/Projects.aspx");
        }

        protected void btnResources_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Resources/Resources.aspx");
        }

        protected void btnAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Account/Profile.aspx");
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Controllers/UserController.aspx");
        }
    }
}