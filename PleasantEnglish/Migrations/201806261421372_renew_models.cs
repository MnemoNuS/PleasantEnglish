namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renew_models : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Comments");
            CreateTable(
                "dbo.ArticleImages",
                c => new
                    {
                        ArticleId = c.Int(nullable: false),
                        ImageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ArticleId, t.ImageId })
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Images", t => t.ImageId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.ImageId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false, identity: true),
                        UserImg = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(maxLength: 500),
                        SecurityStamp = c.String(maxLength: 500),
                        PhoneNumber = c.String(maxLength: 50),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 7, storeType: "datetime2"),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.IdentityUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(maxLength: 150),
                        ClaimValue = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.IdentityUserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.IdentityUserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ArticleId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.ArticleId })
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.ArticleId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Watches",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        ArticleId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.ArticleId, t.Date })
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.ArticleId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Path = c.String(),
                        Name = c.String(),
                        Extention = c.String(),
                        Sizes = c.String(),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.ImageId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            AddColumn("dbo.Articles", "Image", c => c.String());
            AddColumn("dbo.Articles", "ShowOnMain", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articles", "Hide", c => c.Boolean(nullable: false));
            AddColumn("dbo.Articles", "PhotoPost", c => c.Boolean(nullable: false));
            AddColumn("dbo.Comments", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "ArticleId", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "Date", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Comments", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Comments", "CommentId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Comments", new[] { "CommentId", "UserId", "ArticleId" });
            CreateIndex("dbo.Comments", "ArticleId");
            CreateIndex("dbo.Comments", "User_Id");
            AddForeignKey("dbo.Comments", "ArticleId", "dbo.Articles", "ArticleId", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "User_Id", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.ArticleImages", "ImageId", "dbo.Images");
            DropForeignKey("dbo.ArticleImages", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Watches", "User_Id", "dbo.User");
            DropForeignKey("dbo.Watches", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Likes", "User_Id", "dbo.User");
            DropForeignKey("dbo.Likes", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Comments", "User_Id", "dbo.User");
            DropForeignKey("dbo.IdentityUserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.IdentityUserLogin", "UserId", "dbo.User");
            DropForeignKey("dbo.IdentityUserClaim", "UserId", "dbo.User");
            DropForeignKey("dbo.Comments", "ArticleId", "dbo.Articles");
            DropIndex("dbo.Role", "RoleNameIndex");
            DropIndex("dbo.Watches", new[] { "User_Id" });
            DropIndex("dbo.Watches", new[] { "ArticleId" });
            DropIndex("dbo.Likes", new[] { "User_Id" });
            DropIndex("dbo.Likes", new[] { "ArticleId" });
            DropIndex("dbo.IdentityUserRole", new[] { "RoleId" });
            DropIndex("dbo.IdentityUserRole", new[] { "UserId" });
            DropIndex("dbo.IdentityUserLogin", new[] { "UserId" });
            DropIndex("dbo.IdentityUserClaim", new[] { "UserId" });
            DropIndex("dbo.User", "UserNameIndex");
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropIndex("dbo.Comments", new[] { "ArticleId" });
            DropIndex("dbo.ArticleImages", new[] { "ImageId" });
            DropIndex("dbo.ArticleImages", new[] { "ArticleId" });
            DropPrimaryKey("dbo.Comments");
            AlterColumn("dbo.Comments", "CommentId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Comments", "User_Id");
            DropColumn("dbo.Comments", "Date");
            DropColumn("dbo.Comments", "ArticleId");
            DropColumn("dbo.Comments", "UserId");
            DropColumn("dbo.Articles", "PhotoPost");
            DropColumn("dbo.Articles", "Hide");
            DropColumn("dbo.Articles", "ShowOnMain");
            DropColumn("dbo.Articles", "Image");
            DropTable("dbo.Role");
            DropTable("dbo.Images");
            DropTable("dbo.Watches");
            DropTable("dbo.Likes");
            DropTable("dbo.IdentityUserRole");
            DropTable("dbo.IdentityUserLogin");
            DropTable("dbo.IdentityUserClaim");
            DropTable("dbo.User");
            DropTable("dbo.ArticleImages");
            AddPrimaryKey("dbo.Comments", "CommentId");
        }
    }
}
