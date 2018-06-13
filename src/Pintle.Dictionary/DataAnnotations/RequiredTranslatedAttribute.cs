namespace Pintle.Dictionary.DataAnnotations
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using Sitecore;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class RequiredTranslatedAttribute : RequiredAttribute, ITranslateableAttribute
	{
		private IDictionaryService Dictionary => DictionaryServiceFactory.GetConfiguredInstance();

		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public override string FormatErrorMessage(string name)
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefautlPhrases(Context.Language?.Name);

			if (string.IsNullOrWhiteSpace(this.DictionaryKey))
			{
				this.DictionaryKey = $"{Constants.DictionaryKeys.DefaultDataAnnotationsPath}/{nameof(defaultPhrases.Required)}";
			}

			if (string.IsNullOrWhiteSpace(this.DefaultTranslation))
			{
				this.DefaultTranslation = defaultPhrases.Required;
			}

			return !string.IsNullOrEmpty(this.DictionaryKey)
				? this.Dictionary.Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
				: this.DefaultTranslation;
		}
	}
}