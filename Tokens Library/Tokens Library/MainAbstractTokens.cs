using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{

    public enum ETypeTable
    {
        NULL = 0,
        Int = 1,
        Float = 2,
        Double = 3,
        Point = 4,
        Char = 5,
        String = 6,
        Picture = 7,
    }

    public class Typecial
    {
        protected string Type_Name;
        protected ETypeTable Type_ID;

        public ETypeTable Get_type()
        {
            return Type_ID;
        }
    }

    public enum Group_of_Tokens
    {
        Ariphmetical = 1,
        Delimeter = 2,
        Digit = 3,
        Name = 4,
        BooleanOperation = 5,
        Help_symbol = 6,
        Construction = 7,
        Function = 8,
        Type_Definition = 9,
        NoN = 0,
    }

    public class Token
    {
        protected Tuple<int, int> Range; //расположение предтокена в строке (номер первого символа, номер последнего символа)
        protected int Row;               //Номер строки
        protected bool Space_check;      //флаг наличия пробела до предтокена
        protected string Data;           //сам предтокен
        protected Group_of_Tokens Token_Group; //Номер типа токена


        public Token(string data, bool space, Group_of_Tokens id, int row, int FRange_value, int SRange_value) //Конструктор для создания класса Word, сразу со всеми первоначальными данными
        {
            this.Data = data;
            this.Space_check = space;
            this.Token_Group = id;
            this.Row = row;
            this.Range = Tuple.Create(FRange_value, SRange_value);
        }

        public Token()  //Вариант конструктора для простого выделения памяти и создания класса с значениями по умолчанию.
        {
            this.Data = "";
            this.Space_check = false;
            this.Token_Group = 0;
            this.Row = 0;
            this.Range = null;
        }

        public string Get_data()
        {
            return Data;
        }
    }
    
        
}
