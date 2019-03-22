using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace PleasantEnglish.Models
{
	public class Chat
	{
		public int ChatId { get; set; }
		public string Type { get; set; }
		public ChatInfo ChatInfo { get; set; }



		public bool StepIs(string stepName)
		{
			return ChatInfo.Step.Equals( stepName );
		}
		public bool ActionIs(string actionName)
		{
			return ChatInfo.Action.Equals(actionName);
		}
		public void SetToEdle()
		{
			ChatInfo.Action = "Edle";
			ChatInfo.Step = "Zero";
		}
		public void SetActionTo(string actionName)
		{
			ChatInfo.Action = actionName;
		}
		public void SetStepTo(string stepName)
		{
			ChatInfo.Step = stepName;
		}

		public long GetChatId()
		{
			return ChatInfo.ChatId;
		}

		public void SetChatId(long chatId)
		{
			ChatInfo.ChatId = chatId;
		}
	}

	public class TelegramChat : Chat
	{
		public TelegramChat()
		{
			ChatId = -1;
			Type = "telegram";
			ChatInfo = new ChatInfo();
		}

		public void SetMessageId(int messageId)
		{
			ChatInfo.MessageId = messageId;
		}
		public int GetMessageId()
		{
			return ChatInfo.MessageId;
		}
		public void PutTempData<T>(T tempObject)
		{
			ChatInfo.TempData = JsonConvert.SerializeObject(tempObject);
		}

		public T GetTempData<T>()
		{
			return JsonConvert.DeserializeObject<T>(ChatInfo.TempData);
		}

	}





	public class ChatInfo
	{
		public long ChatId { get; set; }
		public int MessageId { get; set; }
		public string Action { get; set; }
		public string Step { get; set; }
		public string TempData { get; set; }


		public ChatInfo()
		{
			ChatId = -1;
			MessageId = 0;
			Action = "Edle";
			Step = "Zero";
		}
	}
}