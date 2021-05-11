namespace Pintle.Dictionary.DataAnnotations
{
	using Sitecore;
	using System.ComponentModel.DataAnnotations;
	using System;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class RegularExpressionTranslatedAttribute : RegularExpressionAttribute, ITranslateableAttribute
	{
		public RegularExpressionTranslatedAttribute(string pattern) : base(pattern)
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefaultPhrases(Context.Language?.Name);
			this.DictionaryKey = $"{Constants.DictionaryKeys.DefaultDataAnnotationsPath}/{nameof(defaultPhrases.RegularExpression)}";
			this.DefaultTranslation = defaultPhrases.RegularExpression;
		}

		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public override string FormatErrorMessage(string name)
		{
			return !string.IsNullOrEmpty(this.DictionaryKey)
				? DictionaryServiceFactory.GetConfiguredInstance().Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
				: this.DefaultTranslation;
		}
	}
}
