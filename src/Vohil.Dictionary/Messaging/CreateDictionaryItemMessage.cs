namespace Vohil.Dictionary.Messaging
{
	using System;

	public class CreateDictionaryItemMessage
	{
		public string DictionaryKey { get; set; }
		public string Language { get; set; }
		public string DefaultValue { get; set; }
		public Guid DictionaryDomainId { get; set; }
		public string Database { get; set; }
	}
}
