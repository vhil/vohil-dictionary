namespace Pintle.Dictionary
{
	using System;

	public class SitecoreDictionarySettings
	{
		public virtual Guid DictionaryFolderTemplateId { get; protected set; }
		public virtual Guid DictionaryPhraseTemplateId { get; protected set; }
		public virtual Guid DictionaryKeyFieldId { get; protected set; }
		public virtual Guid DictionaryPhraseFieldId { get; protected set; }
		public virtual string ItemCreationDatabase { get; protected set; }
	}
}