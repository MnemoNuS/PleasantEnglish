namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Lessons : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        LessonId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        TimeStart = c.DateTime(nullable: false),
                        TimeEnd = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                        HomeTask = c.String(),
                    })
                .PrimaryKey(t => t.LessonId);
            
            CreateTable(
                "dbo.StudentLessons",
                c => new
                    {
                        Student_StudentId = c.Int(nullable: false),
                        Lesson_LessonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_StudentId, t.Lesson_LessonId })
                .ForeignKey("dbo.Students", t => t.Student_StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Lessons", t => t.Lesson_LessonId, cascadeDelete: true)
                .Index(t => t.Student_StudentId)
                .Index(t => t.Lesson_LessonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentLessons", "Lesson_LessonId", "dbo.Lessons");
            DropForeignKey("dbo.StudentLessons", "Student_StudentId", "dbo.Students");
            DropIndex("dbo.StudentLessons", new[] { "Lesson_LessonId" });
            DropIndex("dbo.StudentLessons", new[] { "Student_StudentId" });
            DropTable("dbo.StudentLessons");
            DropTable("dbo.Lessons");
        }
    }
}
