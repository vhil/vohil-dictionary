namespace Pintle.Dictionary.DataAnnotations
{
	using System.ComponentModel.DataAnnotations;
	using Sitecore;
	using System;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class CompareTranslatedAttribute : CompareAttribute, ITranslateableAttribute
	{
		private IDictionaryService Dictionary => DictionaryServiceFactory.GetConfiguredInstance();

		public CompareTranslatedAttribute(string otherProperty) : base(otherProperty)
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefautlPhrases(Context.Language?.Name);
			this.DefaultTranslation = defaultPhrases.Compare;
			this.DictionaryKey = "Validation messages/compare";
		}

		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public override string FormatErrorMessage(string name)
		{
			var phrase = !string.IsNullOrEmpty(this.DictionaryKey)
				? this.Dictionary.Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
				: this.DefaultTranslation;

			phrase = phrase.Replace("{0}", name);
			phrase = phrase.Replace("{1}", this.OtherPropertyDisplayName ?? this.OtherProperty);

			return phrase;
		}
	}
}