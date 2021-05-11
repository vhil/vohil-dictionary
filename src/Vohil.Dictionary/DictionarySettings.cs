namespace Vohil.Dictionary
{
	using System;

	public class DictionarySettings
	{
		public virtual Guid DictionaryDomainTemplateId { get; protected set; }
		public virtual Guid DictionaryFolderTemplateId { get; protected set; }
		public virtual Guid DictionaryPhraseTemplateId { get; protected set; }
		public virtual string DictionaryKeyFieldName { get; protected set; }
		public virtual string DictionaryPhraseFieldName { get; protected set; }
		public virtual string ItemCreationDatabase { get; protected set; }
	}
}