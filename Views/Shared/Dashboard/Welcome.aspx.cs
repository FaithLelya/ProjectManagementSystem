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
                case "technician":
                    lblRoleSpecificMessage.Text = "Access your work logs and payment information";
                    break;
                case "projectmanager":
                    lblRoleSpecificMessage.Text = "Manage projects and team resources";
                    break;
                case "admin":
                    lblRoleSpecificMessage.Text = "Monitor system performance and manage budgets";
                    break;
            }

            // Configure panel visibility based on role
            ConfigurePanelVisibility(userRole);
        }

        private void ConfigurePanelVisibility(string role)
        {
            // Technician panels
            pnlTimeLogging.Visible = role.Equals("technician", StringComparison.OrdinalIgnoreCase);
            pnlPaymentInfo.Visible = role.Equals("technician", StringComparison.OrdinalIgnoreCase);

            // Project Manager panels
            pnlCreateProject.Visible = role.Equals("projectmanager", StringComparison.OrdinalIgnoreCase);
            pnlAssignTechnicians.Visible = role.Equals("projectmanager", StringComparison.OrdinalIgnoreCase);

            // Admin panels
            pnlBudgetControl.Visible = role.Equals("admin", StringComparison.OrdinalIgnoreCase);
        }

        protected bool IsUserTechnician() => Session["UserRole"]?.ToString().Equals("technician", StringComparison.OrdinalIgnoreCase) ?? false;
        protected bool IsProjectManager() => Session["UserRole"]?.ToString().Equals("projectmanager", StringComparison.OrdinalIgnoreCase) ?? false;
        protected bool IsAdmin() => Session["UserRole"]?.ToString().Equals("admin", StringComparison.OrdinalIgnoreCase) ?? false;

        // Button click handlers
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Views/Shared/Login.aspx");
        }

        protected void btnTimeLogging_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Technician/TimeLogging.aspx");
        }

        protected void btnPaymentInfo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Technician/PaymentInfo.aspx");
        }

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/ProjectManager/CreateProject.aspx");
        }

        protected void btnAssignTechnicians_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/ProjectManager/AssignTechnicians.aspx");
        }

        protected void btnBudgetControl_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Admin/BudgetControl.aspx");
        }

        protected void btnProjects_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Projects/Index.aspx");
        }

        protected void btnResources_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Resources/Index.aspx");
        }

        protected void btnAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Account/Profile.aspx");
        }
    }
}