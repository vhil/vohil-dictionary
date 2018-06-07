# Pintle.Dictionary

Pintle.Dictionary is a Sitecore CMS module which provides an abstracted dictionary service for Sitecore 9.x CMS.

Functionality and features
 * designed for Sitecore 9.x CMS and later
 * uses default sitecore Translate mechanism
 * automatic dictionary phrase items creation (through Sitecore service bus)
 * mvc helper extensions for fast access
 * code first approach for dictionary phrase items
 * experience editor support for editable dictionary phrases

The module is a set of NuGet packages that can be used in your solution:
 * [Pintle.Dictionary.Core](https://www.nuget.org/packages/Pintle.Dictionary.Core "Pintle.Dictionary.Core")
 * [Pintle.Dictionary](https://www.nuget.org/packages/Pintle.Dictionary "Pintle.Dictionary")

# Getting started

## Install nuget packages

Within helix modular architecture:
 * `Install-Package Pintle.Dictionary` nuget package for your project layer (includes config files)
 * `Install-Package Pintle.Dictionary.Core` nuget package for your feature or foundation layer module

## Configuration

1. If required, create a dedicated dictionary domain item for your website using default Dictionary domain template:
```
/sitecore/templates/System/Dictionary/Dictionary Domain
```

2. Set dictionary domain ID's to website configuration node (if required). For example default website node can be patched:
```xml
<site name="website">
	<patch:attribute name="dictionaryDomain">{5FE5BF78-AC6B-4AF2-92A9-9F4AE14579C8}</patch:attribute>
</site>
```
## Using dictionary translation in Razor View:
 
```cs
@using Pintle.Dictionary.Extensions
...
@Html.Sitecore().Dictionary().Translate("header/socialNetworks/facebook", "Facebook")
@Html.Sitecore().Dictionary().Translate("header/socialNetworks/twitter", "Twitter", editable:true)
@Html.Sitecore().Dictionary().Translate("header/socialNetworks/linkedIn")
@Html.Sitecore().Dictionary().Translate("header/socialNetworks/instagramm", editable:true)
```
On first page load, relevant dictionary items will be created in your configured Dictionary Domain for the website (if no domain configured, the `/sitecore/system/Dictionary` is used).
```
*[DictionaryDomainItem]/Header/SocialNetworks/Facebook - with default value "Facebook" in language the page were opened
*[DictionaryDomainItem]/Header/SocialNetworks/Twitter - with default value "Twitter" in language the page were opened. In editing mode this phrase will be editable.
*[DictionaryDomainItem]/Header/SocialNetworks/LinkedIn - with empty value in language the page were opened. 
*[DictionaryDomainItem]/Header/SocialNetworks/Instagramm - with empty value in language the page were opened. In editing mode this phrase will be editable.
```
All phrases are automatically published to all available publishing targets.

## Using dictionary translation in class

1. Inject `Pintle.Dictionary.IDictionaryService` into your class:
```cs
using System.Web.Mvc;
using Pintle.Dictionary;

public class MyController : Controller
{
	private readonly IDictionaryService dictionary;

	public MyController(IDictionaryService dictionary)
	{
		this.dictionary = dictionary;
	}
}
```

2. use same notation:
```cs
public ActionResult MyView()
{
	var facebook = this.dictionary.Translate("header/socialNetworks/facebook", "Facebook");
	var twitter = this.dictionary.Translate("header/socialNetworks/twitter", "Twitter", editable: true);
	var linkedIn = this.dictionary.Translate("header/socialNetworks/linkedIn");
	var instagramm = this.dictionary.Translate("header/socialNetworks/instagramm", editable: true);
	//...
}
```

## Content Delivery (CD) and Content Management (CM) sitecore instances

Dictionary item creation is handled through Sitecore message bus (introduced in Sitecore 9 update 1 release). This means if the dictionary service is being called on your CD instance and there are phrase items that need to be created in master database, the CD server will send a message to CM server and the item will be created and published by the Content Management sitecore instance.

## Requirements

* Sitecore CMS 9.0.1 or later
* .NET Framework 4.6.2 or later

## Documentation

The module is completely configuration driven and implemented with proper responsibility separation and abstraction level. Once installed, all dependencies and services can be found in Sitecore configuration files, and this is the entry point in case you need to patch it for your needs.
The most important config file is [Pintle.Dictionary.Services.config](https://github.com/pintle/pintle-dictionary/blob/master/src/Pintle.Dictionary/App_Config/Modules/Pintle.Dictionary/Pintle.Dictionary.Services.config "Pintle.Dictionary.Services.config"):
```xml
<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
  <sitecore>
    <pintle>
      <dictionary>
        <dictionaryService type="Pintle.Dictionary.DictionaryService, Pintle.Dictionary">
          <param name="repository" ref="pintle/dictionary/itemRepository"/>
          <param name="settings" ref="pintle/dictionary/settings"/>
          <param name="cache" ref="pintle/dictionary/cache"/>
          <param name="logger" type="Sitecore.Abstractions.BaseLog, Sitecore.Kernel" resolve="true"/>
        </dictionaryService>
        <cache type="Pintle.Dictionary.DictionaryCache, Pintle.Dictionary" singleInstance="true">
          <param name="cacheName">Pintle.Dictionary.Cache</param>
          <param name="cacheSize">10MB</param>
        </cache>
        <itemRepository type="Pintle.Dictionary.DictionaryItemRepository, Pintle.Dictionary">
          <param name="settings" ref="pintle/dictionary/settings"/>
          <param name="messageBus" type="Sitecore.Framework.Messaging.IMessageBus`1[[Pintle.Dictionary.Messaging.DictionaryMessageBus, Pintle.Dictionary]], Sitecore.Framework.Messaging.Abstractions" resolve="true"/>
          <param name="logger" type="Sitecore.Abstractions.BaseLog, Sitecore.Kernel" resolve="true"/>
        </itemRepository>
        <settings type="Pintle.Dictionary.SitecoreDictionarySettings, Pintle.Dictionary" singleInstnace="true">
          <dictionaryDomainTemplateId>{0A2847E6-9885-450B-B61E-F9E6528480EF}</dictionaryDomainTemplateId>
          <dictionaryFolderTemplateId>{267D9AC7-5D85-4E9D-AF89-99AB296CC218}</dictionaryFolderTemplateId>
          <dictionaryPhraseTemplateId>{6D1CD897-1936-4A3A-A511-289A94C2A7B1}</dictionaryPhraseTemplateId>
          <DictionaryKeyFieldName>Key</DictionaryKeyFieldName>
          <dictionaryPhraseFieldName>Phrase</dictionaryPhraseFieldName>
          <itemCreationDatabase>master</itemCreationDatabase>
        </settings>
        <iconSettings type="Pintle.Dictionary.DictionaryIconSettings, Pintle.Dictionary" singleInstnace="true">
          <DictionaryDomainIcon>Office/32x32/books.png</DictionaryDomainIcon>
          <DictionaryFolderIcon>Office/32x32/book2.png</DictionaryFolderIcon>
          <DictionaryPhraseIcon>Office/32x32/text_field.png</DictionaryPhraseIcon>
        </iconSettings>
      </dictionary>
    </pintle>
  </sitecore>
</configuration>
```

## Contributing

We love it if you would contribute!

Help us! Keep the quality of feature requests and bug reports high

We strive to make it possible for everyone and anybody to contribute to this project. Please help us by making issues easier to resolve by providing sufficient information. Understanding the reasons behind issues can take a lot of time if information is left out.

Thank you, and happy contributing!