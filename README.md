# Address and Postal Code Validation for New Zealand

https://www.addy.co.nz/

Address cleansing, also called address validation or address matching, helps to transform incomplete, 
improperly formatted or duplicate addresses stored in a database into accurate addresses 
validated against official address databases, such as the New Zealand Postal Address File (PAF).

Make it easy for users to find and validate their address in your .NET, C# or VB.NET application.  

## Installation

Get starting using the "Addy.Address.Service.Standard" NuGet package:
 
#### Install-Package Addy.Address.Service.Standard -Version 1.4.1

## Address Search API

Call the Address Search API to find and validate addresses using fuzzy matching to eliminate typos and spelling mistakes.

Address Search Documentation:

https://www.addy.co.nz/address-validation-nz-website

https://www.addy.co.nz/address-finder-and-postcode-api

## Address Details API

Call the Address Details API to retrieve address information, such as New Zealand Post’s Delivery Point Identifier (DPID), LINZ’s Street Address ID or longitude / latitude (x / y) coordinates, street names, city names, suburbs, postal codes, territories, regions and more.

Address Metadata Documentation:

https://www.addy.co.nz/address-details-api

The code sample will automatically convert JSON address search responses into DTO/POCO objects.

## Address Validation API

Addy's address validation API, also know as the address cleansing API or address correction API, will verify, cleanse, standardize and format addresses to deliver complete, accurate and enhanced data.

Address Cleansing Documentation:

https://www.addy.co.nz/faq/address-cleansing-software

https://www.addy.co.nz/address-validation-api

## Sample Code in .NET (C#)

Call the Address Lookup API:

```csharp
// Create a free API Key: https://www.addy.co.nz/
const string apiKey = "your-api-key";
IRequestFactory requestFactory = new RequestFactory(apiKey);

using (IAddressValidationService validationService = new AddressValidationService(requestFactory))
{
   Console.WriteLine("Type an address to validate/cleanse:");
   var criteria = Console.ReadLine();
   if (string.IsNullOrWhiteSpace(criteria)) return;

   var result = await validationService.AddressValidateAsync(criteria);

   if (result.address == null)
   {
      Console.WriteLine("No match found. " + result.reason);
   }
   else
   {
      Console.WriteLine("Full Address: {0}", result.address.full);
   }
}
```

## Links

Official Addy site: <https://www.addy.co.nz/>

Address Search API Documentation: <https://www.addy.co.nz/address-finder-and-postcode-api>
