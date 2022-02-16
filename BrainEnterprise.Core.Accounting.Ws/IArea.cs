using System;

namespace BrainEnterprise.Core.Accounting.Ws
{
    /// <summary>
    /// Definizione di un'Area
    /// </summary>
    public interface IArea
    {
        /// <summary>
        /// Codice dell'Area
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Nome dell'Area
        /// </summary>
        string Name { get; }
    }
}
