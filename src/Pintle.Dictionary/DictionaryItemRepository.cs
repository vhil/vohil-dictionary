namespace Pintle.Dictionary
{
	using System;
	using Messaging;
	using Sitecore.Framework.Messaging;

	public class DictionaryItemRepository
	{
		private readonly SitecoreDictionarySettings settings;
		private readonly IMessageBus<DictionaryMessageBus> messageBus;

		public DictionaryItemRepository(
			SitecoreDictionarySettings settings, 
			IMessageBus<DictionaryMessageBus> messageBus)
		{
			this.settings = settings;
			this.messageBus = messageBus;
		}

		public virtual void Create(string dictionaryKey, string defaultValue, string language)
		{
			this.messageBus.SendAsync(new DictionaryItemMessage
			{
				DictionaryKey = dictionaryKey,
				DefaultValue = defaultValue,
				Database = this.settings.ItemCreationDatabase,
				Language = language ?? Sitecore.Context.Language?.Name,
				DictionaryDomainId = new Guid(Sitecore.Context.Site.DictionaryDomain)
			});
		}
	}
}