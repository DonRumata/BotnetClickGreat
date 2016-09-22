using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{
    abstract class Condition_Construction:Token
        /*Базовый класс конструкций с условиями*/
    {
        private List<Token> Condition_code = null; //Хранит в себе код условия
        public Condition_Construction()
        {
            Condition_code = new List<Token>();
        }

        public void Add_Token_to_condition_code(Token AddingData)
        {
            Condition_code.Add(AddingData);
        }
    }

    class If_Condition_construction:Condition_Construction
        /*Служит для обознчения конструкции IF*/
    {
        private List<Token> Then_body = null;
        private List<Token> Else_body = null;
    }
}
