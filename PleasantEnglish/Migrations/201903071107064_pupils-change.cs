namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pupilschange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PupilCollections", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils");
            DropForeignKey("dbo.PupilSchoolworks", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils");
            DropForeignKey("dbo.Invites", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils");
            DropPrimaryKey("dbo.Pupils");
            AlterColumn("dbo.Pupils", "PupilId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Pupils", new[] { "PupilId", "UserId" });
            AddForeignKey("dbo.PupilCollections", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils", new[] { "PupilId", "UserId" });
            AddForeignKey("dbo.PupilSchoolworks", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils", new[] { "PupilId", "UserId" });
            AddForeignKey("dbo.Invites", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils", new[] { "PupilId", "UserId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invites", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils");
            DropForeignKey("dbo.PupilSchoolworks", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils");
            DropForeignKey("dbo.PupilCollections", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils");
            DropPrimaryKey("dbo.Pupils");
            AlterColumn("dbo.Pupils", "PupilId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Pupils", new[] { "PupilId", "UserId" });
            AddForeignKey("dbo.Invites", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils", new[] { "PupilId", "UserId" });
            AddForeignKey("dbo.PupilSchoolworks", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils", new[] { "PupilId", "UserId" });
            AddForeignKey("dbo.PupilCollections", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils", new[] { "PupilId", "UserId" });
        }
    }
}
