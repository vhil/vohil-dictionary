namespace Vohil.Dictionary
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
			var dictionaryService = Factory.CreateObject("vohil/dictionary/dictionaryService", true)
				as IDictionaryService;

			if (dictionaryService == null)
			{
				throw new ConfigurationException($"[Vohil.Dictionary]: Configuration error. Service on node 'vohil/dictionary/dictionaryService' is not configured properly.");
			}

			return dictionaryService;
		}
	}
}
