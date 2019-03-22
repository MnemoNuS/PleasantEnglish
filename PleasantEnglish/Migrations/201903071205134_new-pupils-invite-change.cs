namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newpupilsinvitechange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PupilInvites", "InviteId", "dbo.Invites");
            DropIndex("dbo.PupilInvites", new[] { "InviteId" });
            DropIndex("dbo.PupilInvites", new[] { "Pupil_PupilId", "Pupil_UserId" });
            DropPrimaryKey("dbo.Invites");
            AddPrimaryKey("dbo.Invites", new[] { "RoomDataId", "PupilId" });
            DropColumn("dbo.Invites", "InviteId");
            DropTable("dbo.PupilInvites");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PupilInvites",
                c => new
                    {
                        PupilId = c.Int(nullable: false),
                        InviteId = c.Int(nullable: false),
                        Pupil_PupilId = c.Int(),
                        Pupil_UserId = c.Int(),
                    })
                .PrimaryKey(t => new { t.PupilId, t.InviteId });
            
            AddColumn("dbo.Invites", "InviteId", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Invites");
            AddPrimaryKey("dbo.Invites", "InviteId");
            CreateIndex("dbo.PupilInvites", new[] { "Pupil_PupilId", "Pupil_UserId" });
            CreateIndex("dbo.PupilInvites", "InviteId");
            AddForeignKey("dbo.PupilInvites", "InviteId", "dbo.Invites", "InviteId", cascadeDelete: true);
        }
    }
}
