namespace Pintle.Dictionary.DataAnnotations
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using Sitecore;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class StringLengthTranslatedAttribute : StringLengthAttribute, ITranslateableAttribute
	{
		private IDictionaryService Dictionary => DictionaryServiceFactory.GetConfiguredInstance();

		public StringLengthTranslatedAttribute(int maximumLength) : base(maximumLength)
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefautlPhrases(Context.Language?.Name);
			this.DefaultTranslation = defaultPhrases.StringLength;
			this.DictionaryKey = "Validation messages/string length";
		}

		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public override string FormatErrorMessage(string name)
		{
			var phrase = !string.IsNullOrEmpty(this.DictionaryKey)
				? this.Dictionary.Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
				: this.DefaultTranslation;

			phrase = phrase.Replace("{0}", this.MinimumLength.ToString());
			phrase = phrase.Replace("{1}", this.MaximumLength.ToString());

			return phrase;
		}
	}
}
