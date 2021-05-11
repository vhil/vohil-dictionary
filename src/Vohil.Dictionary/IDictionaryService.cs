namespace Vohil.Dictionary
{
	public interface IDictionaryService
	{
		string Translate(string key, string defaultValue = null, bool editable = false, string language = null);
	}
}
