namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoomData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoomDatas",
                c => new
                    {
                        RoomDataId = c.Int(nullable: false, identity: true),
                        RoomId = c.String(),
                        IsClosed = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(precision: 7, storeType: "datetime2"),
                        DateUpdated = c.DateTime(precision: 7, storeType: "datetime2"),
                        Data = c.String(),
                        ConnectionLinks = c.String(),
                    })
                .PrimaryKey(t => t.RoomDataId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RoomDatas");
        }
    }
}
