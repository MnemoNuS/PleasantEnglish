using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PleasantEnglish
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.LowercaseUrls = true;

			routes.MapRoute(
				name: "Information",
				url: "information",
				defaults: new { controller = "Home", action = "Information" }
			);
			routes.MapRoute(
				name: "PrivacyPolicy",
				url: "privacy-policy",
				defaults: new { controller = "Home", action = "PrivacyPolicy" }
			);

			routes.MapRoute(
				name: "Cookies",
				url: "cookies",
				defaults: new { controller = "Home", action = "Cookies"}
			);

			routes.MapRoute(
				name: "About",
				url: "about",
				defaults: new { controller = "Home", action = "About"}
			);

			routes.MapRoute(
				name: "Contacts",
				url: "contacts",
				defaults: new { controller = "Home", action = "Contact"}
			);

			routes.MapRoute(
				name: "Blog",
				url: "Blog/{id}",
				defaults: new { controller = "Blog", action = "ArticlesList", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "LikeArticle",
				url: "Article/Like/{id}",
				defaults: new { controller = "Article", action = "Like", id = UrlParameter.Optional }
			);
			routes.MapRoute(
				name: "CommentArticle",
				url: "Article/Comment/{id}",
				defaults: new { controller = "Article", action = "Comment", id = UrlParameter.Optional }
			);
			routes.MapRoute(
				name: "DeleteCommentArticle",
				url: "Article/DeleteComment/{id}",
				defaults: new { controller = "Article", action = "DeleteComment", id = UrlParameter.Optional }
			);
			routes.MapRoute(
				name: "Article",
				url: "Article/{id}",
				defaults: new { controller = "Article", action = "Index", id = UrlParameter.Optional }
			);

			//routes.MapRoute(
			//	name: "DefaultNoAction",
			//	url: "{controller}/{id}",
			//	defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			//);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);


		}
	}
}
