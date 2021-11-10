using System;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var AllSubsCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\Tawzeef\Base\TAWZEEF-subs-6190862.csv");
            var allSubs = await ParseSubscriptions(AllSubsCSV);

            var PayoutsCSV = System.IO.File.ReadAllLines(@"C:\temp\operator_payouts.csv");
            var allPayouts = await ParseOperatorPayouts(PayoutsCSV);

            var AllCurrencyCSV = await System.IO.File.ReadAllLinesAsync(@"C:\temp\currency.xls");
            var currencyList = await ParseCurrency(AllCurrencyCSV);
        }
    }
}
