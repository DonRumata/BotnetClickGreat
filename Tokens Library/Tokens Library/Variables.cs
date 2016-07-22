﻿using System;
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
        public Local_Variable(string Ntype, object Nvalue, string Nname, int ID, int arg_num) : base(Ntype, Nvalue, Nname) { Func_ID = ID; }
    }

    public class Argument:Variable
    {
        public int Func_ID;
        public Argument(string Ntype, string Nname, int FID) : base(Ntype, 0, Nname) { Func_ID = FID; }
    }

    public class Global_Variable : Variable
    {
        public Global_Variable(string Ntype, object Nvalue, string Nname) : base(Ntype, Nvalue, Nname) { }
    }
}