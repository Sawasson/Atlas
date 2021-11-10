using System;
using System.Threading.Tasks;

namespace Jawabsale_Generator
{
    class Program
    {
        static async Task Main(string[] args)
        {

            await Jawabsale.RevenuesLast();
            var lists = await Jawabsale.NewLTVSameMonth();
            var lists2 = await Jawabsale.FirstSubReport(lists);
            await Jawabsale.LTVModels(lists2);


        }
    }
}
