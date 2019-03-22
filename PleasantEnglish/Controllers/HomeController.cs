using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PleasantEnglish.Helpers;
using PleasantEnglish.Models;

namespace PleasantEnglish.Controllers
{
	public class HomeController : BaseBlogController
	{
		public ActionResult Index()
		{
			PrepareData();
			var articles = db.Articles.Include(a => a.Category).Include(a=>a.ArticleWatches.Select(i => i.User)).Include(a=>a.ArticleImages.Select(i=>i.Image)).ToList();
			if (!User.IsInRole("Admin"))
				articles = articles.Where(a => !a.Hide).ToList();



			var model = new IndexModel();

			var suggestedBlock = new IndexViewBlockModel
			{
				Title = "Рекомендованные статьи",
				Articles = articles.OrderBy( a => a.DateCreated ).Where( a => a.ShowOnMain ).Reverse().ToList()
			};

			var newArticlesBlock = new IndexViewBlockModel
			{
				Title = "Новые статьи",
				Articles = articles.OrderBy(a => a.DateCreated).Take(6).Reverse().ToList()
			};

			model.Blocks.Add(suggestedBlock);
			model.Blocks.Add(newArticlesBlock);

			var contextUser = AuthHelper.GetUserFromCookie();
			User user = null;

			if( contextUser != null )
			{
				user = db.Users.Include(u=>u.UserWatches.Select(i=>i.Article)).FirstOrDefault( u => u.Email == contextUser.Email );

				if( user != null )
				{

					var lastWathed = user.UserWatches.DistinctBy(t=>t.ArticleId).Take( 6 ).OrderBy(w=>w.Date).Select( w => w.Article ).Reverse().ToList();

					var watchedBlock = new IndexViewBlockModel
					{
						Title = "Просмотренные",
						Articles = lastWathed
					};

					model.Blocks.Add(watchedBlock);
				}
			}

			ViewBag.Articles = articles.OrderBy(a => a.DateEdited).Reverse().ToList();

			return View(model);
		}

		public ActionResult About()
		{
			PrepareData();
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			PrepareData();
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult Cookies()
		{
			PrepareData();
			ViewBag.Message = "Информация о cookies";

			return View();
		}

		public ActionResult PrivacyPolicy()
		{
			PrepareData();
			ViewBag.Message = "Политика конфиденциальности";
			return View();
		}

		public ActionResult Information()
		{
			PrepareData();
			ViewBag.Message = "Информационные статьи";
			return View();
		}
	}

	public class IndexModel
	{
		public List<IndexViewBlockModel> Blocks { get; set; }

		public IndexModel()
		{
			Blocks = new List<IndexViewBlockModel>();
		}
	}
	public class IndexViewBlockModel
	{
		public string Title { get; set; }
		public List<Models.Article> Articles { get; set; }

		public IndexViewBlockModel()
		{
			Articles = new List<Article>();
		}
	}
}