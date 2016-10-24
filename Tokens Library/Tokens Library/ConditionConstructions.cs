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
        private Token Condition_code = null; //Хранит в себе код условия
        public Condition_Construction(Token InCondition)
        {
            Condition_code = InCondition;
        }
    }

    class If_Condition_construction:Condition_Construction
        /*Служит для обознчения конструкции IF*/
    {
        private List<Token> Then_body = null;
        private List<Token> Else_body = null;

        public If_Condition_construction(Token INCondition, List<Token> ThenBody, List<Token> ElseBody):base(INCondition)
        {
            Then_body = ThenBody;
            Else_body = ElseBody;
        }
    }
}
