using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using Newtonsoft.Json;
using PleasantEnglish.Controllers.CRUD;
using PleasantEnglish.Models;
using Telegram.Bot.Types;

namespace PleasantEnglish.Controllers
{
	public class BaseBotApiController : ApiController
	{
		protected Update Update { get; set; }
		protected bool IsMessage { get; set; }
		protected bool IsEditedMessage { get; set; }
		protected bool IsCallbackQuery { get; set; }
		protected long TelegramChatId { get; set; }
		protected int MessageId { get; set; }
		protected string CallbackData { get; set; }
		protected ChatCallbackData ChatCallbackData { get; set; }
		public Message Message { get; set; }
		public TelegramChat Chat { get; set; }

		protected async Task Prepare(Update update)
		{
			CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
			CancellationToken token = cancelTokenSource.Token;
			try
			{
				CultureInfo ru = new CultureInfo("ru-RU");
				Thread.CurrentThread.CurrentCulture = ru;
				Update = update;
				if (Update.CallbackQuery != null)
				{
					IsCallbackQuery = true;
					Message = Update.CallbackQuery.Message;
					CallbackData = Update.CallbackQuery.Data;
					MessageId = Message.MessageId;
				}
				else if (Update.Message != null)
				{
					IsMessage = true;
					Message = Update.Message;
					MessageId = 0;
				}
				else if (Update.EditedMessage != null)
				{
					cancelTokenSource.Cancel(); //не обрабатываем если сообшение редактировалось
					IsEditedMessage = true;
				}
				TelegramChatId = Message.Chat.Id;
				Chat = await ChatsController.GetChatByTelegramId(TelegramChatId);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		protected void SaveChat()
		{
			ChatsController.EditChat(Chat.ChatId, Chat);
		}

	}
	public class ChatCallbackData
	{
		public string Command { get; set; }
		public object Data { get; set; }
	}

}
