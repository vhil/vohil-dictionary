namespace Pintle.Dictionary.DataAnnotations
{
	using System.ComponentModel.DataAnnotations;
	using Sitecore;
	using System;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class RangeTranslatedAttribute : RangeAttribute, ITranslateableAttribute
	{
		private IDictionaryService Dictionary => DictionaryServiceFactory.GetConfiguredInstance();

		public RangeTranslatedAttribute(int minimum, int maximum) : base(minimum, maximum)
		{
			this.InitializeDefaultValue();
		}

		public RangeTranslatedAttribute(double minimum, double maximum) : base(minimum, maximum)
		{
			this.InitializeDefaultValue();
		}

		public RangeTranslatedAttribute(Type type, string minimum, string maximum) : base(type, minimum, maximum)
		{
			this.InitializeDefaultValue();
		}

		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		private void InitializeDefaultValue()
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefautlPhrases(Context.Language?.Name);
			this.DefaultTranslation = defaultPhrases.Range;
			this.DictionaryKey = "Validation messages/range";
		}

		public override string FormatErrorMessage(string name)
		{
			var phrase = !string.IsNullOrEmpty(this.DictionaryKey)
				? this.Dictionary.Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
				: this.DefaultTranslation;

			phrase = phrase.Replace("{0}", this.Minimum.ToString());
			phrase = phrase.Replace("{1}", this.Maximum.ToString());

			return phrase;
		}
	}
}
