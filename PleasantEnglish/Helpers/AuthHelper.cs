using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using Microsoft.Owin.Security.Cookies;
using PleasantEnglish.Models;

namespace PleasantEnglish.Helpers
{
	public class AuthHelper
	{

		public static void SetUserCookie(User user)
		{
			if (user != null)
			{
				CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
				serializeModel.UserId = user.UserId;
				serializeModel.Img = user.UserImg;
				serializeModel.Name = user.Name;
				serializeModel.Email = user.Email;

				JavaScriptSerializer serializer = new JavaScriptSerializer();

				string userData = EncryptionHelper.Encrypt(serializer.Serialize(serializeModel));

				HttpCookie cookie = new HttpCookie(WebConfigurationManager.AppSettings["authCookie"], userData)
				{
					Name = WebConfigurationManager.AppSettings["authCookie"],
					Expires = DateTime.Now.AddYears(1),
				};
				HttpContext.Current.Response.SetCookie(cookie);
			}
		}

		public static CustomPrincipal GetUserFromCookie()
		{
			HttpCookie authCookie = HttpContext.Current.Request.Cookies[WebConfigurationManager.AppSettings["authCookie"]];

			if (authCookie != null)
			{
				JavaScriptSerializer serializer = new JavaScriptSerializer();
				if (authCookie.Value != null)
				{
					CustomPrincipalSerializeModel serializeModel =
						serializer.Deserialize<CustomPrincipalSerializeModel>(EncryptionHelper.Decrypt(authCookie.Value));

					CustomPrincipal currentUser = new CustomPrincipal(serializeModel.Email)
					{
						UserId = serializeModel.UserId,
						Img = serializeModel.Img,
						Name = serializeModel.Name,
						Email = serializeModel.Email
					};
					return currentUser;
				}
			}
			return null;
		}

		public static void SetUserFromCookie()
		{
			HttpCookie authCookie = HttpContext.Current.Request.Cookies[WebConfigurationManager.AppSettings["authCookie"]];

			if (authCookie != null)
			{
				JavaScriptSerializer serializer = new JavaScriptSerializer();
				if (authCookie.Value != null)
				{
					CustomPrincipalSerializeModel serializeModel =
						serializer.Deserialize<CustomPrincipalSerializeModel>(EncryptionHelper.Decrypt(authCookie.Value));

					CustomPrincipal currentUser = new CustomPrincipal(serializeModel.Email)
					{
						UserId = serializeModel.UserId,
						Img = serializeModel.Img,
						Name = serializeModel.Name,
						Email = serializeModel.Email
					};
					HttpContext.Current.User = currentUser;
				}
			}
		}

		public static void SignOut ()
		{

			HttpCookie authCookie = HttpContext.Current.Request.Cookies[WebConfigurationManager.AppSettings["authCookie"]];

			if (authCookie != null)
			{
				authCookie.Expires = DateTime.Now.AddYears(-10);

				HttpContext.Current.Request.Cookies.Set(authCookie);
				HttpContext.Current.Response.Cookies.Set(authCookie);
			}
		}

		public static List<User> GetUsersInRole(string role)
		{
			var db = new SiteContext();
			var roles = db.Roles.ToList();
			var roleId = roles.FirstOrDefault(r => r.Name.ToLower() == role.ToLower())?.Id;

			var users =
				db.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(roleId)).ToList();
			return users;
		}

	}
}