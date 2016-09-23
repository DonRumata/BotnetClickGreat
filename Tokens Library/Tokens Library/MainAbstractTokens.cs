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

        public string Get_TypeName()
        {
            return Type_Name;
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
        AriphmeticalExpression = 10,
        NoN = 0,
    }

    public class Token
    {
        public Tuple<int, int> Range { get; protected set; } //расположение предтокена в строке (номер первого символа, номер последнего символа)
        public int Row { get; protected set; }               //Номер строки
        public bool Space_check { get; protected set; }      //флаг наличия пробела до предтокена
        public string Data { get; protected set; }           //сам предтокен
        public Group_of_Tokens Token_Group { get; protected set; } //Номер типа токена


        public Token(string data, bool space, Group_of_Tokens id, int row, int FRange_value, int SRange_value) //Конструктор для создания класса Word, сразу со всеми первоначальными данными
        {
            Data = data;
            Space_check = space;
            Token_Group = id;
            Row = row;
            Range = Tuple.Create(FRange_value, SRange_value);
        }

        public Token()  //Вариант конструктора для простого выделения памяти и создания класса с значениями по умолчанию.
        {
            Data = "";
            Space_check = false;
            Token_Group = Group_of_Tokens.NoN;
            Row = 0;
            Range = null;
        }

        public virtual dynamic get_group_of_token()
        {
            return Token_Group;
        }

        public Token List_data_editable(List<Token> In_list, Group_of_Tokens New_Token_Group)
        {
            for (int i =2; i!= In_list.Count; i++)
            {
                Data += In_list[i].Data;
            }
            Space_check = In_list.First().Space_check;
            Range = new Tuple<int, int>(Range.Item1, In_list.Last().Range.Item2);
            Token_Group = New_Token_Group;
            return this;
        }
    }
    
        
}
