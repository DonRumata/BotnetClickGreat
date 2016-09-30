using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{
    public enum Expression_Type
    {
        Err=-1,
        Ariphmetical_expression=1,
        Logical_expression=2,
        Function_body_expression=3,
    }
    public class Expression:Token
    {
        public List<Token> RPN_Expression_data { get; private set; }
        public Expression_Type EXPRType { get; private set; }

        public Expression(List<Token>Input_RPN, Expression_Type Input_Type_OFEXPR)
        {
            RPN_Expression_data = Input_RPN;
            EXPRType = Input_Type_OFEXPR;
        }

        public Expression(Stack<Token> InputStr, Expression_Type Input_Type_OFEXPR)
        {
            RPN_Expression_data = new List<Token>();
            EXPRType = new Expression_Type();
            Token Temp_tok;
            while (InputStr.Count>0)
            {
                if (InputStr.Peek().get_group_of_token()==Group_of_Tokens.AriphmeticalExpression)
                {
                    Temp_tok = InputStr.Pop();
                    RPN_Expression_data = (Temp_tok as Expression).RPN_Expression_data;
                    Range = Temp_tok.Range;
                    Data = Temp_tok.Data;
                    Space_check = Temp_tok.Space_check;
                    Row = Temp_tok.Row;
                }
                RPN_Expression_data.Add(InputStr.Pop());
                Data += RPN_Expression_data.Last().Data;
            }
            Temp_tok = RPN_Expression_data.Last();
            EXPRType = Input_Type_OFEXPR;
            Range = new Tuple<int, int>(Temp_tok.Range.Item1, RPN_Expression_data.First().Range.Item2);
            Space_check = Temp_tok.Space_check;
            Row = Temp_tok.Row;
            Token_Group = Group_of_Tokens.AriphmeticalExpression;
        }

        public void ReConfigureEXPR()
        {

        }
    }
}
