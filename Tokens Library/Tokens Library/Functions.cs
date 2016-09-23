using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{

    public abstract class AnyFunction:Token
    {
        protected HashSet<Local_Variable> Args = new HashSet<Local_Variable>();
    }

    public sealed class Built_InFunction<T> : AnyFunction 
        /*Служит для хранения встроенных функций(не написанных пользователем). */
    {
        public delegate T Inside_function (HashSet<Local_Variable> arguments);  //Определение динамического делегата, создание которого зависит от типа указанного при создании класса
        private Inside_function Interpretation_code;  //Переменная хранит в себе метод, делегированый при составлении класса.
        public Built_InFunction(Inside_function Delegation_code,HashSet<Local_Variable> Local_variables)  //Базовый конструктор, записывает базовые локальные переменные и делегат исполняемого метода
        {
            Interpretation_code = Delegation_code;
            base.Args = Local_variables;
        }

        public T Interpretate(HashSet<Local_Variable> Input_args)  //Вызывает хранимый делегат.
        {
            return Interpretation_code.Invoke(base.Args.Concat(Input_args)as HashSet<Local_Variable>);
        }
    }

    public sealed class User_Function<T>:AnyFunction where T:class 
    {
        private List<Token> Function_code=null;

        public T Interpretate(HashSet<Local_Variable> Input_args)
        {
            T result = default(T);
            return result;
        }
    }
}
