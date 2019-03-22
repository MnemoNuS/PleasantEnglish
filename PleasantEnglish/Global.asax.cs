using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PleasantEnglish.Controllers;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PleasantEnglish.Helpers;
using PleasantEnglish.Models;
using Telegram.Bot.Types;


namespace PleasantEnglish
{
	public class MvcApplication : System.Web.HttpApplication
	{

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion< SiteContext, Migrations.Configuration>());
		}

		protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
		{

			try
			{
			AuthHelper.SetUserFromCookie();
			}
			catch (Exception)
			{
				//somehting went wrong
			}

		}

		protected void Application_OnEndRequest(object sender, EventArgs e)
		{

		}

	}
}
