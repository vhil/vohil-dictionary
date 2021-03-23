namespace Pintle.Dictionary.DataAnnotations
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using Sitecore;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class RequiredTranslatedAttribute : RequiredAttribute, ITranslateableAttribute
	{
		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public RequiredTranslatedAttribute()
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefaultPhrases(Context.Language?.Name);
			this.DictionaryKey = $"{Constants.DictionaryKeys.DefaultDataAnnotationsPath}/{nameof(defaultPhrases.Required)}";
			this.DefaultTranslation = defaultPhrases.Required;
		}

		public override string FormatErrorMessage(string name)
		{
			return !string.IsNullOrEmpty(this.DictionaryKey)
				? DictionaryServiceFactory.GetConfiguredInstance().Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
				: this.DefaultTranslation;
		}
	}
}