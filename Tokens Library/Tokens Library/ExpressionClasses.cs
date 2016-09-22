using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{
    public enum Expression_Type
    {
        Ariphmetical_expression=1,
        Logical_expression=2,
        Function_body_expression=3,
    }
    class Expression:Token
    {
        public List<Token> RPN_Expression_data { get; private set; }
        public Expression_Type EXPRType { get; private set; }

        public Expression(List<Token>Input_RPN, Expression_Type Input_Type_OFEXPR)
        {
            RPN_Expression_data = Input_RPN;
            EXPRType = Input_Type_OFEXPR;
        }
    }
}
