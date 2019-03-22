using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PleasantEnglish.Models;

namespace PleasantEnglish.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PleasantEnglish.Models.SiteContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
	        AutomaticMigrationDataLossAllowed = true;
			ContextKey = "PleasantEnglish.Models.SiteContext";
        }

        protected override void Seed(PleasantEnglish.Models.SiteContext context)
        {
	        // Initialize default identity roles
	        var roleStore = new RoleStore<IdentityRole>(context);
	        var roleManager = new RoleManager<IdentityRole>(roleStore);
	        // RoleTypes is a class containing constant string values for different roles
	        List<IdentityRole> identityRoles = new List<IdentityRole>();
	        identityRoles.Add(new IdentityRole() { Name = "Admin" });
	        identityRoles.Add(new IdentityRole() { Name = "User" });

	        foreach (IdentityRole role in identityRoles)
	        {
		        roleManager.Create(role);
	        }

	        // Initialize default user
	        var userStore = new UserStore<User>(context);
	        var userManager = new UserManager<User>(userStore);

	        User admin = new User(); 
	        admin.Email = "";
	        admin.UserName = "";
	        admin.Name = "";
	        admin. UserImg = @"u1.jpg";

	        var result = userManager.Create(admin, "");
	        if( result.Succeeded )
	        {
		        userManager.AddToRole( admin.Id, "Admin" );
	        }


			User pleasantTeacher = new User();
			pleasantTeacher.Email = "";
			pleasantTeacher.UserName = "";
			pleasantTeacher.Name = "Pleasant_teacher";
			pleasantTeacher.UserImg = @"u1.jpg";

			result = userManager.Create(pleasantTeacher, "");
	        if (result.Succeeded)
	        {
		        userManager.AddToRole(pleasantTeacher.Id, "Admin");
	        }

			User nobody = new User();
	        nobody.Email = "nobody@nowhere.com";
	        nobody.UserName = "nobody@nowhere.com";
	        nobody.Name = "Nobody";
	        nobody.UserImg = @"u1.jpg";

			result = userManager.Create(nobody, "1Nobody!");
	        if (result.Succeeded)
	        {
		        userManager.AddToRole(nobody.Id, "User");
	        }


			// Add code to initialize context tables

			base.Seed(context);
		}
    }
}
