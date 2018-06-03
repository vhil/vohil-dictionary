namespace Pintle.Dictionary
{
	using System;
	using Sitecore;
	using Sitecore.Caching;
	using Sitecore.Configuration;
	using Sitecore.Events;

	public class DictionaryCache : CustomCache
	{
		public DictionaryCache(string cacheName, string cacheSize)
			:base(cacheName, StringUtil.ParseSizeString(cacheSize))
		{
		}

		public DictionaryCache()
			: base("Pintle.Dictionary.Cache", Settings.Caching.DefaultDataCacheSize)
		{
			Event.Subscribe("publish:end", this.OnPublishEnd);
			Event.Subscribe("publish:end:remote", this.OnPublishEnd);
		}

		public string Get(string key, string language)
		{
			key = GenerateCacheKey(key, language);

			return this.GetString(key);
		}

		public void Set(string key, string language, string value)
		{
			key = GenerateCacheKey(key, language);

			this.SetString(key, value);
		}

		private static string GenerateCacheKey(string key, string language)
		{
			return $"{key}_{language}";
		}

		private void OnPublishEnd(object sender, EventArgs e)
		{
			this.Clear();
		}
	}
}
