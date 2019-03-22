using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.DynamicData;
using System.Web.Http;
using System.Web.Http.Description;
using PleasantEnglish.Models;

namespace PleasantEnglish.Controllers.CRUD
{
	public class LessonsController
	{

		public static List<Lesson> GetLessons()
		{
			using (SiteContext db = new SiteContext())
			{
				List<Lesson> lessons = db.Lessons.Include(u => u.LessonStudents).ToList();
				return lessons;
			}
		}

		// GET: api/Lessons/5
		public static Lesson GetLesson(int id)
		{
			using (SiteContext db = new SiteContext())
			{
				Lesson lesson = db.Lessons.Find(id);
				db.Entry(lesson).Collection(u => u.LessonStudents).Load();
				return lesson;
			}
		}

		// PUT: api/Lessons/5
		public static bool SaveLesson(int id, Lesson lesson)
		{
			if (id != lesson.LessonId)
				return false;
			using (SiteContext db = new SiteContext())
			{
				db.Entry(lesson).State = EntityState.Modified;
				try
				{
					db.SaveChanges();
					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}

		public static List<Student> GetLessonStusents(int id)
		{
			using (SiteContext db = new SiteContext())
			{
				var students = db.LessonStudents
					.Include(ls => ls.Student.Lessons)
					.Include(ls => ls.Lesson.LessonStudents)
					.Where(l => l.LessonId == id)
					.Select(l => l.Student)
					.ToList();

				return students;
			}
		}

		// PUT: api/Lessons/5
		public static bool AddLessonStudent(Lesson lesson, Student student)
		{

			using (SiteContext db = new SiteContext())
			{
				var lessonStudent = new LessonStudents ();
				if( lesson.LessonId > 0 )
				{
					lessonStudent.LessonId = lesson.LessonId;
				}
				else
				{
					lessonStudent.Lesson = lesson;
				}
				if( student.StudentId > 0 )
				{
					lessonStudent.StudentId = student.StudentId;
				}
				else
				{
					lessonStudent.Student = student;
				}
				try
				{
					db.LessonStudents.Add(lessonStudent);
					db.SaveChanges();
					return true;
				}
				catch (DbUpdateConcurrencyException)
				{
					return false;
				}
			}
		}

		// POST: api/Lessons
		public static Lesson CreateLesson(Lesson lesson)
		{
			using (SiteContext db = new SiteContext())
			{
				try
				{
					db.Lessons.Add(lesson);
					db.SaveChanges();
					return lesson;
				}
				catch (Exception e)
				{
					throw e;
				}

			}
		}

		// DELETE: api/Lessons/5
		public bool DeleteLesson(int id)
		{
			using (SiteContext db = new SiteContext())
			{
				Lesson lesson = db.Lessons.Find(id);
				if (lesson == null)
				{
					return false;
				}
				db.Lessons.Remove(lesson);
				db.SaveChanges();
				return true;
			}
		}
	}
}