namespace Pintle.Dictionary.DataAnnotations
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using Sitecore;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class RequiredTranslatedAttribute : RequiredAttribute, ITranslateableAttribute
	{
		private IDictionaryService Dictionary => DictionaryServiceFactory.GetConfiguredInstance();

		public RequiredTranslatedAttribute()
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefautlPhrases(Context.Language?.Name);
			this.DefaultTranslation = defaultPhrases.Required;
			this.DictionaryKey = "Validation messages/required";
		}

		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public override string FormatErrorMessage(string name)
		{
			return !string.IsNullOrEmpty(this.DictionaryKey)
				? this.Dictionary.Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
				: this.DefaultTranslation;
		}
	}
}