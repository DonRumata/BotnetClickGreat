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
        public int num_of_args { get; }
        protected HashSet<string> Arg_types = new HashSet<string>();

        public User_Function(string return_type, string types)
        {
            num_of_args = Arg_types.Count();
        }

        public override void Interpretate()
        {

        }
    }
}
