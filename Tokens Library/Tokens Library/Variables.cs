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

        public Variable(Stack<Token> InStack)
        {
            if (InStack.Peek().Token_Group!=Group_of_Tokens.Assignment)
            {
                RPNValue = InStack.Pop();
            }
            else
            {
                RPNValue = null;
            }
            InStack.Pop();
            Data = InStack.Peek().Data;
            Token_Group = Group_of_Tokens.Variable;
            Row = InStack.Peek().Row;
            InStack.Pop();
            Type_Var_Data = new Typecial(InStack.Peek());
            Range = new Tuple<int, int>(InStack.Peek().Range.Item1, RPNValue.Range.Item2);
        }

        public Variable()
        {

        }
    }
}
