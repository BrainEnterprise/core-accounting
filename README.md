# About

This library contains some accounting classes and utilities to check Italian Municipalities, according with dataset published by Istat at address https://www.istat.it/it/archivio/6789

# How To Use

Get a single Municipality

````C#
var client = new MunicipalityServiceClient();
var mantova = client.GetMunicipality("E897");
Assert.IsNotNull(mantova);
````

Get List of Italian Municipalities by various Filters

````C#
var client = new MunicipalityServiceClient();
var mantovaProvinces = client.GetMunicipalities(provinceFilter:"MN");
var lombardiaProvinces = client.GetMunicipalities(regionFilter:"03");
````

Get List of Italian Provinces by various Filters

````C#
var client = new MunicipalityServiceClient();
var provinces = client.GetProvinces();
var piemonte = client.GetProvinces(regionFilter:"01");
````

With the Last Update (1.0.5) you can use your own address

````C#
var client = new MunicipalityServiceClient("https://personalAddress.com");
````

## Caching

All the requests are autamatically cached in a static variables inside the class.
You can enable or disable the caching using a static property UseCache

````C#
MunicipalityServiceClient.UseCache = true;

var client = new MunicipalityServiceClient();
var mantova = client.GetMunicipality("E897");

client.ClearCache();

````

With the Last Update (1.0.5) you can use your own address

````C#
var client = new MunicipalityServiceClient("https://personalAddress.com");
````
