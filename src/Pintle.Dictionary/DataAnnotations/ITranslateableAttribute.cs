namespace Pintle.Dictionary.DataAnnotations
{
	public interface ITranslateableAttribute
	{
		string DictionaryKey { get; set; }
		string DefaultTranslation { get; set; }
		bool Editable { get; set; }
	}
}
