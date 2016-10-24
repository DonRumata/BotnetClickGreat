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
        Cast = 10,
        FuncCallBegin =11,
        FuncCallInStr=12,
        EndOfString=13,
        Equality=14,
        FuncNameFounded=15,
        ArgsOnCall=16,
        WhichNextDigitArgs=17,
        ArgsAfterAriphmetical=18,
        AfterBoolOperation=19,
        IfConstructionFounded=20,
        CastBooleanExpression=21,
        TypeDefinitionBegin=22,
        MustBeNameVarDefinition=23,
        AfterVarDefinition=24,
        AfterVarDefEquality=25,     
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
        int Comma_counter = 0;
        Token Stack_statement=null;

        public bool Rule_check(Token NewElement)
        {
            Token Resulter;
            int BracketCounter=0;
            int HInt = 0;
            int BoperationsInProgress = 0;
            dynamic Changer;
            Group_of_Tokens NewElGroup = NewElement.Token_Group;
            switch (Magazine_state)
            {

                case Rules_Statement.IfConstructionFounded:
                    break;

                case Rules_Statement.TypeDefinitionBegin:
                    String_Translate_Stack.Push(NewElement);
                    Magazine_state = Rules_Statement.MustBeNameVarDefinition;
                    return true;

                case Rules_Statement.MustBeNameVarDefinition:
                    if (NewElGroup == Group_of_Tokens.Name)
                    {
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.AfterVarDefinition;
                        return true;
                    }
                    else
                        return false;

                case Rules_Statement.AfterVarDefinition:
                    if (NewElGroup==Group_of_Tokens.BooleanOperation)
                    {
                        if ((NewElement as Boolean_operation).get_group_of_token() == BooleanSymbol_ID.Equality)
                        {
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.AfterVarDefEquality;
                        }
                        else
                        {
                            String_Translate_Stack.Push(new Variable(String_Translate_Stack));
                        }
                    }
                    break;

                case Rules_Statement.FuncCallBegin:
                    if (NewElGroup == Group_of_Tokens.Function)
                    {
                        String_Translate_Stack.Push(NewElement);
                        Build_magazine_storage.Add(NewElement);
                        Magazine_state = Rules_Statement.FuncNameFounded;
                        return true;
                    }
                    else return false;


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
                            BoperationsInProgress++;
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.AfterBoolOperation;
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
                            String_Translate_Stack.Push(new Expression(String_Translate_Stack, Expression_Type.Ariphmetical_expression,Inst=>((Inst.Count>0)&&(Inst.Peek().Token_Group!=Group_of_Tokens.Delimeter)) && (Inst.Peek().Token_Group != Group_of_Tokens.BooleanOperation)));
                            Magazine_state = Rules_Statement.AfterArifmExpr;
                            return true;
                        case Group_of_Tokens.Delimeter: //Если разделитель
                            Changer = (NewElement as Delimeter).get_group_of_token();
                            if (Changer == Delimeters_ID.LBracket) //Разделитель открывающаяся скобка
                            {
                                String_Translate_Stack.Push(NewElement);
                                BracketCounter++;
                                Magazine_state = Rules_Statement.LBracketDigit;
                                return true;
                            }
                            else if (Changer==Delimeters_ID.RBracket)
                            {
                                if (CastBrackets())
                                { 
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
                case Rules_Statement.AfterBoolOperation: //Состояние анализа после boolean операции, для перехода на линейку создания удобочитаемого логического выражения 19
                    
                    switch (NewElGroup)
                    {
                        case Group_of_Tokens.Digit:
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.WhichNextDigit;
                            return true;
                        case Group_of_Tokens.Delimeter:
                            if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.LBracket)
                            {
                                String_Translate_Stack.Push(NewElement);
                                Magazine_state = Rules_Statement.LBracketDigit;
                                return true;
                            }
                            else return false;
                        case Group_of_Tokens.Function:
                            break;
                        case Group_of_Tokens.Name:
                            break;
                    }
                    break;

                case Rules_Statement.CastBooleanExpression:

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
                
                case Rules_Statement.LBracketDigit:
                    switch(NewElGroup)
                    {
                        case Group_of_Tokens.Digit:
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.WhichNextDigit;
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
                case Rules_Statement.AfterArifmExpr:
                    if (NewElGroup == Group_of_Tokens.Ariphmetical)
                        goto case Rules_Statement.WhichNextDigit;
                    else if (NewElGroup == Group_of_Tokens.BooleanOperation)
                        if (BoperationsInProgress > 0)
                            goto case Rules_Statement.CastBooleanExpression;
                        else
                        {
                            goto case Rules_Statement.WhichNextDigit;
                        }
                    else if (NewElGroup == Group_of_Tokens.Delimeter)
                    {
                        if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.Semicolon)
                        {
                            Magazine_state = Rules_Statement.EndOfString;
                            return true;
                        }
                        else if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.Equality)
                            goto case Rules_Statement.Equality;
                        else if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.RBracket)
                        {
                            CastBrackets();
                            Magazine_state = Rules_Statement.AfterArifmExpr;
                        }
                    }
                    
                    break;
                case Rules_Statement.Default:
                    switch (NewElGroup)
                    {
                        case Group_of_Tokens.Digit:
                            break;
                        case Group_of_Tokens.Function:
                            break;
                        case Group_of_Tokens.Construction:
                            break;
                    }
                    break;
                case Rules_Statement.EndOfString:
                    Build_magazine_storage.Clear();
                    if ((BracketCounter == 0)&&(String_Translate_Stack.Count==1))
                    {
                        String_Translate_Stack.Clear();
                        BracketCounter = 0;
                        Magazine_state = Rules_Statement.Default;
                        return true;
                    }
                    else
                        return false;
                case Rules_Statement.Equality:
                    break;
                case Rules_Statement.FuncNameFounded: //Состояние при котором строка начинается с вызова имени функции.
                    if (NewElGroup == Group_of_Tokens.Delimeter)
                        if (NewElement.get_group_of_token() == Delimeters_ID.LBracket)
                        {
                            Comma_counter = 0;
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.ArgsOnCall;
                            return true;
                        }                    
                        else
                            return false;
                    else return false;

                case Rules_Statement.ArgsOnCall:
                    switch (NewElGroup)
                    {
                        case Group_of_Tokens.Digit:
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.WhichNextDigitArgs;
                            return true;
                        case Group_of_Tokens.Function:
                            String_Translate_Stack.Push(NewElement);
                            Build_magazine_storage.Add(NewElement);
                            Comma_counter = 0;
                            Magazine_state = Rules_Statement.FuncNameFounded;
                            break;
                        case Group_of_Tokens.Delimeter:
                            break;
                        case Group_of_Tokens.Name:
                            break;
                    }
                    break;
                case Rules_Statement.WhichNextDigitArgs:
                    if (NewElGroup == Group_of_Tokens.Delimeter)
                        if (NewElement.get_group_of_token() == Delimeters_ID.RBracket)
                        {
                            Comma_counter++;
                            if (Comma_counter > (Build_magazine_storage.Last() as AnyFunction).Args.Count)
                                return false;
                            else
                                (Build_magazine_storage.Last() as AnyFunction).Args[Comma_counter - 1].RPNValue = String_Translate_Stack.Pop();
                            CastArgsBrackets(Expression_Type.Ariphmetical_expression);
                            return true;
                        }
                        else;
                    else if (NewElGroup==Group_of_Tokens.Help_symbol)
                        if (NewElement.get_group_of_token()==Help_SymbolsID.Comma)
                        {
                            Comma_counter++;
                            if (Comma_counter > (Build_magazine_storage.Last() as AnyFunction).Args.Count)
                                return false;
                            else
                            {
                                (Build_magazine_storage.Last() as AnyFunction).Args[Comma_counter - 1].RPNValue = String_Translate_Stack.Pop();
                                Magazine_state = Rules_Statement.ArgsOnCall;
                            }
                            return true;
                        }
                        else
                        ;
                    else if (NewElGroup==Group_of_Tokens.Ariphmetical)
                    {
                        String_Translate_Stack.Push(NewElement);
                        
                        Magazine_state = Rules_Statement.ArgsAfterAriphmetical;
                        return true;
                    }
                    break;
                case Rules_Statement.ArgsAfterAriphmetical:
                    if (NewElGroup==Group_of_Tokens.Digit)
                    {
                        String_Translate_Stack.Push(NewElement);
                        String_Translate_Stack.Push(new Expression(String_Translate_Stack, Expression_Type.Ariphmetical_expression, Inst => (Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter) && (Inst.Peek().Token_Group != Group_of_Tokens.BooleanOperation)));
                        Magazine_state = Rules_Statement.WhichNextDigitArgs;
                        return true;
                    }
                    else if (NewElGroup==Group_of_Tokens.Function)
                    {
                        String_Translate_Stack.Push(NewElement);
                        Build_magazine_storage.Add(NewElement);
                        Magazine_state = Rules_Statement.FuncNameFounded;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
            return true;
        }

        private int ReCountPreviousComm()
        {
            int Resulter = 0;
            if (Build_magazine_storage.Count > 0)
            {
                while ((Build_magazine_storage.Last() as AnyFunction).Args[Resulter].RPNValue != null)
                {
                    Resulter++;
                }
            }
            return Resulter;
        }

        private void CastArgsBrackets(Expression_Type Type_OF_expr)
        {
            Token Temp;
            dynamic TypeConnection = Build_magazine_storage.Last();
            String_Translate_Stack.Pop();
            Temp =String_Translate_Stack.Pop();
            if (String_Translate_Stack.Any())
                Temp = String_Translate_Stack.Peek();
            else
            {
                Comma_counter = 0;
                String_Translate_Stack.Push(Temp);
                Magazine_state = Rules_Statement.EndOfString;
            }
            switch (Temp.Token_Group)
            {
                case Group_of_Tokens.Ariphmetical:
                    if (TypeConnection.GetDelegateMethodType() == 1)
                    {
                        String_Translate_Stack.Push(Build_magazine_storage.Last());
                        String_Translate_Stack.Push(new Expression(String_Translate_Stack, Expression_Type.Ariphmetical_expression, Inst => (Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter) && (Inst.Peek().Token_Group != Group_of_Tokens.BooleanOperation)));
                        Comma_counter -= TypeConnection.Args.Count;
                        Magazine_state = Rules_Statement.WhichNextDigitArgs;
                    }

                    break;
                case Group_of_Tokens.Delimeter:
                    if (TypeConnection.GetDelegateMethodType()==1)
                    {
                        String_Translate_Stack.Push(TypeConnection);
                        Magazine_state=Rules_Statement.WhichNextDigitArgs;
                    }
                    break;
                case Group_of_Tokens.Help_symbol:
                    if (Temp.get_group_of_token()==Help_SymbolsID.Comma)
                    {
                        if(Build_magazine_storage.Count>1)
                        {
                            String_Translate_Stack.Pop();
                            if (Build_magazine_storage[Build_magazine_storage.Count-2]==String_Translate_Stack.Peek())
                            {

                            }
                        }
                    }
                    break;
            }
            Build_magazine_storage.RemoveAt(Build_magazine_storage.Count - 1);
            Comma_counter = ReCountPreviousComm();
        }

        private bool CheckPriority(Token NewEl)
        {
            if (String_Translate_Stack.Count == 0)
            {
                return true;
            }
            else if (NewEl.get_group_of_token() <= String_Translate_Stack.Peek().get_group_of_token())
            {
                return false;
            }
            else
                return true;
                
        }

        private int get_priority(Token NewE)
        {
            return 1;
        }

        private bool CastBrackets()
            /*Выполняет финальные/семифинальные свертки
             Запускается исключительно для сверток со скобками*/
        {
            Token Temp;
            Stack<Token> Expr_creator = new Stack<Token>();//Вспомогательный стек, позволяет получить диапазон свертки в пределах которого необходимо работать.
            Temp = String_Translate_Stack.Pop();
            while(Temp.Data!="(")  //Заполняет необходимый диапазон внутри скобок
            {
                Expr_creator.Push(Temp);
                Temp = String_Translate_Stack.Pop();
            }
            Temp = String_Translate_Stack.Peek();
            //Дальше выполняется создание необходимого выражения в зависимости от символа.
            if (Temp.Token_Group == Group_of_Tokens.BooleanOperation)
            {
                //Expr_creator.Push(String_Translate_Stack.Pop());
                //Expr_creator.Push(String_Translate_Stack.Pop());
                //String_Translate_Stack.Push(new Expression(Expr_creator, Expression_Type.Logical_expression, Inst => (Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter)));
                String_Translate_Stack.Push(Expr_creator.Pop());
                return true;
            }
            else if (Temp.Token_Group == Group_of_Tokens.Ariphmetical)
            {
                Expr_creator.Push(String_Translate_Stack.Pop());
                Expr_creator.Push(String_Translate_Stack.Pop());
                String_Translate_Stack.Push(new Expression(Expr_creator, Expression_Type.Ariphmetical_expression, Inst => (Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter) && (Inst.Peek().Token_Group != Group_of_Tokens.BooleanOperation)));
                return true;
            }
            else
            {
                String_Translate_Stack.Push(new Expression(Expr_creator, Expression_Type.Err, Inst=>(Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter)));
                return false;
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
        EndOfString = 7,
        HSymbols=8,
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
        Dictionary<string, AnyFunction> Function_storage = new Dictionary<string, AnyFunction>();//Хранит в себе имена функций
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
            List<Variable> Temps = new List<Variable>(1);
            List<Variable> Temps2 = new List<Variable>(2);
            Temps2.Add(new Variable());
            Temps2.Add(new Variable());
            Temps.Add(new Variable());
            Function_storage.Add("testfunc", new Built_InFunction<int>(TestFunc,Temps));
            Function_storage.Add("testfuncs", new Built_InFunction<Token>(TestFunc2, Temps2));
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
                        i = While_delegate_function(c => getTypeChar(c) == nowcharID, nowcharID, i , CodeStorage, Input_Text, RowCount,Temp2);
                        break;
                    case PreTokenGroup.EndOfString:
                        i++;
                        CodeStorage.Add(new Delimeter(nowchar.ToString(), false, RowCount, i - 1, i, Delimeters_ID.New_string));
                        if (!Temp2.Rule_check(CodeStorage.Last()))
                            ;
                        else
                            ;
                        RowCount++;
                        break;
                    case PreTokenGroup.HSymbols:
                    case PreTokenGroup.Delimeter:
                        Token Temp;
                        if (Temp2.Rule_check(Temp = GetToken(nowchar.ToString(), nowcharID, CodeStorage.Last(), i, i)))
                            CodeStorage.Add(Temp);
                        else
                            ;
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
                    case '\"':
                    case '\'':
                    case '(':
                    case ')':
                    case '[':
                    case ']':
                    case '{':
                    case '}':
                        return PreTokenGroup.Delimeter;
                    
                    case ' ':
                        return PreTokenGroup.Space;
                    case '\n':
                    case '\r':
                        return PreTokenGroup.EndOfString;
                    case ',':
                    case '.':
                        return PreTokenGroup.HSymbols;
                    default: return PreTokenGroup.Default;
                }
            }
        }
        private int TestFunc(IEnumerable<Variable> Trulala)
        {
            return 1;
        }

        private Token TestFunc2(IEnumerable<Variable> Trulala)
        {
            return new Token();
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
            HashSet<Variable> Test = new HashSet<Variable>();
            bool space_resulter;
            dynamic IDer = null;
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
                    else if (Function_storage.ContainsKey(inStr))
                    {
                        dynamic Tester = Function_storage[inStr];
                        Tester.ReCreateToken(inStr, space_resulter, Group_of_Tokens.Function, RowCount, FValue, SValue);  // ПОТЕНЦИАЛЬНОЕ БАГОДЕРЬМО С УКАЗАТЕЛЯМИ, ПЕРВИЧНЫЙ FUNCTION STORAGE НЕ ДОЛЖЕН МЕНЯТЬСЯ.
                        return (Tester);
                    }
                    else
                        return new Token(inStr, space_resulter, Group_of_Tokens.Name, RowCount, FValue, SValue);
                case PreTokenGroup.HSymbols: //Cлучай строки составленной из вспомогательных символов
                    if (Is_Help_Symbol(inStr, out IDer))
                        return (new HelpSymbol(inStr, space_resulter, RowCount, FValue, SValue, IDer));
                    else return null;
                case PreTokenGroup.Delimeter: //Случай строки составленной из разделителей
                    if (Is_delimeter(inStr, out IDer))
                        return (new Delimeter(inStr, space_resulter, RowCount, FValue, SValue, IDer));
                        else if (Is_Help_Symbol(inStr, out IDer))
                            return (new HelpSymbol(inStr, space_resulter, RowCount, FValue, SValue, IDer));
                    else
                        return null;
                case PreTokenGroup.RuAlphabet:  //Случай строки составленной из кириллицы
                    return null;
                case PreTokenGroup.Symbol:  //Случай строки составленной из арифметических или логических символов
                    bool Equalty_resulter;
                    if (Is_ariphmetical(inStr, out IDer, out Equalty_resulter))
                        return (new Ariphmetical(inStr, space_resulter, RowCount, FValue, SValue, IDer));
                    else if (Is_logical(inStr, out IDer))
                        return (new Boolean_operation(inStr, space_resulter, RowCount, FValue, SValue, IDer));
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

        private bool Is_delimeter(string Input, out dynamic Delim_ID)  //Вспомогательный метод, проверяет является ли входящая строка разделителем
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

        private bool Is_Help_Symbol(string Input, out dynamic Help_Symbol_ID)  //Вспомогательный метод, проверяет является ли входящая строка вспомогательным символом.
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

        private bool Is_ariphmetical(string Input, out dynamic Ariphm_ID, out bool WasEquality)  //Вспомогательный метод, проверяет является ли входящая строка арифметическим символом
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

        private bool Is_logical(string Input, out dynamic Bool_ID)  //Вспомогательный метод, проверяет является ли входящая строка логическим символом.
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
