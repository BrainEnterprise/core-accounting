using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BrainEnterprise.Core.Accounting.Ws.DataModels
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class MunicipalityRootObject
    {
        [DataMember(Name = "Result")]
        public List<MunicipalityModel> Result { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "ErrorMessagge")]
        public string ErrorMessagge { get; set; }

    }
    /// <summary>
    /// Definizione di un comune
    /// </summary>
    [DataContract]
    public class MunicipalityModel : IMunicipality
    {
        /// <summary>
        /// Codice Catastale
        /// </summary>
        [DataMember(Name = "codiceCatastale")]
        public string CadastralCode { get; set; }

        /// <summary>
        /// Nome del Comune
        /// </summary>
        [DataMember(Name = "descrizioneItaliana")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "codiceArea")]
        public string AreaCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "descrizioneArea")]
        public string AreaName { get; set; }

        /// <summary>
        /// Area di appartenenza
        /// </summary>
        public IArea Area { get { return new AreaModel() { Code = AreaCode, Name = AreaName }; } }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "codiceRegione")]
        public string RegionCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "descrizioneRegione")]
        public string RegionName { get; set; }

        /// <summary>
        /// Region di appartenenza
        /// </summary>
        public IRegion Region { get { return new RegionModel() { Code = RegionCode, Name = RegionName }; } }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "siglaAutomobilistica")]
        public string ProvinceAbbreviation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "descrizioneUnitaTerritoriale")]
        public string ProvinceName { get; set; }

        /// <summary>
        /// Province di appartenenza
        /// </summary>
        public IProvince Province { get { return new ProvinceModel() { Abbreviation = ProvinceAbbreviation, Name = ProvinceName }; } }

    }
}
