namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newpupilsinvite : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => new { t.PupilId, t.InviteId })
                .ForeignKey("dbo.Invites", t => t.InviteId, cascadeDelete: true)
                .ForeignKey("dbo.Pupils", t => new { t.Pupil_PupilId, t.Pupil_UserId })
                .Index(t => t.InviteId)
                .Index(t => new { t.Pupil_PupilId, t.Pupil_UserId });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PupilInvites", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils");
            DropForeignKey("dbo.PupilInvites", "InviteId", "dbo.Invites");
            DropIndex("dbo.PupilInvites", new[] { "Pupil_PupilId", "Pupil_UserId" });
            DropIndex("dbo.PupilInvites", new[] { "InviteId" });
            DropTable("dbo.PupilInvites");
        }
    }
}
