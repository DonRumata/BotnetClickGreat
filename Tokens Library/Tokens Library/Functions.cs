using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{

    public abstract class AnyFunction : Token
    {
        public List<Variable> Args { protected set; get; } =new List<Variable>();

        public AnyFunction(string Ndata, bool Nspace, Group_of_Tokens Ngroup, int Nrow, int NFRange, int NSRange) : base(Ndata, Nspace, Ngroup, Nrow, NFRange, NSRange) { }
        public AnyFunction() { }
        public abstract void ReCreateToken(string NData, bool NSpace, Group_of_Tokens NGroup, int NRow, int NFRange, int NSRange);
        public abstract bool AddArgument(Token InArg, int ArgNumber, ETypeGroup InGroup);
    }

    public sealed class Built_InFunction<T> : AnyFunction 
        /*Служит для хранения встроенных функций(не написанных пользователем). */
    {
        public delegate T Inside_function (List<Variable> arguments);  //Определение динамического делегата, создание которого зависит от типа указанного при создании класса
        private Inside_function Interpretation_code;  //Переменная хранит в себе метод, делегированый при составлении класса.
        public Built_InFunction(Inside_function Delegation_code, List<Variable> Arguments)  //Базовый конструктор, записывает базовые локальные переменные и делегат исполняемого метода
        {
            Interpretation_code = Delegation_code;
            Args = Arguments;
        }

        /*public Built_InFunction(dynamic InsideCopy,string NData, bool NSpace, Group_of_Tokens NGroup, int NRow, int NFRAnge, int NSRAnge)
        {
            Interpretation_code = InsideCopy.Interpretation_code;
            Args = InsideCopy.Args;
        }*/

        public override bool AddArgument(Token InArg, int ArgNumber, ETypeGroup InGroup)
        {
            if(InGroup==Typecial.GetTypeGroup(Args[ArgNumber].GetTypeOfToken()))
            Args[ArgNumber].RPNValue = InArg;
            return true;
        }

        public override void BaseSetPriority()
        {
            Priority = 12;
        }

        public override bool Is_Terminal()
        {
            return false;
        }

        public T Interpretate()  //Вызывает хранимый делегат.
        {
            return Interpretation_code.Invoke(Args);
        }

        public override void ReCreateToken(string NData, bool NSpace, Group_of_Tokens NGroup, int NRow, int NFRange, int NSRange)
        {
            Data = NData;
            Space_check = NSpace;
            Token_Group = NGroup;
            Row = NRow;
            Range = new Tuple<int, int>(NFRange, NSRange);
        }

        public int GetDelegateMethodType()
        {
            switch(Type.GetTypeCode(Interpretation_code.Method.ReturnType))
            {
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Double:
                    return 1;
                case TypeCode.Object:
                    return 0;
                default: return -1;
            }
        }
    }

    public class ComparerForArgVar:IEqualityComparer<Variable>
    {
        public bool Equals(Variable obj1, Variable obj2)
        {
            if (obj1.Data == obj2.Data)
                return true;
            else
                return false;
        }

        public int GetHashCode(Variable obj)
        {
            return obj.GetHashCode();
        }
    }

    public sealed class User_Function:AnyFunction
    {
        private List<Token> Function_code=null;
        private Typecial ReturnTypeCode;

        public User_Function(Stack<Token> InStack)
        {
            Args = new List<Variable>();
            Token Named = InStack.Pop();
            Range = new Tuple<int, int>(-1, Named.Range.Item2);
            while (Named.Token_Group==Group_of_Tokens.Variable)
            {
                Args.Add(Named as Variable);
                Named = InStack.Pop();
            }
            Args.Reverse();
            Data = InStack.Peek().Data;
            Row = InStack.Pop().Row;
            ReturnTypeCode = new Typecial(InStack.Pop());
            Range = new Tuple<int, int>(InStack.Pop().Range.Item1, Range.Item2);
            Function_code = new List<Token>();
        }

        public override bool AddArgument(Token InArg, int ArgNumber, ETypeGroup InGroup)
        {
            return false;
        }

        public void AddNewFunctionBodyString(Token NewStringIn)
        {
            Function_code.Add(NewStringIn);
        }

        public bool IsArgsContains(string Name,out Variable VarTypeName)
        {
            int i = 0;
            VarTypeName = null;
            while (i!=Args.Count)
            {
                if (Args[i].Data == Name)
                {
                    VarTypeName = Args[i];
                    return true;
                }
                else
                    i++;
            }
            return false;
        }

        public bool AddMethodToArgument(string ArgName, Token InValue, bool whichMethod)
        {
            int i = -1;
            i=Args.FindIndex(argspred => argspred.Data == ArgName);
            if (i != -1)
            {
                Args[i].AddMethodToQueue(whichMethod, InValue);
                return true;
            }
            else
                return false;
        }

        public bool ChangeMethodOfArgument(string InToken, Token InValue)
        {
            int i = -1;
            i = Args.FindIndex(argspred => argspred.Data == InToken);
            if (i != -1)
            {
                Args[i].ChangeLastMethodInQueue(InValue);
                return true;
            }
            else
                return false;
        }

        public Token IsArgsContains(string Name)
        {
           return Args.Find(argspis => argspis.Data == Name); 
        }

        public bool AddLocalArgument(Variable NewArgIn)
        {
            if (Args.Contains(NewArgIn, new ComparerForArgVar()))
                return false;
            else
            {
                Args.Add(NewArgIn);
                return true;
            }
        }

        public dynamic Interpretate()
        {
            return 0;
        }
        public override void ReCreateToken(string NData, bool NSpace, Group_of_Tokens NGroup, int NRow, int NFRange, int NSRange)
        {
        }
    }
}
