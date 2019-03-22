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
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using PleasantEnglish.Helpers;
using PleasantEnglish.Controllers;

namespace PleasantEnglish.Helpers
{
	public class BotUpdateHelper
	{

		public static ReplyKeyboardMarkup BottomMenu()
		{
			var bottomMenu =
				new[]
				{
					new[]
					{
						new KeyboardButton( "Управление рассписанием" ),
					},
					new[] // last row
					{
						new KeyboardButton( "Работа со студентами" ),
					}
				};

			var menu = new ReplyKeyboardMarkup(bottomMenu);
			menu.OneTimeKeyboard = true;
			return menu;
		}


	}
}