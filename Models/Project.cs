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
        public List<int> AssignedTenchicianId { get; set; }
        public List<Resource> AllocatedResources { get; set; }
        public List<Technician> Technicians { get; set; }
        public decimal BudgetRangeMin { get; set; }
        public decimal BudgetRangeMax { get; set; }

        //Role-based Permission methods
        public bool CanViewBudget(string userRole)
        {
            return userRole != "Technician";
        }

        public bool CanModifyBudget(string userRole)
        {
            // return userRole == "Management" || userRole == "ProjectManager";
            return userRole == "Management";
        }

        public bool CanCreateProject(string userRole)
        {
            return userRole == "ProjectManager";
        }

        public bool CanAssignTechnicians(string userRole)
        {
            return userRole == "ProjectManager";
        }

        public bool CanAddResources(string userRole, bool isAssignedToProject)
        {
            return userRole != "Technician" || isAssignedToProject;
        }
    }
}
