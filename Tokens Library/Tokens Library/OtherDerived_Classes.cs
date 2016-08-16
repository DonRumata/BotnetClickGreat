using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{

    public enum AriphmeticalSymbol_ID
    {
        Plus = 1,
        Minus = 2,
        Multiplication = 3,
        Division = 4,
        Mod = 5,
        Involution = 6,
    }

    public enum Delimeters_ID
    {
        Equality = 0,
        PlusEquality = 1,
        MinusEquality = 2,
        MultiplicationEquality = 3,
        DivisionEquality = 4,
        LBracket = 5,
        RBracket = 6,
        LBrace = 7,
        RBrace = 8,
        New_string = 9,
        NaN = -1,
    }

    public enum Help_SymbolsID
    {
        Quotes = 1,
        Apostrophe = 2,
        Comma = 3,
        Dot = 4,
        LSqrBracket = 5,
        RSqrBracket = 6,
    }


    public class Delimeter : Token
    {
        public Delimeters_ID DelimeterID { get; set; }
    }

    public class Digit : Typecial, Token
    {

    }

    public class Ariphmetical : Token
    {
        public AriphmeticalSymbol_ID AriphmeticalID { get; set; }
    }

    public class HelpSymbol:Token
    {
        public Help_SymbolsID SymbolID { get; set; }
    }


}
