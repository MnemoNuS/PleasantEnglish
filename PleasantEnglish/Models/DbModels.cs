using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using PleasantEnglish.Migrations;
using PleasantEnglish.Models;

namespace PleasantEnglish.Models
{
	public class SiteContext : IdentityDbContext<User>
	{
		public SiteContext()
			: base("db")
		{
		}

		public static SiteContext Create()
		{
			return new SiteContext();
		}
		//dictionary models
		public DbSet<Word> Words { get; set; }
		public DbSet<Collection> Collections { get; set; }
		public DbSet<WordCollection> WordCollections { get; set; }

		//pupils models
		public DbSet<Pupil> Pupils { get; set; }
		public DbSet<PupilCollection> PupilCollections { get; set; }
		public DbSet<Schoolwork> Schoolworks { get; set; }
		public DbSet<PupilSchoolwork> PupilSchoolworks { get; set; }


		//blog models
		public DbSet<Article> Articles { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<ArticleTag> ArticleTags { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DbSet<Watch> Watches { get; set; }
		public DbSet<Image> Images { get; set; }
		public DbSet<ArticleImage> ArticleImages { get; set; }

		//bot models
		public DbSet<Student> Students { get; set; }
		public DbSet<Chat> Chats { get; set; }
		public DbSet<Lesson> Lessons { get; set; }
		public DbSet<LessonStudents> LessonStudents { get; set; }

		//Rooms data
		public DbSet<RoomData> RoomsData { get; set; }
	    public DbSet<Invite> Invites { get; set; }

        //Identity and Authorization
        public DbSet<IdentityUserLogin> UserLogins { get; set; }
		public DbSet<IdentityUserClaim> UserClaims { get; set; }
		public DbSet<IdentityUserRole> UserRoles { get; set; }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configure Asp Net Identity Tables
			modelBuilder.Entity<User>().ToTable("User");
			modelBuilder.Entity<User>().Property(u => u.PasswordHash).HasMaxLength(500);
			modelBuilder.Entity<User>().Property(u => u.SecurityStamp).HasMaxLength(500);
			modelBuilder.Entity<User>().Property(u => u.PhoneNumber).HasMaxLength(50);

			modelBuilder.Entity<IdentityRole>().ToTable("Role");
			modelBuilder.Entity<IdentityUserRole>().ToTable("IdentityUserRole");
			modelBuilder.Entity<IdentityUserLogin>().ToTable("IdentityUserLogin");
			modelBuilder.Entity<IdentityUserClaim>().ToTable("IdentityUserClaim");
			modelBuilder.Entity<IdentityUserClaim>().Property(u => u.ClaimType).HasMaxLength(150);
			modelBuilder.Entity<IdentityUserClaim>().Property(u => u.ClaimValue).HasMaxLength(500);

			modelBuilder.Properties<DateTime>()
				.Configure(c => c.HasColumnType("datetime2"));

		}

	}

}