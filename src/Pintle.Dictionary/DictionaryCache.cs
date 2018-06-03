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
			Event.Subscribe("publish:end", new EventHandler(this.OnPublishEnd));
			Event.Subscribe("publish:end:remote", new EventHandler(this.OnPublishEnd));
		}

		public string Get(string key)
		{
			key = GenerateCacheKey(key);

			return this.GetString(key);
		}

		public void Set(string key, string value)
		{
			key = GenerateCacheKey(key);

			this.SetString(key, value);
		}

		private static string GenerateCacheKey(string key)
		{
			return $"{key}_{Sitecore.Context.Language?.Name}";
		}

		private void OnPublishEnd(object sender, EventArgs e)
		{
			this.Clear();
		}
	}
}
