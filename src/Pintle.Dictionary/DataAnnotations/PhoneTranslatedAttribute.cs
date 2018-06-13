namespace Pintle.Dictionary.DataAnnotations
{
	using Sitecore;
	using System.ComponentModel.DataAnnotations;
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
	using System.Web.Mvc;
	using Sitecore.Configuration;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class PhoneTranslatedAttribute : DataTypeAttribute, ITranslateableAttribute, IClientValidatable
	{
		private IDictionaryService Dictionary => DictionaryServiceFactory.GetConfiguredInstance();

		public PhoneTranslatedAttribute()
			: base(DataType.PhoneNumber)
		{
		}

		private static string PhoneValidationRegex => Settings.GetSetting(
			"Pintle.Dictionary.PhoneValidationRegex",
			@"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$");

		private static readonly Regex Regex = new Regex(PhoneValidationRegex);

		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public override string FormatErrorMessage(string name)
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefautlPhrases(Context.Language?.Name);

			if (string.IsNullOrWhiteSpace(this.DictionaryKey))
			{
				this.DictionaryKey = $"{Constants.DictionaryKeys.DefaultDataAnnotationsPath}/{nameof(defaultPhrases.Phone)}";
			}

			if (string.IsNullOrWhiteSpace(this.DefaultTranslation))
			{
				this.DefaultTranslation = defaultPhrases.Phone;
			}

			return !string.IsNullOrEmpty(this.DictionaryKey)
				? this.Dictionary.Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
				: this.DefaultTranslation;
		}

		public override bool IsValid(object value)
		{
			if (value == null)
			{
				return false;
			}

			if (Regex != null && value is string input)
			{
				return Regex.IsMatch(input);
			}

			return false;
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			var errorMessage = this.FormatErrorMessage(string.Empty);

			return new[]
			{
				new ModelClientValidationRule
				{
					ErrorMessage = errorMessage,
					ValidationType = "phone"
				},
				new ModelClientValidationRegexRule(errorMessage, PhoneValidationRegex)
				{
					ValidationType = "phoneregex"
				}
			};
		}
	}
}
