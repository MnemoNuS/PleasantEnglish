namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pupils : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PupilCollections",
                c => new
                    {
                        PupilId = c.Int(nullable: false),
                        CollectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PupilId, t.CollectionId })
                .ForeignKey("dbo.Collections", t => t.CollectionId, cascadeDelete: true)
                .ForeignKey("dbo.Pupils", t => t.PupilId, cascadeDelete: true)
                .Index(t => t.PupilId)
                .Index(t => t.CollectionId);
            
            CreateTable(
                "dbo.Pupils",
                c => new
                    {
                        PupilId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PupilId)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.PupilSchoolworks",
                c => new
                    {
                        SchoolworkId = c.Int(nullable: false),
                        PupilId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SchoolworkId, t.PupilId })
                .ForeignKey("dbo.Pupils", t => t.PupilId, cascadeDelete: true)
                .ForeignKey("dbo.Schoolworks", t => t.SchoolworkId, cascadeDelete: true)
                .Index(t => t.SchoolworkId)
                .Index(t => t.PupilId);
            
            CreateTable(
                "dbo.Schoolworks",
                c => new
                    {
                        SchoolworkId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(precision: 7, storeType: "datetime2"),
                        TimeStart = c.DateTime(precision: 7, storeType: "datetime2"),
                        TimeEnd = c.DateTime(precision: 7, storeType: "datetime2"),
                        Duration = c.Int(nullable: false),
                        HomeTask = c.String(),
                    })
                .PrimaryKey(t => t.SchoolworkId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pupils", "User_Id", "dbo.User");
            DropForeignKey("dbo.PupilSchoolworks", "SchoolworkId", "dbo.Schoolworks");
            DropForeignKey("dbo.PupilSchoolworks", "PupilId", "dbo.Pupils");
            DropForeignKey("dbo.PupilCollections", "PupilId", "dbo.Pupils");
            DropForeignKey("dbo.PupilCollections", "CollectionId", "dbo.Collections");
            DropIndex("dbo.PupilSchoolworks", new[] { "PupilId" });
            DropIndex("dbo.PupilSchoolworks", new[] { "SchoolworkId" });
            DropIndex("dbo.Pupils", new[] { "User_Id" });
            DropIndex("dbo.PupilCollections", new[] { "CollectionId" });
            DropIndex("dbo.PupilCollections", new[] { "PupilId" });
            DropTable("dbo.Schoolworks");
            DropTable("dbo.PupilSchoolworks");
            DropTable("dbo.Pupils");
            DropTable("dbo.PupilCollections");
        }
    }
}
