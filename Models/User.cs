using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace ProjectManagementSystem.Models
{
    public abstract class User
    {
        //Unique identifier for each user
        public int UserId { get; set; }
        //basic login credentials
        public string Username { get; set; }
        public string Password { get; set; } //will be hashed in production
       //user's role in the system
        public string Role { get; set; } // "Technician", "ProjectMAnager", "Management"
        //Audit information
        public DateTime CreatedDate { get; set; }
        //Account status
        public bool IsActive { get; set; }
        //should I add email? 
        public abstract void DisplayDashboard(Page page);
    }

    public class Technician : User
    {
        public decimal HourlyRate { get; set; }
        public decimal OvertimeRate { get; }
        public decimal TotalPayment { get; set; }
        public Technician()
        {
            //Placeholder for database info(?)
            Role = "Technician";
        }
        public override void DisplayDashboard(Page page)
        {
            // Redirect to Technician dashboard
            page.Response.Redirect("~/Views/Shared/Dashboard/Welcome.aspx");
        }
        //Use Form for some of these
        public void LogWorkHours() { }
        public void ViewProjectDetails() { }
        public void AddResourcesToProject() { }
        public void CalculatePay() { } 
        public void UpdatePay() { }
    }
    public class ProjectManager : User
    {
        //add more specific variables
        public ProjectManager() 
        { 
            //Placeholder?
            Role = "ProjectManager"; 
        }
        public override void DisplayDashboard(Page page)
        {
            page.Response.Redirect("~/Views/Shared/Dashboard/Welcome.aspx");
        } 
        public void CreateProject() { }
        public void ManageBudget() { } 
        public void AssignTechnicians() { } 
        public void MonitorProgress() { }
        }
    public class Admin : User
    {
        //add admin specific variables
        public Admin() { 
            Role = "Admin"; 
        }
        public override void DisplayDashboard(Page page)
        {
            page.Response.Redirect("~/Views/Shared/Dashboard/Welcome.aspx");
        }
    public void SetBudgetRange() { }
    public void AllocateResources() { }
    }
}