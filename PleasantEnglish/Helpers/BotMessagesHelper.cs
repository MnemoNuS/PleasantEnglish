using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Text;
using PleasantEnglish.Controllers;
using System.Threading.Tasks;
using PleasantEnglish.Controllers.CRUD;
using PleasantEnglish.Models;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace PleasantEnglish.Helpers
{
	public class BotMessagesHelper
	{
		//public static async Task SendGreetingsMessage(long chatId)
		//{
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine("Добро пожаловать!");
		//	messageText.AppendLine("Если нужна помощь введи /help");
		//	await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString());
		//}

		//public static async Task SendHelpMessage(long chatId)
		//{
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine("А тут будут подсказки...");
		//	messageText.AppendLine("Введи /go - что-бы начать работать ");
		//	await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString());
		//}

		//public static async Task SendStudentAddIsDoneMessage(long chatId, int messageId, Student student)
		//{

		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Добавлен студент:");
		//	messageText.AppendFormat(GetStudentFields(student));

		//	await Bot.Api.EditMessageTextAsync(chatId, messageId, messageText.ToString());
		//	await SendMainMenuMessage(chatId);
		//}

		//public static async Task SendLessonAddIsDoneMessage(Lesson lesson, long chatId, int messageId = 0)
		//{

		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Добавлено занятие:");
		//	messageText.AppendFormat(GetLessonFields(lesson));

		//	if (messageId > 0)
		//	{
		//		await Bot.Api.EditMessageTextAsync(chatId, messageId, messageText.ToString());
		//	}
		//	else
		//	{
		//		await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString());
		//	}
		//	await SendMainMenuMessage(chatId);
		//}

		//public static async Task SendStudentEditDoneMessage(Student student, long chatId)
		//{
		//	var keyboard = new InlineKeyboardMarkup(new[]{
		//		new[]{
		//			new InlineKeyboardCallbackButton("Редактировать", $"/editStudentData={student.StudentId}"),
		//			new InlineKeyboardCallbackButton("Удалить", $"/confirmDeleteStudent={student.StudentId}"),
		//			new InlineKeyboardCallbackButton("Назад", "/studentsList")
		//		}
		//	});

		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Данные студента успешно изменены:");
		//	messageText.AppendFormat(GetStudentFields(student));

		//	await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString(), replyMarkup: keyboard);
		//}

		//public static async Task SendCancelMessage(long chatId, int messageId = 0)
		//{
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Действие отменено");
		//	if (messageId > 0)
		//	{
		//		await Bot.Api.EditMessageTextAsync(chatId, messageId, messageText.ToString());
		//	}
		//	else
		//	{
		//		await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString());
		//	}
		//	await SendMainMenuMessage(chatId);
		//}

		//public static async Task SendStudentAddConfirmMessage(long chatId, Student student)
		//{
		//	var keyboard = new InlineKeyboardMarkup(new[]{
		//		new[]{
		//			new InlineKeyboardCallbackButton("Добавить", "/addStudent"),
		//			new InlineKeyboardCallbackButton("Отмена", "/cancel")
		//		}
		//	});
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Будет добавлен студент:");
		//	messageText.AppendFormat(GetStudentFields(student));

		//	await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString(), replyMarkup: keyboard);
		//}

		//public static async Task SendLessonAddConfirmMessage(Lesson lesson, long chatId, int messageId = 0)
		//{
		//	var keyboard = new InlineKeyboardMarkup(new[]{
		//		new[]{
		//			new InlineKeyboardCallbackButton("Добавить", "/createLesson"),
		//			new InlineKeyboardCallbackButton("Отмена", "/cancel")
		//		}
		//	});
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Будет добавлено занятие:");
		//	messageText.AppendFormat(GetLessonFields(lesson));

		//	if (messageId > 0)
		//	{
		//		await Bot.Api.EditMessageTextAsync(chatId, messageId, messageText.ToString(), replyMarkup: keyboard);
		//	}
		//	else
		//	{
		//		await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString(), replyMarkup: keyboard);
		//	}
		//}

		//public static async Task SendLessonInfoMessage(Lesson lesson, long chatId, int messageId = 0)
		//{
		//	List<InlineKeyboardCallbackButton[]> buttonsList = new List<InlineKeyboardCallbackButton[]>();
		//	var addButton = new InlineKeyboardCallbackButton[]
		//	{
		//		new InlineKeyboardCallbackButton( "Редактировать", $"/editLessonInfo={lesson.LessonId}" )
		//	};
		//	buttonsList.Add(addButton);
		//	var cancelButton = new InlineKeyboardCallbackButton[]
		//	{
		//		new InlineKeyboardCallbackButton( "Назад", $"/showLessonsFor={lesson.Date}")
		//	};
		//	buttonsList.Add(cancelButton);

		//	var keyboard = new InlineKeyboardMarkup(buttonsList.ToArray());

		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Информация о занятии:");
		//	messageText.AppendFormat(GetLessonFields(lesson));

		//	if (messageId > 0)
		//	{
		//		await Bot.Api.EditMessageTextAsync(chatId, messageId, messageText.ToString(), replyMarkup: keyboard);
		//	}
		//	else
		//	{
		//		await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString(), replyMarkup: keyboard);
		//	}
		//}

		//public static async Task SendStudentInfoMessage(Student student, long chatId, int messageId = 0)
		//{
		//	var keyboard = new InlineKeyboardMarkup(new[]{
		//		new[]{
		//			new InlineKeyboardCallbackButton("Редактировать", $"/editStudentData={student.StudentId}"),
		//			new InlineKeyboardCallbackButton("Удалить", $"/confirmDeleteStudent={student.StudentId}"),
		//			new InlineKeyboardCallbackButton("Назад", "/studentsList")
		//		}
		//	});
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Информация о студенте:");
		//	messageText.AppendFormat(GetStudentFields(student));

		//	if (messageId > 0)
		//	{
		//		await Bot.Api.EditMessageTextAsync(chatId, messageId, messageText.ToString(), replyMarkup: keyboard);
		//	}
		//	else
		//	{
		//		await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString(), replyMarkup: keyboard);
		//	}
		//}

		//public static async Task SendStudentAddMoreMessage(Lesson lesson, long chatId, int messageId = 0)
		//{
		//	var keyboard = new InlineKeyboardMarkup(new[]{
		//		new[]{
		//			new InlineKeyboardCallbackButton("Добавить", $"/addMoreStudents"),
		//			new InlineKeyboardCallbackButton("Готово", $"/confirmLesson")
		//		}
		//	});
		//	StringBuilder messageText = new StringBuilder();
		//	//var lessonStudents = LessonsController.GetLessonStusents(lesson.LessonId); 
		//	if (lesson.LessonStudents.Count > 0)
		//	{
		//		messageText.AppendLine($"Добавленные студены:");
		//		foreach (var lessonStudent in lesson.LessonStudents.ToList())
		//		{
		//			var student = StudentsController.GetStudent(lessonStudent.StudentId);
		//			messageText.AppendLine($"                {student.Name}");
		//		}
		//	}
		//	messageText.AppendLine($"Добавить еще студентов?");
		//	if (messageId > 0)
		//	{
		//		await Bot.Api.EditMessageTextAsync(chatId, messageId, messageText.ToString(), replyMarkup: keyboard);
		//	}
		//	else
		//	{
		//		await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString(), replyMarkup: keyboard);
		//	}
		//}

		//public static async Task SendStudentDeleteConfirmMessage(Student student, long chatId, int messageId = 0)
		//{
		//	var keyboard = new InlineKeyboardMarkup(new[]{
		//		new[]{
		//			new InlineKeyboardCallbackButton("Удалить", $"/deleteStudent={student.StudentId}"),
		//			new InlineKeyboardCallbackButton("Отмена", "/cancel")
		//		}
		//	});
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Действительно удалить студента:");
		//	messageText.AppendLine($"Имя - {student.FullName}");
		//	if (messageId > 0)
		//	{
		//		await Bot.Api.EditMessageTextAsync(chatId, messageId, messageText.ToString(), replyMarkup: keyboard);
		//	}
		//	else
		//	{
		//		await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString(), replyMarkup: keyboard);
		//	}
		//}

		//public static async Task SendStudentDeleteMessage(Student student, long chatId, int messageId = 0)
		//{
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Студент {student.FullName} был  удален");
		//	if (messageId > 0)
		//	{
		//		await Bot.Api.EditMessageTextAsync(chatId, messageId, messageText.ToString());
		//	}
		//	else
		//	{
		//		await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString());

		//	}
		//	await SendStudentsWorkMenuMessage(chatId);
		//}

		//public static async Task SendStudentEditDataMessage(Student student, long chatId, int messageId = 0)
		//{
		//	var keyboard = new InlineKeyboardMarkup(new[]{
		//		new[]{
		//			new InlineKeyboardCallbackButton("Имя", $"/editStudentName={student.StudentId}"),
		//			new InlineKeyboardCallbackButton("Телефон", $"/editStudentPhone={student.StudentId}"),
		//			new InlineKeyboardCallbackButton("Отмена", "/cancel")
		//		}
		//	});
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Редактирование данныx студента:");
		//	messageText.AppendFormat(GetStudentFields(student));

		//	if (messageId > 0)
		//	{
		//		await Bot.Api.EditMessageTextAsync(chatId, messageId, messageText.ToString(), replyMarkup: keyboard);
		//	}
		//	else
		//	{
		//		await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString(), replyMarkup: keyboard);
		//	}
		//}

		//public static async Task SendErrorMessage(long chatId)
		//{
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine("Произошла ошибка...");
		//	await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString());
		//}

		//public static async Task SendStudentsWorkMenuMessage(long chatId, int messageId = 0)
		//{
		//	var keyboard = new InlineKeyboardMarkup(new[]
		//	{
		//		new[] // first row
		//		{
		//			new InlineKeyboardCallbackButton("Добавить студента", "/addStudent"),
		//			new InlineKeyboardCallbackButton("Посмотреть список", "/studentsList")
		//		}
		//	});
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Выбери действие");
		//	if (messageId > 0)
		//	{
		//		await Bot.Api.EditMessageTextAsync(chatId, messageId, messageText.ToString(),
		//		  replyMarkup: keyboard);
		//	}
		//	else
		//	{
		//		await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString(),
		//			replyMarkup: keyboard);
		//	}
		//}

		//public static async Task SendMainMenuMessage(long chatId)
		//{
		//	var keyboard = new ReplyKeyboardMarkup(new[] {
		//		new[]
		//		{
		//			new KeyboardButton( "Управление рассписанием" ),
		//		},
		//		new[] // last row
		//		{
		//			new KeyboardButton( "Работа со студентами" ),
		//		}});
		//	keyboard.ResizeKeyboard = true;
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Продолжим? Выбери с чем будешь работать");

		//	await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString(), replyMarkup: keyboard);
		//}

		//public static async Task SendCantDoMessage(long chatId, int messageId = 0)
		//{
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Я этого пока не умею...");

		//	if (messageId > 0)
		//	{
		//		await Bot.Api.EditMessageTextAsync(chatId, messageId, messageText.ToString());
		//	}
		//	else
		//	{
		//		await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString());
		//	}
		//	await SendMainMenuMessage(chatId);
		//}

		//public static async Task SendSchedualeMenuMessage(long chatId, int messageId = 0)
		//{

		//	var keyboard = new InlineKeyboardMarkup(new[]
		//	{
		//		new[] // first row
		//		{
		//			new InlineKeyboardCallbackButton("вчера", $"/showLessonsFor={DateTime.Today.AddDays(-1)}"),
		//			new InlineKeyboardCallbackButton("сегодня", $"/showLessonsFor={DateTime.Today}"),
		//			new InlineKeyboardCallbackButton("завтра", $"/showLessonsFor={DateTime.Today.AddDays(1)}"),
		//			new InlineKeyboardCallbackButton("неделя", "/showWeekLessons"),
		//		},
		//		new[]
		//		{
		//			new InlineKeyboardCallbackButton("Показать календарь", "/showCalendar"),
		//		}

		//	});
		//	StringBuilder messageText = new StringBuilder();
		//	messageText.AppendLine($"Выбери день");

		//	if (messageId > 0)
		//	{
		//		await Bot.Api.EditMessageTextAsync(chatId, messageId, messageText.ToString(),
		//			replyMarkup: keyboard);
		//	}
		//	else
		//	{
		//		await Bot.Api.SendTextMessageAsync(chatId, "Работа с рассписанием");

		//		await Bot.Api.SendTextMessageAsync(chatId, messageText.ToString(),
		//			replyMarkup: keyboard);
		//	}

		//}

		public static string GetStudentFields(Student student)
		{
			StringBuilder messageText = new StringBuilder();
			messageText.AppendLine($"Полное Имя - {student.FullName}");
			messageText.AppendLine($"Имя коротко - {student.Name}");
			messageText.AppendLine($"Телефон - {student.Phone}");
			return messageText.ToString();
		}

		public static string GetLessonFields(Lesson lesson)
		{
			StringBuilder messageText = new StringBuilder();
			messageText.AppendLine($"Дата - {lesson.Date.Value.ToString("M", CultureInfo.InvariantCulture)}");
			messageText.AppendLine($"Время начала - {lesson.TimeStart.Value.ToString("hh:mm", CultureInfo.CurrentCulture)}");
			messageText.AppendLine($"Время окончания - {lesson.TimeEnd.Value.ToString("hh:mm", CultureInfo.CurrentCulture)}");
			messageText.AppendLine($"Продолжительность - {lesson.Duration} мин.");
			if (lesson.LessonStudents.Count > 0)
			{
				messageText.AppendLine($"Студенты:");
				foreach (var lessonStudent in lesson.LessonStudents.ToList())
				{
					var student = StudentsController.GetStudent(lessonStudent.StudentId);
					messageText.AppendLine($"                {student.Name}");
				}
			}
			return messageText.ToString();
		}
	}
}