using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PleasantEnglish.Models;

namespace PleasantEnglish.Controllers
{
    public class CollectionsController : BaseController
    {
        // GET: Collections
        public ActionResult List()
        {
	        var collections = db.Collections.Include(w => w.Words).Include(c => c.Words.Select(w => w.Word)).ToList();
			return View(collections);
        }

	    public ActionResult AddCollection()
	    {
		    return View(new Collection());
	    }


	    // GET: Articles/Edit/5
	    public ActionResult EditCollection(int? id)
	    {
		    if (id == null)
		    {
			    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		    }
		    Collection collection = db.Collections.ToList().FirstOrDefault(c => c.CollectionId == id);
		    if (collection == null)
		    {
			    return HttpNotFound();
		    }
		    return View(collection);
	    }

	    [HttpPost]
	    [ValidateInput(false)]
	    public ActionResult EditCollection(Collection collection)
	    {
		    if (ModelState.IsValid)
		    {
			    var collections = db.Collections.Include(a => a.Words).ToList();
			    var editCollection = collections.FirstOrDefault(a => a.CollectionId == collection.CollectionId);
			    if (editCollection != null)
			    {
				    editCollection.Image = collection.Image;
				    editCollection.Level = collection.Level;
				    editCollection.ValueEn = collection.ValueEn;
				    editCollection.ValueRu = collection.ValueRu;
					db.Entry(editCollection).State = EntityState.Modified;
				    db.SaveChanges();
				    return RedirectToAction("List", "Collections");

				}
				else
			    {
				    db.Collections.Add(collection);
				    db.SaveChanges();
				    return RedirectToAction("ViewCollection", "Collections", new { id = collection.CollectionId});
				}
			}
		    return View(collection);
	    }

	    [HttpPost]
	    public ActionResult DeleteCollection(int? id)
	    {
		    var collection = db.Collections.FirstOrDefault(c => c.CollectionId == id);
		    if (collection != null)
		    {
			    db.Entry(collection).State = EntityState.Deleted;
			    db.SaveChanges();
		    }

		    return RedirectToAction("List");
	    }

	    public ActionResult ViewCollection( int? id )
	    {
		    var collection = db.Collections.Include(a => a.Words).Include(c => c.Words.Select(w => w.Word)).FirstOrDefault(c => c.CollectionId == id);
		    if (collection != null)
		    {
			    return View(collection);
			}
			return RedirectToAction("List");
	    }

	    public bool AddWordToCollection(int collectionId, int wordId)
	    {
		    var collection = db.Collections.Include(a => a.Words).FirstOrDefault(c => c.CollectionId == collectionId);
		    if (collection == null)
		    {
			    return false;
		    }
		    var word = db.Words.Include(a => a.Collections).FirstOrDefault(w => w.WordId == wordId);
		    if (word == null)
		    {
			    return false;
		    }
			var wordCollection = db.WordCollections.FirstOrDefault(wc=> wc.WordId == wordId && wc.CollectionId == collectionId);

		    if( wordCollection == null )
		    {
			    wordCollection = new WordCollection
				    { WordId = word.WordId, Word = word, CollectionId = collectionId, Collection = collection };

			    db.WordCollections.Add( wordCollection );
			    db.SaveChanges();

			    return true;
			}

			return false;
	    }

	    public bool DeleteWordFromCollection(int collectionId, int wordId)
	    {
		    var wordCollection = db.WordCollections.FirstOrDefault(wc => wc.WordId == wordId && wc.CollectionId == collectionId);

		    if (wordCollection != null)
		    {
			    db.Entry(wordCollection).State = EntityState.Deleted;
			    db.SaveChanges();
			    return true;
		    }

		    return false;
	    }


	}
}