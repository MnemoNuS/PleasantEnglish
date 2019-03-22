namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chatmodelchanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chats", "ChatInfo_TempData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chats", "ChatInfo_TempData");
        }
    }
}
