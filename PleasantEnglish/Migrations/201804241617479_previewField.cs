namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class previewField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Preview", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Preview");
        }
    }
}
