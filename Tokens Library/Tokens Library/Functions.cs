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
}
