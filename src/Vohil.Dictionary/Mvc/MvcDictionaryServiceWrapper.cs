namespace Vohil.Dictionary.Mvc
{
	using System.Web;

	public class MvcDictionaryServiceWrapper
	{
		private readonly IDictionaryService dictionary;

		public MvcDictionaryServiceWrapper(IDictionaryService dictionary)
		{
			this.dictionary = dictionary;
		}

		public IHtmlString Translate(
			string key,
			string defaultValue = null,
			bool editable = false,
			string language = null)
		{
			return new HtmlString(this.dictionary.Translate(key, defaultValue, editable, language));
		}
	}
}
