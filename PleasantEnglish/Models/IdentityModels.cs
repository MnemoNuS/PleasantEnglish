using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PleasantEnglish.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
 //   public class ApplicationUser : IdentityUser
 //   {
 //       public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
 //       {
 //           // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
 //           var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
 //           // Add custom user claims here
 //           return userIdentity; 
	//	}
 //   }

 //   public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
 //   {
 //       public ApplicationDbContext()
 //           : base("db")
 //       {
 //       }

 //       public static ApplicationDbContext Create()
 //       {
 //           return new ApplicationDbContext();
 //       }
 //   }

	//public class PEInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
	//{

	//	protected override void Seed(ApplicationDbContext context )
	//	{
	//		// Initialize default identity roles
	//		var roleStore = new RoleStore<IdentityRole>( context );
	//		var roleManager = new RoleManager<IdentityRole>(roleStore);
	//		// RoleTypes is a class containing constant string values for different roles
	//		List<IdentityRole> identityRoles = new List<IdentityRole>();
	//		identityRoles.Add( new IdentityRole() { Name = "Admin"} );
	//		identityRoles.Add( new IdentityRole() { Name = "User"} );

	//		foreach( IdentityRole role in identityRoles )
	//		{
	//			roleManager.Create( role );
	//		}

	//		// Initialize default user
	//		var userStore = new UserStore<ApplicationUser>( context );
	//		var userManager = new UserManager<ApplicationUser>(userStore);
	//		ApplicationUser admin = new ApplicationUser();
	//		admin.Email = "notformail@yandex.ru";
	//		admin.UserName = "MnemoN";

	//		userManager.Create( admin, "1MnemoN!" );
	//		userManager.AddToRole( admin.Id, "Admin" );

	//		var userStoreT = new UserStore<ApplicationUser>(context);
	//		var userManagerT = new UserManager<ApplicationUser>(userStoreT);

	//		ApplicationUser pleasantTeacher = new ApplicationUser();
	//		pleasantTeacher.Email = "victoryaspace@gmail.com";
	//		pleasantTeacher.UserName = "Pleasant teacher";

	//		userManagerT.Create(pleasantTeacher, "1Teacher!" );
	//		userManagerT.AddToRole(pleasantTeacher.Id, "Admin" );

	//		// Add code to initialize context tables

	//		base.Seed( context );
	//	}
	//}

}