using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BrainEnterprise.Core.Accounting.Vat
{
    /// <summary>
    /// Helper class for Vat Utilities
    /// </summary>
    public static class VatHelper
    {
        /// <summary>
        /// Decimals used for calculation roundings
        /// </summary>
        public static byte VatDecimalRound { get; set; } = 2;

        /// <summary>
        /// Vat Amount calculation
        /// </summary>
        /// <param name="baseAmount">Base Amount</param>
        /// <param name="vatRate">Vat Rate</param>
        /// <returns>Vat Amount</returns>
        /// <remarks>
        /// Result rounded by VatDecimalRound
        /// </remarks>
        public static Decimal GetVatAmount(decimal baseAmount, decimal vatRate)
        {
            decimal toReturn = (baseAmount / 100 * vatRate);
            return Math.Round(toReturn, VatDecimalRound);
        }

        /// <summary>
        /// Unbundling of VAT from total amount
        /// </summary>
        /// <param name="amountIncludingVat">Totale IVA inclusa</param>
        /// <param name="vatRate">Aliquota IVA appplicata</param>
        /// <returns>Imposta estratta</returns>
        public static Decimal VatUnbundling(decimal amountIncludingVat, decimal vatRate)
        {
            decimal toReturn = (amountIncludingVat / (100 + vatRate) * vatRate);
            return Math.Round(toReturn, VatDecimalRound);
        }

        /// <summary>
        /// VatCode regex of a specificountry
        /// </summary>
        /// <param name="countryCode">Codice Nazione</param>
        private static string _countryRegEx(string countryCode)
        {
            switch (countryCode)
            {
                case CountryIsoCodes.AT: return "^(ATU)[A-Z0-9]{8}";
                case CountryIsoCodes.BE: return "^(BE)[0-9]{10}";
                case CountryIsoCodes.BG: return "^(BG)[0-9]{9}|^( BG[0-9]{10}";
                case CountryIsoCodes.CY: return "^(CY)[0-9]{8}[A-Z]{1}";
                case CountryIsoCodes.CZ: return "^(CZ)[0-9]{8,10}";
                case CountryIsoCodes.DE: return "^(DE)[0-9]{9}";
                case CountryIsoCodes.DK: return "^(DK)[0-9]{8}";
                case CountryIsoCodes.EE: return "^(EE)[0-9]{9}";
                case CountryIsoCodes.ES: return "^(ES)[A-Z0-9]{1}[0-9]{7}[A-Z0-9]{1}";
                case CountryIsoCodes.FI: return "^(FI)[0-9]{8}";
                case CountryIsoCodes.FR: return "^(FR)[A-Z0-9]{2}[0-9]{9}";
                case CountryIsoCodes.GB: return "^(GB)[0-9]{9}|^(GB)[0-9]{12}|^(GBGD)[0-9]{3}|^(GBHA)[0-9]{3}";
                case CountryIsoCodes.HU: return "^(HU)[0-9]{8}";
                case CountryIsoCodes.IE: return "^(IE)[0-9]{1}[A-Z0-9]{1}[0-9]{5,5}[A-Z]{1}|^(IE)[0-9]{7}[A-W]{1}[A-I]{1}";
                case CountryIsoCodes.IT: return "^(IT)[0-9]{11}";
                case CountryIsoCodes.LT: return "^(LT)[0-9]{9}|^(LT)[0-9]{12}";
                case CountryIsoCodes.LU: return "^(LU)[0-9]{8}";
                case CountryIsoCodes.LV: return "^(LV)[0-9]{11}";
                case CountryIsoCodes.MT: return "^(MT)[0-9]{8}";
                case CountryIsoCodes.NL: return "^(NL)[A-Z0-9]{9}B[A-Z0-9]{2}";
                case CountryIsoCodes.PL: return "^(PL)[0-9]{10}";
                case CountryIsoCodes.PT: return "^(PT)[0-9]{9}";
                case CountryIsoCodes.SE: return "^(SE)[0-9]{10}01";
                case CountryIsoCodes.SI: return "^(SI)[0-9]{8}";
                case CountryIsoCodes.SK: return "^(SK)[0-9]{10}";
                case CountryIsoCodes.RO: return "^(RO)[1-9]{1}[0-9]{1,9}";
                case CountryIsoCodes.EL: return "^(EL)[0-9]{9}";
                case CountryIsoCodes.HR: return "^(HR)[0-9]{11}";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Verifica formale del codice Iva, Italia
        /// </summary>
        /// <see href="http://aino.it/algoritmi-calcolo-partita-iva/"/>
        /// <param name="vatCode">Codice da verificare</param>
        /// <returns>Esito dell'Operazione</returns>
        private static bool _checkDigit_IT(string vatCode)
        {
            int tot = 0;
            int odd = 0;
            for (int i = 0; i < 10; i += 2)
                odd += int.Parse(vatCode.Substring(i, 1));
            for (int i = 1; i < 10; i += 2)
            {
                tot = (int.Parse(vatCode.Substring(i, 1))) * 2;
                tot = (tot / 10) + (tot % 10);
                odd += tot;
            }
            int ctrl = int.Parse(vatCode.Substring(10, 1));
            if (((odd % 10) == 0 && (ctrl == 0)) || ((10 - (odd % 10)) == ctrl))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check Vat Code
        /// </summary>
        /// <param name="vatCode">Vat code in formato [Country Iso Code][Vat Number]</param>
        /// <returns>Esito dell'Operazione</returns>
        /// <remarks>
        /// Vat code can be in format: "IT02201060981" or "IT 02201060981"
        /// If it is expressed without the country code, it is automatically considered Italian
        /// </remarks>
        public static Boolean CheckVatCode(String vatCode)
        {
            if (vatCode.Length < 3)
                return false;
            vatCode = vatCode.Replace(" ", string.Empty);
            // Validazione dell'espressione regolare
            // - rimozione di eventuali spazi di formattazione del codice
            // - verifica a cascata le casistiche:
            //   - che il codice sia senza prefisso di nazione ed allora indica "IT"
            //   - che il codice abbia un prefisso di nazione a due caratteri
            //   - che il codice abbia un prefisso di nazione a tre caratteri
            string countryCode = vatCode.Substring(0, 2);
            string countryRegEx = _countryRegEx(countryCode);
            if (countryRegEx == string.Empty)
            {
                countryCode = "IT";
                countryRegEx = _countryRegEx(countryCode);
                vatCode = countryCode + vatCode;
            }
            if (!Regex.Match(vatCode, countryRegEx).Success)
                return false;
            // Verifica formale del codice in base alla Nazione
            if (countryCode == "IT")
                return _checkDigit_IT(vatCode.Substring(2));
            // Operazione completata con successo
            return true;
        }
    }
}