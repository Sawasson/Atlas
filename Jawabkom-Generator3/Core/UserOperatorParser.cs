using CsvFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jawabkom_Generator3.Core
{
    class UserOperatorParser : IPaseringAsync<Operator>
    {
        public UserOperatorParser()
        {

        }

        public Task<ParserResult> ParsingAsync(Operator item)
        {
            throw new NotImplementedException();
        }
    }
}
