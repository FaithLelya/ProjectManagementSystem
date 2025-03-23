using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManagementSystem.Models
{
    public class ProjectTask
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public decimal Progress { get; set; }
        public string Id { get; set; } // This is for the client-side task ID (e.g., task_abc123)
    }
}