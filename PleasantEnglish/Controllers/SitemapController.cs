using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PleasantEnglish.Models;
using SimpleMvcSitemap;

namespace PleasantEnglish.Controllers
{
	public class SitemapController : Controller
	{
		internal SiteContext db = new SiteContext();

		// GET: Sitemap
		public ActionResult Index()
		{


			List<SitemapNode> nodes = new List<SitemapNode>
			{
				new SitemapNode( Url.Action( "Index", "Home" ) ),
				new SitemapNode( Url.Action( "Contact", "Home" ) ),
				new SitemapNode( Url.Action( "About", "Home" ) ),
				new SitemapNode( Url.Action( "Cookies", "Home" ) ),
				new SitemapNode( Url.Action( "Information", "Home" ) ),
				new SitemapNode( Url.Action( "PrivacyPolicy", "Home" ) ),
				new SitemapNode( Url.Action( "ArticlesList", "Blog" ) ),
			};

			var articles = db.Articles.Where(a => !a.Hide).ToList().OrderBy(a => a.DateCreated).Reverse().ToList();
			if( articles.Count != 0 )
			{
				foreach( var article in articles )
				{
					nodes.Add(new SitemapNode(Url.Action("Index", "Article", new {id = article.ArticleId})));
				}
			}

			return new SitemapProvider().CreateSitemap( new SitemapModel( nodes ) );
		}
	}
}