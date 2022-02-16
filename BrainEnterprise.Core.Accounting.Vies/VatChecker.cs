using checkVatService;
using System;
using System.ServiceModel;

namespace BrainEnterprise.Core.Accounting.Vies
{
    /// <summary>
    /// Check Vat Identification Number using Europe VIES Service
    /// </summary>
    /// <see href="http://ec.europa.eu/taxation_customs/vies/checkVatService.wsdl"/>
    public class VatChecker
    {
        /// <summary>
        /// Indirizzo del server servizio
        /// </summary>
        public string _serviceAddress { get; private set; } = "http://ec.europa.eu/taxation_customs/vies/services/checkVatService";

        /// <summary>
        /// Costruttore Standard della Classe
        /// </summary>
        public VatChecker()
        {
            _reset();
        }

        /// <summary>
        /// Costruttore Standard della Classe
        /// </summary>
        public VatChecker(string serviceAddress)
        {
            _serviceAddress = serviceAddress;
            _reset();
        }

        /// <summary>
        /// Inizializzazione dei dati
        /// </summary>
        private void _reset()
        {
            CountryCode = string.Empty;
            VatCode = string.Empty;
            IsValid = false;
            LastExecution = DateTime.MinValue;
            CompanyName = string.Empty;
            CompanyAddress = string.Empty;
        }

        /// <summary>
        /// Configurazione del Binding Http
        /// </summary>
        /// <returns>Istanza del binding Http</returns>
        private BasicHttpBinding _getBinding()
        {
            BasicHttpBinding toReturn = new BasicHttpBinding();
            toReturn.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            return toReturn;
        }

        /// <summary>
        /// Codice Paese
        /// </summary>
        public String CountryCode { get; private set; }

        /// <summary>
        /// Partita Iva
        /// </summary>
        public String VatCode { get; private set; }

        /// <summary>
        /// Test Valido
        /// </summary>
        public Boolean IsValid { get; private set; }

        /// <summary>
        /// Data/Ora ultima esecuzione Richiesta
        /// </summary>
        public DateTime LastExecution { get; private set; }

        /// <summary>
        /// Identificativo della Società estratta con l'ultima Interrogazione
        /// </summary>
        public String CompanyName { get; private set; }

        /// <summary>
        /// Indirizzo della Società estratta con l'ultima Interrogazione
        /// </summary>
        public String CompanyAddress { get; private set; }

        /// <summary>
        /// Verifica della Partita Iva intracomunitaria
        /// </summary>
        /// <param name="countryCode">Codice Nazione</param>
        /// <param name="vatCode">Codice IVA</param>
        /// <returns></returns>
        public Boolean CheckVat(String countryCode, String vatCode)
        {
            _reset();
            if (string.IsNullOrEmpty(vatCode) || string.IsNullOrEmpty(countryCode))
                return false;
            var port = new checkVatPortTypeClient(_getBinding(), new EndpointAddress(_serviceAddress));
            bool isValid = false;
            string name = string.Empty;
            string address = string.Empty;
            var result = port.checkVat(ref countryCode, ref vatCode, out isValid, out name, out address);
            this.CountryCode = countryCode;
            this.VatCode = VatCode;
            this.IsValid = isValid;
            if (isValid)
            {
                this.CompanyName = name;
                this.CompanyAddress = address;
            }
            this.LastExecution = DateTime.Now;
            return isValid;
        }

        /// <summary>
        /// The country code of the uid to check
        /// </summary>
        /// <remarks>
        /// This parameter can be one of these country codes
        /// 
        /// country --> code to use
        /// ************************************************
        /// Austria --> AT 
        /// Belgium --> BE 
        /// Bulgaria --> BG 
        /// Cyprus --> CY 
        /// Czech Republic --> CZ 
        /// Germany --> DE 
        /// Denmark --> DK 
        /// Estonia EE 
        /// Greece EL 
        /// Spain ES 
        /// Finland FI 
        /// France FR 
        /// United Kingdom GB 
        /// Hungary HU 
        /// Ireland IE 
        /// Italy IT 
        /// Lithuania LT 
        /// Luxembourg LU 
        /// Malta MT 
        /// The Netherlands NL 
        /// Poland PL 
        /// Portugal PT 
        /// Romania RO 
        /// Sweden SE 
        /// Slovenia SI 
        /// Slovakia SK
        /// </remarks>

        /// <summary>
        /// Verifica della Partita Iva intracomunitaria
        /// </summary>
        /// <param name="vatCode">Codice IVA</param>
        /// <returns></returns>
        public Boolean CheckVat(String vatCode)
        {
            return CheckVat("IT", vatCode);
        }
    }
}
