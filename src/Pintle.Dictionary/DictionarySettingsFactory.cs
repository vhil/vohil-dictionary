namespace Pintle.Dictionary
{
	using Sitecore.Abstractions;
	using Sitecore.Configuration;
	using Sitecore.Exceptions;

	public class DictionarySettingsFactory
	{
		protected readonly BaseLog Logger;

		public DictionarySettingsFactory(BaseLog logger)
		{
			this.Logger = logger;
		}

		public static DictionarySettingsFactory ConfiguredInstance =>
			Factory.CreateObject("pintle/dictionary/dictionarySettingsFactory", true) as DictionarySettingsFactory;

		public virtual DictionarySettings DictionarySettings 
			=> Factory.CreateObject("pintle/dictionary/settings", true) as DictionarySettings;

		public virtual DictionaryIconSettings DictionaryIconSettings 
			=> Factory.CreateObject("pintle/dictionary/iconSettings", true) as DictionaryIconSettings;

		public virtual DataAnnotationDefaultPhrases GetDefautlPhrases(string language)
		{
			var defaultConfigNode = "pintle/dictionary/defaultPhrases";

			var languageConfigNode = !string.IsNullOrWhiteSpace(language) 
				? $"{defaultConfigNode}.{language.Trim().ToUpper().Replace("-", string.Empty)}"
				: defaultConfigNode;

			var settings = Factory.CreateObject(languageConfigNode, false) as DataAnnotationDefaultPhrases;

			if (settings == null)
			{
				this.Logger.Warn(
					$"Unable to find default data annotation settings for target language '{language}' " +
					$"in configuration. Default phrase settings from node '{defaultConfigNode}' will be used instead. " +
					$"Please make sure configuration node by path '{languageConfigNode}' exists and properly configured.", this);

				settings = Factory.CreateObject(defaultConfigNode, false) as DataAnnotationDefaultPhrases;
			}

			if (settings == null)
			{
				throw new ConfigurationException($"Default phrases are not configured. Please make sure configuration node by path '{defaultConfigNode}' exists and properly configured.");
			}

			return settings;
		}

	}
}
