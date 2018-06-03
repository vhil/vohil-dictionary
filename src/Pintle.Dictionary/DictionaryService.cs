namespace Pintle.Dictionary
{
	using System;
	using Sitecore.Abstractions;

	public class DictionaryService : IDictionaryService
	{
		private readonly DictionaryItemRepository repository;
		private readonly DictionaryCache cache;
		private readonly BaseLog logger;

		public DictionaryService(DictionaryItemRepository repository, DictionaryCache cache, BaseLog logger)
		{
			this.repository = repository;
			this.cache = cache;
			this.logger = logger;
		}

		public string Translate(
			string key, 
			string defaultValue = null, 
			bool editable = false, 
			string language = null,
			bool autoCreate = true)
		{
			throw new NotImplementedException();
		}
	}
}
