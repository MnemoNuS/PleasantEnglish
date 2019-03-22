using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PleasantEnglish.Models
{
	public class Student
	{
		public int StudentId { get; set; }
		public string Name{ get; set; }
		public string FullName { get; set; }
		public string Phone { get; set; }
		public virtual ICollection<LessonStudents> Lessons { get; set; }
	}
}