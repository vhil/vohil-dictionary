namespace Pintle.Dictionary.Messaging
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using Extensions;
	using Sitecore.Abstractions;
	using Sitecore.Data;
	using Sitecore.Data.Items;
	using Sitecore.Data.Managers;
	using Sitecore.Framework.Messaging;
	using Sitecore.Globalization;
	using Sitecore.SecurityModel;
	using System.Collections.Generic;
	using Sitecore.Publishing;

	public class DictionaryItemMessageHandler : IMessageHandler<DictionaryItemMessage>
	{
		private readonly SitecoreDictionarySettings settings;
		private readonly BaseLog logger;

		public DictionaryItemMessageHandler(
			SitecoreDictionarySettings settings,
			BaseLog logger)
		{
			this.settings = settings;
			this.logger = logger;
		}

		public Task Handle(
			DictionaryItemMessage message,
			IMessageReceiveContext receiveContext,
			IMessageReplyContext replyContext)
		{
			if (this.ValidateMessage(message))
			{
				var database = Database.GetDatabase(message.Database);
				var language = LanguageManager.GetLanguage(message.Language);

				using (new LanguageSwitcher(language))
				{
					var phrasePath = GetPhrasePath(message, database, language);

					var folderTemplate = database.GetTemplate(new ID(this.settings.DictionaryFolderTemplateId));
					var phraseTemplate = database.GetTemplate(new ID(this.settings.DictionaryPhraseTemplateId));

					using (new SecurityDisabler())
					{
						var phraseItem = database.CreateItemPath(phrasePath, folderTemplate, phraseTemplate);

						using (new EditContext(phraseItem))
						{
							phraseItem[new ID(this.settings.DictionaryKeyFieldId)] = message.DictionaryKey;
							phraseItem[new ID(this.settings.DictionaryPhraseFieldId)] = message.DefaultValue;
						}

						this.logger.Debug(
								$"[Pintle.Dictionary]: Dictionary phrase item with key '{message.DictionaryKey}' " +
								$"has been created with default value '{message.DefaultValue}'. " +
								$"Language: '{phraseItem.Language.Name}', " +
								$"item path: '{phraseItem.Paths.FullPath}'",
							this);

						this.PublishItem(phraseItem, message);
					}
				}
			}

			return Task.CompletedTask;
		}

		protected virtual void PublishItem(Item item, DictionaryItemMessage message)
		{
			var toPublishList = new List<Item>();
			var dictionaryRoot = GetDictionaryDomainItem(message, item.Database, item.Language);

			if (dictionaryRoot != null)
			{
				var publishingItem = item;

				while (publishingItem.ID != dictionaryRoot.ID)
				{
					toPublishList.Add(publishingItem);
					publishingItem = publishingItem.Parent;
				}

				toPublishList.Reverse();

				foreach (var toPublish in toPublishList)
				{
					PublishManager.PublishItem(toPublish, PublishingTargets(item.Database), new[] { toPublish.Language }, false, false);

					this.logger.Debug(
							$"[Pintle.Dictionary]: Dictionary item is being published... Language: '{item.Language.Name}', " +
							$"dictionary item path: '{item.Paths.FullPath}'",
						this);
				}
			}
			else
			{
				PublishManager.PublishItem(item, PublishingTargets(item.Database), new[] { item.Language }, false, false);

				this.logger.Debug(
					$"[Pintle.Dictionary]: Dictionary item is being published... Language: '{item.Language.Name}', " +
					$"dictionary item path: '{item.Paths.FullPath}'",
					this);
			}
		}

		private static Database[] PublishingTargets(Database database)
		{
			var targets = new List<Database>();

			var publishingTargetsRoot = database.GetItem("{D9E44555-02A6-407A-B4FC-96B9026CAADD}");
			foreach (Item target in publishingTargetsRoot.Children)
			{
				var value = target[ID.Parse("{39ECFD90-55D2-49D8-B513-99D15573DE41}")];
				if (!string.IsNullOrWhiteSpace(value))
				{
					var db = Database.GetDatabase(value);
					if (db != null)
					{
						targets.Add(db);
					}
				}
			}

			return targets.ToArray();
		}

		private static string GetPhrasePath(DictionaryItemMessage message, Database database, Language language)
		{
			var splittedValue = message.DictionaryKey.Split(new[] { ".", "/" }, StringSplitOptions.RemoveEmptyEntries);
			var relativeItemPath = string.Join("/", splittedValue.Select(x => ItemUtil.ProposeValidItemName(x).ToPascalCase()));
			return GetDictionaryDomainItem(message, database, language).Paths.FullPath + "/" + relativeItemPath;
		}

		private static Item GetDictionaryDomainItem(DictionaryItemMessage message, Database database, Language language)
		{
			var dictionaryDomain = database.GetItem(new ID(message.DictionaryDomainId), language);
			if (dictionaryDomain == null)
			{
				return database.GetItem("/sitecore/system/Dictionary/", language);
			}

			return dictionaryDomain;
		}

		private bool ValidateMessage(DictionaryItemMessage message)
		{
			if (message == null)
			{
				this.logger.Error($"[Pintle.Dictionary]: Message is null.", this);
				return false;
			}

			if (string.IsNullOrWhiteSpace(message.DictionaryKey))
			{
				this.logger.Error($"[Pintle.Dictionary]: Dictionary keyis null or empty.", this);
				return false;
			}

			var database = Database.GetDatabase(message.Database);
			if (database == null)
			{
				this.logger.Error($"[Pintle.Dictionary]: Target database not found.", this);
				return false;
			}

			var language = LanguageManager.GetLanguage(message.Language);
			if (language == null)
			{
				this.logger.Error($"[Pintle.Dictionary]: Target language not found.", this);
				return false;
			}

			var dictionaryDomain = database.GetItem(new ID(message.DictionaryDomainId), language);
			if (dictionaryDomain == null)
			{
				this.logger.Error($"[Pintle.Dictionary]: Target dictionary domain with id '{message.DictionaryDomainId}' not found.", this);
				return false;
			}

			return true;
		}
	}
}
