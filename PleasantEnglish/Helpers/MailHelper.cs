using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using Less.Core.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PleasantEnglish.Models;
using User = PleasantEnglish.Models.User;

namespace PleasantEnglish.Helpers
{
	public class MailHelper
	{

		public static void CommentMotification(Comment comment)
		{


			var subject = $"Комментарий к статье '{comment.Article.Title}'";


			StringBuilder msgTitle = new StringBuilder();
			msgTitle.AppendLine($"Комментарий к статье");
			msgTitle.AppendLine($"<br/>");
			msgTitle.AppendLine($"{comment.Article.Title}");

			StringBuilder msgText = new StringBuilder();
			msgText.AppendLine( $"Комментарий от пользователя {comment.User.Name}:" );
			msgText.AppendLine( $"" );
			msgText.AppendLine( $"" );
			msgText.AppendLine( $"{comment.Text}" );
			msgText.AppendLine($"{comment.Date.ToString("f", CultureInfo.InvariantCulture)}");

			Dictionary<string, string> replacements = new Dictionary<string, string>();
			replacements.Add("subject", subject);
			replacements.Add("title", subject);
			replacements.Add("msgTitle", msgTitle.ToString());
			replacements.Add("text", msgText.ToString());

			var admins = AuthHelper.GetUsersInRole("admin");

			if (admins != null && admins.Count > 0)
			{
				SmtpClient smtpClient = new SmtpClient();

				foreach (var admin in admins)
				{
					MailMessage mail = CreateMail(admin.Email, subject, replacements);

					smtpClient.Send(mail);
				}
			}
		}

		public static void RegistrationNotification(User user)
		{
			var subject = $"Регистрация пользователя '{user.Name}'";

			StringBuilder msgTitle = new StringBuilder();
			msgTitle.AppendLine($"Зарегистрировался новый пользователь");

			StringBuilder msgText = new StringBuilder();
			msgText.AppendLine( $"Имя - {user.Name}" );
			msgText.AppendLine( $"" );
			msgText.AppendLine( $"Email - {user.Email}" );


			Dictionary<string, string> replacements = new Dictionary<string, string>();

			replacements.Add("subject", subject);
			replacements.Add("title", subject);
			replacements.Add("msgTitle", msgTitle.ToString());
			replacements.Add("text", msgText.ToString());

			var admins = AuthHelper.GetUsersInRole("admin");

			if (admins != null && admins.Count > 0)
			{
				SmtpClient smtpClient = new SmtpClient();

				foreach (var admin in admins)
				{
					MailMessage mail = CreateMail(admin.Email, subject, replacements);

					smtpClient.Send(mail);
				}
			}
		}

		public static void ForgotPassword(User user, string link)
		{
			var subject = $"Восстановление пароля";

			StringBuilder msgTitle = new StringBuilder();
			msgTitle.AppendLine($"Восстановление пароля");

			StringBuilder msgText = new StringBuilder();
			msgText.AppendLine( $"<a href='{link}'>Для восстановления пароля пройдите по этой ссылке</a>" );


			Dictionary<string, string> replacements = new Dictionary<string, string>();

			replacements.Add("subject", subject);
			replacements.Add("title", subject);
			replacements.Add("msgTitle", msgTitle.ToString());
			replacements.Add("text", msgText.ToString());


			MailMessage mail = CreateMail(user.Email, subject, replacements);

			SmtpClient smtpClient = new SmtpClient();
			smtpClient.Send(mail);

		}

		public static void RegistrationMail(User user)
		{

			var subject = $"Спасибо за регистрацию";

			StringBuilder msgTitle = new StringBuilder();
			msgTitle.AppendLine($"{user.Name}, cпасибо за регистрацию!");

			StringBuilder msgText = new StringBuilder();
			msgText.AppendLine( $"" );

			Dictionary<string, string> replacements = new Dictionary<string, string>();

			replacements.Add("subject", subject);
			replacements.Add("title", subject);
			replacements.Add("text", msgText.ToString());
			replacements.Add("msgTitle", msgTitle.ToString());

			MailMessage mail = CreateMail( user.Email, subject, replacements);

			SmtpClient smtpClient = new SmtpClient();
			smtpClient.Send(mail);

			RegistrationNotification( user );
		}

		public static MailMessage CreateMail( string to, string subject, Dictionary<string, string> rep, string templateUrl = null)
		{

			if (templateUrl == null)
				templateUrl =  HttpContext.Current.Server.MapPath("~/Content/mails/mailTemplate.html")
			;

			MailDefinition md = new MailDefinition();
			md.From = WebConfigurationManager.AppSettings["notificationEmail"]; 
			md.IsBodyHtml = true;
			md.Subject = subject;

			ListDictionary replacements = new ListDictionary();
			foreach( var r in rep )
			{
				replacements.Add("{"+r.Key+"}", r.Value);

			}

			string body = "<div>Hello {name} You're from {country}.</div>";


			var fileStream = new FileStream(templateUrl, FileMode.Open, FileAccess.Read);
			using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
			{
				body = streamReader.ReadToEnd();
			}

			MailMessage msg = md.CreateMailMessage(to, replacements, body, new System.Web.UI.Control());

			return msg;
		}
	}
}