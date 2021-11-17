using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15_InitialPriceLTV.Core
{
    public class LtvLookupCountry<T1, T2, T3, TOut>
    {
        private ILookup<Tuple<T1, T2, T3>, TOut> lookup;
        public LtvLookupCountry(IEnumerable<TOut> source, Func<TOut, Tuple<T1, T2, T3>> keySelector)
        {
            lookup = source.ToLookup(keySelector);
        }

        public IEnumerable<TOut> this[T1 Project, T2 country_code, T3 site_lang]
        {
            get
            {
                return lookup[Tuple.Create(Project, country_code, site_lang)];
            }
        }
    }
}
