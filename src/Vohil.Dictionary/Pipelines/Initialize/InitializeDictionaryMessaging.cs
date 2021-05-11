namespace Vohil.Dictionary.Pipelines.Initialize
{
	using System;
	using Messaging;
	using Sitecore.Framework.Messaging;
	using Sitecore.Pipelines;

	public class InitializeDictionaryMessaging
	{
		private readonly IServiceProvider serviceProvider;

		public InitializeDictionaryMessaging(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public void Process(PipelineArgs args)
		{
			this.serviceProvider.StartMessageBus<DictionaryMessageBus>();
		}
	}
}
