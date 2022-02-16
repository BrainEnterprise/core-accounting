using System;

namespace BrainEnterprise.Core.Accounting.Ws
{
    /// <summary>
    /// Definizione di una provincia
    /// </summary>
    public interface IProvince
    {
        /// <summary>
        /// Abbreviazione / Sigla Automobilistica
        /// </summary>
        string Abbreviation { get; }

        /// <summary>
        /// Nome della Provincia
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Area di appartenenza
        /// </summary>
        IArea Area { get; }

        /// <summary>
        /// Regione di Appartenenza
        /// </summary>
        IRegion Region { get; }
    }
}
