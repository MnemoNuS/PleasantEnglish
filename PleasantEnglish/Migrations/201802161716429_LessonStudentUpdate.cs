namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LessonStudentUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Lessons", "Date", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Lessons", "TimeStart", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Lessons", "TimeEnd", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Lessons", "TimeEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Lessons", "TimeStart", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Lessons", "Date", c => c.DateTime(nullable: false));
        }
    }
}
