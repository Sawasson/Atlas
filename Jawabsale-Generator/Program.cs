using System;
using System.Threading.Tasks;

namespace Jawabsale_Generator
{
    class Program
    {
        static async Task Main(string[] args)
        {

            await Jawabsale.RevenuesLast();
        }
    }
}
