# About

This library contains some accounting classes and utilities:
- Margin: static class for calculating margins and markups
- VatHelper: static class for the calculation / unbundling of VAT and verification of the VAT number
- TaxCodeHelper: static class class for the calculation and verification of the Italian Tax Code


# How To Use

## MarginHelper

Used to calculate various types of margins and profits:
- MarginPercent;
- MarkupPercent;
- OperatingProfit: from revenues and characteristics costs;
- GrossProfit: from sales price and variable costs;


## VatHelper

Check if Vat Registration Number is Valid
````C#
BrainEnterprise.Core.Accounting.Vat.VatHelper.CheckVatCode("02201060981");
BrainEnterprise.Core.Accounting.Vat.VatHelper.CheckVatCode("IT 02201060981");
BrainEnterprise.Core.Accounting.Vat.VatHelper.CheckVatCode("IT02201060981");
````
This function actually supports only Italian Vat Registration Number.
For other European countries only a RegEx format check is performed

### Vat Calculation and Unbundling
````C#
var vat = BrainEnterprise.Core.Accounting.Vat.VatHelper.GetVatAmount(500, 22);
var basePrice = BrainEnterprise.Core.Accounting.Vat.VatHelper.VatUnbundling(1220, 22);
````

### Rounding
Change the value of VatDecimalRound (default value 2) to set different Rounding Rule
````C#
BrainEnterprise.Core.Accounting.Vat.VatHelper.VatDecimalRound = 3;
var vat = BrainEnterprise.Core.Accounting.Vat.VatHelper.GetVatAmount(500, 22);
````

## TaxCodeHelper

Used to calculate and Check Italian Fiscal Code
````C#
using BrainEnterprise.Core.Accounting.Vat

// Checks only the control character
TaxCodeHelper.Italian.CheckFiscalCode("PLVGLC73M14B157A");

// Checks the Entire Fiscal Code against a specific name and Data
Assert.IsTrue(TaxCodeHelper.Italian.CheckFiscalCode("PLVGLC73M14B157A", "Gianluca", "Plevani", new DateTime(1973, 8, 14), 'M', "B157"));

// Fiscal Code Calculation
var foo1 = TaxCodeHelper.Italian.CalculateFiscalCode("Gianluca", "Plevani", new DateTime(1974, 8, 12), 'M', "B157");
var foo2 = TaxCodeHelper.Italian.CalculateFiscalCode("Luigi", "Fo", new DateTime(1980, 1, 1), 'M', "B157");
var foo3 = TaxCodeHelper.Italian.CalculateFiscalCode("Anna", "Mia", new DateTime(1980, 1, 1), 'F', "B157");
````