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
        public decimal Budget { get; set; }
        // Other relevant properties
    }

    public class Technician
    {
        public int TechnicianId { get; set; }
        public string TechnicianName { get; set; }
        public decimal HourlyRate { get; set; }
        // Other relevant properties }

    }

}