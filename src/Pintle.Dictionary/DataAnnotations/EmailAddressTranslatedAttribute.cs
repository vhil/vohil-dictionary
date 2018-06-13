namespace Pintle.Dictionary.DataAnnotations
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.Text.RegularExpressions;
	using System.Web.Mvc;
	using Sitecore;
	using Sitecore.Configuration;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class EmailAddressTranslatedAttribute : DataTypeAttribute, ITranslateableAttribute, IClientValidatable
	{
		private IDictionaryService Dictionary => DictionaryServiceFactory.GetConfiguredInstance();

		public EmailAddressTranslatedAttribute() : base(DataType.EmailAddress)
		{
		}

		private static string EmailValidationRegex => Settings.GetSetting(
			"Pintle.Dictionary.EmailValidationRegex",
			@"^[a-zA-Z0-9_-]+(?:\.[a-zA-Z0-9_-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$");

		private static readonly Regex Regex = new Regex(EmailValidationRegex);

		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public override string FormatErrorMessage(string name)
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefautlPhrases(Context.Language?.Name);

			if (string.IsNullOrWhiteSpace(this.DictionaryKey))
			{
				this.DictionaryKey = $"{Constants.DictionaryKeys.DefaultDataAnnotationsPath}/{nameof(defaultPhrases.EmailAddress)}";
			}

			if (string.IsNullOrWhiteSpace(this.DefaultTranslation))
			{
				this.DefaultTranslation = defaultPhrases.EmailAddress;
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
				if (input.Contains("@"))
				{
					var localPart = input.Substring(0, input.IndexOf("@", StringComparison.InvariantCulture));
					if (localPart.Length > 64) return false;
				}

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
					ValidationType = "email"
				},
				new ModelClientValidationRegexRule(errorMessage, EmailValidationRegex)
				{
					ValidationType = "emailregex"
				}
			};
		}
	}
}