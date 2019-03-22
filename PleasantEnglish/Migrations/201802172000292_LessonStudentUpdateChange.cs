namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LessonStudentUpdateChange : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.LessonStudents", name: "StudentId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.LessonStudents", name: "LessonId", newName: "StudentId");
            RenameColumn(table: "dbo.LessonStudents", name: "__mig_tmp__0", newName: "LessonId");
            RenameIndex(table: "dbo.LessonStudents", name: "IX_StudentId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.LessonStudents", name: "IX_LessonId", newName: "IX_StudentId");
            RenameIndex(table: "dbo.LessonStudents", name: "__mig_tmp__0", newName: "IX_LessonId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.LessonStudents", name: "IX_LessonId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.LessonStudents", name: "IX_StudentId", newName: "IX_LessonId");
            RenameIndex(table: "dbo.LessonStudents", name: "__mig_tmp__0", newName: "IX_StudentId");
            RenameColumn(table: "dbo.LessonStudents", name: "LessonId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.LessonStudents", name: "StudentId", newName: "LessonId");
            RenameColumn(table: "dbo.LessonStudents", name: "__mig_tmp__0", newName: "StudentId");
        }
    }
}
