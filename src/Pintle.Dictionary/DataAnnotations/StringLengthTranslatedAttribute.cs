namespace Pintle.Dictionary.DataAnnotations
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using Sitecore;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class StringLengthTranslatedAttribute : StringLengthAttribute, ITranslateableAttribute
	{
		public StringLengthTranslatedAttribute(int maximumLength) : base(maximumLength)
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefaultPhrases(Context.Language?.Name);
			this.DictionaryKey = $"{Constants.DictionaryKeys.DefaultDataAnnotationsPath}/{nameof(defaultPhrases.StringLength)}";
			this.DefaultTranslation = defaultPhrases.StringLength;
		}

		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public override string FormatErrorMessage(string name)
		{
			var phrase = !string.IsNullOrEmpty(this.DictionaryKey)
				? DictionaryServiceFactory.GetConfiguredInstance().Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
				: this.DefaultTranslation;

			phrase = phrase.Replace("{0}", this.MinimumLength.ToString());
			phrase = phrase.Replace("{1}", this.MaximumLength.ToString());

			return phrase;
		}
	}
}
