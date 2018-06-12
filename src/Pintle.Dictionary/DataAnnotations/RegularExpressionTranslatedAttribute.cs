namespace Pintle.Dictionary.DataAnnotations
{
	using Sitecore;
	using System.ComponentModel.DataAnnotations;
	using System;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class RegularExpressionTranslatedAttribute : RegularExpressionAttribute, ITranslateableAttribute
	{
		private IDictionaryService Dictionary => DictionaryServiceFactory.GetConfiguredInstance();

		public RegularExpressionTranslatedAttribute(string pattern) : base(pattern)
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefautlPhrases(Context.Language?.Name);
			this.DefaultTranslation = defaultPhrases.RegularExpression;
			this.DictionaryKey = "Validation messages/regular expression";
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
