namespace Pintle.Dictionary.Extensions
{
	using Sitecore.Mvc.Helpers;
	using Mvc;

	public static class SitecoreHelperExtensions
	{
		public static MvcDictionaryServiceWrapper Dictionary(this SitecoreHelper helper)
		{
			return new MvcDictionaryServiceWrapper(DictionaryServiceFactory.GetConfiguredInstance());
		}
	}
}
