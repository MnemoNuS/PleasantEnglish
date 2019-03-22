using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;
using Owin;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;
using PleasantEnglish.Helpers;
using PleasantEnglish.Controllers;

namespace PleasantEnglish.Helpers
{
	public class PupilsManager
	{
		public static async Task AddPupil( long chatId, int messageId, string name)
		{
			await Bot.Api.SendChatActionAsync(chatId, ChatAction.Typing);
			var keyboard = new InlineKeyboardMarkup(new[]{
								new[]{
									new InlineKeyboardCallbackButton("Назад", "/backToStudentsManager")
								}
							});
			await Bot.Api.SendTextMessageAsync(chatId, $"Добавлен - {name}", replyMarkup: keyboard);
		}
	}
}