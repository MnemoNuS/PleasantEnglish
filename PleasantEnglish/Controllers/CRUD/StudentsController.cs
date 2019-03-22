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
    public class StudentsController 
    {
        // GET: api/Students
        public static List<Student> GetStudents()
        {
	        using( SiteContext db = new SiteContext() )
			{
				return db.Students.Include(u => u.Lessons).ToList();
	        }
        }

        // GET: api/Students/5
        public static Student GetStudent(int id)
        {
	        using( SiteContext db = new SiteContext() )
	        {
		        Student student = db.Students.Find( id );
		        db.Entry(student).Collection(u => u.Lessons).Load();
				return student;
	        }
        }

        // PUT: api/Students/5
        public static  bool EditStudent(int id, Student student)
        {

	        if( id == student.StudentId )
	        {
		        using( SiteContext db = new SiteContext() )
		        {
			        db.Entry( student ).State = EntityState.Modified;
					db.SaveChanges();
			        return true;
		        }
	        }
	        return false;
        }

        // POST: api/Students
        public static Student CreateStudent(Student student)
        {
	        using( SiteContext db = new SiteContext() )
	        {
		        db.Students.Add( student );
		        db.SaveChanges();
		        return student;
	        }
		}

        public static  bool DeleteStudent(int id)
        {
	        using( SiteContext db = new SiteContext() )
	        {
		        Student student = db.Students.Find( id );
		        if( student == null )
		        {
			        return false;
		        }

		        db.Students.Remove( student );
		        db.SaveChanges();
		        return true;
			}
        }

        private static bool StudentExists(int id)
        {
	        using( SiteContext db = new SiteContext() )
	        {
		        return db.Students.Any( e => e.StudentId == id );
	        }
        }
    }
}