using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PleasantEnglish.Models
{
	public enum EnglishLanguageLevel { Beginner, Elementary, PreIntermediate, Intermediate, UpperIntermediate, Advanced }
	public enum PartOfSpeach { Noun, Pronoun, Adjective, Verb, Adverb, Preposition, Conjunction, Interjection }

	public class Word{
	public int WordId { get; set; }
	public string ValueEn { get; set; }
	public string ValueRu { get; set; }
	public string Transcription { get; set; }
	public EnglishLanguageLevel Level { get; set; }
	public PartOfSpeach PartOfSpeach { get; set; }
	public string Pronunciation { get; set; }
	public string Image { get; set; }
	public virtual ICollection<WordCollection> Collections { get; set; }

	}

	public class WordViewModel
	{
		public int WordId { get; set; }
		public string ValueEn { get; set; }
		public string ValueRu { get; set; }
		public string Transcription { get; set; }
		public EnglishLanguageLevel Level { get; set; }
		public PartOfSpeach PartOfSpeach { get; set; }
		public string Pronunciation { get; set; }
		public string Image { get; set; }

		public WordViewModel()
		{

		}

		public WordViewModel(Word word)
		{
			WordId = word.WordId;
			ValueEn = word.ValueEn;
			ValueRu = word.ValueRu;
			Transcription = word.Transcription;
			Level = word.Level;
			PartOfSpeach = word.PartOfSpeach;
			Pronunciation = !string.IsNullOrEmpty(word.Pronunciation)?$"/words/pronounce/{word.WordId}":null ;
			Image = word.Image;
		}
	}

	public class Collection {
		public int CollectionId { get; set; }
		public string ValueEn { get; set; }
		public string ValueRu { get; set; }
		public EnglishLanguageLevel Level { get; set; }
		public string Image { get; set; }
		public virtual ICollection<WordCollection> Words { get; set; }
	}

	public class WordCollection
	{
		[Key, Column(Order = 0)]
		public int WordId { get; set; }
		[Key, Column(Order = 1)]
		public int CollectionId { get; set; }
		public Word Word { get; set; }
		public Collection Collection { get; set; }
	}
}