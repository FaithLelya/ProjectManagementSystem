using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementSystem.Models
{ 
    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } //In Progress, Completed
        public List<User> AssignedTechnicians { get; set; }
        public List<Resource> AllocatedResources { get; set; }
        //public List<Technician> Technicians { get; set; }
        public decimal BudgetRangeMin { get; set; }
        public decimal BudgetRangeMax { get; set; }
        public decimal TotalResourceCost { get; set; }
        public decimal TotalTechnicianPayments { get; set; }
        public decimal TotalExpense { get; set; } 

        // Method to calculate total expenses
        public void CalculateTotalExpense()
        {
            TotalExpense = TotalResourceCost + TotalTechnicianPayments;
        }

        public Project()
        {
            AssignedTechnicians = new List<User>();
            AllocatedResources = new List<Resource>();
        }
    }
}
