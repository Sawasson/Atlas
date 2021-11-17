using HawiyyahGenerator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HawiyyahGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Hawiyyah.RevenuesLast();
            var lists = await Hawiyyah.NewLTVSameMonth();
            var lists2 = await Hawiyyah.FirstSubReport(lists);
            await Hawiyyah.LTVModels(lists2);
        }
    }
}
