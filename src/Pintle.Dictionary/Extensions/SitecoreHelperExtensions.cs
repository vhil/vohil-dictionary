namespace Pintle.Dictionary.Extensions
{
	using Sitecore.Mvc.Helpers;

	public static class SitecoreHelperExtensions
	{
		public static IDictionaryService Dictionary(this SitecoreHelper helper)
		{
			return DictionaryServiceFactory.GetConfiguredInstance();
		}
	}
}
