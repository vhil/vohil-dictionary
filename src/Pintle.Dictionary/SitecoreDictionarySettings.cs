namespace Pintle.Dictionary
{
	using System;

	public class SitecoreDictionarySettings
	{
		public virtual Guid DictionaryDomainTemplateId { get; protected set; }
		public virtual Guid DictionaryFolderTemplateId { get; protected set; }
		public virtual Guid DictionaryPhraseTemplateId { get; protected set; }
		public virtual string DictionaryKeyFieldName { get; protected set; }
		public virtual string DictionaryPhraseFieldName { get; protected set; }
		public virtual string ItemCreationDatabase { get; protected set; }
	}
}