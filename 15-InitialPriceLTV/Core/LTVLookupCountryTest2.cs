using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15_InitialPriceLTV.Core
{
    public class LtvLookupCountryTest2<T1, T2, T3, T4, TOut>
    {
        private ILookup<Tuple<T1, T2, T3, T4>, TOut> lookup;
        public LtvLookupCountryTest2(IEnumerable<TOut> source, Func<TOut, Tuple<T1, T2, T3, T4>> keySelector)
        {
            lookup = source.ToLookup(keySelector);
        }

        public IEnumerable<TOut> this[T1 created_date, T2 Project, T3 country_code, T4 site_lang ]
        {
            get
            {
                return lookup[Tuple.Create(created_date, Project, country_code, site_lang)];
            }
        }
    }
}
