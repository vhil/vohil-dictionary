using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Web.UI.WebControls;

namespace Pintle.Dictionary
{
	using System;
	using Sitecore.Abstractions;

	public class DictionaryService : IDictionaryService
	{
		private readonly DictionaryItemRepository repository;
		private readonly SitecoreDictionarySettings setting;
		private readonly DictionaryCache cache;
		private readonly BaseLog logger;

		public DictionaryService(
			DictionaryItemRepository repository,
			SitecoreDictionarySettings seStting,
			DictionaryCache cache, 
			BaseLog logger)
		{
			this.repository = repository;
			this.setting = setting;
			this.cache = cache;
			this.logger = logger;
		}

		public string Translate(
			string key, 
			string defaultValue = null, 
			bool editable = false, 
			string language = null)
		{
			try
			{
				language = language ?? Sitecore.Context.Language.Name;

				if (this.IsInEditingMode)
				{
					return this.Process(key, defaultValue, editable, language);
				}

				var value = this.cache.Get(key, language);

				if (value == null)
				{
					value = this.Process(key, defaultValue, editable, language);
					this.cache.Set(key, language, value);
				}

				return value;
			}
			catch
			{
				return Sitecore.Globalization.Translate.Text(key);
			}
		}

		private string Process(string key, string defaultValue, bool editable, string language)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(key))
				{
					return defaultValue ?? key;
				}

				string localizedString;


				var item = this.GetDictionaryPhraseItem(key, language);
				if (item == null)
				{
					this.repository.Create(key, defaultValue, language);
				}

				if (this.IsInEditingMode && editable)
				{
					
					if (item != null)
					{
						return new FieldRenderer { Item = item, FieldName = this.DictionaryPhraseFieldName }.Render();
					}
				}

				Sitecore.Globalization.Translate.RemoveKeyFromCache(key);
				localizedString = this.TranslateText(key, language);

				if (localizedString.Equals(key, StringComparison.InvariantCultureIgnoreCase))
				{
					Sitecore.Globalization.Translate.ResetCache(true);
					localizedString = this.TranslateText(key, language);
				}

				if (localizedString.Equals(key, StringComparison.InvariantCultureIgnoreCase))
				{
					localizedString = this.GetOrCreateDictionaryText(key, defaultValue, editable, language);
				}

				if (!editable || !this.IsInEditingMode)
				{
					localizedString = this.TranslateText(key, language);
				}

				if (!this.IsInEditingMode && !string.IsNullOrWhiteSpace(defaultValue) &&
				    (string.IsNullOrWhiteSpace(localizedString) ||
				     localizedString.Equals(key, StringComparison.InvariantCultureIgnoreCase)))
				{
					localizedString = defaultValue;
				}

				return localizedString;
			}
			catch (Exception ex)
			{
				this.logger.Error($"[Pintle.Dictionary]: Error while translating phrase with key '{key}'" + ex.Message, ex, this);

				return Sitecore.Globalization.Translate.Text(key);
			}
		}

		private Item GetDictionaryPhraseItem(string key, string language)
		{
			throw new NotImplementedException();
		}

		protected virtual string TranslateText(string key, string language)
		{
			if (string.IsNullOrEmpty(language))
			{
				return Sitecore.Globalization.Translate.Text(key);
			}

			var lang = LanguageManager.GetLanguage(language);
			if (lang != null)
			{
				return Sitecore.Globalization.Translate.TextByLanguage(key, lang);
			}

			return Sitecore.Globalization.Translate.Text(key);
		}

		protected virtual bool IsInEditingMode => 
			Sitecore.Context.PageMode.IsExperienceEditor 
			|| Sitecore.Context.PageMode.IsExperienceEditorEditing;
	}
}
