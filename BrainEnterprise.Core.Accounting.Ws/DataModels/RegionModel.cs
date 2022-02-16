using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BrainEnterprise.Core.Accounting.Ws.DataModels
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class RegionsRootObject
    {
        [DataMember(Name = "Result")]
        public List<RegionModel> Result { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "ErrorMessagge")]
        public string ErrorMessagge { get; set; }

    }
    /// <summary>
    /// Definizione di una Regione
    /// </summary>
    [DataContract]
    public class RegionModel : IRegion
    {
        /// <summary>
        /// Codice della Regione
        /// </summary>
        [DataMember(Name = "codiceRegione")]
        public string Code { get; set; }

        /// <summary>
        /// Nome della Regione
        /// </summary>
        [DataMember(Name = "descrizioneRegione")]
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
    }
}
