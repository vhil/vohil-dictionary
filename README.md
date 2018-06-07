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
