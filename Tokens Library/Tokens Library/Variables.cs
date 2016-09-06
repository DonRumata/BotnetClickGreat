using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{
    public class Variable : Typecial, Token
    {
        public string Value { get; set; }
    }

    public class Local_Variable : Variable
        /*Служит для обозначения локальных переменных и аргументов*/
    {
        public int Func_ID { get; set; } //Содержит в себе ID функции к которой принадлежит аргумент или локальная переменная
    }
}
