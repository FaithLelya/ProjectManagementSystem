using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementSystem.Views.Shared
{
    public partial class SidebarPartial : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Only perform initial setup on first load
            if (!IsPostBack)
            {
                // Any initialization logic
            }
        }

        // Event handlers for LinkButtons
        protected void btnDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Dashboard.aspx");
        }

        protected void btnProjects_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Projects/ProjectList.aspx");
        }

        protected void btnResources_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Resources/ResourceManagement.aspx");
        }

        protected void btnAttendance_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Technician/Attendance.aspx");
        }

        protected void btnPaymentInfo_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Technician/PaymentDetails.aspx");
        }

        protected void btnAssignTechnicians_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ProjectManager/TechnicianAssignment.aspx");
        }

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Projects/CreateProject.aspx");
        }

        protected void btnReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Reports.aspx");
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/CreateUser.aspx");
        }

        protected void btnAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/MyAccount.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Implement logout logic
            // For example:
            // FormsAuthentication.SignOut();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }

        // Role-based visibility methods
        protected bool IsUserTechnician()
        {
            // Implement actual role checking logic
            return Session["UserRole"] != null && Session["UserRole"].ToString() == "Technician";
        }

        protected bool IsProjectManager()
        {
            // Implement actual role checking logic
            return Session["UserRole"] != null && Session["UserRole"].ToString() == "ProjectManager";
        }

        protected bool IsAdmin()
        {
            // Implement actual role checking logic
            return Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin";
        }
    }
}