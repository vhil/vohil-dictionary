namespace Vohil.Dictionary
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
			Factory.CreateObject("vohil/dictionary/dictionarySettingsFactory", true) as DictionarySettingsFactory;

		public virtual DictionarySettings DictionarySettings 
			=> Factory.CreateObject("vohil/dictionary/settings", true) as DictionarySettings;

		public virtual DictionaryIconSettings DictionaryIconSettings 
			=> Factory.CreateObject("vohil/dictionary/iconSettings", true) as DictionaryIconSettings;

		public virtual DataAnnotationDefaultPhrases GetDefaultPhrases(string language)
		{
			var defaultConfigNode = "vohil/dictionary/defaultPhrases";

			var languageConfigNode = !string.IsNullOrWhiteSpace(language) 
				? $"{defaultConfigNode}.{language.Trim().ToUpper().Replace("-", string.Empty)}"
				: defaultConfigNode;

			var settings = Factory.CreateObject(languageConfigNode, false) as DataAnnotationDefaultPhrases;

			if (settings == null)
			{
				this.Logger.Warn(
					$"[Vohil.Dictionary]: Unable to find default data annotation settings for target language '{language}' " +
					$"in configuration. Default phrase settings from node '{defaultConfigNode}' will be used instead. " +
					$"Please make sure configuration node by path '{languageConfigNode}' exists and properly configured.", this);

				settings = Factory.CreateObject(defaultConfigNode, false) as DataAnnotationDefaultPhrases;
			}

			if (settings == null)
			{
				throw new ConfigurationException($"[Vohil.Dictionary]: Default phrases are not configured. Please make sure configuration node by path '{defaultConfigNode}' exists and properly configured.");
			}

			return settings;
		}

	}
}
