namespace Vohil.Dictionary.DataAnnotations
{
	using System.ComponentModel.DataAnnotations;
	using Sitecore;
	using System;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class RangeTranslatedAttribute : RangeAttribute, ITranslateableAttribute
	{
		public RangeTranslatedAttribute(int minimum, int maximum) : base(minimum, maximum)
		{
			this.Initialize();
		}

		public RangeTranslatedAttribute(double minimum, double maximum) : base(minimum, maximum)
		{
			this.Initialize();
		}

		public RangeTranslatedAttribute(Type type, string minimum, string maximum) : base(type, minimum, maximum)
		{
			this.Initialize();
		}

		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public override string FormatErrorMessage(string name)
		{
			var phrase = !string.IsNullOrEmpty(this.DictionaryKey)
				? DictionaryServiceFactory.GetConfiguredInstance().Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
				: this.DefaultTranslation;

			phrase = phrase.Replace("{0}", this.Minimum.ToString());
			phrase = phrase.Replace("{1}", this.Maximum.ToString());

			return phrase;
		}

		private void Initialize()
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefaultPhrases(Context.Language?.Name);
			this.DictionaryKey = $"{Constants.DictionaryKeys.DefaultDataAnnotationsPath}/{nameof(defaultPhrases.Range)}";
			this.DefaultTranslation = defaultPhrases.Range;
		}
	}
}
