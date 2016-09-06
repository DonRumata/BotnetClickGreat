using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{
    class Condition_Construction:Token
        /*Служит для обозначения конструкции IF*/
    {
        private List<Token> Condition_code = null; //Хранит в себе код условия
        private List<Token> Then_code = null;
        private List<Token> Else_code = null;
        public Condition_Construction()
        {
            Condition_code = new List<Token>();
        }

        public void Add_Token_to_condition_code(Token AddingData)
        {
            Condition_code.Add(AddingData);
        }
    }
}
