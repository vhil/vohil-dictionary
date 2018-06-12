namespace Pintle.Dictionary.Configuration
{
	using Microsoft.Extensions.DependencyInjection;
	using Sitecore.DependencyInjection;
	using Sitecore.Configuration;

	public class ServicesConfigurator : IServicesConfigurator
	{
		public void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient(provider => DictionaryServiceFactory.GetConfiguredInstance());

			serviceCollection.AddSingleton(provider => DictionarySettingsFactory.ConfiguredInstance);
			serviceCollection.AddSingleton(provider => DictionarySettingsFactory.ConfiguredInstance.DictionarySettings);
			serviceCollection.AddSingleton(provider => DictionarySettingsFactory.ConfiguredInstance.DictionaryIconSettings);

			serviceCollection.AddTransient(provider => Factory.CreateObject("pintle/dictionary/itemRepository", true) as DictionaryItemRepository);
			serviceCollection.AddSingleton(provider => Factory.CreateObject("pintle/dictionary/cache", true) as DictionaryCache);
		}
	}
}
