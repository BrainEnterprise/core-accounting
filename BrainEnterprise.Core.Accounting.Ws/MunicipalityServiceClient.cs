using BrainEnterprise.Core.Accounting.Ws.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;

namespace BrainEnterprise.Core.Accounting.Ws
{
    /// <summary>
    /// Classe Service per gestione delle interrogazioni sui comuni
    /// </summary>
    public class MunicipalityServiceClient
    {
        /// <summary>
        /// Indirizzo Base del Servizio
        /// </summary>
        private string _baseServiceAddress = "https://ws.brain-enterprise.com/comuni/";

        /// <summary>
        /// Costruttore Standard della Classe
        /// </summary>
        public MunicipalityServiceClient()
        {
        }
        
        /// <summary>
        /// Costruttore Standard della Classe
        /// </summary>
        /// <param name="serviceAddress">Indirizzo personale da chiamare</param>
        public MunicipalityServiceClient(string serviceAddress)
        {
            _baseServiceAddress = serviceAddress;
        }

        /// <summary>
        /// Indica se utilizzare la cache delle Richieste
        /// </summary>
        private static bool _useCache = true;

        /// <summary>
        /// Indica se utilizzare la cache delle Richieste
        /// </summary>
        public static bool UseCache
        {
            get { return _useCache; }
            set
            {
                if (value != _useCache)
                {
                    _clearCache();
                    _useCache = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void _clearCache()
        {
            if (_cache != null)
                _cache.Clear();
        }

        /// <summary>
        /// Svuotamento della cache
        /// </summary>
        public void ClearCache()
        {
            _clearCache();
        }

        #region Gestione Cache

        /// <summary>
        /// 
        /// </summary>
        private class CachedRequest
        {
            /// <summary>
            /// 
            /// </summary>
            public string ServiceAddress { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public object CachedObject { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static List<CachedRequest> _cache = new List<CachedRequest>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceAddress"></param>
        /// <returns></returns>
        private static CachedRequest _getRequestFromCache(string serviceAddress)
        {
            if (_cache != null)
                return _cache.FirstOrDefault(x => x.ServiceAddress == serviceAddress);
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceAddress"></param>
        /// <param name="cachedObject"></param>
        private static void _addRequestToCache(string serviceAddress, object cachedObject)
        {
            if (_cache == null)
                _cache = new List<CachedRequest>();
            _cache.Add(new CachedRequest()
            {
                ServiceAddress = serviceAddress,
                CachedObject = cachedObject
            });
        }

        #endregion

        /// <summary>
        /// Caricamento delle collezioni di Aree
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IArea> GetAreas()
        {
            var serviceAddress = $"{_baseServiceAddress}aree.php";
            if (UseCache)
            {
                var cached = _getRequestFromCache(serviceAddress);
                if (cached != null)
                    return cached.CachedObject as IEnumerable<IArea>;
            }
            try
            {
                HttpWebRequest request = WebRequest.Create(serviceAddress) as HttpWebRequest;
                request.Method = "POST";
                //...
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(AreasRootObject));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    AreasRootObject jsonResponse = objResponse as AreasRootObject;
                    if (jsonResponse.ErrorMessagge != string.Empty)
                        throw new Exception(jsonResponse.ErrorMessagge);
                    var toreturn = jsonResponse.Result;
                    if (UseCache)
                        _addRequestToCache(serviceAddress, toreturn);
                    return toreturn;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exception on GetAreas", ex);
            }
        }

        /// <summary>
        /// Caricamento delle collezioni di Regioni
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IRegion> GetRegions(string areaFilter = "")
        {
            var serviceAddress = $"{_baseServiceAddress}/regioni.php?codiceArea={areaFilter}";
            if (UseCache)
            {
                var cached = _getRequestFromCache(serviceAddress);
                if (cached != null)
                    return cached.CachedObject as IEnumerable<IRegion>;
            }
            try
            {
                HttpWebRequest request = WebRequest.Create(serviceAddress) as HttpWebRequest;
                request.Method = "POST";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    }
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(RegionsRootObject));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    RegionsRootObject jsonResponse = objResponse as RegionsRootObject;
                    if (jsonResponse.ErrorMessagge != string.Empty)
                        throw new Exception(jsonResponse.ErrorMessagge);
                    var toreturn = jsonResponse.Result;
                    if (UseCache)
                        _addRequestToCache(serviceAddress, toreturn);
                    return toreturn;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exception on GetRegions", ex);
            }
        }

        /// <summary>
        /// Caricamento delle collezioni di Province
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IProvince> GetProvinces(string areaFilter = "", string regionFilter = "")
        {
            var serviceAddress = $"{_baseServiceAddress}province.php?codiceArea={areaFilter}&codiceRegione={regionFilter}";
            if (UseCache)
            {
                var cached = _getRequestFromCache(serviceAddress);
                if (cached != null)
                    return cached.CachedObject as IEnumerable<IProvince>;
            }
            try
            {
                HttpWebRequest request = WebRequest.Create(serviceAddress) as HttpWebRequest;
                request.Method = "POST";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    }
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ProvinceRootObject));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    ProvinceRootObject jsonResponse = objResponse as ProvinceRootObject;
                    if (jsonResponse.ErrorMessagge != string.Empty)
                        throw new Exception(jsonResponse.ErrorMessagge);
                    var toreturn = jsonResponse.Result;
                    if (UseCache)
                        _addRequestToCache(serviceAddress, toreturn);
                    return toreturn;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exception on GetProvinces", ex);
            }
        }

        /// <summary>
        /// Caricamento delle collezioni di Comuni
        /// </summary>
        /// <param name="provinceFilter">Filtro per Provincia</param>
        /// <param name="regionFilter">Filtro per Regione di appartenenza</param>
        /// <param name="areaFilter">Filtro per Area di appartenenza</param>
        /// <returns></returns>
        public IEnumerable<IMunicipality> GetMunicipalities(string provinceFilter = "", string regionFilter = "", string areaFilter = "")
        {
            var serviceAddress = $"{_baseServiceAddress}comuni.php?siglaAutomobilistica={provinceFilter}&codiceRegione={regionFilter}&codiceArea={areaFilter}";
            if (UseCache)
            {
                var cached = _getRequestFromCache(serviceAddress);
                if (cached != null)
                    return cached.CachedObject as IEnumerable<IMunicipality>;
            }
            try
            {
                HttpWebRequest request = WebRequest.Create(serviceAddress) as HttpWebRequest;
                request.Method = "POST";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(MunicipalityRootObject));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    MunicipalityRootObject jsonResponse = objResponse as MunicipalityRootObject;
                    if (jsonResponse.ErrorMessagge != string.Empty)
                        throw new Exception(jsonResponse.ErrorMessagge);
                    var toreturn = jsonResponse.Result;
                    if (UseCache)
                        _addRequestToCache(serviceAddress, toreturn);
                    return toreturn;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exception on GetMunicipalities", ex);
            }
        }

        /// <summary>
        /// Caricamento del comune dal Codice Catastale
        /// </summary>
        /// <returns></returns>
        public IMunicipality GetMunicipality(string cadastralCode)
        {
            var serviceAddress = $"{_baseServiceAddress}comuni.php?codiceCatastale={cadastralCode}";
            if (UseCache)
            {
                var cached = _getRequestFromCache(serviceAddress);
                if (cached != null)
                    return cached.CachedObject as IMunicipality;
            }
            try
            {
                HttpWebRequest request = WebRequest.Create(serviceAddress) as HttpWebRequest;
                request.Method = "POST";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(MunicipalityRootObject));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    MunicipalityRootObject jsonResponse = objResponse as MunicipalityRootObject;
                    if (jsonResponse.ErrorMessagge != string.Empty)
                        throw new Exception(jsonResponse.ErrorMessagge);
                    var toreturn = jsonResponse.Result[0];
                    if (UseCache)
                        _addRequestToCache(serviceAddress, toreturn);
                    return toreturn;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Exception on GetMunicipality", ex);
            }
        }
    }
}
