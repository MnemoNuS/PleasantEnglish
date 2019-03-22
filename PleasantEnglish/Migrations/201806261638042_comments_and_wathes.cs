namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comments_and_wathes : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Comments");
            AddPrimaryKey("dbo.Comments", new[] { "UserId", "ArticleId", "Date" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Comments");
            AddPrimaryKey("dbo.Comments", new[] { "UserId", "ArticleId" });
        }
    }
}
