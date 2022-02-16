using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BrainEnterprise.Core.Accounting.Ws.DataModels
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class AreasRootObject
    {
        [DataMember(Name = "Result")]
        public List<AreaModel> Result { get; set; }

        [DataMember(Name = "Status")]
        public string Status { get; set; }

        [DataMember(Name = "ErrorMessagge")]
        public string ErrorMessagge { get; set; }

    }

    /// <summary>
    /// Definizione di un'Area
    /// </summary>
    [DataContract]
    public class AreaModel : IArea
    {
        /// <summary>
        /// Codice dell'Area
        /// </summary>
        [DataMember(Name = "codiceArea")]
        public string Code { get; set; }

        /// <summary>
        /// Nome dell'Area
        /// </summary>
        [DataMember(Name = "descrizioneArea")]
        public string Name { get; set; }
    }
}
