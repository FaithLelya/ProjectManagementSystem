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
        public int TaskCount { get; set; }
        public int CompletedTasks { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Resources { get; set; }
        public string Status { get; set; } //In Progress, Completed
        public List<Technician> AssignedTechnicians { get; set; }
        public List<Resource> AllocatedResources { get; set; }
        public decimal BudgetRangeMin { get; set; }
        public decimal BudgetRangeMax { get; set; }
        public decimal TotalResourceCost { get; set; }
        public decimal MaterialsCost { get; set; }
        public decimal ProjectManagerId { get; set; }
        public decimal TechnicianPayment { get; set; }
        public decimal TotalExpense { get; set; }

        // New properties to support completion confirmation - fixed for nullable DateTime?
        public bool IsEndDatePassed => EndDate.HasValue && DateTime.Now > EndDate.Value;
        public bool NeedsCompletionConfirmation => IsEndDatePassed && Status != "Completed";
        public DateTime? CompletionDate { get; set; }
        public string CompletionNotes { get; set; }
        public int? CompletedByUserId { get; set; }

        // Additional properties that could be useful - fixed for nullable DateTime?
        public decimal BudgetRemaining => Budget - TotalExpense;
        public decimal BudgetUtilizationPercentage => Budget > 0 ? (TotalExpense / Budget) * 100 : 0;

        // Fixed properties that handle nullable EndDate
        public int DaysRemaining
        {
            get
            {
                if (!EndDate.HasValue) return 0;
                return EndDate.Value > DateTime.Now ? (EndDate.Value - DateTime.Now).Days : 0;
            }
        }

        public int TotalDays
        {
            get
            {
                if (!EndDate.HasValue) return 0;
                return (EndDate.Value - StartDate).Days;
            }
        }

        public int ElapsedDays
        {
            get
            {
                return (DateTime.Now > StartDate) ? (DateTime.Now - StartDate).Days : 0;
            }
        }

        // Keeping only the existing ProgressPercentage based on tasks
        public int ProgressPercentage
        {
            get
            {
                return TaskCount > 0 ? (int)Math.Round((double)CompletedTasks / TaskCount * 100) : 0;
            }
        }

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