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

    public enum Rules_Statement
    {
        DefaultInStr=-2,
        Default=-1,
        DigitFormError=0,
        BeginDigit=1,
        WhichNextDigit =2,
        DigitInStr=3,
        DigitLogicalNext=4,
        DigitAriphmeticalNext=5,
        FormAriphmExpression=6,
        FormLogicalExpression=7,
        AfterArifmExpr=8,
        LBracketDigit = 9,
        FuncCallBegin =11,
        FuncCallInStr=12,
        
    }
    public enum Which_builder
    {
        Digit=1,
    }

    public class Builder
    {
        private Rules_Statement Magazine_state;
        private List<Token> Build_magazine_storage=new List<Token>();
        private List<Token> Strange_names=new List<Token>();
        private Stack<Token> String_Translate_Stack=new Stack<Token>();
        Token Stack_statement=null;

        public bool Rule_check(Token NewElement)
        {
            Token Resulter;
            int counter=0;
            int HInt = 0;
            dynamic Changer;
            Group_of_Tokens NewElGroup = NewElement.Token_Group;
            switch (Magazine_state)
            {
                case Rules_Statement.BeginDigit:  //Состояние при котором строка начинается с Digit 1
                    if (NewElGroup == Group_of_Tokens.Digit)
                    {
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.WhichNextDigit;
                        return true;
                    }
                    else
                        return false;
                case Rules_Statement.WhichNextDigit:  //Состояние проверки элементов после Digit 2
                    if (NewElGroup == Group_of_Tokens.Ariphmetical) //Если арифметический символ
                    {
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.FormAriphmExpression;
                        return true;
                    }
                    else if (NewElGroup == Group_of_Tokens.BooleanOperation) //Если символ логической операции
                    {
                        Changer = (NewElement as Boolean_operation).get_group_of_token();
                        if ((Changer == BooleanSymbol_ID.And) || (Changer == BooleanSymbol_ID.Or)) //Этот символ не && или ||
                            return false;
                        else
                        {
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.FormLogicalExpression;
                            return true;
                        }
                    }
                    else
                        return false;
                case Rules_Statement.FormAriphmExpression: //Состояние формирующее AriphmeticalExpression по возможности 6
                    switch (NewElGroup)
                    {
                        case Group_of_Tokens.Digit: //Если число
                            String_Translate_Stack.Push(NewElement);
                            String_Translate_Stack.Push(new Expression(String_Translate_Stack, Expression_Type.Ariphmetical_expression));
                            Magazine_state = Rules_Statement.AfterArifmExpr;
                            return true;
                        case Group_of_Tokens.Delimeter: //Если разделитель
                            Changer = (NewElement as Delimeter).get_group_of_token();
                            if (Changer == Delimeters_ID.LBracket) //Разделитель открывающаяся скобка
                            {
                                String_Translate_Stack.Push(NewElement);
                                Magazine_state = Rules_Statement.LBracketDigit;
                                return true;
                            }
                            else if (Changer==Delimeters_ID.RBracket)
                            {
                                if (CastBrackets())
                                {
                                    if()
                                    Magazine_state = Rules_Statement.DefaultInStr;
                                    return true;
                                }
                                else
                                    return false;
                            }
                            else if (Changer == Delimeters_ID.Equality) //Разделитель - равенство
                                ;
                                break;
                        case Group_of_Tokens.Function: //Если функция
                            Stack_To_list();
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.FuncCallInStr;
                            return true;
                        case Group_of_Tokens.Name: //Если неизвестное имя
                            Strange_names.Add(NewElement);
                            return false;
                        default:
                            return false;
                    }
                    break;
                case Rules_Statement.DigitInStr:  //Состояние начала Digit внутри уже начавшейся строки 3
                    break;
                case Rules_Statement.FuncCallInStr: //Состояние начала FuncCall внутри уже начавшейся строки 12
                    switch(NewElGroup)
                    {
                        case Group_of_Tokens.Digit:
                            break;
                        case Group_of_Tokens.Function:
                            break;
                        case Group_of_Tokens.Name:
                            break;
                    }
                    break;
                case Rules_Statement.FormLogicalExpression: //Состояние формирующее LogicalExpression по возможности 7
                    break;
                case Rules_Statement.LBracketDigit:
                    switch(NewElGroup)
                    {
                        case Group_of_Tokens.Digit:
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.AfterArifmExpr;
                            break;
                        case Group_of_Tokens.Function:
                            break;
                        case Group_of_Tokens.Name:
                            break;
                    }
                    break;
                case Rules_Statement.DefaultInStr:
                    switch(NewElGroup)
                    {
                        
                    }
                    break;
            }
            return true;
        }

        private bool CastBrackets()
        {
            Token Temp;
            List<Token> Expr_creator = new List<Token>();
            Temp = new Expression(String_Translate_Stack, Expression_Type.Ariphmetical_expression);
            String_Translate_Stack.Pop();
            String_Translate_Stack.Push(Temp);
            if ((String_Translate_Stack.Count == 0)&&(Temp.Data!="("))
                return false;
            else
            {
                String_Translate_Stack.Push(new Expression(Expr_creator, Expression_Type.Ariphmetical_expression));
                return true;
            }
            
        }

        private void Stack_To_list()
        {
            while (String_Translate_Stack.Count>0)
            {
                Build_magazine_storage.Add(String_Translate_Stack.Pop());
            }
            Build_magazine_storage.Add(new Token());
        }

        public int Get_magazine_count()
        {
            return Build_magazine_storage.Count();
        }
        public Builder()
        {
            Magazine_state = Rules_Statement.BeginDigit; //Test Condition
        }
    }

    public enum PreTokenGroup  //Содержит в себе базовые типы предопределенного токена.
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

    public class FirstParser
        /*Первый и главный парсер, преобразует входящий текст в список готовых к интерпретации токенов */
    {
        private List<AnyFunction> FuncStorage = null; //Хранилище функций.
        private List<Variable> VarStorage = null;    //Хранилище глобальных переменных.
        private List<Token> CodeStorage = null;     //Выходной код, по факту явялется выходным списком кода.
        private string Text = null;                //Базовый текст программы
        private int RowCount = 0;                 //Счетчик строк программы
        HashSet<string> Type_definitions = new HashSet<string>() { "null", "void", "int", "float", "double", "point", "char", "string", "picture" }; //Хранит в себе зарезервированные имена типов.
        Dictionary<string, AnyFunction> Function_storage = new Dictionary<string, AnyFunction>();
        HashSet<string> Built_In_Functions = new HashSet<string>() { "testfunc" };  //Хранит в себе имена встроенных функций
        HashSet<string> Construction_reservation = new HashSet<string>() { "if", "while", "for", "function", "procedure", "do", "repeat", "until", "begin", "end" };  //Хранит в себе имена зарезервированных конструкций


        public void First_Parse(List<AnyFunction>InFunc, List<Variable>InVar, string Input_Text)
            /*Метод первого прохода парсера, здесь идет базовый синтаксический анализ и запись определений функций*/
        {
            FuncStorage = InFunc;
            VarStorage = InVar;
            Text = Input_Text;
            PreTokenGroup nowcharID;
            PreTokenGroup PreviousCharID = PreTokenGroup.NaN;
            CodeStorage = new List<Token>();
            //Тестовые
            Builder Temp2 = new Builder();
            //Конец тестовых
            int last_list_count = 0;
            int i = 0;
            char nowchar;
            while (i!=Input_Text.Length)
            {
                nowchar = Input_Text[i];
                nowcharID = getTypeChar(nowchar);
                switch(nowcharID)
                {
                    case PreTokenGroup.Alphabet:
                    case PreTokenGroup.Numeric:
                    case PreTokenGroup.RuAlphabet:
                    case PreTokenGroup.Symbol:
                    case PreTokenGroup.Delimeter:
                        i = While_delegate_function(c => getTypeChar(c) == nowcharID, nowcharID, i , CodeStorage, Input_Text, RowCount,Temp2);
                        break;
                }
                i++;
            }
        }

        private int While_delegate_function(Func<char, bool> Cycle_condition, PreTokenGroup second_cycle_condition, int i_counter, List<Token> Word_list, string input_text, int row_count, Builder BldClass)
        /* Делегирует функцию, для сокращения кода похожих циклов While в коде*/
        {
            int helper_counter = i_counter;
            string Data_former = "";
            
            Token Temp; //Test Condition
            while ((helper_counter != input_text.Length) && (Cycle_condition(input_text[helper_counter])))  //Проходит циклом по тексту и формирует строку, пока входящая функция удовлетворяет второму условию
            {
                Data_former += input_text[helper_counter];
                helper_counter++;
            }
            if (Word_list.Count == 0)
                Temp=(GetToken(Data_former, second_cycle_condition, null, i_counter, helper_counter - 1));
            else
                Temp=(GetToken(Data_former, second_cycle_condition, Word_list.Last(), i_counter, helper_counter - 1));
            if (BldClass.Rule_check(Temp))
                Word_list.Add(Temp);//Сформировав строку, вызывает метод определения и составления токена для слова, после чего добавляет его в хранилище кода программы.
            return helper_counter-1;
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
            /*Метод определяющий группу символов, к которой относится входящий символ*/
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
                switch (c)   //Определяет группу символов, к которой относится входящий символ
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
        private int TestFunc(IEnumerable<Local_Variable> Trulala)
        {
            return 1;
        }

        private Token GetToken(string inStr, PreTokenGroup preID, Token ID_Of_previous, int FValue, int SValue)
            /*Один из основных методов, определяет из входящей строки токен и создает его полную структуру
             inStr - входящая строка из которой формируется токен.
             preID - передает ID символов использованных для составления строки.
             ID_Of_previous - передает предыдущий токен
             FValue - содержит информацию о номере в строке первого символа
             SValue - содержит информацию о номере последнего в строке символа*/
        {
            //Тестовые начало
            HashSet<Local_Variable> Test = new HashSet<Local_Variable>();
            bool space_resulter;
            //Тестовые конец
            Token Resulter;
            if (ID_Of_previous == null)
            {
                space_resulter = true;
            }
            else
                space_resulter = ID_Of_previous.Data == " ";
            switch (preID)
            {
                case PreTokenGroup.Alphabet: //Случай строки составленной из алфавитных символов
                    if (Construction_reservation.Contains(inStr))
                        return (new Token(inStr, space_resulter, Group_of_Tokens.Construction, RowCount, FValue, SValue));
                    else if (Type_definitions.Contains(inStr))
                        return (new Token(inStr, space_resulter, Group_of_Tokens.Type_Definition, RowCount, FValue, SValue));
                    else if (Built_In_Functions.Contains(inStr))
                        return (new Token(inStr, space_resulter, Group_of_Tokens.Function, RowCount, FValue, SValue));
                    else
                        return null;
                case PreTokenGroup.Delimeter: //Случай строки составленной из разделителей
                    Delimeters_ID DelimID;
                    Help_SymbolsID HSymbID;
                    if (Is_delimeter(inStr, out DelimID))
                        return (new Delimeter(inStr, space_resulter, RowCount, FValue, SValue, DelimID));
                        else if (Is_Help_Symbol(inStr, out HSymbID))
                            return (new HelpSymbol(inStr, space_resulter, RowCount, FValue, SValue, HSymbID));
                    else
                        return null;
                case PreTokenGroup.RuAlphabet:  //Случай строки составленной из кириллицы
                    return null;
                case PreTokenGroup.Symbol:  //Случай строки составленной из арифметических или логических символов
                    AriphmeticalSymbol_ID Ariphmetic_ID;
                    BooleanSymbol_ID Boolean_ID;
                    bool Equalty_resulter;
                    if (Is_ariphmetical(inStr, out Ariphmetic_ID, out Equalty_resulter))
                        return (new Ariphmetical(inStr, space_resulter, RowCount, FValue, SValue, Ariphmetic_ID));
                    else if (Is_logical(inStr, out Boolean_ID))
                        return (new Boolean_operation(inStr, space_resulter, RowCount, FValue, SValue, Boolean_ID));
                    return null;
                case PreTokenGroup.Numeric:  //Случай строки составленной из цифр
                    return (new Token(inStr, space_resulter, Group_of_Tokens.Digit, RowCount, FValue, SValue));
                default: return null;
            }
        }


        private bool Reserve_name_check(string Input)  //Вспомогательный метод, проверяет является ли входящая строка зарезервированным именем
        {
            if (Construction_reservation.Contains(Input))
                return true;
            else return false;
        }

        private bool Is_delimeter(string Input, out Delimeters_ID Delim_ID)  //Вспомогательный метод, проверяет является ли входящая строка разделителем
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

        private bool Is_Help_Symbol(string Input, out Help_SymbolsID Help_Symbol_ID)  //Вспомогательный метод, проверяет является ли входящая строка вспомогательным символом.
            /*Вспомогательный метод, определяет */
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

        private bool Is_ariphmetical(string Input, out AriphmeticalSymbol_ID Ariphm_ID, out bool WasEquality)  //Вспомогательный метод, проверяет является ли входящая строка арифметическим символом
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

        private bool Is_logical(string Input, out BooleanSymbol_ID Bool_ID)  //Вспомогательный метод, проверяет является ли входящая строка логическим символом.
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
