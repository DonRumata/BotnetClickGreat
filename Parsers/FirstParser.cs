using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tokens_Library;
using Exceptions_Library;
using Parsers;

namespace Parsers
{

    public enum PreTokenGroup
    {
        Symbol = 1,
        Numeric = 2,
        Alphabet = 3,
        RuAlphabet = 4,
        Delimeter = 5,
        NaN = -1,
        Default = 0,
        Space = 6,
    }

    class FirstParser
    {
        private List<AnyFunction> FuncStorage = null;
        private List<Variable> VarStorage = null;
        private List<Token> CodeStorage = null;
        private string Text = null;
        private int RowCount = 0;
        private HashSet<string> Type_definitions = null;
        private HashSet<string> Built_In_Functions = null;
        private HashSet<string> Construction_reservation = null;

        public void PARSETEXT(List<AnyFunction>InFunc, List<Variable>InVar, string Input_Text)
        {
            FuncStorage = InFunc;
            VarStorage = InVar;
            Text = Input_Text;
            PreTokenGroup nowcharID;
            PreTokenGroup PreviousCharID = PreTokenGroup.NaN;
            bool[] Checker_storage = new bool[6] {false,false,false,false,false,false };
            bool If_construct_translate = false;
            bool If_then_body_translate = false;
            bool If_else_body_translate = false;
            bool Function_definition_translate = false;
            bool Function_body_translate = false;
            bool Procedure_definition_translate = false;
            CodeStorage = new List<Token>();
            Hashsets_initialization();
            char nowchar;
            int i = 0;
            while (i!=Input_Text.Length)
            {
                nowchar = Input_Text[i];
                nowcharID = getTypeChar(nowchar);
                switch (nowcharID)
                {
                    case PreTokenGroup.Alphabet:
                    case PreTokenGroup.Numeric:
                    case PreTokenGroup.RuAlphabet:
                    case PreTokenGroup.Symbol:
                        i=While_delegate_function(c => getTypeChar(c)==nowcharID,nowcharID,i,PreviousCharID,CodeStorage,Input_Text,)
                }
            }
           
            
        }

        private void Hashsets_initialization() //Инициализирует значения множеств, при запуске парсера.
        {
            Type_definitions = new HashSet<string>() { "null","void", "int", "float", "double", "point", "char", "string", "picture" };
            Built_In_Functions = new HashSet<string>() { "" };
            Construction_reservation = new HashSet<string>() { "if", "while", "for", "function", "procedure", "do", "repeat", "until", "begin", "end" };
        }

        private int While_delegate_function(Func<char, bool> Cycle_condition, PreTokenGroup second_cycle_condition, int i_counter, Token previous_ID, List<Token> Word_list, string input_text, int row_count)
        /* Делегирует функцию, для сокращения кода похожих циклов While в коде*/
        {
            int helper_counter = i_counter;
            string Data_former = "";
            while ((helper_counter != input_text.Length) && (Cycle_condition(input_text[helper_counter])))  //Проходит циклом по тексту и формирует строку, пока входящая функция удовлетворяет второму условию
            {
                Data_former += input_text[helper_counter];
                helper_counter++;
            }
            Word_list.Add(GetToken(Data_former, second_cycle_condition, previous_ID, i_counter, helper_counter - 1));
            return helper_counter - 1;
        }

        private bool Numeric_check(char symbol)  //Выполняет проверку, является ли символ цифрой.
        {
            if ((symbol >= '0') && (symbol <= '9'))
                return true;
            else
                return false;
        }

        private int Alphabet_check(char symbol) //Проверяет является ли текущий символ кириллицей или латиницей.
        {
            if (((symbol >= 'a') && (symbol <= 'z')) || ((symbol >= 'A') && (symbol <= 'Z')))
                return 1;
            else if (((symbol >= 'а') && (symbol <= 'я')) || ((symbol >= 'А') && (symbol <= 'Я'))||(symbol=='ё')||(symbol=='Ё'))
                return 2;
            else
                return 0;
        }


        private PreTokenGroup getTypeChar(char c)
        {
            int Temp = Alphabet_check(c);
            if (Temp == 1)
                return PreTokenGroup.Alphabet;
            else if (Temp == 2)
                return PreTokenGroup.RuAlphabet;
            else if (Numeric_check(c))
                return PreTokenGroup.Numeric;
            else
            {
                switch (c)   //Отдельно создает заготовки токенов под различные возможные значения,
                {
                    case '+':
                    case '-':
                    case '=':
                    case '*':
                    case '/':
                    case '&':
                    case '|':
                    case '>':
                    case '<':
                    case '^':
                    case '%':
                        return PreTokenGroup.Symbol;
                    case '(':
                    case ')':
                    case '[':
                    case ']':
                    case '{':
                    case '}':
                    case '\n':
                    case '\r':
                    case '\"':
                    case '\'':
                    case ',':
                    case '.':
                        return PreTokenGroup.Delimeter;
                    case ' ':
                        return PreTokenGroup.Space;
                    default: return PreTokenGroup.Default;
                }
            }
        }

        private Token GetToken(string inStr, PreTokenGroup preID, Token ID_Of_previous, int FValue, int SValue)
        {
            Token Resulter;
            bool space_resulter = ID_Of_previous.Get_data() == " ";
            switch (preID)
            {
                case PreTokenGroup.Alphabet:
                    if (Construction_reservation.Contains(inStr))
                        construction_translation;
                    else if (Type_definitions.Contains(inStr))
                        return (new Token(inStr, space_resulter, Group_of_Tokens.Type_Definition, RowCount, FValue, SValue));
                    else if (Built_In_Functions.Contains(inStr))

                        break;
                case PreTokenGroup.Delimeter:
                    Delimeters_ID DelimID;
                    Help_SymbolsID HSymbID;
                    if (Is_delimeter(inStr, out DelimID))
                        return (new Delimeter(inStr, space_resulter, RowCount, FValue, SValue, DelimID));
                    else if (Is_Help_Symbol(inStr, out HSymbID))
                        return (new HelpSymbol(inStr, space_resulter, RowCount, FValue, SValue, HSymbID));
                    else
                        return null;
                case PreTokenGroup.RuAlphabet:
                    break;
                case PreTokenGroup.Symbol:
                    AriphmeticalSymbol_ID Ariphmetic_ID;
                    BooleanSymbol_ID Boolean_ID;
                    bool Equalty_resulter;
                    if (Is_ariphmetical(inStr, out Ariphmetic_ID, out Equalty_resulter))
                        return (new Ariphmetical(inStr, space_resulter, RowCount, FValue, SValue, Ariphmetic_ID));
                    else if (Is_logical(inStr, out Boolean_ID))
                        return (new Boolean_operation(inStr, space_resulter, RowCount, FValue, SValue, Boolean_ID));
                    break;
                case PreTokenGroup.Numeric:
                    return (new Token(inStr, space_resulter, Group_of_Tokens.Digit, RowCount, FValue, SValue));
            }
        }

        private Token construction_translation(string construction_name)
        {
            Token resulter = null;
            switch (construction_name)
            {
                case "if": break;
                case "while":break;
                case "for":break;
                case "function":
                    
                    break;
                case "procedure":break;
                case "repeat":break;
                case "until":break;
                case "begin":break;
                case "end":break;
                case "do":break;
            }
            return null;
        }



        private bool Reserve_name_check(string Input)
        {
            if (Construction_reservation.Contains(Input))
                return true;
            else return false;
        }

        private bool Is_delimeter(string Input, out Delimeters_ID Delim_ID)
        {
            switch(Input)
            {
                case "=": Delim_ID = Delimeters_ID.Equality; return true;
                case "(": Delim_ID = Delimeters_ID.LBracket; return true;
                case ")": Delim_ID = Delimeters_ID.RBracket; return true;
                case "{": Delim_ID = Delimeters_ID.LBrace; return true;
                case "}": Delim_ID = Delimeters_ID.RBrace; return true;
                case "\n":
                case "\r": Delim_ID = Delimeters_ID.New_string; return true;
                case ";": Delim_ID = Delimeters_ID.Semicolon; return true;
                default: Delim_ID = Delimeters_ID.NaN; return false;
            }
        }

        private bool Is_Help_Symbol(string Input, out Help_SymbolsID Help_Symbol_ID)
        {
            switch(Input)
            {
                case "\"": Help_Symbol_ID = Help_SymbolsID.Quotes; return true;
                case "'": Help_Symbol_ID = Help_SymbolsID.Apostrophe; return true;
                case ",": Help_Symbol_ID = Help_SymbolsID.Comma; return true;
                case ".": Help_Symbol_ID = Help_SymbolsID.Dot; return true;
                case "[": Help_Symbol_ID = Help_SymbolsID.LSqrBracket; return true;
                case "]": Help_Symbol_ID = Help_SymbolsID.RSqrBracket; return true;
                default: Help_Symbol_ID = Help_SymbolsID.NaN; return false;
            }
        }

        private bool Is_ariphmetical(string Input, out AriphmeticalSymbol_ID Ariphm_ID, out bool WasEquality)
        {
            WasEquality = false;
            switch (Input)
            {
                case "+":
                    Ariphm_ID = AriphmeticalSymbol_ID.Plus; return true;
                case "+=":
                    WasEquality = true;
                    goto case "+";
                case "-":
                    Ariphm_ID = AriphmeticalSymbol_ID.Minus; return true;
                case "-=":
                    WasEquality = true;
                    goto case "-";
                case "*":
                    Ariphm_ID = AriphmeticalSymbol_ID.Multiplication; return true;
                case "*=":
                    WasEquality = true;
                    goto case "*";
                case "/":
                    Ariphm_ID = AriphmeticalSymbol_ID.Division; return true;
                case "/=":
                    WasEquality = true;
                    goto case "*";
                case "%":
                    Ariphm_ID = AriphmeticalSymbol_ID.Mod; return true;
                case "^":
                    Ariphm_ID = AriphmeticalSymbol_ID.Involution; return true;
                default: Ariphm_ID = AriphmeticalSymbol_ID.NaN; return false;
            }
        }

        private bool Is_logical(string Input, out BooleanSymbol_ID Bool_ID)
        {
            switch (Input)
            {
                case ">": Bool_ID = BooleanSymbol_ID.Greater; return true;
                case ">=": Bool_ID = BooleanSymbol_ID.GreaterOrEqual; return true;
                case "<": Bool_ID = BooleanSymbol_ID.Less; return true;
                case "<=": Bool_ID = BooleanSymbol_ID.LessOrEqual; return true;
                case "==": Bool_ID = BooleanSymbol_ID.Equality; return true;
                case "!=": Bool_ID = BooleanSymbol_ID.NotEqual; return true;
                case "&&": Bool_ID = BooleanSymbol_ID.And; return true;
                case "||": Bool_ID = BooleanSymbol_ID.Or; return true;
                default: Bool_ID = BooleanSymbol_ID.NaN; return false;
            }
        }
    }
}
