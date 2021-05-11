using System.Web.Mvc;

namespace Vohil.Dictionary.DataAnnotations
{
	using System.ComponentModel.DataAnnotations;
	using Sitecore;
	using System;
	using System.Collections.Generic;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class CompareTranslatedAttribute : CompareAttribute, ITranslateableAttribute, IClientValidatable
	{
		public CompareTranslatedAttribute(string otherProperty) : base(otherProperty)
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefaultPhrases(Context.Language?.Name);
			this.DictionaryKey = $"{Constants.DictionaryKeys.DefaultDataAnnotationsPath}/{nameof(defaultPhrases.Compare)}";
			this.DefaultTranslation = defaultPhrases.Compare;
		}

		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public override string FormatErrorMessage(string name)
		{
			var phrase = !string.IsNullOrEmpty(this.DictionaryKey)
				? DictionaryServiceFactory.GetConfiguredInstance().Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
				: this.DefaultTranslation;

			phrase = phrase.Replace("{0}", name);
			phrase = phrase.Replace("{1}", this.OtherPropertyDisplayName ?? this.OtherProperty);

			return phrase;
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			yield return new ModelClientValidationEqualToRule(
				this.FormatErrorMessage(metadata.DisplayName), 
				this.FormatPropertyForClientValidation(this.OtherPropertyDisplayName));
		}

		protected virtual string FormatPropertyForClientValidation(string property)
		{
			return "*." + property;
		}
	}
}