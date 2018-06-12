namespace Pintle.Dictionary.Pipelines.Initialize
{
	using System.Web.Mvc;
	using DataAnnotations;
	using DataAnnotations.ClientAdapters;
	using Sitecore.Pipelines;

	public class RegisterCustomDataAnnotations
	{
		public void Process(PipelineArgs args)
		{
			DataAnnotationsModelValidatorProvider.RegisterAdapter(
				typeof(CompareTranslatedAttribute),
				typeof(CompareAttributeAdapter));

			DataAnnotationsModelValidatorProvider.RegisterAdapter(
				typeof(RequiredTranslatedAttribute),
				typeof(RequiredAttributeAdapter));

			DataAnnotationsModelValidatorProvider.RegisterAdapter(
				typeof(StringLengthTranslatedAttribute),
				typeof(StringLengthAttributeAdapter));

			DataAnnotationsModelValidatorProvider.RegisterAdapter(
				typeof(RangeTranslatedAttribute),
				typeof(RangeAttributeAdapter));

			DataAnnotationsModelValidatorProvider.RegisterAdapter(
				typeof(RegularExpressionTranslatedAttribute),
				typeof(RegularExpressionAttributeAdapter));
		}
	}
}