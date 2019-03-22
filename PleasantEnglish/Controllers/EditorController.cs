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
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using Image = PleasantEnglish.Models.Image;
using Tag = PleasantEnglish.Models.Tag;

namespace PleasantEnglish.Controllers
{

	[Authorize(Roles = "admin")]
	public class EditorController : BaseBlogController
	{
		public EditorController()
		{
		}

		// GET: Articles/Create
		public ActionResult CreateNew()
		{
			PrepareData();
			return View();
		}
		// GET: Articles/Create
		public ActionResult CreateArticle()
		{
			PrepareData();
			var newArticle = new ArticleEditModel();
			return View(newArticle);
		}
		// GET: Articles/Create
		public ActionResult CreatePhotoPost()
		{
			PrepareData();
			var newPhotoPost = new ArticleEditModel();
			return View(newPhotoPost);
		}

		// POST: Articles/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult CreatePhotoPost([Bind(Include = "ArticleId,Title,Preview,Category,Tags,ShowOnMain,Hide,PhotoPost,ArticleImages")] ArticleEditModel article)
		{

			PrepareData();

			if (ModelState.IsValid)
			{
				MakeArticle( article );
				return RedirectToAction("ArticlesList", "Blog");
			}
			return View(article);
		}
		
		// POST: Articles/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateInput(false)]
		public ActionResult CreateArticle([Bind(Include = "ArticleId,Title,Text,Preview,Category,Tags,ShowOnMain,Hide,PhotoPost,ArticleImages")] ArticleEditModel article)
		{
			PrepareData();

			if (ModelState.IsValid)
			{
				MakeArticle( article );
				return RedirectToAction("ArticlesList", "Blog");
			}
			return View(article);
		}

		private void MakeArticle( ArticleEditModel article )
		{
			var newArticle = new Article();

			newArticle.Title = article.Title;
			newArticle.Preview = article.Preview;
			newArticle.Text = !string.IsNullOrWhiteSpace(article.Text) ? article.Text : "";
			newArticle.Image = !string.IsNullOrEmpty(article.Image) ? article.Image : "";
			newArticle.ShowOnMain = article.ShowOnMain;
			newArticle.Hide = article.Hide;
			newArticle.PhotoPost = article.PhotoPost;
			var category = db.Categories.FirstOrDefault(c => c.Name == article.Category);
			if (category != null)
			{
				newArticle.CategoryId = category.CategoryId;
				newArticle.Category = category;
			}
			else
			{
				newArticle.Category = new Category { Name = article.Category };
			}


			var tags = string.IsNullOrWhiteSpace(article.Tags) ? new List<string>() : article.Tags.Trim().Replace(Environment.NewLine, "").Split('#').ToList().Select(t => t.Trim().Trim('.').Trim(',').ToLower()).Where(t => !t.IsNullOrWhiteSpace()).ToList();

			if (tags.Any())
			{
				newArticle.ArticleTags = new List<ArticleTag>();

				foreach (var tag in tags)
				{
					var addedTag = db.Tags.FirstOrDefault(t => t.Name.Equals(tag));

					var articleTag = new ArticleTag
					{
						Tag = addedTag ?? new Tag { TagId = -1, Name = tag },
						TagId = -1
					};
					newArticle.ArticleTags.Add(articleTag);
				}
			}
			if (article.ArticleImages != null && article.ArticleImages.Count() > 0)
			{
				newArticle.ArticleImages = new List<ArticleImage>();

				foreach (var articleArticleImage in article.ArticleImages)
				{
					var articleStoredImage = new ArticleImage
					{
						Image = new Image(articleArticleImage),
						ImageId = articleArticleImage.StoredImageId
					};
					newArticle.ArticleImages.Add(articleStoredImage);
				}
				newArticle.Image = "/Content/images/articles/" + article.ArticleImages[0].Name + article.ArticleImages[0].Extention + ".[wide]" + article.ArticleImages[0].Extention + "?timestamp=" + new DateTime().Millisecond;
			}

			db.Articles.Add(newArticle);
			db.SaveChanges();
		}

		// GET: Articles/Edit/5
		public ActionResult EditArticle(int? id)
		{
			PrepareData();

			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Article article = db.Articles.Include(a=>a.ArticleImages).ToList().FirstOrDefault(a=>a.ArticleId==id);
			if (article == null)
			{
				return HttpNotFound();
			}
			ArticleEditModel editArticle = new ArticleEditModel(article);

			return View(editArticle);
		}

		// POST: Articles/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult EditArticle([Bind(Include = "ArticleId,Title,Preview,Text,Category,Tags,ShowOnMain,Hide,PhotoPost,ArticleImages")] ArticleEditModel article)
		{
			PrepareData();


			if (ModelState.IsValid)
			{
				var articles = db.Articles.Include(a => a.Category);
				var editArticle = articles.FirstOrDefault(a => a.ArticleId == article.ArticleId);
				if (editArticle != null)
				{
					editArticle.Title = article.Title;
					editArticle.Preview = article.Preview;
					editArticle.Text = article.Text;
					editArticle.Image = !string.IsNullOrEmpty(article.Image) ? article.Image : "";
					editArticle.ShowOnMain = article.ShowOnMain;
					editArticle.Hide = article.Hide;
					editArticle.PhotoPost = article.PhotoPost;
					var category = db.Categories.FirstOrDefault(c => c.Name == article.Category);
					if (category != null)
					{
						editArticle.CategoryId = category.CategoryId;
						editArticle.Category = category;
					}
					else
					{
						editArticle.Category = new Category { Name = article.Category };
					}


					var tags = string.IsNullOrWhiteSpace(article.Tags) ? new List<string>() : article.Tags.Trim().Replace(Environment.NewLine, "").Split('#').ToList()
						.Select(t => t.Trim().Trim('.').Trim(',').ToLower()).Where(t => !t.IsNullOrWhiteSpace()).ToList();

					if (tags.Any())
					{
						editArticle.ArticleTags = new List<ArticleTag>();

						foreach (var tag in tags)
						{
							var addedTag = db.Tags.FirstOrDefault(t => t.Name.Equals(tag));

							var articleTag = new ArticleTag
							{
								Tag = addedTag ?? new Tag { TagId = -1, Name = tag },
								TagId = -1
							};
							editArticle.ArticleTags.Add(articleTag);
						}
					}
					if (article.ArticleImages != null && article.ArticleImages.Any())
					{
						List<ArticleImage> left = new List<ArticleImage>();
						foreach( var articleArticleImage in editArticle.ArticleImages )
						{
							if(article.ArticleImages.All(a=>a.StoredImageId != articleArticleImage.ImageId))
								left.Add(articleArticleImage);
						}
						if(left.Count>0)
							foreach( var image in left )
							{
								editArticle.ArticleImages.Remove( image );
							}

						foreach (var articleArticleImage in article.ArticleImages)
						{
							var articleStoredImage = editArticle.ArticleImages.FirstOrDefault( i => i.ImageId == articleArticleImage.StoredImageId );

							if( articleStoredImage == null )
							{
								articleStoredImage = new ArticleImage
								{
									Image = new Image( articleArticleImage ),
									ImageId = articleArticleImage.StoredImageId
								};
								editArticle.ArticleImages.Add( articleStoredImage );
							}
						}
						editArticle.Image = "/Content/images/articles/" + article.ArticleImages[0].Name + article.ArticleImages[0].Extention + ".[wide]" + article.ArticleImages[0].Extention + "?timestamp=" + new DateTime().Millisecond;
					}
					db.Entry(editArticle).State = EntityState.Modified;
					db.SaveChanges();

					return RedirectToAction("ArticlesList", "Blog");
				}
			}
			return View(article);
		}

		// GET: Articles/Edit/5
		public ActionResult EditPhotoPost(int? id)
		{
			PrepareData();

			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Article article = db.Articles.Find(id);
			if (article == null)
			{
				return HttpNotFound();
			}
			ArticleEditModel editArticle = new ArticleEditModel(article);

			return View(editArticle);
		}

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult EditPhotoPost([Bind(Include = "ArticleId,Title,Preview,Category,Tags,ShowOnMain,Hide,PhotoPost,ArticleImages")] ArticleEditModel article)
		{
			PrepareData();


			if (ModelState.IsValid)
			{
				var articles = db.Articles.Include(a => a.Category);
				var editArticle = articles.FirstOrDefault(a => a.ArticleId == article.ArticleId);
				if (editArticle != null)
				{
					editArticle.Title = article.Title;
					editArticle.Preview = article.Preview;
					editArticle.Image = !string.IsNullOrEmpty(article.Image) ? article.Image : "";
					editArticle.ShowOnMain = article.ShowOnMain;
					editArticle.Hide = article.Hide;
					editArticle.PhotoPost = article.PhotoPost;
					var category = db.Categories.FirstOrDefault(c => c.Name == article.Category);
					if (category != null)
					{
						editArticle.CategoryId = category.CategoryId;
						editArticle.Category = category;
					}
					else
					{
						editArticle.Category = new Category { Name = article.Category };
					}


					var tags = string.IsNullOrWhiteSpace(article.Tags) ? new List<string>() : article.Tags.Trim().Replace(Environment.NewLine, "").Split('#').ToList()
						.Select(t => t.Trim().Trim('.').Trim(',').ToLower()).Where(t => !t.IsNullOrWhiteSpace()).ToList();

					if (tags.Any())
					{
						editArticle.ArticleTags = new List<ArticleTag>();

						foreach (var tag in tags)
						{
							var addedTag = db.Tags.FirstOrDefault(t => t.Name.Equals(tag));

							var articleTag = new ArticleTag
							{
								Tag = addedTag ?? new Tag { TagId = -1, Name = tag },
								TagId = -1
							};
							editArticle.ArticleTags.Add(articleTag);
						}
					}

					if (article.ArticleImages != null && article.ArticleImages.Any())
					{
						List<ArticleImage> left = new List<ArticleImage>();
						foreach (var articleArticleImage in editArticle.ArticleImages)
						{
							if (article.ArticleImages.All(a => a.StoredImageId != articleArticleImage.ImageId))
								left.Add(articleArticleImage);
						}
						if (left.Count > 0)
							foreach (var image in left)
							{
								editArticle.ArticleImages.Remove(image);
							}

						foreach (var articleArticleImage in article.ArticleImages)
						{
							var articleStoredImage = editArticle.ArticleImages.FirstOrDefault(i => i.ImageId == articleArticleImage.StoredImageId);

							if (articleStoredImage == null)
							{
								articleStoredImage = new ArticleImage
								{
									Image = new Image(articleArticleImage),
									ImageId = articleArticleImage.StoredImageId
								};
								editArticle.ArticleImages.Add(articleStoredImage);
							}
							else
							{
								articleStoredImage.Image.Name = articleArticleImage.Name;
								articleStoredImage.Image.Text = articleArticleImage.Text;
								articleStoredImage.Image.Extention = articleArticleImage.Extention;
								articleStoredImage.Image.Path = articleArticleImage.Path;
								articleStoredImage.Image.Sizes = articleArticleImage.Sizes;
								articleStoredImage.Image.Type = articleArticleImage.Type;
							}
						}
						editArticle.Image = "/Content/images/articles/" + article.ArticleImages[0].Name + article.ArticleImages[0].Extention + ".[wide]" + article.ArticleImages[0].Extention + "?timestamp=" + new DateTime().Millisecond;
					}

					db.Entry(editArticle).State = EntityState.Modified;
					db.SaveChanges();

					return RedirectToAction("ArticlesList", "Blog");
				}
			}
			return View(article);
		}

		// GET: Articles/Delete/5
		public ActionResult DeleteArticle(int? id)
		{
			PrepareData();


			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Article article = db.Articles.Find(id);
			if (article == null)
			{
				return HttpNotFound();
			}
			return View(article);
		}

		// POST: Articles/Delete/5
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			PrepareData();
			Article article = db.Articles.Find(id);
			db.Articles.Remove(article);
			db.SaveChanges();
			return RedirectToAction("ArticlesList", "Blog");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		public ActionResult Categories(string term)
		{
			var categories = db.Categories.Where(c => c.Name.Contains(term)).Select(c => c.Name).ToList();
			return Json(categories, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public async Task<JsonResult> UploadFile(int imageNumber, string fileName, bool? photoPost = false)
		{

			try
			{
				foreach (string file in Request.Files)
				{

					var fileContent = Request.Files[file];
					if (fileContent != null && fileContent.ContentLength > 0)
					{
						// get a stream
						var stream = fileContent.InputStream;
						// and optionally write the file to disk
						var fileFullName = "IMG" + fileName + Path.GetExtension(fileContent.FileName);
						var path = Path.Combine(Server.MapPath("~/Content/images/articles/"), fileFullName);

						var storedFile = new ImageEditModel();
						storedFile.Name = "IMG" + fileName;
						storedFile.Path = Server.MapPath("~/Content/images/articles/");
						storedFile.Extention = Path.GetExtension(fileContent.FileName);
						storedFile.Sizes = "";
						storedFile.Text = "";
						storedFile.Type = imageNumber == 0 ? Image.StoredImageType.Main : Image.StoredImageType.Additional;

						using (var image = System.Drawing.Image.FromStream(stream))
						{
							//image.Save(path);
							if( photoPost!=null && photoPost.Value)
							{
								using( var smallImage = ImageManager.ScaleImage( image, 300, 400 ) )
								{
									smallImage.Save( path );
									using ( var bigImage = ImageManager.ScaleImageTo( image, 400, 600 ) )
									{
										bigImage.Save(path + ".[big]" + Path.GetExtension(fileContent.FileName));
									}
								}
							}
							else
							{
								using (var wideImage = ImageManager.ScaleImage(image, 400, 300))
								{
									wideImage.Save(path);
								}
							}

							using (var scaledImage = ImageManager.ScaleImage(image, 40, 40))
							{
								scaledImage.Save(path + ".[tiny]" + storedFile.Extention);
								storedFile.Sizes += "tiny ";
							}
							using (var scaledImage = ImageManager.ScaleImage(image, 300, 400))
							{
								scaledImage.Save(path + ".[small]" + storedFile.Extention);
								storedFile.Sizes += "small ";
							}
							using (var scaledImage = ImageManager.ScaleImageTo(image, 400, 600))
							{
								scaledImage.Save(path + ".[big]" + storedFile.Extention);
								storedFile.Sizes += "big ";
							}
							using (var scaledImage = ImageManager.ScaleImageTo(image, 400, 300))
							{
								scaledImage.Save(path + ".[wide]" + storedFile.Extention);
								storedFile.Sizes += "wide ";
							}
						}
							var storedImage = new Image( storedFile );
							db.Images.Add( storedImage );
							db.SaveChanges();

							storedFile = new ImageEditModel( storedImage );
						
						return Json(new { storedFile = storedFile });
					}
				}
			}
			catch (Exception e)
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json("Upload failed");
			}

			return Json("File uploaded successfully");
		}

		public void PushToVkWall()
		{
			//int chanelId = -11;
			//string secret = "test";

			//int appId = 1234567; // указываем id приложения
			//string email = "example@example.ru"; // email для авторизации
			//string password = "qwerty123"; // пароль
			//Settings settings = Settings.All; // уровень доступа к данным

			//var api = new VkApi();
			//api.Authorize(new ApiAuthParams(secret)); // авторизуемся

		}
	}
}
