namespace Pintle.Dictionary.DataAnnotations
{
	using Sitecore;
	using System.ComponentModel.DataAnnotations;
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Web.Mvc;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class CreditCardTranslatedAttribute : DataTypeAttribute, ITranslateableAttribute, IClientValidatable
	{
		private IDictionaryService Dictionary => DictionaryServiceFactory.GetConfiguredInstance();

		public CreditCardTranslatedAttribute() 
			: base(DataType.CreditCard)
		{
			var defaultPhrases = DictionarySettingsFactory.ConfiguredInstance.GetDefautlPhrases(Context.Language?.Name);
			this.DefaultTranslation = defaultPhrases.CreditCard;
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

		public override bool IsValid(object value)
		{
			if (value == null)
			{
				return false;
			}

			if (!(value is string stringValue))
			{
				return false;
			}

			var source = stringValue.Replace("-", "").Replace(" ", "");
			var num1 = 0;
			var flag = false;

			foreach (char ch in source.Reverse<char>())
			{
				if ((int)ch < 48 || (int)ch > 57) return false;

				var num2 = ((int)ch - 48) * (flag ? 2 : 1);
				flag = !flag;
				while (num2 > 0)
				{
					num1 += num2 % 10;
					num2 /= 10;
				}
			}

			return num1 % 10 == 0;
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			return new[] { new ModelClientValidationRule
			{
				ErrorMessage = this.ErrorMessage,
				ValidationType = "creditcard"
			}};
		}
	}
}
