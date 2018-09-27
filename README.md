# Pintle.Dictionary

Pintle.Dictionary is a Sitecore CMS module which provides an abstracted dictionary service for Sitecore 9.x CMS.

Functionality and features
 * designed for Sitecore 9.x CMS and later
 * uses default sitecore Translate mechanism
 * automatic dictionary phrase items creation (through [Sitecore service bus](https://www.pintle.dk/insights/using-sitecore-service-bus/))
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

## Translateable data annotations

This module implements list of data annotations attributes that can be used while implementing custom forms. Given attributes fully compatible with regular ASP.NET unobstrictive validation and still can be used with jQuery plugin for it. Please refer to example below

### Form class:
```cs
public class SignupForm
{
	[DisplayNameTranslated(DictionaryKey = "Signup/Email", DefaultTranslation = "E-mail address")]
	[RequiredTranslated(DictionaryKey = "Signup/Validation/EmailRequired")]
	[EmailAddressTranslated(DictionaryKey = "Signup/Validation/EmailInvalid")]
	public string Email { get; set; }

	[DisplayNameTranslated(DictionaryKey = "Signup/Phone", DefaultTranslation = "Phone number")]
	[RequiredTranslated(DictionaryKey = "Signup/Validation/PhoneRequired")]
	[PhoneTranslated(DictionaryKey = "Signup/Validation/PhoneInvalid")]
	public string Phone { get; set; }

	[DisplayNameTranslated(DictionaryKey = "Signup/CreditCard", DefaultTranslation = "Credit card")]
	[RequiredTranslated(DictionaryKey = "Signup/Validation/CardRequired")]
	[CreditCardTranslated(DictionaryKey = "Signup/Validation/CardInvalid")]
	public string CreditCard { get; set; }

	[DisplayNameTranslated(DictionaryKey = "Signup/Password", DefaultTranslation = "Your password")]
	[RequiredTranslated(DictionaryKey = "Signup/Validation/PasswordRequired")]
	[StringLengthTranslated(12, MinimumLength = 6, DictionaryKey = "Signup/Validation/PasswordsLength")]
	public string Password { get; set; }

	[DisplayNameTranslated(DictionaryKey = "Signup/ConfirmPassword", DefaultTranslation = "Confirm password")]
	[RequiredTranslated]
	[CompareTranslated(nameof(Password))]
	public string ConfirmPassword { get; set; }

	[DisplayNameTranslated(DictionaryKey = "Signup/Url", DefaultTranslation = "Website url")]
	[RequiredTranslated(DictionaryKey = "Signup/Validation/UrlRequired")]
	[UrlTranslated(DictionaryKey = "Signup/Validation/UrlInvalid")]
	public string Url { get; set; }

	[DisplayNameTranslated(DictionaryKey = "Signup/AcceptTerms", DefaultTranslation = "I accept terms and conditions")]
	[RangeTranslated(typeof(bool), "true", "true", DictionaryKey = "Signup/Validation/AcceptTermsRequired", DefaultTranslation = "Please accept our terms and conditions.")]
	public bool AcceptTerms { get; set; }

	[DisplayNameTranslated(DictionaryKey = "Signup/NumberRegex", DefaultTranslation = "Amount of something")]
	[RequiredTranslated(DictionaryKey = "Signup/Validation/NumberRequired")]
	[RegularExpressionTranslated("^[0-9]*$", DictionaryKey = "Signup/Validation/NumberRegex")]
	public string NumberRegex { get; set; }
}
```

### Form view

```html
@using System.Web.Mvc
@using System.Web.Mvc.Html
@using Pintle.Dictionary.Extensions
@using Sitecore.Mvc
@model SignupForm
@{
	HtmlHelper.ClientValidationEnabled = true;
	HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
}
<form method="post" novalidate>
	<div>
		<label for="@Html.NameFor(x => x.Email)">
			@Html.DisplayNameFor(x => x.Email)
		</label>
		@Html.EditorFor(x => x.Email)
		@Html.ValidationMessageFor(x => x.Email, null)
	</div>
	<div>
		<label for="@Html.NameFor(x => x.Phone)">
			@Html.DisplayNameFor(x => x.Phone)
		</label>
		@Html.EditorFor(x => x.Phone)
		@Html.ValidationMessageFor(x => x.Phone, null)
	</div>
	<div>
		<label for="@Html.NameFor(x => x.CreditCard)">
			@Html.DisplayNameFor(x => x.CreditCard)
		</label>
		@Html.EditorFor(x => x.CreditCard)
		@Html.ValidationMessageFor(x => x.CreditCard, null)
	</div>
	<div>
		<label for="@Html.NameFor(x => x.Password)">
			@Html.DisplayNameFor(x => x.Password)
		</label>
		@Html.PasswordFor(x => x.Password)
		@Html.ValidationMessageFor(x => x.Password, null)
	</div>
	<div>
		<label for="@Html.NameFor(x => x.ConfirmPassword)">
			@Html.DisplayNameFor(x => x.ConfirmPassword)
		</label>
		@Html.PasswordFor(x => x.ConfirmPassword)
		@Html.ValidationMessageFor(x => x.ConfirmPassword, null)
	</div>
	<div>
		<label for="@Html.NameFor(x => x.Url)">
			@Html.DisplayNameFor(x => x.Url)
		</label>
		@Html.EditorFor(x => x.Url)
		@Html.ValidationMessageFor(x => x.Url, null)
	</div>
	<div>
		<label for="@Html.NameFor(x => x.AcceptTerms)">
			@Html.DisplayNameFor(x => x.AcceptTerms)
		</label>
		@Html.CheckBoxFor(x => x.AcceptTerms)
		@Html.ValidationMessageFor(x => x.AcceptTerms, null)
	</div>
	<div>
		<label for="@Html.NameFor(x => x.NumberRegex)">
			@Html.DisplayNameFor(x => x.NumberRegex)
		</label>
		@Html.EditorFor(x => x.NumberRegex)
		@Html.ValidationMessageFor(x => x.NumberRegex, null)
	</div>
	<button type="submit">
		<span>@Html.Sitecore().Dictionary().Translate("Signup/Continue", "Continue", true) </span>
	</button>
</form>
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

## License
pintle-dictionary is made available under the terms of the GNU Affero General Public License 3.0 (AGPL 3.0). For other licenses [contact us](mailto:info@pintle.dk).
