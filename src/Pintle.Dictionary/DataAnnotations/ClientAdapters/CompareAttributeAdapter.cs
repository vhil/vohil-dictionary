using System.Collections.Generic;
using System.Web.Mvc;

namespace Pintle.Dictionary.DataAnnotations.ClientAdapters
{
	public class CompareAttributeAdapter : DataAnnotationsModelValidator<CompareTranslatedAttribute>
	{
		public CompareAttributeAdapter(
			ModelMetadata metadata, 
			ControllerContext context, 
			CompareTranslatedAttribute attribute)
		  : base(metadata, context, new CompareAttributeWrapper(attribute, metadata))
		{
		}

		public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
		{
			yield return new ModelClientValidationEqualToRule(this.ErrorMessage, FormatPropertyForClientValidation(this.Attribute.OtherProperty));
		}

		private static string FormatPropertyForClientValidation(string property)
		{
			return "*." + property;
		}

		private sealed class CompareAttributeWrapper : CompareTranslatedAttribute
		{
			private IDictionaryService Dictionary => DictionaryServiceFactory.GetConfiguredInstance();

			private readonly string otherPropertyDisplayName;

			public CompareAttributeWrapper(CompareTranslatedAttribute attribute, ModelMetadata metadata)
			  : base(attribute.OtherProperty)
			{
				this.otherPropertyDisplayName = attribute.OtherPropertyDisplayName;
				if (this.otherPropertyDisplayName == null && metadata.ContainerType != null)
					this.otherPropertyDisplayName = ModelMetadataProviders.Current.GetMetadataForProperty(() => metadata.Model, metadata.ContainerType, attribute.OtherProperty).GetDisplayName();
				if (this.otherPropertyDisplayName == null)
					this.otherPropertyDisplayName = attribute.OtherProperty;
				if (string.IsNullOrEmpty(attribute.ErrorMessage) && string.IsNullOrEmpty(attribute.ErrorMessageResourceName) && !(attribute.ErrorMessageResourceType != null))
					return;

				this.ErrorMessage = attribute.ErrorMessage;
				this.ErrorMessageResourceName = attribute.ErrorMessageResourceName;
				this.ErrorMessageResourceType = attribute.ErrorMessageResourceType;
			}

			public override string FormatErrorMessage(string name)
			{
				var phrase = !string.IsNullOrEmpty(this.DictionaryKey)
					? this.Dictionary.Translate(this.DictionaryKey, this.DefaultTranslation, this.Editable)
					: this.DefaultTranslation;

				phrase = phrase.Replace("{0}", name);
				phrase = phrase.Replace("{1}", this.otherPropertyDisplayName);

				return phrase;
			}
		}
	}
}