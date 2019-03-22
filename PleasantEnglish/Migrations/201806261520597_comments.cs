namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comments : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Comments");
            AddPrimaryKey("dbo.Comments", new[] { "UserId", "ArticleId" });
            DropColumn("dbo.Comments", "CommentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "CommentId", c => c.Int(nullable: false));
            DropPrimaryKey("dbo.Comments");
            AddPrimaryKey("dbo.Comments", new[] { "CommentId", "UserId", "ArticleId" });
        }
    }
}
