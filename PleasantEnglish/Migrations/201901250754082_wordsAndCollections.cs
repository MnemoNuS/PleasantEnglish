namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wordsAndCollections : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Collections",
                c => new
                    {
                        CollectionId = c.Int(nullable: false, identity: true),
                        ValueEn = c.String(),
                        ValueRu = c.String(),
                        Level = c.Int(nullable: false),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.CollectionId);
            
            CreateTable(
                "dbo.WordCollections",
                c => new
                    {
                        WordId = c.Int(nullable: false),
                        CollectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WordId, t.CollectionId })
                .ForeignKey("dbo.Collections", t => t.CollectionId, cascadeDelete: true)
                .ForeignKey("dbo.Words", t => t.WordId, cascadeDelete: true)
                .Index(t => t.WordId)
                .Index(t => t.CollectionId);
            
            CreateTable(
                "dbo.Words",
                c => new
                    {
                        WordId = c.Int(nullable: false, identity: true),
                        ValueEn = c.String(),
                        ValueRu = c.String(),
                        Transcription = c.String(),
                        Level = c.Int(nullable: false),
                        PartOfSpeach = c.Int(nullable: false),
                        Pronunciation = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.WordId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WordCollections", "WordId", "dbo.Words");
            DropForeignKey("dbo.WordCollections", "CollectionId", "dbo.Collections");
            DropIndex("dbo.WordCollections", new[] { "CollectionId" });
            DropIndex("dbo.WordCollections", new[] { "WordId" });
            DropTable("dbo.Words");
            DropTable("dbo.WordCollections");
            DropTable("dbo.Collections");
        }
    }
}
