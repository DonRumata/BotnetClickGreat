using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{
    public class Variable : Typecial, Token
    {
        public string Value { get; set; }
    }

    public class Local_Variable : Variable
    {
        public int Func_ID { get; set; }
    }
}
