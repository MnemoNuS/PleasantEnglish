using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using PleasantEnglish.Models;

namespace PleasantEnglish.Controllers
{
	public class BaseBlogController : BaseController
	{
		internal List<Category> Categories = new List<Category>();
		internal List<Tag> Tags = new List<Tag>();

		public void PrepareData()
		{
			FillCategories();
			FillTags();
			GetBestPosts();
		}

		public void GetBestPosts()
		{
			var posts = db.Articles.Include( a => a.Category ).Where( a => a.ArticleLikes.Count > 0 ).ToList()
				.OrderBy( a => a.ArticleLikes.Count ).ThenBy( c => c.ArticleWatches.Count ).Reverse().ToList();
			var bestPosts = new List<Article>();
			int maxarticlesCount = 5;
			int articlesCount = posts.Count >= maxarticlesCount ? maxarticlesCount : posts.Count;

			for ( var index = 0; index < articlesCount; index++ )
			{
				var article = posts[index];
				bestPosts.Add( article );
			}
			ViewBag.BestPosts = bestPosts;
		}

		public void FillCategories()
		{
			Categories = db.Categories.Include( a => a.Articles ).ToList();
			ViewBag.Categories = Categories;
		}

		public void FillTags()
		{
			Tags = db.Tags.Include( a => a.Articles ).Where(t=>t.Articles.Count>0 ).ToList();
			ViewBag.Tags = Tags;
		}

	}
}