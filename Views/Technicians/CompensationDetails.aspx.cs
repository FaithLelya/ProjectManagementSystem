using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementSystem.Controllers;

namespace ProjectManagementSystem.Views.Technicians
{
    public partial class CompensationDetails : System.Web.UI.Page
    {

        private ProjectController _projectController;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _projectController = new ProjectController();
                int userId = int.Parse(Request.QueryString["userId"]);
              //LoadCompensationDetails(userId);
            }
        }
        /*
        private void LoadCompensationDetails(int userId)
        {
            var technician = _projectController.GetTechnicianDetails(userId);
            lblHourlyRate.Text += technician.HourlyRate.ToString("C");
            lblOvertimeRate.Text += technician.OvertimeRate.ToString("C");
            lblBonuses.Text += technician.BonusesEarned.ToString("C");
            lblTotalPayment.Text += technician.TotalPaymentEstimate.ToString("C");
        }
        */
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Logic to save the compensation details if needed
        }
    }
}
