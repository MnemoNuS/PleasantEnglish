using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.Provider;

namespace PleasantEnglish.Models
{

	public class Article
	{
		public int ArticleId { get; set; }
		public string Title { get; set; }
		public string Preview { get; set; }
		public string Text { get; set; }
		public string Image { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateEdited { get; set; }
		public int Likes { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
		public bool ShowOnMain { get; set; }
		public bool Hide { get; set; }
		public bool PhotoPost { get; set; }
		public virtual ICollection<ArticleTag> ArticleTags { get; set; }
		public virtual ICollection<Like> ArticleLikes { get; set; }
		public virtual ICollection<Watch> ArticleWatches { get; set; }
		public virtual ICollection<Comment> ArticleComments { get; set; }
		public virtual ICollection<ArticleImage> ArticleImages { get; set; }

		public Article()
		{
			ArticleId = -1;
			Preview = "";
			Title = "";
			Text = "";
			DateCreated = DateTime.Now;
			DateEdited = DateTime.Now;
			Likes = 0;
		}

	}

	public class ArticleEditModel
	{
		public int ArticleId { get; set; }
		[Required(ErrorMessage = "Как корабль назовешь...Нужно бы заполнить...")]
		[Display(Name = "Заголовок")]
		public string Title { get; set; }
		[Required(ErrorMessage = "Краткость сестра таланта. Нужно бы заполнить...")]
		[Display(Name = "Превью статьи")]
		public string Preview { get; set; }
		[Display(Name = "Текст статьи")]
		public string Text { get; set; }
		[Required(ErrorMessage = "Ну куда без категории? Нужно бы заполнить...")]
		[Display(Name = "Категория")]
		public string Category { get; set; }
		[Display(Name = "Тэги")]
		public string Tags { get; set; }
		[Display(Name = "Изображение")]
		public string Image { get; set; }
		[Display(Name = "Показать на главной")]
		public bool ShowOnMain { get; set; }
		[Display(Name = "Скрыть")]
		public bool Hide { get; set; }
		[Display(Name = "Фотопост")]
		public bool PhotoPost { get; set; }
		public List<ImageEditModel> ArticleImages { get; set; }

		public ArticleEditModel()
		{
			ArticleId = -1;
			Title = "";
			Preview = "";
			Text = "";
			Image = "";
			Category = "";
			Tags = "";
			ArticleImages = new List<ImageEditModel>();
		}

		public ArticleEditModel(Article article)
		{
			ArticleId = article.ArticleId;
			Title = article.Title;
			Preview = article.Preview;
			Text = article.Text;
			Category = article.Category.Name;
			Image = article.Image;
			Tags = article.ArticleTags.Count > 0 ? string.Join(",", article.ArticleTags.Select(t => "#" + t.Tag.Name).ToList()) : "";
			ShowOnMain = article.ShowOnMain;
			Hide = article.Hide;
			ArticleImages = article.ArticleImages != null && article.ArticleImages.Count() > 0
				? article.ArticleImages.Select(i => new ImageEditModel(i.Image)).ToList()
				: new List<ImageEditModel>();
		}
	}

	public class Image
	{
		public enum StoredImageType { Main, Additional }

		public int ImageId { get; set; }
		public StoredImageType Type { get; set; }
		public string Path { get; set; }
		public string Name { get; set; }
		public string Extention { get; set; }
		public string Sizes { get; set; }
		public string Text { get; set; }
		public virtual ICollection<ArticleImage> Articles { get; set; }

		public Image()
		{
		}

		public Image(ImageEditModel image)
		{
			ImageId = image.StoredImageId;
			Type = image.Type;
			Path = image.Path;
			Name = image.Name;
			Extention = image.Extention;
			Sizes = image.Sizes;
			Text = image.Text.IsNullOrEmpty() ? "" : image.Text;
		}
	}

	public class ImageEditModel
	{
		public Image.StoredImageType Type { get; set; }
		public int StoredImageId { get; set; }
		public string Path { get; set; }
		public string Name { get; set; }
		public string Extention { get; set; }
		public string Sizes { get; set; }
		public string Text { get; set; }

		public ImageEditModel()
		{
			StoredImageId = -1;
		}

		public ImageEditModel(Image image)
		{
			StoredImageId = image.ImageId;
			Type = image.Type;
			Path = image.Path;
			Name = image.Name;
			Extention = image.Extention;
			Sizes = image.Sizes;
			Text = image.Text;
		}
	}

	public class Like
	{
		[Key, Column(Order = 0)]
		public int? UserId { get; set; }
		[Key, Column(Order = 1)]
		public int ArticleId { get; set; }
		public User User { get; set; }
		public Article Article { get; set; }
	}

	public class Watch
	{
		[Key, Column(Order = 0)]
		public int? UserId { get; set; }
		[Key, Column(Order = 1)]
		public int ArticleId { get; set; }
		[Key, Column(Order = 2)]
		public DateTime Date { get; set; }
		[Key, Column(Order = 3)]
		public User User { get; set; }
		public Article Article { get; set; }
	}

	public class Category
	{
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public virtual ICollection<Article> Articles { get; set; }
	}

	public class Tag
	{
		public int TagId { get; set; }
		public string Name { get; set; }
		public virtual ICollection<ArticleTag> Articles { get; set; }
	}

	public class Comment
	{
		[Key, Column(Order = 0)]
		public int? UserId { get; set; }
		[Key, Column(Order = 1)]
		public int ArticleId { get; set; }
		[Key, Column(Order = 2)]
		public DateTime Date { get; set; }
		[Key, Column(Order = 3)]
		public User User { get; set; }
		public Article Article { get; set; }
		public string Text { get; set; }
	}

	public class ArticleTag
	{
		[Key, Column(Order = 0)]
		public int ArticleId { get; set; }
		[Key, Column(Order = 1)]
		public int TagId { get; set; }
		public Article Article { get; set; }
		public Tag Tag { get; set; }
	}

	public class ArticleImage
	{
		[Key, Column(Order = 0)]
		public int ArticleId { get; set; }
		[Key, Column(Order = 1)]
		public int ImageId { get; set; }
		public Article Article { get; set; }
		public Image Image { get; set; }
	}

}