namespace Omda_School.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class omdaSchool : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Degrees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentDegree = c.Int(nullable: false),
                        ResultID = c.Int(nullable: false),
                        SubjectID = c.Int(nullable: false),
                        StudentID = c.Int(nullable: false),
                        IsDeleted = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Results", t => t.ResultID, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectID, cascadeDelete: true)
                .Index(t => t.ResultID)
                .Index(t => t.SubjectID)
                .Index(t => t.StudentID);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        LevelAdmin = c.String(),
                        LevelID = c.Int(nullable: false),
                        YearID = c.Int(nullable: false),
                        IsDeleted = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Levels", t => t.LevelID, cascadeDelete: true)
                .ForeignKey("dbo.Years", t => t.YearID, cascadeDelete: true)
                .Index(t => t.LevelID)
                .Index(t => t.YearID);
            
            CreateTable(
                "dbo.Levels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepartmentID = c.Int(nullable: false),
                        Name = c.String(),
                        IsDeleted = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Years",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.String(),
                        IsCurrent = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LevelID = c.Int(nullable: false),
                        YearID = c.Int(nullable: false),
                        Name = c.String(),
                        Phone = c.String(),
                        address = c.String(),
                        BirthDay = c.String(),
                        Fees = c.Double(nullable: false),
                        PaidFees = c.Double(nullable: false),
                        IsDeleted = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Levels", t => t.LevelID, cascadeDelete: false)
                .ForeignKey("dbo.Years", t => t.YearID, cascadeDelete: false)
                .Index(t => t.LevelID)
                .Index(t => t.YearID);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ExamDegree = c.Int(nullable: false),
                        LevelID = c.Int(nullable: false),
                        IsDeleted = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Levels", t => t.LevelID, cascadeDelete: false)
                .Index(t => t.LevelID);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentDate = c.String(),
                        Note = c.String(),
                        PaymentYearDateID = c.Int(nullable: false),
                        Amount = c.Single(nullable: false),
                        StudentID = c.Int(nullable: false),
                        IsDeleted = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .Index(t => t.StudentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Degrees", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "LevelID", "dbo.Levels");
            DropForeignKey("dbo.Degrees", "StudentID", "dbo.Students");
            DropForeignKey("dbo.Students", "YearID", "dbo.Years");
            DropForeignKey("dbo.Students", "LevelID", "dbo.Levels");
            DropForeignKey("dbo.Degrees", "ResultID", "dbo.Results");
            DropForeignKey("dbo.Results", "YearID", "dbo.Years");
            DropForeignKey("dbo.Results", "LevelID", "dbo.Levels");
            DropForeignKey("dbo.Levels", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.Payments", new[] { "StudentID" });
            DropIndex("dbo.Subjects", new[] { "LevelID" });
            DropIndex("dbo.Students", new[] { "YearID" });
            DropIndex("dbo.Students", new[] { "LevelID" });
            DropIndex("dbo.Levels", new[] { "DepartmentID" });
            DropIndex("dbo.Results", new[] { "YearID" });
            DropIndex("dbo.Results", new[] { "LevelID" });
            DropIndex("dbo.Degrees", new[] { "StudentID" });
            DropIndex("dbo.Degrees", new[] { "SubjectID" });
            DropIndex("dbo.Degrees", new[] { "ResultID" });
            DropTable("dbo.Payments");
            DropTable("dbo.Subjects");
            DropTable("dbo.Students");
            DropTable("dbo.Years");
            DropTable("dbo.Departments");
            DropTable("dbo.Levels");
            DropTable("dbo.Results");
            DropTable("dbo.Degrees");
        }
    }
}
