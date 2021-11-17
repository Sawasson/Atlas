using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15_InitialPriceLTV.Core
{
    public class LtvLookupSource<T1, T2, T3, T4, TOut>
    {
        private ILookup<Tuple<T1, T2, T3, T4>, TOut> lookup;
        public LtvLookupSource(IEnumerable<TOut> source, Func<TOut, Tuple<T1, T2, T3, T4>> keySelector)
        {
            lookup = source.ToLookup(keySelector);
        }

        public IEnumerable<TOut> this[T1 Project, T2 Source, T3 country_code, T4 site_lang]
        {
            get
            {
                return lookup[Tuple.Create(Project, Source, country_code, site_lang)];
            }
        }
    }
}
