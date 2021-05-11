namespace Vohil.Dictionary
{
	using System;
	using Sitecore;
	using Sitecore.Caching;
	using Sitecore.Configuration;
	using Sitecore.Events;

	public class DictionaryCache : CustomCache
	{
        private readonly DictionarySettings _settings;
		public DictionaryCache(string cacheName, string cacheSize, DictionarySettings settings)
			:base(cacheName, StringUtil.ParseSizeString(cacheSize))
		{
            this._settings = settings;
		}

		public DictionaryCache(DictionarySettings settings)
			: base("Vohil.Dictionary.Cache", Settings.Caching.DefaultDataCacheSize)
		{
			Event.Subscribe("publish:end", this.OnPublishEnd);
			Event.Subscribe("publish:end:remote", this.OnPublishEnd);
			this._settings = settings;
		}

		public string Get(string key, string language)
		{
            key = _settings.SiteAwareCaches ? GenerateCacheKeySiteAware(key, language) : GenerateCacheKey(key, language);

			return this.GetString(key);
		}

		public void Set(string key, string language, string value)
		{
				key = _settings.SiteAwareCaches ? GenerateCacheKeySiteAware(key, language) : GenerateCacheKey(key, language);

			this.SetString(key, value);
		}

		private static string GenerateCacheKey(string key, string language)
        {

            return $"{key}_{language}";
		}

        private static string GenerateCacheKeySiteAware(string key, string language)
        {
            return $"{Sitecore.Context.GetSiteName()}_{key}_{language}";
        }

		private void OnPublishEnd(object sender, EventArgs e)
		{
			this.Clear();
		}
	}
}
