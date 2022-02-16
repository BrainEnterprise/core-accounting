using System;

namespace BrainEnterprise.Core.Accounting
{
    /// <summary>
    /// Allow various margin calculations
    /// </summary>
    /// <see href="https://farenumeri.it/margine-calcolo-e-tipologie/"/>
    public static class Margin
    {
        /// <summary>
        /// Calculate Margin Percent
        /// </summary>
        /// <param name="salesAmount">Sales Amount</param>
        /// <param name="costsAmount">Costs Amount</param>
        /// <returns>Margin Percent</returns>
        /// <remarks>
        /// ((salesAmount - costsAmount) / salesAmount) * 100
        /// </remarks>
        public static decimal MarginPercent(decimal salesAmount, decimal costsAmount)
        {
            if (costsAmount <= 0)
                return 0;
            if (salesAmount == 0)
                return costsAmount == 0 ? 0 : -100;
            return ((salesAmount - costsAmount) / salesAmount) * 100;

            // ![](6831F0BD31508BBE0A6D11A7F42E8704_2.png;;;0.02384,0.02384)
        }

        /// <summary>
        /// Calculate Markup Percent
        /// </summary>
        /// <param name="salesAmount">Sales Amount</param>
        /// <param name="costsAmount">Costs Amount</param>
        /// <returns>Margine percentuale</returns>
        /// <remarks>
        /// ((salesAmount - costsAmount) / costsAmount) * 100
        /// </remarks>        
        public static decimal MarkupPercent(decimal salesAmount, decimal costsAmount)
        {
            if (costsAmount <= 0)
                return 0;
            if ((costsAmount == 0) && (costsAmount > 0))
                return salesAmount == 0 ? 0 : -100;
            return ((salesAmount - costsAmount) / costsAmount) * 100;

            // ![](46FD825E9E6784334F40C18026A0CB5B.png;;;0.02694,0.03008)
        }

        /// <summary>
        /// Calculate Operating Profit Percent
        /// </summary>
        /// <param name="revenues">Revenues / Ricavi</param>
        /// <param name="characteristicCosts">Characteristic Costs / Costi Caratteristici</param>
        /// <returns></returns>
        public static decimal OperatingProfit(decimal revenues, decimal characteristicCosts)
        {
            if (characteristicCosts <= 0)
                return 0;
            if ((characteristicCosts == 0) && (characteristicCosts > 0))
                return revenues == 0 ? 0 : -100;
            return ((revenues - characteristicCosts) / characteristicCosts) * 100;


            //![](238FB6F293FCAC32708412687329891D.png;;;0.02283,0.02283)
        }

        /// <summary>
        /// Calculate Gross Profit Percent
        /// </summary>
        /// <param name="salesPrice">Sales Amount</param>
        /// <param name="variableCostsAmount">Costs Amount</param>
        /// <returns>Margin Percent</returns>
        public static decimal GrossProfit(decimal salesPrice, decimal variableCostsAmount)
        {
            if (variableCostsAmount <= 0)
                return 0;
            if (salesPrice == 0)
                return variableCostsAmount == 0 ? 0 : -100;
            return ((salesPrice - variableCostsAmount) / salesPrice) * 100;

            // ![](C648FCF81FF1445DCFD26CCB77DD21B9.png;;;0.03041,0.03041)
        }


    }
}
