using System;

namespace BrainEnterprise.Core.Accounting.Ws
{
    /// <summary>
    /// Definizione di un comune
    /// </summary>
    public interface IMunicipality
    {
        /// <summary>
        /// Codice Catastale
        /// </summary>
        string CadastralCode { get; }

        /// <summary>
        /// Nome del Comune
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Provincia di appartenenza
        /// </summary>
        IProvince Province { get; }

        /// <summary>
        /// Regione di appartenenza
        /// </summary>
        IRegion Region { get; }

        /// <summary>
        /// Area di appartenenza
        /// </summary>
        IArea Area { get; }
    }
}
