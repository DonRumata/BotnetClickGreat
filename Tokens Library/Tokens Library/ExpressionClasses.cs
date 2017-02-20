using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{
    public enum Expression_Type
    {
        NaN=-2,
        Err=-1,
        Ariphmetical_expression=1,
        Logical_expression=2,
    }

    public class Expression : Token
    {
        public Queue<Token> RPN_Expression_data { get; private set; }
        public ETypeTable ExpressionTypeResult { get; private set; }
        public bool Check_Semantical { get; private set; }

        public Expression(Stack<Token> InputStr, Func<Stack<Token>, bool> InWhileConditionDelegate, bool WithFinalReverse, bool SemanticalCheck=false, dynamic SemanticalParser=null)
        /*Конструктор выражений, использует входной стек для формирования необходимого выражения*/
        {
            RPN_Expression_data = new Queue<Token>();
            Stack<Token> OperandStack = new Stack<Token>();
            Queue<Token> TempList = new Queue<Token>();
            Check_Semantical = false;
            Token_Group = Group_of_Tokens.AriphmeticalExpression;
            while (InWhileConditionDelegate.Invoke(InputStr))
            {
                TempList.Enqueue(InputStr.Pop());
            }
            if (WithFinalReverse)
                TempList = new Queue<Token>(TempList.Reverse());
            Range = new Tuple<int, int>(TempList.Peek().Range.Item1, TempList.Last().Range.Item2);
            Space_check = TempList.Peek().Space_check;
            Row = TempList.Peek().Row;
            Data = null;
            while (TempList.Count!=0)
            {
                if (TempList.Peek().Token_Group == Group_of_Tokens.AriphmeticalExpression)
                {
                    Expression Temp_tok = TempList.Dequeue() as Expression;
                    ReWriteRPNExpression(Temp_tok);
                }
                else
                {
                    if (TempList.Peek().Is_Terminal())
                    {
                        if (OperandStack.Count > 0)
                            if (TempList.Peek().Priority <= OperandStack.Peek().Priority)
                                RPN_Expression_data.Enqueue(OperandStack.Pop());
                        OperandStack.Push(TempList.Dequeue());
                    }
                    else
                        RPN_Expression_data.Enqueue(TempList.Dequeue());
                }
            }
            while (OperandStack.Count > 0)
            {
                RPN_Expression_data.Enqueue(OperandStack.Pop());
            }
            if (SemanticalCheck)
            {
                ExpressionTypeResult = SemanticalParser.String_semantical_check(this);
            }
                
        }

        private void ReWriteRPNExpression(Expression Tempe)
            /*Переписывает значение встреченного выражения в ново создаваемое*/
        {
            RPN_Expression_data= new Queue<Token>(RPN_Expression_data.Concat(Tempe.RPN_Expression_data));
        }

        public void SemanticalWasChecked()
        {
            Check_Semantical = true;
        }

        public void SetExpressionResultType(ETypeTable SettingType)
        {
            ExpressionTypeResult = SettingType;
        }
    }
}
