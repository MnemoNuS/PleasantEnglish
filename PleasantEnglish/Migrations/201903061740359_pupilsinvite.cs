namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pupilsinvite : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Invites", "User_Id", "dbo.User");
            DropForeignKey("dbo.PupilCollections", "PupilId", "dbo.Pupils");
            DropForeignKey("dbo.PupilSchoolworks", "PupilId", "dbo.Pupils");
            DropIndex("dbo.Invites", new[] { "User_Id" });
            DropIndex("dbo.PupilCollections", new[] { "PupilId" });
            DropIndex("dbo.PupilSchoolworks", new[] { "PupilId" });
            DropPrimaryKey("dbo.Pupils");
            AddColumn("dbo.Invites", "PupilId", c => c.Int(nullable: false));
            AddColumn("dbo.Invites", "Pupil_PupilId", c => c.Int());
            AddColumn("dbo.Invites", "Pupil_UserId", c => c.Int());
            AddColumn("dbo.PupilCollections", "Pupil_PupilId", c => c.Int());
            AddColumn("dbo.PupilCollections", "Pupil_UserId", c => c.Int());
            AddColumn("dbo.PupilSchoolworks", "Pupil_PupilId", c => c.Int());
            AddColumn("dbo.PupilSchoolworks", "Pupil_UserId", c => c.Int());
            AlterColumn("dbo.Pupils", "PupilId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Pupils", new[] { "PupilId", "UserId" });
            CreateIndex("dbo.Invites", new[] { "Pupil_PupilId", "Pupil_UserId" });
            CreateIndex("dbo.PupilCollections", new[] { "Pupil_PupilId", "Pupil_UserId" });
            CreateIndex("dbo.PupilSchoolworks", new[] { "Pupil_PupilId", "Pupil_UserId" });
            AddForeignKey("dbo.Invites", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils", new[] { "PupilId", "UserId" });
            AddForeignKey("dbo.PupilCollections", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils", new[] { "PupilId", "UserId" });
            AddForeignKey("dbo.PupilSchoolworks", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils", new[] { "PupilId", "UserId" });
            DropColumn("dbo.Invites", "UserId");
            DropColumn("dbo.Invites", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Invites", "User_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Invites", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.PupilSchoolworks", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils");
            DropForeignKey("dbo.PupilCollections", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils");
            DropForeignKey("dbo.Invites", new[] { "Pupil_PupilId", "Pupil_UserId" }, "dbo.Pupils");
            DropIndex("dbo.PupilSchoolworks", new[] { "Pupil_PupilId", "Pupil_UserId" });
            DropIndex("dbo.PupilCollections", new[] { "Pupil_PupilId", "Pupil_UserId" });
            DropIndex("dbo.Invites", new[] { "Pupil_PupilId", "Pupil_UserId" });
            DropPrimaryKey("dbo.Pupils");
            AlterColumn("dbo.Pupils", "PupilId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.PupilSchoolworks", "Pupil_UserId");
            DropColumn("dbo.PupilSchoolworks", "Pupil_PupilId");
            DropColumn("dbo.PupilCollections", "Pupil_UserId");
            DropColumn("dbo.PupilCollections", "Pupil_PupilId");
            DropColumn("dbo.Invites", "Pupil_UserId");
            DropColumn("dbo.Invites", "Pupil_PupilId");
            DropColumn("dbo.Invites", "PupilId");
            AddPrimaryKey("dbo.Pupils", "PupilId");
            CreateIndex("dbo.PupilSchoolworks", "PupilId");
            CreateIndex("dbo.PupilCollections", "PupilId");
            CreateIndex("dbo.Invites", "User_Id");
            AddForeignKey("dbo.PupilSchoolworks", "PupilId", "dbo.Pupils", "PupilId", cascadeDelete: true);
            AddForeignKey("dbo.PupilCollections", "PupilId", "dbo.Pupils", "PupilId", cascadeDelete: true);
            AddForeignKey("dbo.Invites", "User_Id", "dbo.User", "Id");
        }
    }
}
