using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BrainEnterprise.Core.Accounting.Ws.DataModels
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ProvinceRootObject
    {
        [DataMember(Name = "Result")]
        public List<ProvinceModel> Result { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "ErrorMessagge")]
        public string ErrorMessagge { get; set; }
    }
    /// <summary>
    /// Definizione di una provincia
    /// </summary>
    [DataContract]
    public class ProvinceModel : IProvince
    {
        /// <summary>
        /// Abbreviazione / Sigla Automobilistica
        /// </summary>
        [DataMember(Name = "siglaAutomobilistica")]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Nome della Provincia
        /// </summary>
        [DataMember(Name = "descrizioneUnitaTerritoriale")]
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
    }
}
