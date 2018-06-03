﻿namespace Pintle.Dictionary.Messaging
{
	using System;

	public class DictionaryItemMessage
	{
		public string DictionaryKey { get; set; }
		public string Language { get; set; }
		public string DefaultValue { get; set; }
		public Guid DictionaryDomainId { get; set; }
	}
}
