using Jawabkom_Generator3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabkom_Generator3
{
    public class DataBuilder
    {
        //public static List<Currency> CurrencyGenerator(IList<IList<Object>> values)
        //{
        //    List<Currency> currencyList = new List<Currency>();

        //    int currentRow = 1;
        //    foreach (var row in values.Skip(1))
        //    {
        //        for (var i = 0; i < row.Count; i++)
        //        {
        //            Currency currency = new Currency();
        //            if (i > 0)
        //            {
        //                string line = row[i].ToString();
        //                currency.Line = Int32.Parse(line);
        //            }
        //            i++;

        //            currency.index = row[i].ToString();
        //            i++;
        //            string quotes = row[i].ToString();
        //            currency.quotes = decimal.Parse(quotes, System.Globalization.NumberStyles.);
        //            //data.Quotes = (int)row[i];
        //            i++;
        //            currency.CountryCode = row[i].ToString();
        //            currencyList.Add(currency);
        //        }
        //        currentRow++;
        //        continue;
        //    }
        //    return currencyList;
        //}
    }
}
