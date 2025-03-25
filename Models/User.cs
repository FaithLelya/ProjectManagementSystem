using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace ProjectManagementSystem.Models
{
    public abstract class User
    {
        //Unique identifier for each user
        public int UserId { get; set; }
        //basic login credentials
        public string Username { get; set; }
        public string PasswordHash { get; set; } //store the hashed password
       //user's role in the system
        public string Role { get; set; } // "Technician", "ProjectManager", "Management"
        public string Email { get; set; }
        //Audit information
        public DateTime CreatedDate { get; set; }
        //Account status
        public bool IsActive { get; set; }

        //method to get the tole of the user
        public string GetRole()
        {
            return Role;
        }
    }
    public class ConcreteUser : User
    {
    }

    public class Technician : User
    {
        public Technician()
        {
            //Placeholder for database info(?)
            Role = "Technician";
        }
        public int TechnicianId { get; set; }
        public string UserName {get; set; }
        public double HourlyRate { get; set; }
        public decimal OvertimeRate { get; set; }
        public decimal TotalPayment { get; set; }
        public string TechnicianLevel { get; set; }
        public bool IsSenior { get; set; }
        //add projects assigned?

        //Use Form for some of these
        public void LogWorkHours() { }
        public void ViewProjectDetails() { }
        public void AddResourcesToProject() { }
        public void CalculatePay() { } 
        public void UpdatePay() { }
        public double GetTechnicianHourlyRate(int userId)
        {
            // Fetch the technician's hourly rate from the database
            HourlyRate = 230;
            return HourlyRate;
        }
    }
    public class ProjectManager : User
    {
        //add more specific variables
        public ProjectManager() 
        { 
            //Placeholder?
            Role = "ProjectManager"; 
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
    public void SetBudgetRange() { }
    public void AllocateResources() { }
    }
}