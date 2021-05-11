namespace Vohil.Dictionary.DataAnnotations
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
		public EmailAddressTranslatedAttribute() : base(DataType.EmailAddress)
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefaultPhrases(Context.Language?.Name);
			this.DictionaryKey = $"{Constants.DictionaryKeys.DefaultDataAnnotationsPath}/{nameof(defaultPhrases.EmailAddress)}";
			this.DefaultTranslation = defaultPhrases.EmailAddress;
		}

		private static string EmailValidationRegex => Settings.GetSetting(
			"Vohil.Dictionary.EmailValidationRegex",
			@"^[a-zA-Z0-9_-]+(?:\.[a-zA-Z0-9_-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$");

		private static readonly Regex Regex = new Regex(EmailValidationRegex);

		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public override string FormatErrorMessage(string name)
		{
			return !string.IsNullOrEmpty(this.DictionaryKey)
				? DictionaryServiceFactory.GetConfiguredInstance().Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
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
			var errorMessage = this.FormatErrorMessage(metadata.DisplayName);

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