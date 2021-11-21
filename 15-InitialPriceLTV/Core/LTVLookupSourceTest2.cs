using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15_InitialPriceLTV.Core
{
    public class LtvLookupSourceTest2<T1, T2, T3, T4, T5, TOut>
    {
        private ILookup<Tuple<T1, T2, T3, T4, T5>, TOut> lookup;
        public LtvLookupSourceTest2(IEnumerable<TOut> source, Func<TOut, Tuple<T1, T2, T3, T4, T5>> keySelector)
        {
            lookup = source.ToLookup(keySelector);
        }

        public IEnumerable<TOut> this[T1 created_date, T2 Project, T3 Source, T4 country_code, T5 site_lang]
        {
            get
            {
                return lookup[Tuple.Create(created_date, Project, Source, country_code, site_lang)];
            }
        }
    }
}
