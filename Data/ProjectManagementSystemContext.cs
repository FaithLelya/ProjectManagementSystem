using ProjectManagementSystem.Models;
using System.Data.Entity;


namespace ProjectManagementSystem.Data
{
    public class ProjectManagementSystemContext : DbContext
    {
        public ProjectManagementSystemContext() { 

        }
        public DbSet<User> User { get; set; }
        public DbSet<Resource> Resource { get; set; }

        public DbSet<Project> Project { get; set; } 

        
    }
}