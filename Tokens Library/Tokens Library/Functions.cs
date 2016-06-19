using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{
    public abstract class AnyFunction
    {
        public abstract void Interpretate();
    }

    public class BuiltIn_Function : AnyFunction
    {
        public override void Interpretate()
        {

        }
    }

    public class User_Function : AnyFunction
    {
        private string return_type;
        protected string Func_body;
        protected HashSet<Local_Variable> L_variables = new HashSet<Local_Variable>();
        protected HashSet<Argument> Args = new HashSet<Argument>();
        public User_Function(string rtype, string Bdy, HashSet<Local_Variable> locals, HashSet<Argument> Arguments)
        {
            return_type = rtype;
            Func_body = Bdy;
            L_variables = locals;
            Args = Arguments;
        }

        public override void Interpretate()
        {

        }
    }
}
