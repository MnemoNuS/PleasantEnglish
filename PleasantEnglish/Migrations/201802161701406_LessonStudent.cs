namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LessonStudent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentLessons", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.StudentLessons", "Lesson_LessonId", "dbo.Lessons");
            DropIndex("dbo.StudentLessons", new[] { "Student_StudentId" });
            DropIndex("dbo.StudentLessons", new[] { "Lesson_LessonId" });
            CreateTable(
                "dbo.LessonStudents",
                c => new
                    {
                        LessonId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LessonId, t.StudentId })
                .ForeignKey("dbo.Lessons", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.LessonId, cascadeDelete: true)
                .Index(t => t.LessonId)
                .Index(t => t.StudentId);
            
            AlterColumn("dbo.Lessons", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Lessons", "TimeStart", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Lessons", "TimeEnd", c => c.DateTime(nullable: false));
            DropTable("dbo.StudentLessons");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StudentLessons",
                c => new
                    {
                        Student_StudentId = c.Int(nullable: false),
                        Lesson_LessonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_StudentId, t.Lesson_LessonId });
            
            DropForeignKey("dbo.LessonStudents", "LessonId", "dbo.Students");
            DropForeignKey("dbo.LessonStudents", "StudentId", "dbo.Lessons");
            DropIndex("dbo.LessonStudents", new[] { "StudentId" });
            DropIndex("dbo.LessonStudents", new[] { "LessonId" });
            AlterColumn("dbo.Lessons", "TimeEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Lessons", "TimeStart", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Lessons", "Date", c => c.DateTime(nullable: false));
            DropTable("dbo.LessonStudents");
            CreateIndex("dbo.StudentLessons", "Lesson_LessonId");
            CreateIndex("dbo.StudentLessons", "Student_StudentId");
            AddForeignKey("dbo.StudentLessons", "Lesson_LessonId", "dbo.Lessons", "LessonId", cascadeDelete: true);
            AddForeignKey("dbo.StudentLessons", "Student_StudentId", "dbo.Students", "StudentId", cascadeDelete: true);
        }
    }
}
