namespace Pintle.Dictionary
{
	using Sitecore.Configuration;
	using Sitecore.Exceptions;

	public class DictionaryServiceFactory
	{
		/// <summary>
		/// Creates instance of <see cref="IDictionaryService"/> from configuration.
		/// </summary>
		public static IDictionaryService GetConfiguredInstance()
		{
			var dictionaryService = Factory.CreateObject("pintle/dictionary/dictionaryService", true)
				as IDictionaryService;

			if (dictionaryService == null)
			{
				throw new ConfigurationException($"Configuration error. Service on node 'pintle/dictionary/dictionaryService' is not configured properly.");
			}

			return dictionaryService;
		}
	}
}
