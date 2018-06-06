namespace Pintle.Dictionary
{
	using System;
	using Sitecore;
	using Sitecore.Abstractions;
	using Sitecore.Data.Items;
	using Sitecore.Data.Managers;
	using Sitecore.Web.UI.WebControls;

	public class DictionaryService : IDictionaryService
	{
		private readonly DictionaryItemRepository repository;
		private readonly SitecoreDictionarySettings setting;
		private readonly DictionaryCache cache;
		private readonly BaseLog logger;

		public DictionaryService(
			DictionaryItemRepository repository,
			SitecoreDictionarySettings setting,
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
				language = language ?? Context.Language.Name;

				if (this.IsInEditingMode)
				{
					return this.Process(key, defaultValue, editable, language);
				}

				var value = this.cache.Get(key, language);

				if (value == null || value.Equals(key, StringComparison.OrdinalIgnoreCase))
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

				// if in editing mode - get or create item and render editable
				var localizedString = this.IsInEditingMode && editable
					? this.GetOrCreateAndRender(key, defaultValue, true, language)
					: this.TranslateText(key, language);

				// try clear cache and get again
				if (localizedString.Equals(key, StringComparison.OrdinalIgnoreCase))
				{
					Sitecore.Globalization.Translate.RemoveKeyFromCache(key);
					localizedString = this.TranslateText(key, language);
				}

				// try reset cache and get again
				if (localizedString.Equals(key, StringComparison.OrdinalIgnoreCase))
				{
					Sitecore.Globalization.Translate.ResetCache(true);
					localizedString = this.TranslateText(key, language);
				}

				// create item if does not exist
				if (localizedString.Equals(key, StringComparison.OrdinalIgnoreCase))
				{
					localizedString = this.GetOrCreateAndRender(key, defaultValue, editable, language);
				}

				if (!this.IsInEditingMode && !string.IsNullOrWhiteSpace(defaultValue)
					 && (string.IsNullOrWhiteSpace(localizedString) || 
						localizedString.Equals(key, StringComparison.OrdinalIgnoreCase)))
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

		private string GetOrCreateAndRender(string key, string defaultValue, bool editable, string language)
		{
			string localizedString;

			var item = this.repository.Get(key, language);
			if (item == null)
			{
				this.repository.Create(key, defaultValue, language);
				localizedString = defaultValue;
			}
			else
			{
				localizedString = this.RenderPhraseItem(key, item, editable);
			}

			return localizedString;
		}

		private string RenderPhraseItem(string key, Item item, bool editable)
		{
			if (this.IsInEditingMode && editable)
			{
				if (item != null)
				{
					var renderer = new FieldRenderer
					{
						Item = item,
						FieldName = this.setting.DictionaryPhraseFieldName
					};

					return renderer.Render();
				}
			}

			return item?[this.setting.DictionaryPhraseFieldName] ?? key;
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

		protected virtual bool IsInEditingMode => Context.PageMode.IsExperienceEditor || Context.PageMode.IsExperienceEditorEditing;
	}
}
