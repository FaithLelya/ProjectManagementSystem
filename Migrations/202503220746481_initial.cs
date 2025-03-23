namespace ProjectManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(),
                        Description = c.String(),
                        Location = c.String(),
                        Budget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Resources = c.String(),
                        Status = c.String(),
                        BudgetRangeMin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BudgetRangeMax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalResourceCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaterialsCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProjectManagerId = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TechnicianPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalExpense = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ProjectId);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        ResourceId = c.Int(nullable: false, identity: true),
                        ResourceName = c.String(),
                        Description = c.String(),
                        Quantity = c.Int(nullable: false),
                        CostPerunit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Project_ProjectId = c.Int(),
                    })
                .PrimaryKey(t => t.ResourceId)
                .ForeignKey("dbo.Projects", t => t.Project_ProjectId)
                .Index(t => t.Project_ProjectId);
            
            CreateTable(
                "dbo.Technicians",
                c => new
                    {
                        TechnicianId = c.Int(nullable: false, identity: true),
                        HourlyRate = c.Double(nullable: false),
                        OvertimeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TechnicianLevel = c.String(),
                        IsSenior = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                        Username = c.String(),
                        PasswordHash = c.String(),
                        Role = c.String(),
                        Email = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Project_ProjectId = c.Int(),
                    })
                .PrimaryKey(t => t.TechnicianId)
                .ForeignKey("dbo.Projects", t => t.Project_ProjectId)
                .Index(t => t.Project_ProjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Technicians", "Project_ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Resources", "Project_ProjectId", "dbo.Projects");
            DropIndex("dbo.Technicians", new[] { "Project_ProjectId" });
            DropIndex("dbo.Resources", new[] { "Project_ProjectId" });
            DropTable("dbo.Technicians");
            DropTable("dbo.Resources");
            DropTable("dbo.Projects");
        }
    }
}
