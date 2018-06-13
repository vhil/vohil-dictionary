using System.Web.Mvc;

namespace Pintle.Dictionary.DataAnnotations
{
	using System.ComponentModel.DataAnnotations;
	using Sitecore;
	using System;
	using System.Collections.Generic;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class CompareTranslatedAttribute : CompareAttribute, ITranslateableAttribute, IClientValidatable
	{
		private IDictionaryService Dictionary => DictionaryServiceFactory.GetConfiguredInstance();

		public CompareTranslatedAttribute(string otherProperty) : base(otherProperty)
		{
		}

		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public override string FormatErrorMessage(string name)
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefautlPhrases(Context.Language?.Name);

			if (string.IsNullOrWhiteSpace(this.DictionaryKey))
			{
				this.DictionaryKey = $"{Constants.DictionaryKeys.DefaultDataAnnotationsPath}/{nameof(defaultPhrases.Compare)}";
			}

			if (string.IsNullOrWhiteSpace(this.DefaultTranslation))
			{
				this.DefaultTranslation = defaultPhrases.Compare;
			}

			var phrase = !string.IsNullOrEmpty(this.DictionaryKey)
				? this.Dictionary.Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
				: this.DefaultTranslation;

			phrase = phrase.Replace("{0}", name);
			phrase = phrase.Replace("{1}", this.OtherPropertyDisplayName ?? this.OtherProperty);

			return phrase;
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			yield return new ModelClientValidationEqualToRule(
				this.FormatErrorMessage(metadata.PropertyName), 
				this.FormatPropertyForClientValidation(this.OtherProperty));
		}

		protected virtual string FormatPropertyForClientValidation(string property)
		{
			return "*." + property;
		}
	}
}