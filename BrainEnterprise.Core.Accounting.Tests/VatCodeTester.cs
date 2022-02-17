using BrainEnterprise.Core.Accounting.Vies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace BrainEnterprise.Core.Accounting.Tests
{
    /// <summary>
    /// Test della Partita Iva
    /// </summary>
    [TestClass]
    public class VatCodeTester
    {
        [TestMethod]
        public void CheckRegularExpression()
        {
            string vatCode = "0sdaeterters22010609834";
            Assert.IsTrue(Regex.Match(vatCode, "[0-9]").Success);
            Assert.IsTrue(Regex.Match(vatCode, "[0-9]{11}").Success);
            vatCode = "IT02201060981";
            Assert.IsTrue(Regex.Match(vatCode, "^(IT)[0-9]").Success);
            Assert.IsTrue(Regex.Match(vatCode, "^(IT)[0-9]{11}").Success);
        }

        [TestMethod]
        public void CheckVatCode()
        {
            // Check Positivo
            Assert.IsTrue(BrainEnterprise.Core.Accounting.Vat.VatHelper.CheckVatCode("02201060981"));
            Assert.IsTrue(BrainEnterprise.Core.Accounting.Vat.VatHelper.CheckVatCode("IT 02201060981"));
            Assert.IsTrue(BrainEnterprise.Core.Accounting.Vat.VatHelper.CheckVatCode("IT02201060981"));
            // Check Lunghezza
            Assert.IsFalse(BrainEnterprise.Core.Accounting.Vat.VatHelper.CheckVatCode("0220"));
            Assert.IsFalse(BrainEnterprise.Core.Accounting.Vat.VatHelper.CheckVatCode("IT 0220"));
            Assert.IsFalse(BrainEnterprise.Core.Accounting.Vat.VatHelper.CheckVatCode("IT0220"));
            // Check Formale Negativo
            Assert.IsFalse(BrainEnterprise.Core.Accounting.Vat.VatHelper.CheckVatCode("02201060982"));
            Assert.IsFalse(BrainEnterprise.Core.Accounting.Vat.VatHelper.CheckVatCode("IT 02201060982"));
            Assert.IsFalse(BrainEnterprise.Core.Accounting.Vat.VatHelper.CheckVatCode("IT02201060982"));
        }

        [TestMethod]
        public void CheckFiscalCode()
        {
            // Check Positivo
            Assert.IsTrue(BrainEnterprise.Core.Accounting.Vat.TaxCodeHelper.Italian.CheckFiscalCode("PLVGLC74M12B157A"));
            Assert.IsTrue(BrainEnterprise.Core.Accounting.Vat.TaxCodeHelper.Italian.CheckFiscalCode("PLVGLC74M12B157A", "Gianluca", "Plevani", new DateTime(1974, 8, 12), 'M', "B157"));
            // Controllo Formale Negativo
            Assert.IsFalse(BrainEnterprise.Core.Accounting.Vat.TaxCodeHelper.Italian.CheckFiscalCode("PLVGLC74M12B157B"));
            Assert.IsFalse(BrainEnterprise.Core.Accounting.Vat.TaxCodeHelper.Italian.CheckFiscalCode("PLVGLC74M12B157A", "Gianluca", "Prevani", new DateTime(1974, 8, 12), 'M', "B157"));
            // Verifica Casi Standard e Casi Particolari
            Assert.IsTrue(BrainEnterprise.Core.Accounting.Vat.TaxCodeHelper.Italian.CalculateFiscalCode("Gianluca", "Plevani", new DateTime(1974, 8, 12), 'M', "B157") == "PLVGLC74M12B157A");
            Assert.IsTrue(BrainEnterprise.Core.Accounting.Vat.TaxCodeHelper.Italian.CalculateFiscalCode("Luigi", "Fo", new DateTime(1980, 1, 1), 'M', "B157") == "FOXLGU80A01B157L");
            Assert.IsTrue(BrainEnterprise.Core.Accounting.Vat.TaxCodeHelper.Italian.CalculateFiscalCode("Anna", "Mia", new DateTime(1980, 1, 1), 'F', "B157") == "MIANNA80A41B157D");

        }

        [TestMethod]
        public void CheckVat()
        {
            VatChecker test = new VatChecker();
            // Test partita Iva Valida
            Assert.IsTrue(test.CheckVat("IT", "02524120207"));
            //Assert.IsTrue(test.CheckVat("IT", "02556300206"));            
            // Test partita Iva non Valida
            Assert.IsFalse(test.CheckVat("IT", "02201060981"));
        }
    }
}
