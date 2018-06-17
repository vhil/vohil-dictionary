namespace Pintle.Dictionary.DataAnnotations
{
	using System;
	using System.ComponentModel;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class DisplayNameTranslatedAttribute : DisplayNameAttribute, ITranslateableAttribute
	{
		public string DictionaryKey { get; set; }
		public string DefaultTranslation { get; set; }
		public bool Editable { get; set; }

		public override string DisplayName
		{
			get
			{
				var defaultTranslation = !string.IsNullOrWhiteSpace(this.DefaultTranslation)
					? this.DefaultTranslation
					: base.DisplayName;

				return !string.IsNullOrEmpty(this.DictionaryKey)
					? DictionaryServiceFactory.GetConfiguredInstance().Translate(this.DictionaryKey, defaultTranslation, this.Editable)
					: defaultTranslation;
			}
		}
	}
}
