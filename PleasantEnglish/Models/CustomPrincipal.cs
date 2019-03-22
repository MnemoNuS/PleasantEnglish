using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using PleasantEnglish.Helpers;

namespace PleasantEnglish.Models
{

	interface ICustomPrincipal : IPrincipal
	{
		int UserId { get; set; }
		string Email { get; set; }
		string Name { get; set; }
		string Img { get; set; }
	}

	public class CustomPrincipal : ICustomPrincipal
	{
		internal SiteContext db = new SiteContext();
		public IIdentity Identity { get; private set; }
		public bool IsInRole(string role)
		{
			var roleStore = new RoleStore<IdentityRole>(db);
			var roleManager = new RoleManager<IdentityRole>(roleStore);
			var userId = Identity.GetUserId();

			if (roleManager.Roles.Where(n => n.Users.Any(u => u.UserId == userId)).Any(m => m.Name.ToLower().Equals(role.ToLower())))
			{
				return true;
			}
			return false;
		}

		private CustomPrincipal()
		{
		}

		public CustomPrincipal(IIdentity identity)
		{
			this.Identity = identity;
		}

		public CustomPrincipal(string email)
		{
			var uS = new UserStore<User>(db);
			var uM = new UserManager<User>(uS);
			var user = uM.FindByEmail(email);
			this.Identity = uM.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
		}

		public int UserId { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string Img { get; set; }

	}

	public class CustomPrincipalSerializeModel
	{
		public int UserId { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string Img { get; set; }
	}

	public abstract class BaseViewPage : WebViewPage
	{
		public CustomPrincipal CurrentUser => AuthHelper.GetUserFromCookie();
	}

	public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
	{
		public CustomPrincipal CurrentUser => AuthHelper.GetUserFromCookie();
	}

}