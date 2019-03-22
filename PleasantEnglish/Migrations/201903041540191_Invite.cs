namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invite : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invites",
                c => new
                    {
                        InviteId = c.Int(nullable: false, identity: true),
                        RoomDataId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ConnectionLink = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.InviteId)
                .ForeignKey("dbo.RoomDatas", t => t.RoomDataId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.RoomDataId)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invites", "User_Id", "dbo.User");
            DropForeignKey("dbo.Invites", "RoomDataId", "dbo.RoomDatas");
            DropIndex("dbo.Invites", new[] { "User_Id" });
            DropIndex("dbo.Invites", new[] { "RoomDataId" });
            DropTable("dbo.Invites");
        }
    }
}
