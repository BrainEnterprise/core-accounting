using System;

namespace BrainEnterprise.Core.Accounting.Ws
{
    /// <summary>
    /// Definizione di una Regione
    /// </summary>
    public interface IRegion
    {
        /// <summary>
        /// Codice della Regione
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Nome della Regione
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Area di appartenenza
        /// </summary>
        IArea Area { get; }
    }
}
