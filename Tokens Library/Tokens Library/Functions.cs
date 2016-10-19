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

    public sealed class User_Function<T>:AnyFunction where T:class 
    {
        private List<Token> Function_code=null;

        public T Interpretate(HashSet<Variable> Input_args)
        {
            T result = default(T);
            return result;
        }
        public override void ReCreateToken(string NData, bool NSpace, Group_of_Tokens NGroup, int NRow, int NFRange, int NSRange)
        {
        }
    }
}
