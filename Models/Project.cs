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
        public decimal ResourceCost { get; set; }
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

        public decimal TechnicianPlannedPayment { get; set; } // planned
        public decimal ResourcePlannedCost { get; set; } // planned
        public decimal MaterialPlannedCost { get; set; } // planned

        // New actual expenses
        public decimal TechnicianExpenses { get; set; }
        public decimal ResourceExpenses { get; set; }
        public decimal MaterialExpenses { get; set; }

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
            TotalExpense = TechnicianExpenses + ResourceExpenses + MaterialExpenses;
        }
        // In Project.cs model
        public void ComputeExpenses()
        {
            // Reset all calculated values first
            TotalResourceCost = 0;
            ResourceCost = 0;

            // Calculate resource costs from actual resources
            if (AllocatedResources != null)
            {
                foreach (var res in AllocatedResources)
                {
                    if (res.Quantity > 0 && res.CostPerunit > 0)
                    {
                        decimal itemCost = res.Quantity * res.CostPerunit;
                        ResourceCost += itemCost;
                        TotalResourceCost += itemCost;
                    }
                }
            }

            // IMPORTANT: Set total expense to ONLY include actual costs
            TotalExpense = ResourceCost;

            // Add other costs ONLY if they are greater than zero
            // (They should be zero by default until costs are incurred)
            if (TechnicianPayment > 0)
            {
                TotalExpense += TechnicianPayment;
            }

            if (MaterialsCost > 0)
            {
                TotalExpense += MaterialsCost;
            }
        }

        public string GetBudgetUsagePercentage()
        {
            if (Budget <= 0) return "N/A";
            decimal percentage = (TotalExpense / Budget) * 100;
            return $"{percentage:N1}% used";
        }

        public string GetBudgetVarianceText()
        {
            if (Budget <= 0) return "";

            decimal variance = Budget - TotalExpense;

            if (variance < 0)
                return $"<span class='text-danger'>Over Budget by KES {-variance:N2}</span>";
            else if (variance == 0)
                return "<span class='text-success'>Exactly On Budget</span>";
            else
                return $"<span class='text-success'>Under Budget by KES {variance:N2}</span>";
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