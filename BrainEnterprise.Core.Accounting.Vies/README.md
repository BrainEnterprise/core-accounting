# About

This library contains a class to check Vat Identification Number using VIES Europe Service:
- https://ec.europa.eu/taxation_customs/vies/?locale=it

# How To Use

## BrainEnterprise.Core.Accounting.Vies.VatChecker

Check if Vat Registration Number is Valid
````C#
VatChecker test = new VatChecker();
// Test partita Iva Valida
if(test.CheckVat("IT", "02524120207"))
{
	Console.Write(test.CompanyName);
	Console.Write(test.CompanyAddress);
	Console.Write(test.CountryCode);
	Console.Write(test.VatCode);
	Console.Write(test.IsValid);
}
````
