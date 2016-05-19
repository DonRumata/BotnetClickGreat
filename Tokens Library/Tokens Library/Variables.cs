using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{
    public abstract class Variable
    {
        public string Var_name { get; set; }
        protected string Val_type;
        protected object Value;
        public Variable(string type, object value, string name)
        {
            Var_name = name;
            Val_type = type;
            Value = value;
        }

        public object Get_value()
        {
            return Value;
        }

        public string Get_val_type()
        {
            return Val_type;
        }
    }

    public class Local_Variable : Variable
    {
        public int Func_ID;
        public int arg_chainer;
        public Local_Variable(string Ntype, object Nvalue, string Nname, int ID, int arg_num) : base(Ntype, Nvalue, Nname) { Func_ID = ID; arg_chainer = arg_num; }
    }

    public class Global_Variable : Variable
    {
        public Global_Variable(string Ntype, object Nvalue, string Nname) : base(Ntype, Nvalue, Nname) { }
    }

    public class Argument
    {
        public string Arg_name { get; set; }
        protected string Arg_type;
        protected int Func_ID;

        public Argument(string Name, string type, int ID)
        {
            Arg_name = Name;
            Arg_type = type;
            Func_ID = ID;
        }
    }

}
