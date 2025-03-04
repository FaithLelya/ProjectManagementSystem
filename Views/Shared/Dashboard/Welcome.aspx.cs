using ProjectManagementSystem.Views.Technicians;
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
            string userRole = Session["UserRole"].ToString();
            string username = Session["Username"]?.ToString() ?? "User";

            lblUsername.Text = username;
            lblRole.Text = userRole;

            // Set role-specific welcome message
            switch (userRole.ToLower())
            {
                case "Technician":
                    lblRoleSpecificMessage.Text = "Wecome! Access your work logs and payment information";
                    break;
                case "ProjectManager":
                    lblRoleSpecificMessage.Text = "Welcome! Manage projects and team resources";
                    break;
                case "Admin":
                    lblRoleSpecificMessage.Text = "Welcome! Monitor system performance and manage budgets";
                    break;
            }

            // Configure panel visibility based on role
            ConfigurePanelVisibility(userRole);
        }

        private void ConfigurePanelVisibility(string Role)
        {
            // Technician panels
            pnlAttendance.Visible = Role.Equals("Technician", StringComparison.OrdinalIgnoreCase);
            pnlPaymentInfo.Visible = Role.Equals("Technician", StringComparison.OrdinalIgnoreCase);

            // Project Manager panels
            pnlAssignTechnicians.Visible = Role.Equals("ProjectManager", StringComparison.OrdinalIgnoreCase);

            // Admin panels
            pnlReports.Visible = Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            pnlCreateProject.Visible = Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            pnlCreateUser.Visible = Role.Equals("Admin", StringComparison.OrdinalIgnoreCase); 
        }

        protected bool IsUserTechnician() => Session["UserRole"]?.ToString().Equals("Technician", StringComparison.OrdinalIgnoreCase) ?? false;
        protected bool IsProjectManager() => Session["UserRole"]?.ToString().Equals("ProjectManager", StringComparison.OrdinalIgnoreCase) ?? false;
        protected bool IsAdmin() => Session["UserRole"]?.ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase) ?? false;

        // Button click handlers
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
            Response.Redirect("~/Views/Technician/PaymentInfo.aspx");
        }

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Projects/CreateProject.aspx");
        }

        protected void btnAssignTechnicians_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/ProjectManager/AssignTechnicians.aspx");
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
            Response.Redirect("~/Views/Resources/Index.aspx");
        }

        protected void btnAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Account/Profile.aspx");
        }
        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Admin/CreateUser.aspx");
        }
    }
}