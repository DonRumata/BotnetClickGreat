using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{
    public sealed class Variable : Token
    {
        public Token RPNValue { get; set; }
        Typecial Type_Var_Data { get; set; }

        public Variable(Expression InValue, Typecial InType)
        {
            RPNValue = InValue;
            Type_Var_Data = InType;
        }

        public Variable(Expression InValue)
        {
            RPNValue = InValue;
            Type_Var_Data = new Typecial();
        }

        public Variable()
        {

        }
    }
}
