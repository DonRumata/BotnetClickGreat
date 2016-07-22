using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{
    public abstract class AnyFunction
    {
        protected HashSet<Argument> Args = new HashSet<Argument>();
        public abstract void Interpretate();
        public abstract new string GetType();
        public abstract string Get_arg_type(int arg_num);
    }

    public class BuiltIn_Function : AnyFunction
    {
        
        public override void Interpretate()
        {

        }

        public override string GetType()
        {
            throw new NotImplementedException();
        }

        public override string Get_arg_type(int arg_num)
        {
            return base.Args.ElementAt(arg_num).Get_val_type();
        }
    }

    public class User_Function : AnyFunction
    {
        private string return_type;
        protected string Func_body;
        protected HashSet<Local_Variable> L_variables = new HashSet<Local_Variable>();
        public User_Function(string rtype, string Bdy, HashSet<Local_Variable> locals, HashSet<Argument> Arguments)
        {
            return_type = rtype;
            Func_body = Bdy;
            L_variables = locals;
            base.Args = Arguments;
        }

        public override void Interpretate()
        {

        }

        public override string GetType()
        {
            return return_type;
        }

        public override string Get_arg_type(int arg_num)
        {
            return base.Args.ElementAt(arg_num).Get_val_type();
        }
    }

    public class Condition_function
    {
        private List<string> First_body;
        private List<string> Second_body;
        private List<string> Condition;
        private int condition_id;
    }
}
