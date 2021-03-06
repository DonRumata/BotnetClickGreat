﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{

    public enum ETypeTable
    {
        NonStarted=-5,
        Delimeter=-4,
        BoolTerminal=-3,
        AriphmeticTerminal=-2,
        ERR=-1,
        NULL = 0,
        Byte=1,
        Short=2,
        Int = 3,
        Long=4,
        Float = 5,
        Double = 6,
        Point = 7,
        Char = 8,
        String = 9,
        Overflowed = 10,
        Picture = 11,
        Boolean=12,
    }

    public enum ETypeGroup
    {
        IntegerGroup=0,
        StringGroup=1,
    }

    //public enum ETypeGroup
    //{
    //    NULLGROUP = -3,
    //    AriphmeticalOperandGroup = -2,
    //    BoolOperandGroup = -1,
    //    DelimeterGroup = 0,
    //    DigitGroup = 1,
    //    PictureGroup = 2,
    //    StringGroup = 3,
    //    UniversalGroup = 4,
    //}



    public class Typecial
    {
        public string Type_Name { get; set; }
        public ETypeTable Type_ID { get; set; }
        public Typecial(string Type_name)
        {
            Type_Name = Type_name.ToLower();
            Type_ID = GetETypeFromString(Type_Name);
        }

        public static ETypeGroup GetTypeGroup(ETypeTable InType)
        {
            switch (InType)
            {
                case ETypeTable.Byte:
                case ETypeTable.Short:
                case ETypeTable.Int:
                case ETypeTable.Long:
                case ETypeTable.Float:
                case ETypeTable.Double:
                    return ETypeGroup.IntegerGroup;
                case ETypeTable.String:
                case ETypeTable.Overflowed:
                    return ETypeGroup.StringGroup;
                default: return ETypeGroup.StringGroup;
            }
        }

        public Typecial(Token InTok)
        {
            Type_Name = InTok.Data.ToLower();
            Type_ID = GetETypeFromString(Type_Name);
        }

        public Typecial(ETypeTable InType_ID)
        {
            Type_ID = InType_ID;
            Type_Name = GetTypeNameFromEType(Type_ID);
        }

        public Typecial(string ByValue, bool[] AnyBoolFlags)
        {

        }

        private ETypeTable GetETypeFromString(string Name)
        {
            switch (Name)
            {
                case "byte":
                    return ETypeTable.Byte;
                case "short":
                    return ETypeTable.Short;
                case "int":
                    return ETypeTable.Int;
                case "long":
                    return ETypeTable.Long;
                case "float":
                    return ETypeTable.Float;
                case "double":
                    return ETypeTable.Double;
                case "point":
                    return ETypeTable.Point;
                case "char":
                    return ETypeTable.Char;
                case "string":
                    return ETypeTable.String;
                case "picture":
                    return ETypeTable.Picture;
                case "bool":
                    return ETypeTable.Boolean;
                case "overflowed":
                    return ETypeTable.Overflowed;
                default:
                    return ETypeTable.NULL;
            }
        }

        private string GetTypeNameFromEType(ETypeTable Type_id)
        {
            switch (Type_id)
            {
                case ETypeTable.Byte:
                    return "byte";
                case ETypeTable.Short:
                    return "short";
                case ETypeTable.Int:
                    return "int";
                case ETypeTable.Long:
                    return "long";
                case ETypeTable.Float:
                    return "float";
                case ETypeTable.Double:
                    return "double";
                case ETypeTable.Point:
                    return "point";
                case ETypeTable.Char:
                    return "char";
                case ETypeTable.String:
                    return "string";
                case ETypeTable.Picture:
                    return "picture";
                case ETypeTable.Boolean:
                    return "bool";
                case ETypeTable.Overflowed:
                    return "overflowed";
                case ETypeTable.NULL:
                    return "null";
                default:
                case ETypeTable.ERR:
                    return "ERR";
            }
        }

        public string Get_TypeName()
        {
            return Type_Name;
        }
    }
    

    public class GenericType<T>
    {
        public T Value { get; protected set; }

        public GenericType(T t)
        {
            Value = t;
        }

        public virtual void Convert(byte In) { }
        public virtual void Convert(short In) { }
        public virtual void Convert(int In) { }
        public virtual void Convert(long In) { }
        public virtual void Convert(float In) { }
        public virtual void Convert (double In) { }
        public virtual void Convert(char In) { }
        public virtual void Convert(string In) { }
        public virtual void Convert(bool In) { }
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
        Variable=11,
        Assignment=12,
        EndOfString=13,
        VariableMethodCall=14,
        NoN = 0,
    }

    public enum EndOfStringID
    {
        NewString=1,
        Enter=2,
        Semicolon=3,
        NaN=0,
    }

    public class Byt:GenericType<byte>
    {
        public Byt(byte inV) : base(inV) { }

        public override void Convert(short In)
        {
            
        }

        public override void Convert(int In)
        {

        }

        public override void Convert(long In)
        {
            
        }

        public override void Convert(float In)
        {
            
        }

        public override void Convert(double In)
        {
            
        }

        public override void Convert(string In)
        {
            
        }

        public override void Convert(byte In)
        {
             
        }

        public override void Convert(bool In)
        {
            
        }
    }

    public class Shor : GenericType<short>
    {
        public Shor(short inV) : base(inV) { }

        public override void Convert(byte In)
        {
            
        }

        public override void Convert(int In)
        {
            
        }

        public override void Convert(long In)
        {
            
        }

        public override void Convert(float In)
        {
            
        }

        public override void Convert(short In)
        {
            
        }

        public override void Convert(double In)
        {
            
        }

        public override void Convert(string In)
        {
            
        }

        public override void Convert(bool In)
        {
            
        }

    }

    public class Integ:GenericType<int>
    {
        public Integ(int inV) : base(inV) { }

        public override void Convert(byte In)
        {

        }

        public override void Convert(short In)
        {

        }

        public override void Convert(int In)
        {

        }

        public override void Convert(long In)
        {

        }

        public override void Convert(double In)
        {

        }

        public override void Convert(float In)
        {

        }

        public override void Convert(string In)
        {

        }

        public override void Convert(bool In)
        {

        }
    }

    public class Lon:GenericType<long>
    {
        public Lon(long inV) : base(inV) { }

        public override void Convert(byte In)
        {
            
        }

        public override void Convert(short In)
        {
            
        }

        public override void Convert(int In)
        {
            
        }

        public override void Convert(long In)
        {
            
        }

        public override void Convert(bool In)
        {
            
        }

        public override void Convert(float In)
        {
            
        }

        public override void Convert(double In)
        {
            
        }

        public override void Convert(string In)
        {
            
        }
    }

    public class Floa:GenericType<float>
    {
        public Floa(float inV) : base(inV) { }

        public override void Convert(byte In)
        {

        }

        public override void Convert(short In)
        {

        }

        public override void Convert(int In)
        {

        }

        public override void Convert(float In)
        {

        }

        public override void Convert(long In)
        {

        }

        public override void Convert(double In)
        {

        }

        public override void Convert(string In)
        {

        }

        public override void Convert(bool In)
        {

        }
    }

    public class Doubl:GenericType<double>
    {
        public Doubl(double inV) : base(inV) { }

        public override void Convert(byte In)
        {

        }

        public override void Convert(short In)
        {

        }

        public override void Convert(int In)
        {

        }

        public override void Convert(float In)
        {

        }

        public override void Convert(long In)
        {

        }

        public override void Convert(double In)
        {

        }

        public override void Convert(string In)
        {

        }

        public override void Convert(bool In)
        {

        }
    }

    public class Cha:GenericType<char>
    {
        public Cha(char inV) : base(inV) { }
    }

    public class Str:GenericType<string>
    {
        public Str(string inV) : base(inV) { }
    }

    //public class Point:GenericType<Point>
    //{
    //    public Point
    //}

    //public class Picture:Typecial
    //{

    //}

    public class Boole:GenericType<bool>
    {
        public Boole(bool inV) : base(inV) { }
    }


    public class Token
    {
        public Tuple<int, int> Range { get; protected set; } //расположение предтокена в строке (номер первого символа, номер последнего символа)
        public int Row { get; protected set; }               //Номер строки
        public bool Space_check { get; protected set; }      //флаг наличия пробела до предтокена
        public string Data { get; protected set; }           //сам предтокен
        public Group_of_Tokens Token_Group { get; protected set; } //Номер типа токена
        public int Priority { get; protected set; }         //Приоритет токена может изменятся в зависимости от положения токена в строке.

        private void GetBasePriority()
        {
            switch(Token_Group)
            {
                case Group_of_Tokens.Ariphmetical:
                    break;
                case Group_of_Tokens.Assignment:
                    break;
                case Group_of_Tokens.BooleanOperation:
                    break;
                case Group_of_Tokens.Construction:
                    break;
                case Group_of_Tokens.Function:
                    break;
                case Group_of_Tokens.Variable:
                    break;
            }
        }

        public virtual ETypeTable GetTypeOfToken()
        {
            return ETypeTable.NULL;
        }

        public void IncreasePriority()
        {
            Priority++;
        }

        public void DecreasePriority()
        {
            Priority--;
        }

        public virtual void BaseSetPriority()
        {

        }

        public virtual bool Is_Terminal()
        {
            return false;
        }

        public virtual int get_priority()
        {
            return Priority;
        }

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

        public Token(Token InCopy, Group_of_Tokens New_Token_Group)
        {
            Data = InCopy.Data;
            Row = InCopy.Row;
            Space_check = InCopy.Space_check;
            Token_Group = New_Token_Group;
            Range = InCopy.Range;
        }

        public Token(Token InCopy, string data, int SRange_value)
        {
            Data = InCopy.Data+data;
            Row = InCopy.Row;
            Space_check = InCopy.Space_check;
            Token_Group = InCopy.Token_Group;
            Range = new Tuple<int, int>(InCopy.Range.Item1, SRange_value);
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

        public EndOfStringID GetEOS_ID()
        {
            if (Token_Group == Group_of_Tokens.EndOfString)
            {
                switch (Data)
                {
                    case ";": return EndOfStringID.Semicolon;
                    case "\n": return EndOfStringID.NewString;
                    case "\r": return EndOfStringID.Enter;
                    default: return EndOfStringID.NaN;
                }
            }
            else return EndOfStringID.NaN;
        }

        public Constructions_ID GetID_of_Construction()
        {
            if (Token_Group == Group_of_Tokens.Construction)
            {
                switch (Data.ToLower())
                {
                    case "function": return Constructions_ID.Function;
                    case "if": return Constructions_ID.If;
                    case "for": return Constructions_ID.For;
                    case "while": return Constructions_ID.While;
                    case "repeat": return Constructions_ID.Repeat;
                    case "else": return Constructions_ID.Else;
                    default: return Constructions_ID.NoN;
                }
            }
            else return Constructions_ID.NoN;
        }
    }
    
        
}
