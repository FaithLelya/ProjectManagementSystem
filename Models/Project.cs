using System;
using System.Collections.Generic;

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
        public DateTime? EndDate { get; set; }
        public string Resources { get; set; }
        public string Status { get; set; } //In Progress, Completed
        public List<Technician> AssignedTechnicians { get; set; }
        public List<Resource> AllocatedResources { get; set; }
        //public List<Technician> Technicians { get; set; }
        public decimal BudgetRangeMin { get; set; }
        public decimal BudgetRangeMax { get; set; }
        public decimal TotalResourceCost { get; set; }
        public decimal MaterialsCost { get; set; }
        public decimal ProjectManagerId { get; set; }
        public decimal TechnicianPayment { get; set; }
        public decimal TotalExpense { get; set; }

        // Method to calculate total expenses
        public void CalculateTotalExpense()
        {
            TotalExpense = TotalResourceCost + TechnicianPayment;
        }

        public Project()
        {
            AssignedTechnicians = new List<Technician>();
            AllocatedResources = new List<Resource>();
        }
        public class TimeEntry
        {
            public DateTime Date { get; set; }
            public double HoursWorked { get; set; }
        }
    }
}
