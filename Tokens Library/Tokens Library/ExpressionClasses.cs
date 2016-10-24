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
        public LinkedList<Token> RPN_Expression_data { get; private set; }
        public Expression_Type EXPRType { get; private set; }

        public Expression(Stack<Token> InputStr, Expression_Type Input_Type_OFEXPR, Func<Stack<Token>, bool> InWhileConditionDelegate)
            /*Конструктор выражений, использует входной стек для формирования необходимого выражения*/
        {
            RPN_Expression_data = new LinkedList<Token>();
            EXPRType = new Expression_Type();
            dynamic Temp_tok;
            //(InputStr.Count > 0) && (InputStr.Peek().Token_Group != Group_of_Tokens.Delimeter) && (InputStr.Peek().Token_Group != Group_of_Tokens.BooleanOperation)
            while (InWhileConditionDelegate.Invoke(InputStr)) //Цикл идущий по стеку до его конца или до первого разделителя
            {
                if (InputStr.Peek().Token_Group==Group_of_Tokens.AriphmeticalExpression) //Условие для нахождения токенов формата AriphmeticalExpression
                {
                    Temp_tok = InputStr.Pop();
                    if (RPN_Expression_data.Count>0)
                    {
                        ReWriteRPNExpression(Temp_tok);
                    }
                    else
                    {
                        RPN_Expression_data = Temp_tok.RPN_Expression_data;
                        Data = Temp_tok.Data;
                    }
                    Range = Temp_tok.Range;
                    Space_check = Temp_tok.Space_check;
                    Row = Temp_tok.Row;
                }
                else
                {
                    RPN_Expression_data.AddFirst(InputStr.Pop());
                    Data += RPN_Expression_data.First().Data;
                }
            }
            Temp_tok = RPN_Expression_data.First() as Token;
            EXPRType = Input_Type_OFEXPR;
            Range = new Tuple<int, int>(Temp_tok.Range.Item2, RPN_Expression_data.Last().Range.Item1);
            Space_check = Temp_tok.Space_check;
            Row = Temp_tok.Row;
            Token_Group = Group_of_Tokens.AriphmeticalExpression;
        }

        private void ReWriteRPNExpression(Expression Tempe)
            /*Переписывает значение встреченного выражения в ново создаваемое*/
        {
            while(Tempe.RPN_Expression_data.Count>0)
            {
                RPN_Expression_data.AddFirst(Tempe.RPN_Expression_data.Last());
                Data += RPN_Expression_data.First().Data;
                Tempe.RPN_Expression_data.RemoveLast();
            }
        }
    }
}
