namespace Vohil.Dictionary.Messaging
{
	using System.Threading.Tasks;
	using Sitecore.Abstractions;
	using Sitecore.Data;
	using Sitecore.Data.Managers;
	using Sitecore.Framework.Messaging;
	using Sitecore.Globalization;

	public class DictionaryItemMessageHandler : IMessageHandler<CreateDictionaryItemMessage>
	{
		private readonly DictionaryItemRepository repository;
		private readonly BaseLog logger;

		public DictionaryItemMessageHandler(
			DictionaryItemRepository repository,
			BaseLog logger)
		{
			this.repository = repository;
			this.logger = logger;
		}

		public Task Handle(
			CreateDictionaryItemMessage message,
			IMessageReceiveContext receiveContext,
			IMessageReplyContext replyContext)
		{
			if (this.ValidateMessage(message))
			{
				var database = Database.GetDatabase(message.Database);
				var language = LanguageManager.GetLanguage(message.Language);

				using(new DatabaseSwitcher(database))
				using (new LanguageSwitcher(language))
				{
					this.repository.CreateItem(
						message.DictionaryKey, 
						message.DefaultValue, 
						message.DictionaryDomainId, 
						language, 
						database);
				}
			}

			return Task.CompletedTask;
		}

		private bool ValidateMessage(CreateDictionaryItemMessage message)
		{
			if (message == null)
			{
				this.logger.Error($"[Vohil.Dictionary]: Message is null.", this);
				return false;
			}

			if (string.IsNullOrWhiteSpace(message.DictionaryKey))
			{
				this.logger.Error($"[Vohil.Dictionary]: Dictionary keyis null or empty.", this);
				return false;
			}

			var database = Database.GetDatabase(message.Database);
			if (database == null)
			{
				this.logger.Error($"[Vohil.Dictionary]: Target database not found.", this);
				return false;
			}

			var language = LanguageManager.GetLanguage(message.Language);
			if (language == null)
			{
				this.logger.Error($"[Vohil.Dictionary]: Target language not found.", this);
				return false;
			}

			var dictionaryDomain = database.GetItem(new ID(message.DictionaryDomainId), language);
			if (dictionaryDomain == null)
			{
				this.logger.Error($"[Vohil.Dictionary]: Target dictionary domain with id '{message.DictionaryDomainId}' not found.", this);
				return false;
			}

			return true;
		}
	}
}
