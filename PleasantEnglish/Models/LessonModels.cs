using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PleasantEnglish.Models
{
	public class Lesson
	{
		public int LessonId { get; set; }
		public DateTime? Date { get; set; }
		public DateTime? TimeStart { get; set; }
		public DateTime? TimeEnd { get; set; }
		public int Duration { get; set; }
		public string HomeTask { get; set; }
		public virtual ICollection<LessonStudents> LessonStudents { get; set; }

		public Lesson()
		{
			LessonId = -1;
			Date = DateTime.Now;
			TimeStart = DateTime.Now;
			TimeEnd = DateTime.Now;
			Duration = 0;
			HomeTask = "no";
		}
	}

	public class LessonStudents
		{
			[Key, Column(Order = 0)]
		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }
			[Key, Column(Order = 1)]
		public int StudentId { get; set; }
			public Student Student { get; set; }
		}
	

}