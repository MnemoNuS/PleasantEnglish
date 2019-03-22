using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using PleasantEnglish.Models;

namespace PleasantEnglish.Controllers
{
    public class WordsController : BaseController
    {
        // GET: Words
        public ActionResult Index()
        {
            return View();
        }

	    public ActionResult AddWord()
	    {
		    return View(new Word());
	    }

	    // GET: Articles/Edit/5
	    public ActionResult EditWord(int? id)
	    {
		    if (id == null)
		    {
			    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		    }
		    Word word = db.Words.ToList().FirstOrDefault(w => w.WordId == id);
		    if (word == null)
		    {
			    return HttpNotFound();
		    }
		    return View(word);
	    }

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult EditWord(Word word)
		{
			if (ModelState.IsValid)
			{
				var words = db.Words.Include(a => a.Collections).ToList();
				var editWord = words.FirstOrDefault(a => a.WordId == word.WordId);
				if (editWord != null)
				{
					editWord.Image = word.Image;
					editWord.Level = word.Level;
					editWord.PartOfSpeach = word.PartOfSpeach;
					editWord.Pronunciation = word.Pronunciation;
					editWord.Transcription = word.Transcription;
					editWord.ValueEn = word.ValueEn;
					editWord.ValueRu = word.ValueRu;

					db.Entry(editWord).State = EntityState.Modified;
					db.SaveChanges();
				}
				else
				{
					db.Words.Add( word );
					db.SaveChanges();
				}
				return RedirectToAction("Dictionary", "Words");
			}
			return View(word);
		}

		public ActionResult Dictionary()
		{
			var words = new List<WordViewModel>();

				var selectedWords = db.Words
					.ToList();
				if (selectedWords.Count > 0)
				{
					foreach (var selectedWord in selectedWords)
					{
						words.Add(WordToWordViewModel(selectedWord));
					}
				}

			return View(words);
		}

	    [HttpPost]
	    public ActionResult DeleteWord( int? id )
	    {
		    var word = db.Words.FirstOrDefault( w => w.WordId == id );
		    if( word != null )
		    {
			    db.Entry( word ).State = EntityState.Deleted;
			    db.SaveChanges();
		    }

		    return RedirectToAction( "Dictionary" );
	    }

	    public ActionResult Pronounce(int? id)
	    {
		    if (id == null)
		    {
			    return null;
		    }
		    Word word = db.Words.ToList().FirstOrDefault(w => w.WordId == id);
		    if (word == null)
		    {
			    return null;
		    }

		    byte[] buff = Convert.FromBase64String( word.Pronunciation.Replace("data:audio/wav;base64,", "") );

		    MemoryStream ms = new MemoryStream();
		    ms.Write(buff, 0, buff.Length);
		    ms.Seek(0, SeekOrigin.Begin);

		    MemoryStream outputStream = new MemoryStream();

		    ms.WriteTo(outputStream);

			return File(buff, "audio/wav");
			//return word.Pronunciation;
		}

		public ActionResult FindWord( string term )
	    {
		    var words = new List<WordViewModel>();

			if ( !string.IsNullOrEmpty( term ) )
		    {
			    var selectedWords = db.Words
				    .Where( w =>
					    w.ValueEn.ToLower().Contains( term.ToLower() ) ||
					    w.ValueRu.ToLower().Contains( term.ToLower() ) ).OrderBy( w => w.ValueEn.Length )
				    .ToList();
			    if( selectedWords.Count > 0 )
			    {
				    foreach( var selectedWord in selectedWords )
				    {
					    words.Add(WordToWordViewModel(selectedWord));
				    }
			    }
		    }

		    return Json(words, JsonRequestBehavior.AllowGet);
	    }

		public static WordViewModel WordToWordViewModel( Word word )
	    {
			return new WordViewModel(word);
	    }

	}
}