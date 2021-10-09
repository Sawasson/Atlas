using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CsvFramework
{
    public class ParserResult
    {
        public bool SkipThis { get; set; }
    }

    public interface IPasering<T>
    {
        ParserResult Parsing(T item);
    }



    public interface IPaseringAsync<T>
    {
        Task<ParserResult> ParsingAsync(T item);
    }
}
