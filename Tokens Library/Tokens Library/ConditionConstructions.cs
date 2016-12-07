using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{
    public abstract class Condition_Construction:Token
        /*Базовый класс конструкций с условиями*/
    {
        private Token Condition_code = null; //Хранит в себе код условия
        protected Condition_Construction(Token InCondition)
        {
            Condition_code = InCondition;
        }
        public Condition_Construction(Stack<Token> InData, Group_of_Tokens InGroupOfTokens)
        {
            Range = new Tuple<int, int>(0, InData.Peek().Range.Item2);
            Condition_code = InData.Pop();
            Token TempPeekStack = InData.Peek();
            Data = TempPeekStack.Data;
            Range = new Tuple<int, int>(TempPeekStack.Range.Item1, Range.Item2);
            Space_check = TempPeekStack.Space_check;
            Row = TempPeekStack.Row;
            InData.Pop();
        }
    }

    public class If_Condition_construction:Condition_Construction
        /*Служит для обознчения конструкции IF*/
    {
        private List<Token> Then_body = null;
        private List<Token> Else_body = null;
        public int BraceCount { get; protected set; }
        public bool ThenInTranslation { get; set; }
        public If_Condition_construction(Stack<Token> DataIN):base(DataIN, Group_of_Tokens.Construction)
        {
            BraceCount = 0;
            ThenInTranslation = true;
            Then_body = new List<Token>();
            Else_body = new List<Token>();
            //try
        }

        public void AddMethodToBody(Token InExpression)
        {
            if (ThenInTranslation)
                Then_body.Add(InExpression);
            else
                Else_body.Add(InExpression);
        }

        public bool ThenBodyContains()
        {
            return Then_body.Count > 0;
        }

        public bool ThenBodyNOTContains()
        {
            return Then_body.Count == 0;
        }

        public void IncreaseBraceCount()
        {
            BraceCount++;
        }

        public void DecreaseBraceCount()
        {
            BraceCount--;
        }

    }
}
