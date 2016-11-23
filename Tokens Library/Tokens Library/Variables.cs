using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Tokens_Library
{
    class VariableMethods
    {
        
        dynamic InsideFunction;
        private Token InvokeParameterIfNeeded = null;

        public VariableMethods(Delegate InFunc, Token ParamIfNeeded)
        {
            InsideFunction = InFunc;
            InvokeParameterIfNeeded = ParamIfNeeded;
        }

    }

    public sealed class Variable : Token
    {
        public Token RPNValue { get; set; }
        Typecial Type_Var_Data { get; set; }
        Queue<VariableMethods> CallQueue;
        
        public Variable(Expression InValue, Typecial InType)
        {
            RPNValue = InValue;
            Type_Var_Data = InType;
        }

        public void AddMethodToQueue(bool WhichOne, Token InsideArgument)
        {
            if (CallQueue==null)
            {
                CallQueue = new Queue<VariableMethods>();
            }
            if (WhichOne) //Get Method
            {
                CallQueue.Enqueue(new VariableMethods(new Func<Token>(GetValueRPN), null));
            }
            else
                CallQueue.Enqueue(new VariableMethods(new Action<Token>(SetValueRPN),InsideArgument));
        }


        public Token GetValueRPN()
        {
            return RPNValue;
        }

        public void SetValueRPN(Token InValue)
        {
            RPNValue = InValue;
        }


        public Variable(Stack<Token> InStack)
        {
            if (InStack.Peek().Token_Group == Group_of_Tokens.Assignment)
                InStack.Pop();
            int SRange = InStack.Peek().Range.Item2;
            if (InStack.Peek().Token_Group!=Group_of_Tokens.Name)
            {
                RPNValue = InStack.Pop();
                InStack.Pop();
                SRange = InStack.Peek().Range.Item2;
            }
            else
            {
                RPNValue = null; 
            }
            Data = InStack.Peek().Data;
            Token_Group = Group_of_Tokens.Variable;
            Range = new Tuple<int, int>(InStack.Peek().Range.Item1, SRange);
            Row = InStack.Pop().Row;
            Type_Var_Data = new Typecial(InStack.Pop());
            if (RPNValue != null)
            {

            }
        }

        public Variable(Variable InVarData, int row, Group_of_Tokens TokenGroup, int fvalue, int svalue)
        {
            RPNValue = InVarData.RPNValue;
            Type_Var_Data = InVarData.Type_Var_Data;
            Row = row;
            Token_Group = TokenGroup;
            Range = new Tuple<int, int>(fvalue, svalue);
            Data = InVarData.Data;
        }

        public Variable()
        {

        }
    }
}
