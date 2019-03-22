using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PleasantEnglish.Models;

namespace PleasantEnglish.Controllers.CRUD
{
	public class ChatsController
	{
		private SiteContext db = new SiteContext();

		// GET: api/Chats
		public static IQueryable<Chat> GetChats()
		{
			using( SiteContext db = new SiteContext() )
			{
				return db.Chats;
			}
		}

		// GET: api/Chats/5
		public static async Task<Chat> GetChat(int id)
		{
			using( SiteContext db = new SiteContext() )
			{
				Chat chat = await db.Chats.FindAsync( id );
				return chat;
			}
		}
		public static async Task<TelegramChat> GetChatByTelegramId(long id)
		{
		
				TelegramChat chat = new TelegramChat();
				if( ChatsController.ChatExists( id ) )
				{
					using( SiteContext db = new SiteContext() )
					{
						chat = await db.Chats.FirstOrDefaultAsync( c => c.Type == "telegram" && c.ChatInfo.ChatId == id ) as TelegramChat;
					}
				}
				else
				{
					chat.SetChatId(id);
					chat.SetToEdle();
					chat = await CreateChat( chat ) as TelegramChat;
				}
				return chat;
		}

		// PUT: api/Chats/5
		public static bool EditChat(int id, Chat chat)
		{
			if (id != chat.ChatId)
			{
				return false;
			}
			using( SiteContext db = new SiteContext() )
			{
				db.Entry( chat ).State = EntityState.Modified;

				try
				{
					db.SaveChanges();
					return true;
				}
				catch( DbUpdateConcurrencyException )
				{
					if( !ChatExists( id ) )
					{
						return false;
					}
					else
					{
						throw;
					}
				}

				return false;
			}
		}

		// POST: api/Chats
		public static async Task<Chat> CreateChat(Chat chat)
		{
			using( SiteContext db = new SiteContext() )
			{
				db.Chats.Add( chat );
				await db.SaveChangesAsync();
				return chat;
			}
		}

		// DELETE: api/Chats/5
		public static async Task<Chat> DeleteChat(int id)
		{
			using( SiteContext db = new SiteContext() )
			{
				Chat chat = await db.Chats.FindAsync( id );
				if( chat == null )
				{
					return null;
				}

				db.Chats.Remove( chat );
				await db.SaveChangesAsync();

				return chat;
			}
		}


		public static bool ChatExists(long id)
		{
			using( SiteContext db = new SiteContext() )
			{
				return db.Chats.Any( e => e.ChatInfo.ChatId == id );
			}
		}
	}
}