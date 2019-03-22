using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PleasantEnglish.Helpers;
using PleasantEnglish.Models;

namespace PleasantEnglish.Controllers
{
	public class BlogController : BaseBlogController
	{
		public BlogController()
		{ }

		// GET: Articles
		[Route( "~/blog" )]
		public ActionResult ArticlesList( int? page, int? onPage, int? category, int? tag , string search)
		{
			PrepareData();

			var articles = db.Articles.Include( a => a.Category ).Include(a => a.ArticleImages).ToList().OrderBy( a => a.DateCreated ).Reverse().ToList();

			if( !User.IsInRole( "Admin" ) )
				articles = articles.Where( a => !a.Hide ).ToList();

			if( category != null )
			{
				ViewBag.Category = base.Categories.FirstOrDefault( c => c.CategoryId == category );
				articles = articles.Where( a => a.CategoryId == category ).ToList();
			}
			if( tag != null )
			{
				ViewBag.Tag = base.Tags.FirstOrDefault( c => c.TagId == tag );
				articles = articles.Where( a => a.ArticleTags.Any( at => at.TagId == tag ) ).ToList();
			}
			if ( !string.IsNullOrEmpty( search ) )
			{
				ViewBag.Search = search;
				articles = articles.Where( a => a.Text.ToLower().Contains(search.ToLower()) || a.Title.ToLower().Contains(search.ToLower())).ToList();
			}

			var articlesOnPage = onPage ?? 10;
			var pages = (int) ( articles.Count() / articlesOnPage ) + 1;
			var currentPage = page == null ? 1 : page <= pages ? page : pages;
			var from = articlesOnPage * ( currentPage - 1 );
			var left = articles.Count - from;
			var showOnPage = left < articlesOnPage ? left : articlesOnPage;
			List<Article> pageArticles = articles.GetRange( from.Value, showOnPage.Value );


			ViewBag.OnPage = articlesOnPage;
			ViewBag.CurrentPage = currentPage;
			ViewBag.Pages = pages;
			ViewBag.TotalArticles = articles.Count();

			return View(
				pageArticles
			);
		}
	}
}
