namespace Pintle.Dictionary.Events
{
	using System;

	public class DictionaryCacheClearer
	{
		private readonly DictionaryCache cache;

		public DictionaryCacheClearer(DictionaryCache cache)
		{
			this.cache = cache;
		}

		public void ClearCache(object sender, EventArgs args)
		{
			this.cache?.Clear();
		}
	}
}
