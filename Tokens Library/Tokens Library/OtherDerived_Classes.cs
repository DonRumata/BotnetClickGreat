﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{

    public enum AriphmeticalSymbol_ID //Обозначения для арифметических символов
    {
        Plus = 1,
        Minus = 2,
        Multiplication = 3,
        Division = 4,
        Mod = 5,
        Involution = 6,
        NaN=0,
    }

    public enum Delimeters_ID //Обозначения для разделителей
    {
        Equality = 0,
        LBracket = 1,
        RBracket = 2,
        LBrace = 3,
        RBrace = 4,
        New_string = 5,
        Semicolon = 6,
        NaN = -1,
    }

    public enum Help_SymbolsID //Обозначения для вспомогательных символов
    {
        Quotes = 1,
        Apostrophe = 2,
        Comma = 3,
        Dot = 4,
        LSqrBracket = 5,
        RSqrBracket = 6,
        NaN=-1,
    }

    public enum BooleanSymbol_ID
    {
        Greater = 1,
        Less = 2,
        GreaterOrEqual = 3,
        LessOrEqual = 4,
        Equality = 5,
        NotEqual = 6,
        And = 7,
        Or = 8,
        NaN = 0,
    }

    public enum Names_ID
    {
        NaN=-1,
        Default=0,
        Type_definition=1,

    }


    public sealed class Delimeter : Token
    {
        public Delimeters_ID DelimeterID { get; set; }

        public Delimeter(string Ndata, bool Nspace, int Nrow, int NFRange, int NSRange, Delimeters_ID Delimeter_iD):base(Ndata,Nspace,Group_of_Tokens.Delimeter,Nrow,NFRange,NSRange)
        {
            DelimeterID = Delimeter_iD;
        }

        public override dynamic get_group_of_token()
        {
            return DelimeterID;
        }
    }

    public sealed class Digit : Token
    {
        public Typecial Inside_Type_info { get; private set; }
        public bool Is_fracture { get; private set; }

        public Digit(bool Fractured, Typecial InInfo, Token InValue)
        {
            Data = InValue.Data;
            Space_check = InValue.Space_check;
            Row = InValue.Row;
            Inside_Type_info = InInfo;
            Is_fracture = Fractured;
        }
    }

    public sealed class Ariphmetical : Token
    {
        public AriphmeticalSymbol_ID AriphmeticalID { get; private set; }

        public Ariphmetical(string Ndata, bool Nspace, int NRow, int NFRange, int NSRange, AriphmeticalSymbol_ID AriphmID):base(Ndata,Nspace,Group_of_Tokens.Ariphmetical, NRow,NFRange,NSRange)
        {
            AriphmeticalID = AriphmID;
        }

        public override dynamic get_group_of_token()
        {
            return AriphmeticalID;
        }
    }

    public sealed class HelpSymbol:Token
    {
        public Help_SymbolsID SymbolID { get; private set; }

        public HelpSymbol(string ndata, bool Nspace, int NRow, int NFRange, int NSRange, Help_SymbolsID SymbID):base(ndata,Nspace,Group_of_Tokens.Help_symbol,NRow,NFRange,NSRange)
        {
            SymbolID = SymbID;
        }

        public override dynamic get_group_of_token()
        {
            return SymbolID;
        }
    }

    public sealed class Boolean_operation : Token
    {
        public BooleanSymbol_ID BooleanID { get; private set; }

        public Boolean_operation(string Ndata, bool Nspace, int NRow, int NFRange, int NSRange, BooleanSymbol_ID BoolID) : base(Ndata, Nspace, Group_of_Tokens.BooleanOperation, NRow, NFRange, NSRange)
        {
            BooleanID = BoolID;
        }

        public override dynamic get_group_of_token()
        {
            return BooleanID;
        }
    }
}