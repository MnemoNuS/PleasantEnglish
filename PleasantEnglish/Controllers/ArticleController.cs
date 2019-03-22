using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using PleasantEnglish.Helpers;
using PleasantEnglish.Models;

namespace PleasantEnglish.Controllers
{
	public class ArticleController : BaseBlogController
	{

		private SiteContext db = new SiteContext();

		// GET: Article
		[RememberLast]
		[Route("article/{id:int, category:int?, tag:int?}")]
		public ActionResult Index(int id, int? category, int? tag)
		{
			PrepareData();

			// GET: Articles
			var articles = db.Articles.Include(a => a.Category).Include(i => i.ArticleImages).Include(c=>c.ArticleComments.Select(ac=>ac.User)).ToList();

			if (category != null)
			{
				ViewBag.Category = base.Categories.FirstOrDefault(c => c.CategoryId == category);
				articles = articles.Where(a => a.CategoryId == category).ToList();
			}

			if (tag != null)
			{
				ViewBag.Tag = base.Tags.FirstOrDefault(c => c.TagId == tag);
				articles = articles.Where(a => a.ArticleTags.Any(at => at.TagId == tag)).ToList();
			}

			var article = articles.FirstOrDefault(a => a.ArticleId == id);

			if (article != null)
			{
				if (Request.Cookies[".PEVISITED"] != null)
				{
					var myCookie = Request.Cookies[".PEVISITED"];
					var cookieCollection = myCookie.Values;
					string lastUrl = System.Web.HttpUtility.UrlDecode(cookieCollection["lastPage"]);
					string currentUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;

					if (lastUrl != currentUrl && !Request.Browser.Crawler)
					{
						var contextUser = AuthHelper.GetUserFromCookie();
						User user = null;
						if (contextUser != null)
						{
							user = db.Users.FirstOrDefault(u => u.Email == contextUser.Email);
						}
						else
						{
							
							user = db.Users.FirstOrDefault(u => u.Name.Equals("Nobody"));
						}

						if( !User.IsInRole( "admin" ))
						{
							var watch = new Watch();
							watch.User = null;
							watch.UserId = null;
							if( user != null )
							{
								watch.UserId = user.UserId;
								watch.User = user;
							}
							watch.Article = article;
							watch.ArticleId = article.ArticleId;
							watch.Date = DateTime.Now;
							db.Watches.Add( watch );
							db.SaveChanges();
						}
					}
				}
			}
			var articlesCount = articles.Count();

			if (articlesCount > 1)
			{
				var index = articles.IndexOf(article);

				var nextIndex = index == articlesCount - 1 ? 0 : index + 1;
				ViewBag.NextArticle = articles[nextIndex];

				if (articlesCount > 2)
				{
					var previousIndex = index == 0 ? articlesCount - 1 : index - 1;
					ViewBag.PreviousArticle = articles[previousIndex];
				}
			}
			var articleImages = db.ArticleImages.Where(i => i.ArticleId == article.ArticleId).Select(a => a.Image).ToList();
			ViewBag.ArticleImages = articleImages;

			var comments = db.Comments.Where(i => i.ArticleId == article.ArticleId).Include(c=>c.User).OrderBy(c=>c.Date).ToList();
			ViewBag.Comments = comments;

			ViewBag.TotalArticles = articles.Count();

			return View(article);
		}

		[Route("article/like/{articleId:int}")]
		public ActionResult Like(int articleId)
		{

			var userId = ( User as CustomPrincipal )?.UserId;
			if( userId == null )
				return Json( false, JsonRequestBehavior.AllowGet );

			var likes = db.Likes.ToList();
			var like = likes.FirstOrDefault(a => a.UserId == userId && a.ArticleId == articleId);

			if (like != null)
			{
				db.Likes.Remove(like);
				db.SaveChanges();
				return Json(true, JsonRequestBehavior.AllowGet);
			}

			var article = db.Articles.Include(a => a.Category).FirstOrDefault(a => a.ArticleId == articleId);
			var user = db.Users.FirstOrDefault(u => u.UserId == userId);

			if (user != null && article != null)
			{
				like = new Like();
				like.UserId = userId;
				like.User = user;
				like.Article = article;
				like.ArticleId = articleId;
				db.Likes.Add(like);
				db.SaveChanges();
				return Json(true, JsonRequestBehavior.AllowGet);
			}

			return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
		}

		[ValidateInput(false)]
		public ActionResult Comment(CommentViewModel model)
		{
			var userId = (User as CustomPrincipal)?.UserId;

			if (userId == null)
				return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

			var user = db.Users.FirstOrDefault(u => u.UserId == userId);

			var article = db.Articles.Include(a => a.Category).FirstOrDefault(a => a.ArticleId == model.ArticleId);

			if (user != null && article != null && !model.Text.IsNullOrWhiteSpace())
			{
				try
				{
					var comment = new Comment();
					comment.UserId = user.UserId;
					comment.User = user;
					comment.Article = article;
					comment.ArticleId = model.ArticleId;
					comment.Date = DateTime.Now;
					comment.Text = model.Text;
					db.Comments.Add(comment);
					db.SaveChanges();

					MailHelper.CommentMotification(comment);
				}
				catch (Exception e)
				{
					return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
				}


				return Json(true, JsonRequestBehavior.AllowGet);
			}

			return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
		}

		[Route("article/deleteComment/commentId:int}")]
		public ActionResult DeleteComment(int commentId)
		{
			var userId = (User as CustomPrincipal)?.UserId;

			if (userId == null)
				return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

			var user = db.Users.FirstOrDefault(u => u.UserId == userId);

			var comments = db.Comments.ToList();
			var comment = comments.FirstOrDefault(a => a.UserId == userId && a.ArticleId == commentId);

			if (comment != null)
			{
				if (comment.UserId != userId && !User.IsInRole("admin"))
					return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

				db.Comments.Remove(comment);
				db.SaveChanges();
				return Json(true, JsonRequestBehavior.AllowGet);
			}

			return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
		}

	}

	public class RememberLast : FilterAttribute, IActionFilter
	{

		public RememberLast()
		{
		}


		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
			string url = HttpContext.Current.Request.Url.AbsoluteUri;
			string title = "lastPage";
			
			if (!url.Contains("browserLink"))
				if (filterContext.HttpContext.Request.Cookies[".PEVISITED"] == null)
				{
					HttpCookie myCookie = new HttpCookie(".PEVISITED");
					myCookie.Expires = DateTime.Now.AddYears(1);
					myCookie.Values[title] = System.Web.HttpUtility.UrlEncode(url);
					filterContext.HttpContext.Response.Cookies.Add(myCookie);
				}
				else
				{
					var myCookie = filterContext.HttpContext.Request.Cookies[".PEVISITED"];
					myCookie.Expires = DateTime.Now.AddYears(1);
					var cookieCollection = myCookie.Values;
					string[] cookieTitles = cookieCollection.AllKeys;
					cookieCollection.Set(title, System.Web.HttpUtility.UrlEncode(url));
					if (cookieCollection.Count > 15) // store just 15 item         
						cookieCollection.Remove(cookieTitles[0]);
					filterContext.HttpContext.Response.SetCookie(myCookie);
				}
		}
	}

	public class CommentViewModel
	{
		public int ArticleId { get; set; }
		public string Text { get; set; }
	}

}