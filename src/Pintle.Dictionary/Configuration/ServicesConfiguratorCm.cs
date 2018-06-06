namespace Pintle.Dictionary.Configuration
{
	using Messaging;
	using Sitecore.Framework.Messaging;
	using Microsoft.Extensions.DependencyInjection;
	using Sitecore.DependencyInjection;

	public class ServicesConfiguratorCm : IServicesConfigurator
	{
		public void Configure(IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<IMessageHandler<CreateDictionaryItemMessage>, DictionaryItemMessageHandler>();
		}
	}
}