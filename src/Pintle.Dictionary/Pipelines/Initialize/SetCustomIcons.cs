namespace Pintle.Dictionary.Pipelines.Initialize
{
	using Sitecore.Pipelines;
	using System;
	using Sitecore.Abstractions;
	using Sitecore;
	using Sitecore.Data;
	using Sitecore.Data.Items;
	using Sitecore.SecurityModel;

	public class SetCustomIcons
	{
		private readonly DictionarySettings settings;
		private readonly DictionaryIconSettings iconSettings;
		private readonly BaseLog logger;

		public SetCustomIcons(
			DictionarySettings settings,
			DictionaryIconSettings iconSettings,
			BaseLog logger)
		{
			this.settings = settings;
			this.iconSettings = iconSettings;
			this.logger = logger;
		}

		public void Process(PipelineArgs args)
		{
			try
			{
				this.SetCustomIcon(this.settings.DictionaryDomainTemplateId, this.iconSettings.DictionaryDomainIcon);
				this.SetCustomIcon(this.settings.DictionaryFolderTemplateId, this.iconSettings.DictionaryFolderIcon);
				this.SetCustomIcon(this.settings.DictionaryPhraseTemplateId, this.iconSettings.DictionaryPhraseIcon);
			}
			catch(Exception ex)
			{
				this.logger.Error($"[Pintle.Dictionary]: Unable to set custom icons", ex, this);
			}
		}

		private void SetCustomIcon(Guid templateId, string icon)
		{
			var database = Database.GetDatabase(this.settings.ItemCreationDatabase);
			var template = database.GetItem(new ID(templateId));

			if (template[FieldIDs.Icon].Equals(icon, StringComparison.OrdinalIgnoreCase)) return;

			using (new SecurityDisabler())
			using (new EditContext(template))
			{
				template[FieldIDs.Icon] = icon;
			}

			this.logger.Debug($"[Pintle.Dictionary]: The icon on template '{template.Paths.FullPath}' changed to '{icon}'.", this);
		}
	}
}
