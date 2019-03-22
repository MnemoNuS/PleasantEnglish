using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Owin;
using PleasantEnglish.Controllers.CRUD;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using PleasantEnglish.Helpers;
using PleasantEnglish.Models;

namespace PleasantEnglish.Controllers
{
	public class TBotController : BaseBotApiController
	{
		[AcceptVerbs("GET", "POST")]
		[Route("api/TBot/SetWebHook")]
		public IHttpActionResult SetWebHook(string ngrokUrl)
		{
			//var cert = new FileToSend("certificate.cer", new FileStream("D:/botCertificate.crt", FileMode.Open));
			//ngrok http 19873
			//ngrok http 19873 -host-header=localhost:19873
			// https://24299ed4.ngrok.io/api/TBot/SetWebHook?ngrokUrl=https://24299ed4.ngrok.io
			Bot.Api.SetWebhookAsync($"{ngrokUrl}/api/TBot/WebHook").Wait();
			return Ok();
		}

		[AcceptVerbs("GET", "POST")]
		[Route("api/TBot/WebHook")]
		public async Task<IHttpActionResult> WebHook(Update update)
		{
			try
			{
				await Prepare( update );
				if( IsCancelCommand() )
				{
					await OnCancel();
				}
				else if( Update.EditedMessage != null )
				{
					return Ok();
				}
				else if( !Chat.ActionIs( "Edle" ) )
				{
					if( Chat.ActionIs( "EditStudent" ) )
					{
						await EditStudentDialog();
					}
					else if( Chat.ActionIs( "AddStudent" ) )
					{
						await AddStudentDialog();
					}
					else if( Chat.ActionIs( "AddLesson" ) )
					{
						await AddLessonDialog();
					}
					else
					{
						await OnError();
					}
				}
				else if( IsCallbackQuery )
				{
					await OnCallbackQuery();
				}
				if( IsMessage )
				{
					await OnMessage();
				}
			}
			catch( Exception e )
			{
				await OnError();
				//return Ok();
				throw;
			}
			return Ok();
		}

		private bool IsCancelCommand()
		{
			if (IsCallbackQuery && Update.CallbackQuery.Data.StartsWith("/cancel"))
			{
				return true;
			}
			if (IsMessage && (Message.Text.StartsWith("/cancel") || Message.Text.StartsWith("Отмена действия")))
			{
				return true;
			}
			return false;
		}

		private async Task OnCallbackQuery()
		{
			if (CallbackData != null)
			{
				if (Update.CallbackQuery.Data.StartsWith("/addStudent")) // send inline keyboard
				{
					await AddStudentDialog();
				}
				else if (Update.CallbackQuery.Data.StartsWith("/cancel"))
				{
					await EndDialog();
					await BotMessagesHelper.SendCancelMessage(Chat.GetChatId(), MessageId);
				}
				else if (Update.CallbackQuery.Data.StartsWith("/deleteList"))
				{
					var keyboard = new InlineKeyboardMarkup(new[]{
							new[]{new InlineKeyboardCallbackButton("Иванов А.В.", "/choose") },
							new[]{new InlineKeyboardCallbackButton("Петров А.В.", "/choose")},
							new[]{new InlineKeyboardCallbackButton("Сидоров А.В.", "/choose")},
							new[]{new InlineKeyboardCallbackButton("Назад", "/back")}
							});
					await Bot.Api.EditMessageTextAsync(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.MessageId, "Удалить:",
						replyMarkup: keyboard);
				}
				else if (Update.CallbackQuery.Data.StartsWith("/plan"))
				{
					var keyboard = new InlineKeyboardMarkup(new[]{
								new[]{
									new InlineKeyboardCallbackButton("Добавить", "/add"),
									new InlineKeyboardCallbackButton("Назад", "/back")
								}
							});
					await Bot.Api.EditMessageTextAsync(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.MessageId, "Не составлен",
						replyMarkup: keyboard);
				}
				else if (Update.CallbackQuery.Data.StartsWith("/homework"))
				{
					var keyboard = new InlineKeyboardMarkup(new[]{
								new[]{
									new InlineKeyboardCallbackButton("Добавить", "/add"),
									new InlineKeyboardCallbackButton("Назад", "/back")
								}
							});
					await Bot.Api.EditMessageTextAsync(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.MessageId, "Не назначено",
						replyMarkup: keyboard);
				}
				else if (Update.CallbackQuery.Data.StartsWith("/chooseList"))
				{
					var keyboard = new InlineKeyboardMarkup(new[]{
							new[]{new InlineKeyboardCallbackButton("Иванов А.В.", "/choose") },
							new[]{new InlineKeyboardCallbackButton("Петров А.В.", "/choose")},
							new[]{new InlineKeyboardCallbackButton("Сидоров А.В.", "/choose")},
							new[]{new InlineKeyboardCallbackButton("Назад", "/back")}
							});
					await Bot.Api.EditMessageTextAsync(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.MessageId, "Выбрать:",
						replyMarkup: keyboard);
				}
				else if (Update.CallbackQuery.Data.StartsWith("/chooseList"))
				{
					var keyboard = new InlineKeyboardMarkup(new[]{
							new[]{new InlineKeyboardCallbackButton("Иванов А.В.", "/choose") },
							new[]{new InlineKeyboardCallbackButton("Петров А.В.", "/choose")},
							new[]{new InlineKeyboardCallbackButton("Сидоров А.В.", "/choose")},
							new[]{new InlineKeyboardCallbackButton("Назад", "/back")}
							});
					await Bot.Api.EditMessageTextAsync(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.MessageId, "Выбрать:",
						replyMarkup: keyboard);
				}
				else if (Update.CallbackQuery.Data.StartsWith("/studentsList"))
				{
					var students = StudentsController.GetStudents().ToList();
					if (students.Count == 0)
					{
						await Bot.Api.EditMessageTextAsync(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.MessageId,
							"Список пуст");
						await BotMessagesHelper.SendMainMenuMessage(Update.CallbackQuery.Message.Chat.Id);
					}
					else
					{
						List<InlineKeyboardCallbackButton[]> studentsList = new List<InlineKeyboardCallbackButton[]>();
						for (int i = 0; i < students.Count; i++)
						{
							var button = new InlineKeyboardCallbackButton[] { new InlineKeyboardCallbackButton($"{i + 1}. {students[i].FullName}", $"/showStudent={students[i].StudentId}") };
							studentsList.Add(button);
						}
						var cancelButton = new InlineKeyboardCallbackButton[] {
							new InlineKeyboardCallbackButton( "Назад", "/StudentsManager" )
						}
						;
						studentsList.Add(cancelButton);

						var keyboard = new InlineKeyboardMarkup(studentsList.ToArray());
						await Bot.Api.EditMessageTextAsync(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.MessageId,
							"Список студентов:",
							replyMarkup: keyboard);
					}
				}
				else if (Update.CallbackQuery.Data.StartsWith("/showStudent"))
				{
					var data = Update.CallbackQuery.Data;
					var startSumbol = data.IndexOf('=') + 1;
					var studentIdStr = Update.CallbackQuery.Data.Substring(startSumbol, data.Length - startSumbol);
					int studentId;
					if (int.TryParse(studentIdStr, out studentId))
					{
						var student = StudentsController.GetStudent(studentId);
						await BotMessagesHelper.SendStudentInfoMessage(student, Message.Chat.Id, Update.CallbackQuery.Message.MessageId);
					}
					else
					{
						await BotMessagesHelper.SendCancelMessage(Message.Chat.Id, Update.CallbackQuery.Message.MessageId);
					}
				}
				else if (Update.CallbackQuery.Data.StartsWith("/confirmDeleteStudent"))
				{
					var data = Update.CallbackQuery.Data;
					var startSumbol = data.IndexOf('=') + 1;
					var studentIdStr = Update.CallbackQuery.Data.Substring(startSumbol, data.Length - startSumbol);
					int studentId;
					if (int.TryParse(studentIdStr, out studentId))
					{
						var student = StudentsController.GetStudent(studentId);
						await BotMessagesHelper.SendStudentDeleteConfirmMessage(student, Message.Chat.Id, Update.CallbackQuery.Message.MessageId);
					}
					else
					{
						await BotMessagesHelper.SendCancelMessage(Message.Chat.Id, Update.CallbackQuery.Message.MessageId);
					}
				}
				else if (Update.CallbackQuery.Data.StartsWith("/deleteStudent"))
				{
					var data = Update.CallbackQuery.Data;
					var startSumbol = data.IndexOf('=') + 1;
					var studentIdStr = Update.CallbackQuery.Data.Substring(startSumbol, data.Length - startSumbol);
					int studentId;
					if (int.TryParse(studentIdStr, out studentId))
					{
						var student = StudentsController.GetStudent(studentId);
						if (StudentsController.DeleteStudent(studentId))
						{
							await BotMessagesHelper.SendStudentDeleteMessage(student, Message.Chat.Id,
								Update.CallbackQuery.Message.MessageId);
						}
						else
						{
							await BotMessagesHelper.SendCancelMessage(Message.Chat.Id, Update.CallbackQuery.Message.MessageId);
						}
					}
					else
					{
						await BotMessagesHelper.SendCancelMessage(Message.Chat.Id, Update.CallbackQuery.Message.MessageId);
					}
				}
				else if (Update.CallbackQuery.Data.StartsWith("/editStudentData"))
				{
					var data = Update.CallbackQuery.Data;
					var startSumbol = data.IndexOf('=') + 1;
					var studentIdStr = Update.CallbackQuery.Data.Substring(startSumbol, data.Length - startSumbol);
					int studentId;
					if (int.TryParse(studentIdStr, out studentId))
					{
						var student = StudentsController.GetStudent(studentId);
						await BotMessagesHelper.SendStudentEditDataMessage(student, Message.Chat.Id, Update.CallbackQuery.Message.MessageId);
						await EditStudentDialog();
					}
					else
					{
						await BotMessagesHelper.SendCancelMessage(Message.Chat.Id, Update.CallbackQuery.Message.MessageId);
					}
				}
				else if (Update.CallbackQuery.Data.StartsWith("/StudentsManager"))
				{
					await BotMessagesHelper.SendStudentsWorkMenuMessage(Message.Chat.Id, Update.CallbackQuery.Message.MessageId);
				}
				else if (Update.CallbackQuery.Data.StartsWith("/SchedualeManager")) // send inline keyboard
				{
					await BotMessagesHelper.SendSchedualeMenuMessage(TelegramChatId, MessageId);
				}
				//lessons commands 
				else if (Update.CallbackQuery.Data.StartsWith("/addLessonTo"))
				{
					await AddLessonDialog();
				}
				else if (Update.CallbackQuery.Data.StartsWith("/showLessonsFor"))
				{
					var data = Update.CallbackQuery.Data;
					var startSumbol = data.IndexOf('=') + 1;
					var dateStr = Update.CallbackQuery.Data.Substring(startSumbol, data.Length - startSumbol);
					DateTime date;
					if (DateTime.TryParse(dateStr, out date))
					{
						var lessons = LessonsController.GetLessons().Where(l => l.Date.Value.Date == date.Date).ToList();
						if (lessons.Count == 0)
						{
							List<InlineKeyboardCallbackButton[]> buttonsList = new List<InlineKeyboardCallbackButton[]>();
							var addButton = new InlineKeyboardCallbackButton[]
							{
								new InlineKeyboardCallbackButton( "Добавить занятие", $"/addLessonTo={date}" )
							};
							buttonsList.Add(addButton);
							var cancelButton = new InlineKeyboardCallbackButton[]
							{
								new InlineKeyboardCallbackButton( "Назад", "/SchedualeManager" )
							};
							buttonsList.Add(cancelButton);
							var keyboard = new InlineKeyboardMarkup(buttonsList.ToArray());

							await Bot.Api.EditMessageTextAsync(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.MessageId,
								"Список пуст", replyMarkup: keyboard);
						}
						else
						{
							List<InlineKeyboardCallbackButton[]> lessonsList = new List<InlineKeyboardCallbackButton[]>();
							for (int i = 0; i < lessons.Count; i++)
							{
								var lessonStudents = LessonsController.GetLessonStusents(lessons[i].LessonId).ToList();
								var lessonWith = lessonStudents.Count <= 0
									? "Не задано"
									: lessonStudents.Count == 1
										? lessonStudents.FirstOrDefault()?.Name
										: "Группа";

								var button = new InlineKeyboardCallbackButton[]
								{
									new InlineKeyboardCallbackButton(
										$"{i + 1}. с {lessons[i].TimeStart.Value.ToString( "hh:mm", CultureInfo.CurrentCulture )} до {lessons[i].TimeEnd.Value.ToString( "hh:mm", CultureInfo.CurrentCulture )} c {lessonWith}",
										$"/showLessonInfo={lessons[i].LessonId}" )
								};
								lessonsList.Add(button);
							}
							var addButton = new InlineKeyboardCallbackButton[]
							{
								new InlineKeyboardCallbackButton( "Добавить занятие", $"/addLessonTo={date.Date}" )
							};
							lessonsList.Add(addButton);
							var cancelButton = new InlineKeyboardCallbackButton[]
							{
								new InlineKeyboardCallbackButton( "Назад", "/SchedualeManager" )
							};
							lessonsList.Add(cancelButton);

							var keyboard = new InlineKeyboardMarkup(lessonsList.ToArray());
							await Bot.Api.EditMessageTextAsync(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.MessageId,
								$"Список занятий на {date.ToString("M", CultureInfo.InvariantCulture)} :",
								replyMarkup: keyboard);
						}
					}
					else
					{
						await OnError();
					}
				}
				else if (Update.CallbackQuery.Data.StartsWith("/showWeekLessons"))
				{
					DayOfWeek fdow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek; 
					DayOfWeek today = DateTime.Now.DayOfWeek;
					DateTime monday = DateTime.Now.AddDays(-(fdow -today)).Date;

					var sunday = monday.Date.AddDays( 7 );

						var lessons = LessonsController.GetLessons().Where(l => l.Date.Value.Date >= monday.Date && l.Date.Value.Date <= sunday.Date).ToList();
						if (lessons.Count == 0)
						{
							List<InlineKeyboardCallbackButton[]> buttonsList = new List<InlineKeyboardCallbackButton[]>();
							var cancelButton = new InlineKeyboardCallbackButton[]
							{
								new InlineKeyboardCallbackButton( "Назад", "/SchedualeManager" )
							};
							buttonsList.Add(cancelButton);
							var keyboard = new InlineKeyboardMarkup(buttonsList.ToArray());

							await Bot.Api.EditMessageTextAsync(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.MessageId,
								$"Список  занятий c {monday.ToString("M", CultureInfo.CurrentCulture)} по {sunday.ToString("M", CultureInfo.CurrentCulture)} пуст", replyMarkup: keyboard);
						}
						else
						{
							List<InlineKeyboardCallbackButton[]> lessonsList = new List<InlineKeyboardCallbackButton[]>();
							for (int i = 0; i < lessons.Count; i++)
							{
								var lessonStudents = LessonsController.GetLessonStusents(lessons[i].LessonId).ToList();
								var lessonWith = lessonStudents.Count <= 0
									? "Не задано"
									: lessonStudents.Count == 1
										? lessonStudents.FirstOrDefault()?.Name
										: "Группа";

								var button = new InlineKeyboardCallbackButton[]
								{
									new InlineKeyboardCallbackButton(
										$"{i + 1}. с {lessons[i].TimeStart.Value.ToString( "hh:mm", CultureInfo.CurrentCulture )} до {lessons[i].TimeEnd.Value.ToString( "hh:mm", CultureInfo.CurrentCulture )} c {lessonWith}",
										$"/showLessonInfo={lessons[i].LessonId}" )
								};
								lessonsList.Add(button);
							}
							var cancelButton = new InlineKeyboardCallbackButton[]
							{
								new InlineKeyboardCallbackButton( "Назад", "/SchedualeManager" )
							};
							lessonsList.Add(cancelButton);

							var keyboard = new InlineKeyboardMarkup(lessonsList.ToArray());
							await Bot.Api.EditMessageTextAsync(Update.CallbackQuery.Message.Chat.Id, Update.CallbackQuery.Message.MessageId,
								$"Список занятий c {monday.ToString("M", CultureInfo.CurrentCulture)} по {sunday.ToString("M", CultureInfo.CurrentCulture)} :",
								replyMarkup: keyboard);
						}
					}
				else if (Update.CallbackQuery.Data.StartsWith("/showLessonInfo"))
				{
					int lessonId;
					if (GetIndexFromCallbackQueryData(out lessonId))
					{
						var lesson = LessonsController.GetLesson( lessonId );
						if (lesson != null)
						{
							await BotMessagesHelper.SendLessonInfoMessage( lesson, TelegramChatId, MessageId );
						}
					}
					else
					{
						await OnError();
					}
				}
				//***
				else
				{
					await BotMessagesHelper.SendCantDoMessage( TelegramChatId, MessageId );
				}
			}
		}

		private async Task OnMessage()
		{
			var message = Update.Message;

			if (message.Type == MessageType.TextMessage)
			{
				await ProcessTextMessage();
			}
			else if (message.Type == MessageType.PhotoMessage)
			{
				// Download Photo
				var file = await Bot.Api.GetFileAsync(message.Photo.LastOrDefault()?.FileId);

				var filename = "D://" + file.FileId + "." + file.FilePath.Split('.').Last();

				using (var saveImageStream = File.Open(filename, FileMode.Create))
				{
					await file.FileStream.CopyToAsync(saveImageStream);
				}

				await Bot.Api.SendTextMessageAsync(message.Chat.Id, "Thx for the Pics");
			}
		}

		private async Task OnCancel()
		{
			await EndDialog();
			await BotMessagesHelper.SendCancelMessage(TelegramChatId, MessageId);
		}

		private async Task ProcessTextMessage()
		{
			if (Message.Text.StartsWith("/start"))
			{
				await BotMessagesHelper.SendGreetingsMessage(Message.Chat.Id);
			}
			else if (Message.Text.StartsWith("/help"))
			{
				await BotMessagesHelper.SendHelpMessage(Message.Chat.Id);
			}
			else if (Message.Text.StartsWith("/go"))
			{
				await BotMessagesHelper.SendMainMenuMessage(Message.Chat.Id);
			}
			else if (Message.Text.StartsWith("Управление рассписанием")) // send inline keyboard
			{
				await BotMessagesHelper.SendSchedualeMenuMessage(Message.Chat.Id);
			}
			else if (Message.Text.StartsWith("Работа со студентами"))
			{
				await BotMessagesHelper.SendStudentsWorkMenuMessage(Message.Chat.Id);
			}
			else
			{
				//await BotMessagesHelper.SendHelp(message.Chat.Id, message.MessageId);
			}
		}

		private async Task AddLessonDialog()
		{
			if (Chat.ActionIs("Edle"))
			{

				var data = Update.CallbackQuery.Data;
				var startSumbol = data.IndexOf('=') + 1;
				var dateStr = Update.CallbackQuery.Data.Substring(startSumbol, data.Length - startSumbol);
				DateTime date;
				if (DateTime.TryParse(dateStr, out date))
				{
					var newLesson = new Lesson { Date = date, LessonStudents = new List<LessonStudents>() };

					//newLesson = LessonsController.CreateLesson( newLesson );
					if (newLesson.LessonId != 0)
					{
						Chat.PutTempData(newLesson);
						//Chat.PutTempData(newLesson.LessonId);

						Chat.SetActionTo("AddLesson");
						Chat.SetStepTo("StartTime");
						SaveChat();

						await Bot.Api.EditMessageTextAsync(TelegramChatId, MessageId,
							$"Добавление занятия на {date.ToString("M", CultureInfo.InvariantCulture)} :");
						var keyboard = new ReplyKeyboardMarkup(
							new[]
							{
								new[]
								{
									new KeyboardButton( "Отмена действия" ),
								}
							});
						keyboard.ResizeKeyboard = true;
						await Bot.Api.SendTextMessageAsync(TelegramChatId, "Введите время начала занятия (HH:mm)",
							replyMarkup: keyboard);
					}
					else
					{
						await OnError();
					}


				}
				else
				{
					await OnError();
				}
			}
			else if (Chat.ActionIs("AddLesson"))
			{
				var newLesson = new Lesson();

				if (Chat.StepIs("Duration"))
				{

					if (Chat.ChatInfo.TempData == null)
					{
						await OnError();
					}

					//var newLessonId = Chat.GetTempData<int>();
					//newLesson = LessonsController.GetLesson(newLessonId);
					newLesson = Chat.GetTempData<Lesson>();

					int duration;
					if (Int32.TryParse(Message.Text, out duration))
					{
						newLesson.Duration = duration;
						if (newLesson.TimeStart != null) newLesson.TimeEnd = newLesson.TimeStart.Value.AddMinutes(newLesson.Duration);
						//LessonsController.SaveLesson( newLessonId, newLesson );
						Chat.PutTempData(newLesson);
						Chat.SetStepTo("AppendStudents");
						SaveChat();
					}
					else
					{
						await OnError();
					}
				}

				if (Chat.StepIs("StartTime"))
				{
					if (Chat.ChatInfo.TempData == null)
					{
						await OnError();
					}

					//var newLessonId = Chat.GetTempData<int>();
					//newLesson = LessonsController.GetLesson(newLessonId);
					newLesson = Chat.GetTempData<Lesson>();
					DateTime startTime;
					if (DateTime.TryParse(Message.Text, out startTime))
					{
						newLesson.TimeStart = startTime;
						//LessonsController.SaveLesson( newLessonId, newLesson );
						Chat.PutTempData(newLesson);
						Chat.SetStepTo("Duration");
						SaveChat();
						await Bot.Api.SendTextMessageAsync(TelegramChatId, "Введите длительность занятия (в минутах)");
					}
					else
					{
						await OnError();
					}
				}

				if (Chat.StepIs("AddMore"))
				{
					if (Chat.ChatInfo.TempData == null)
					{
						await OnError();
					}
					if (Update.CallbackQuery.Data.StartsWith("/appendStudent"))
					{
						var data = Update.CallbackQuery.Data;
						var startSumbol = data.IndexOf('=') + 1;
						var studentIdStr = Update.CallbackQuery.Data.Substring(startSumbol, data.Length - startSumbol);
						int studentId;
						if (int.TryParse(studentIdStr, out studentId))
						{
							//var newLessonId = Chat.GetTempData<int>();
							//newLesson = LessonsController.GetLesson(newLessonId);
							newLesson = Chat.GetTempData<Lesson>();
							//var student = StudentsController.GetStudent(studentId);
							//LessonsController.AddLessonStudent(newLesson, student);
							var lessonStudent = new LessonStudents { StudentId = studentId, LessonId = newLesson.LessonId };
							newLesson.LessonStudents.Add(lessonStudent);
							Chat.PutTempData(newLesson);
							SaveChat();
							await BotMessagesHelper.SendStudentAddMoreMessage(newLesson, TelegramChatId, MessageId);
						}
						else
						{
							await OnError();
						}
					}
					if (Update.CallbackQuery.Data.StartsWith("/addMoreStudents"))
					{
						Chat.SetStepTo("AppendStudents");
						SaveChat();
					}
					if (Update.CallbackQuery.Data.StartsWith("/confirmLesson"))
					{
						Chat.SetStepTo("Confirm");
						SaveChat();
					}
				}
				if (Chat.StepIs("AppendStudents"))
				{
					if (Chat.ChatInfo.TempData == null)
					{
						await OnError();
					}

					//var newLessonId = Chat.GetTempData<int>();
					//newLesson = LessonsController.GetLesson(newLessonId);

					newLesson = Chat.GetTempData<Lesson>();

					var students = StudentsController.GetStudents().ToList();
					if (students.Count == 0)
					{
						await OnError();
					}
					else
					{
						List<InlineKeyboardCallbackButton[]> studentsList = new List<InlineKeyboardCallbackButton[]>();
						var lessonStudents = newLesson.LessonStudents.ToList();
						for (int i = 0; i < students.Count; i++)
						{
							var studentButtonName = $"{i + 1}. {students[i].FullName}";
							var studentButtonAction = $"/appendStudent={students[i].StudentId}";
							if (lessonStudents.All(s => s.StudentId != students[i].StudentId))
							{
								var button = new InlineKeyboardCallbackButton[]
									{ new InlineKeyboardCallbackButton( studentButtonName, studentButtonAction ) };
								studentsList.Add(button);
							}
						}
						var cancelButton = new InlineKeyboardCallbackButton[]
							{
								new InlineKeyboardCallbackButton( "Назад", "/StudentsManager" )
							}
							;
						studentsList.Add(cancelButton);

						var keyboard = new InlineKeyboardMarkup(studentsList.ToArray());

						StringBuilder messageText = new StringBuilder();
						if (lessonStudents.Count > 0)
						{
							messageText.AppendLine($"Добавленные студены:");
							foreach (var lessonStudent in newLesson.LessonStudents.ToList())
							{
								var student = StudentsController.GetStudent(lessonStudent.StudentId);
								messageText.AppendLine($"                {student.Name}");
							}
						}
						messageText.AppendLine($"Выберите студента:");
						if (MessageId > 0)
						{
							await Bot.Api.EditMessageTextAsync(TelegramChatId, MessageId, messageText.ToString(), replyMarkup: keyboard);
						}
						else
						{
							await Bot.Api.SendTextMessageAsync(TelegramChatId, messageText.ToString(), replyMarkup: keyboard);
						}
						Chat.SetStepTo("AddMore");
						SaveChat();
					}
				}
				if (Chat.StepIs("Confirm"))
				{
					if (Chat.ChatInfo.TempData == null)
					{
						await OnError();
					}

					//var newLessonId = Chat.GetTempData<int>();
					//newLesson = LessonsController.GetLesson(newLessonId);
					newLesson = Chat.GetTempData<Lesson>();

					Chat.SetStepTo("Save");
					SaveChat();

					await BotMessagesHelper.SendLessonAddConfirmMessage(newLesson, TelegramChatId, MessageId);

				}
				if (Chat.StepIs("Save"))
				{
					if (Update.CallbackQuery.Data.StartsWith("/createLesson"))
					{
						if (Chat.ChatInfo.TempData == null)
						{
							await OnError();
						}

						//var newLessonId = Chat.GetTempData<int>();
						//newLesson = LessonsController.GetLesson(newLessonId);
						newLesson = Chat.GetTempData<Lesson>();

						//LessonsController.SaveLesson(newLesson.LessonId, newLesson);
						//LessonsController.SaveLesson(newLessonId, newLesson);
						newLesson = LessonsController.CreateLesson(newLesson);
						if (newLesson.LessonId > 0)
						{
							await BotMessagesHelper.SendLessonAddIsDoneMessage(newLesson, TelegramChatId, MessageId);
							await EndDialog();
						}
						else
						{
							await OnError();
						}
					}
				}

			}
		}

		private async Task AddStudentDialog()
		{
			if (Chat.ActionIs("Edle"))
			{
				Chat.SetActionTo("AddStudent");
				Chat.SetStepTo("FullName");
				Chat.SetMessageId(Message.MessageId);
				SaveChat();
				await Bot.Api.EditMessageTextAsync(TelegramChatId, MessageId, "Добавление нового студента");
				var keyboard = new ReplyKeyboardMarkup(
					new[]
					{
						new[]
						{
							new KeyboardButton( "Отмена действия" ),
						}
					});
				keyboard.ResizeKeyboard = true;
				await Bot.Api.SendTextMessageAsync(TelegramChatId, "Введите полное имя", replyMarkup: keyboard);
			}
			else if (Chat.ActionIs("AddStudent"))
			{
				var newStudent = new Student();

				if (Chat.StepIs("FullName"))
				{
					newStudent.FullName = Message.Text;
					Chat.PutTempData(newStudent);
					Chat.SetStepTo("Name");
					SaveChat();
					await Bot.Api.SendTextMessageAsync(TelegramChatId, "Введите имя коротко");
				}

				else if (Chat.StepIs("Name"))
				{
					if (Chat.ChatInfo.TempData == null)
					{
						await OnError();
					}

					newStudent = Chat.GetTempData<Student>();

					newStudent.Name = Message.Text;
					Chat.PutTempData(newStudent);
					Chat.SetStepTo("Phone");
					SaveChat();
					await Bot.Api.SendTextMessageAsync(TelegramChatId, "Введите телефон");
				}
				else if (Chat.StepIs("Phone"))
				{
					if (Chat.ChatInfo.TempData == null)
					{
						await OnError();
					}

					newStudent = Chat.GetTempData<Student>();

					newStudent.Phone = Message.Text;
					Chat.PutTempData(newStudent);
					Chat.SetStepTo("Confirm");
					SaveChat();
					await BotMessagesHelper.SendStudentAddConfirmMessage(TelegramChatId, newStudent);

				}
				else if (Chat.StepIs("Confirm"))
				{
					if (Chat.ChatInfo.TempData == null)
					{
						await OnError();
					}

					newStudent = Chat.GetTempData<Student>();

					newStudent = StudentsController.CreateStudent(newStudent);
					if (newStudent.StudentId > 0)
					{
						await BotMessagesHelper.SendStudentAddIsDoneMessage(TelegramChatId, Update.CallbackQuery.Message.MessageId, newStudent);
						await EndDialog();
					}
					else
					{
						await OnError();
					}
				}
			}
		}

		private async Task EditStudentDialog()
		{

			if (Chat.ActionIs("Edle"))
			{
				Chat.SetActionTo("EditStudent");
				Chat.SetStepTo("Choose");
				Chat.SetMessageId(Message.MessageId);
				SaveChat();
			}
			else if (Chat.ActionIs("EditStudent"))
			{
				if (Chat.StepIs("Choose"))
				{
					var data = Update.CallbackQuery.Data;
					var startSumbol = data.IndexOf('=') + 1;
					var studentIdStr = Update.CallbackQuery.Data.Substring(startSumbol, data.Length - startSumbol);
					int studentId;

					if (int.TryParse(studentIdStr, out studentId))
					{
						var student = StudentsController.GetStudent(studentId);
						var step = "Choose";
						StringBuilder messageText = new StringBuilder();


						if (Update.CallbackQuery.Data.StartsWith("/editStudentPhone"))
						{
							step = "Phone";
							messageText.AppendLine($"Введите телефон:");
							messageText.AppendLine($"Старые данные - {student.Phone}");
						}
						if (Update.CallbackQuery.Data.StartsWith("/editStudentName"))
						{
							step = "Name";
							messageText.AppendLine($"Введите имя");
							messageText.AppendLine($"Старые данные - {student.Name}");
						}
						Chat.PutTempData(student);
						Chat.SetStepTo(step);
						SaveChat();
						await Bot.Api.EditMessageTextAsync(TelegramChatId, MessageId, "Изменение данных студента");
						var keyboard = new ReplyKeyboardMarkup(
							new[]
							{
								new[]
								{
									new KeyboardButton( "Отмена действия" ),
								}
							});
						keyboard.ResizeKeyboard = true;
						await Bot.Api.SendTextMessageAsync(TelegramChatId, messageText.ToString(), replyMarkup: keyboard);
					}
					else
					{
						await BotMessagesHelper.SendCancelMessage(Message.Chat.Id, Update.CallbackQuery.Message.MessageId);
					}
				}
				else if (Chat.StepIs("Phone"))
				{
					if (Chat.ChatInfo.TempData == null)
					{
						await OnError();
					}

					var student = Chat.GetTempData<Student>();

					student.Phone = Message.Text;
					if (StudentsController.EditStudent(student.StudentId, student))
					{
						await BotMessagesHelper.SendStudentEditDoneMessage(student, TelegramChatId);
						await EndDialog();
					}
					else
					{
						await OnError();
					}
				}
				else if (Chat.StepIs("Name"))
				{
					if (Chat.ChatInfo.TempData == null)
					{
						await OnError();
					}

					var student = Chat.GetTempData<Student>();

					student.Name = Message.Text;
					student.FullName = Message.Text;
					if (StudentsController.EditStudent(student.StudentId, student))
					{
						await BotMessagesHelper.SendStudentEditDoneMessage(student, TelegramChatId);
						await EndDialog();
					}
					else
					{
						await OnError();
					}
				}
			}
		}

		private async Task OnError()
		{
			await EndDialog();
			await BotMessagesHelper.SendErrorMessage(TelegramChatId);
		}

		private async Task EndDialog()
		{
			Chat.SetToEdle();
			SaveChat();
		}

		private bool GetIndexFromCallbackQueryData(out int result )
		{
			var data = Update.CallbackQuery.Data;
			var startSumbol = data.IndexOf('=') + 1;
			var dataNumder = Update.CallbackQuery.Data.Substring(startSumbol, data.Length - startSumbol);
			return ( Int32.TryParse(dataNumder, out result ) );
		}

		private bool GetDate( string data, out DateTime result )
		{
			var startSumbol = data.IndexOf('=') + 1;
			var dataNumder = Update.CallbackQuery.Data.Substring(startSumbol, data.Length - startSumbol);
			return ( DateTime.TryParse( data, out result ) );
		}
		[HttpGet]
		[Route("api/Test/{test}")]
		public string Test(string test)
		{
			return $"Test- {test} - {DateTime.Now}";
		}

	}
	static class Bot
	{
		public static readonly TelegramBotClient Api = new TelegramBotClient("519794341:AAFx5f3PPPfcSqpC0disZQoubUDZmpqS-xM");
	}
}
