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
        protected HashSet<Local_Variable> L_variables = new HashSet<Local_Variable>();
        protected HashSet<Argument> Args = new HashSet<Argument>();
        public User_Function(string rtype, HashSet<Local_Variable> locals)
        {
            return_type = rtype;
            L_variables = locals;
        }

        public override void Interpretate()
        {

        }
    }
}
