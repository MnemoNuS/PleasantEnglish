namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstmigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chats", "ChatInfo_ChatId", c => c.Long(nullable: false));
            AddColumn("dbo.Chats", "ChatInfo_MessageId", c => c.Int(nullable: false));
            AddColumn("dbo.Chats", "ChatInfo_Action", c => c.String());
            AddColumn("dbo.Chats", "ChatInfo_Step", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chats", "ChatInfo_Step");
            DropColumn("dbo.Chats", "ChatInfo_Action");
            DropColumn("dbo.Chats", "ChatInfo_MessageId");
            DropColumn("dbo.Chats", "ChatInfo_ChatId");
        }
    }
}
