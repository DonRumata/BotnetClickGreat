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
        FuncDefinitionReturnTypeDefinition=3,
        DigitLogicalNext=4,
        DigitAriphmeticalNext=5,
        FormAriphmExpression=6,
        FormLogicalExpression=7,
        AfterArifmExpr=8,
        LBracketDigit = 9,
        Cast = 10,
        FuncCallBegin =11,
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
        FuncDefinitionBegin=26,
        FuncDefinitionFuncName=27,
        LBracketFuncDefinition=28,
        FuncDefinitionArgNameDefinition=29,
        FuncDefinitionEqualityArgsDef=30,
        FuncDefinitionArgTypeDefinition=31,
        BodyFuncStarting=32,
        LBracketAfterIFConstruction=33,
        DefaultInIFConstruction=34,
        LBracketDefaultInStr=35,
        VarCallFounded=36,
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
        private Stack<Token> VariableCallStack = new Stack<Token>();
        private List<Token> TranslateCode = new List<Token>();
        int Comma_counter = 0;
        private int BracketCounter = 0;
        private int VarAssignmentInProgress = 0;
        Token Stack_statement=null;
        private Variable VarCallInProgress = null;
        private int BoperationsInProgress = 0;
        private bool VarDefinitionInProgress = false;
        private User_Function FunctionBodyInProgress = null;
        private bool IFConstructionInProgress = false;

        private Variable AddMethodToQueue_OfVariable(string VarName, bool WhichOneMethod, Token AnyExpression)
        {
            if (FunctionBodyInProgress!=null)
            {
                FunctionBodyInProgress.
            }
        }

        public Token CheckArgLocalName(string CheckName)
        {
            if ((FunctionBodyInProgress!=null))
            {
                return FunctionBodyInProgress.IsArgsContains(CheckName);
            }
            else
                return null;
        }


        public bool Rule_check(Token NewElement)
        {
            Token Resulter;
            int HInt = 0;
            dynamic Changer;
            Group_of_Tokens NewElGroup = NewElement.Token_Group;
            switch (Magazine_state)
            {

                case Rules_Statement.IfConstructionFounded:
                    String_Translate_Stack.Push(NewElement);
                    Magazine_state = Rules_Statement.LBracketAfterIFConstruction;
                    return true;

                case Rules_Statement.LBracketAfterIFConstruction:
                    if (NewElGroup == Group_of_Tokens.Delimeter)
                        if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.LBracket)
                        {
                            BracketCounter++;
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.DefaultInIFConstruction;
                            return true;
                        }
                        else return false;
                    else return false;

                case Rules_Statement.VarCallFounded:
                    if (NewElGroup==Group_of_Tokens.Assignment)
                    {
                        VariableCallStack.Push(NewElement);
                        VarAssignmentInProgress++;
                        VarCallInProgress = NewElement as Variable;
                        Magazine_state = Rules_Statement.DefaultInStr;
                    }
                    else
                    {
                        String_Translate_Stack.Push(new Token(NewElement, Group_of_Tokens.VariableMethodCall));

                    }
                    break;


                case Rules_Statement.DefaultInIFConstruction:
                    switch(NewElGroup)
                    {
                        case Group_of_Tokens.Digit:
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.WhichNextDigit;
                            break;
                        case Group_of_Tokens.Function:
                            String_Translate_Stack.Push(NewElement);
                            Build_magazine_storage.Add(NewElement);
                            Magazine_state = Rules_Statement.FuncNameFounded;
                            break;
                        case Group_of_Tokens.Variable:
                            String_Translate_Stack.Push(NewElement);
                            return true;
                        case Group_of_Tokens.Name:

                            break;
                        case Group_of_Tokens.Delimeter:
                            if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.LBracket)
                            {
                                String_Translate_Stack.Push(NewElement);
                                BracketCounter++;
                                Magazine_state = Rules_Statement.LBracketDigit;
                                return true;
                            }
                            else return false;
                        default: return false;
                    }

                    break;

                case Rules_Statement.FuncDefinitionBegin:  //Положение в котором строка начинается со слова Function
                    String_Translate_Stack.Push(NewElement);
                    Magazine_state = Rules_Statement.FuncDefinitionReturnTypeDefinition;
                    return true;

                case Rules_Statement.FuncDefinitionReturnTypeDefinition:
                    if (NewElGroup == Group_of_Tokens.Type_Definition)
                    {
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.FuncDefinitionFuncName;
                        return true;
                    }
                    else return false;

                case Rules_Statement.FuncDefinitionFuncName: //Положение в котором ожидается имя функции
                    if (NewElGroup == Group_of_Tokens.Name)
                    {
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.LBracketFuncDefinition;
                        return true;
                    }
                    else return false;

                case Rules_Statement.LBracketFuncDefinition:  //Положение, ожидающее начала ввода аргументов функции.
                    if (NewElGroup == Group_of_Tokens.Delimeter)
                        if ((NewElement as Delimeter).get_group_of_token() == Delimeters_ID.LBracket)
                        {
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.FuncDefinitionArgTypeDefinition;
                            return true;
                        }
                        else return false;
                    else return false;

                case Rules_Statement.FuncDefinitionArgTypeDefinition:  //Положение записывающее аргументы функции и при их конце выполняющее свертку в функцию(без тела)
                    if (NewElGroup == Group_of_Tokens.Type_Definition)
                    {
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.FuncDefinitionArgNameDefinition;
                        return true;
                    }
                    else  if (NewElGroup==Group_of_Tokens.Delimeter)
                    {
                        if ((NewElement as Delimeter).get_group_of_token() == Delimeters_ID.RBracket)
                        {
                            String_Translate_Stack.Push(new User_Function(String_Translate_Stack));
                            Magazine_state = Rules_Statement.BodyFuncStarting;
                            return true;
                        }
                        else
                            return false;
                    }
                    else return false;

                case Rules_Statement.BodyFuncStarting:  //Положение в котором начинается трансляция тела функции, дальнейший код в фигурных скобках будет записываться в тело ранее объявленной функции
                    if (NewElGroup == Group_of_Tokens.Delimeter)
                        if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.LBrace)
                        {
                            Magazine_state = Rules_Statement.DefaultInStr;
                            FunctionBodyInProgress = String_Translate_Stack.Peek() as User_Function;
                            return true;
                        }
                        else return false;
                    else if (NewElGroup == Group_of_Tokens.EndOfString)
                        if (NewElement.Data==";")
                            return false;
                        else
                            return true;
                    else
                        return false;

                case Rules_Statement.FuncDefinitionArgNameDefinition:  //Положение объявления имени аргумента при объявлении функции.
                    if (NewElGroup == Group_of_Tokens.Name)
                    {
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.FuncDefinitionEqualityArgsDef;
                        return true;
                    }
                    else return false;

                case Rules_Statement.FuncDefinitionEqualityArgsDef:  //Положение для перехода на начало задание default значения аргумента, или записи аргумента без default значения.
                    if (NewElGroup==Group_of_Tokens.Assignment)
                    {

                    }
                    else if (NewElGroup==Group_of_Tokens.Help_symbol)
                    {
                        if ((NewElement as HelpSymbol).get_group_of_token()==Help_SymbolsID.Comma)
                        {
                            String_Translate_Stack.Push(new Variable(String_Translate_Stack));
                            Magazine_state = Rules_Statement.FuncDefinitionArgTypeDefinition;
                        }
                    }
                    else if (NewElGroup==Group_of_Tokens.Delimeter)
                    {
                        if ((NewElement as Delimeter).DelimeterID==Delimeters_ID.RBracket)
                        {
                            String_Translate_Stack.Push(new Variable(String_Translate_Stack));
                            String_Translate_Stack.Push(new User_Function(String_Translate_Stack));
                            Magazine_state = Rules_Statement.BodyFuncStarting;
                        }
                    }
                    break;

                case Rules_Statement.TypeDefinitionBegin:  //Положение в котором строка начинается с имени типа(Type Definition).
                    String_Translate_Stack.Push(NewElement);
                    Magazine_state = Rules_Statement.MustBeNameVarDefinition;
                    return true;

                case Rules_Statement.MustBeNameVarDefinition:  //Положение в котором должно быть объявлено имя переменной
                    if (NewElGroup == Group_of_Tokens.Name)
                    {
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.AfterVarDefinition;
                        return true;
                    }
                    else
                        return false;

                case Rules_Statement.AfterVarDefinition: //Положение после объявления имени переменной.
                    if (NewElGroup == Group_of_Tokens.Assignment) //Если дальше идет присваивание, то он сохраняет строку для дальнейшей свертки.
                    {
                        VarDefinitionInProgress = true;
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.AfterVarDefEquality;
                        return true;
                    }
                    else if (NewElGroup == Group_of_Tokens.BooleanOperation)
                        if ((NewElement as Boolean_operation).BooleanID == BooleanSymbol_ID.Equality)
                        {
                            String_Translate_Stack.Push(new Variable(String_Translate_Stack));
                            VarDefinitionInProgress = true;
                            String_Translate_Stack.Push(NewElement);
                            return true;
                        }
                        else return false;
                    else if (NewElGroup == Group_of_Tokens.Ariphmetical)
                    {
                        return true;
                    }
                    else return false;

                case Rules_Statement.AfterVarDefEquality:
                    if (NewElGroup == Group_of_Tokens.Digit)
                        goto case Rules_Statement.BeginDigit;
                    else if (NewElGroup == Group_of_Tokens.Function)
                        goto case Rules_Statement.FuncCallBegin;
                    else if (NewElGroup == Group_of_Tokens.Delimeter)
                    {
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.LBracketDigit;
                        return true;
                    }
                    else if (NewElGroup == Group_of_Tokens.Variable)
                    {
                        String_Translate_Stack.Push(NewElement);
                        String_Translate_Stack.Push(new Variable(String_Translate_Stack));
                        Magazine_state = Rules_Statement.EndOfString;
                        return true;
                    }
                    else if (NewElGroup == Group_of_Tokens.Name)
                    {
                        break;
                    }
                    else
                        return false;

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
                    else if (NewElGroup==Group_of_Tokens.EndOfString)
                    {
                        if (VarDefinitionInProgress)
                        {
                            /*VarDefinitionInProgress = false;
                            String_Translate_Stack.Push(new Variable(String_Translate_Stack));
                            Magazine_state = Rules_Statement.DefaultInStr;*/
                            goto case Rules_Statement.EndOfString;
                        }
                        else return false;
                    }
                    else
                        return false;
                case Rules_Statement.FormAriphmExpression: //Состояние формирующее AriphmeticalExpression по возможности 6
                    switch (NewElGroup)
                    {
                        case Group_of_Tokens.Digit: //Если число
                            String_Translate_Stack.Push(NewElement);
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
                                if (CastBrackets(false))
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
                            String_Translate_Stack.Push(NewElement);
                            Build_magazine_storage.Add(NewElement);
                            Magazine_state = Rules_Statement.FuncNameFounded;
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
                        case Group_of_Tokens.Help_symbol:
                            break;
                        case Group_of_Tokens.Digit: //Если число
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.WhichNextDigit;
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
                            else if (Changer == Delimeters_ID.RBracket)
                            {
                                if (CastBrackets(false))
                                {
                                    Magazine_state = Rules_Statement.DefaultInStr;
                                    return true;
                                }
                                else
                                    return false;
                            }
                            else return false;
                        case Group_of_Tokens.Function: //Если функция
                            String_Translate_Stack.Push(NewElement);
                            Build_magazine_storage.Add(NewElement);
                            Magazine_state = Rules_Statement.FuncNameFounded;
                            return true;
                        case Group_of_Tokens.Name: //Если неизвестное имя
                            Strange_names.Add(NewElement);
                            return false;
                        default:
                            return false;
                    }
                    break;

                case Rules_Statement.CastBooleanExpression:
                    //break;
                
                case Rules_Statement.LBracketDigit:
                    switch(NewElGroup)
                    {
                        case Group_of_Tokens.Digit:
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.WhichNextDigit;
                            return true;
                        case Group_of_Tokens.Function:
                            String_Translate_Stack.Push(NewElement);
                            Build_magazine_storage.Add(NewElement);
                            Magazine_state = Rules_Statement.FuncNameFounded;
                            return true;
                        case Group_of_Tokens.Name:
                            break;
                    }
                    break;

                case Rules_Statement.LBracketDefaultInStr:
                    break;

                case Rules_Statement.DefaultInStr:
                    dynamic InStrChanger = NewElement;
                    switch(NewElGroup)
                    {
                        case Group_of_Tokens.Digit:
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.WhichNextDigit;
                            return true;

                        case Group_of_Tokens.Function:
                            String_Translate_Stack.Push(NewElement);
                            Build_magazine_storage.Add(NewElement);
                            Magazine_state = Rules_Statement.FuncNameFounded;
                            return true;

                        case Group_of_Tokens.EndOfString:
                            return true;

                        case Group_of_Tokens.Variable:
                            String_Translate_Stack.Push(NewElement);
                            break;

                        case Group_of_Tokens.Name:

                            break;
                        case Group_of_Tokens.Delimeter:
                            if (InStrChanger.DelimeterID==Delimeters_ID.LBracket)
                            {
                                String_Translate_Stack.Push(NewElement);
                                BracketCounter++;
                                Magazine_state = Rules_Statement.LBracketDefaultInStr;
                            }
                            else if (InStrChanger.DelimeterID==Delimeters_ID.RBrace)
                            {

                            }
                            break;
                        case Group_of_Tokens.Help_symbol:
                            if (InStrChanger.SymbolID == Help_SymbolsID.Quotes)
                            {

                            }
                            else if (InStrChanger.SymbolID == Help_SymbolsID.Apostrophe)
                            {

                            }
                            else return false;
                            break;
                        case Group_of_Tokens.Construction:
                            switch(NewElement.GetID_of_Construction())
                            {
                                case Constructions_ID.Function:
                                    return false;
                                case Constructions_ID.For:
                                    return true;
                                case Constructions_ID.If:
                                    String_Translate_Stack.Push(NewElement);
                                    Magazine_state = Rules_Statement.LBracketAfterIFConstruction;
                                    return true;
                                case Constructions_ID.Repeat:
                                    return true;
                                case Constructions_ID.While:
                                    return true;
                                default: return false;
                            }
                            break;
                        case Group_of_Tokens.Type_Definition:
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.MustBeNameVarDefinition;
                            return true;
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
                        if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.Equality)
                            goto case Rules_Statement.Equality;
                        else if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.RBracket)
                        {
                            CastBrackets(false);
                            Magazine_state = Rules_Statement.AfterArifmExpr;
                        }
                    }
                    else if (NewElGroup==Group_of_Tokens.EndOfString)
                    {
                        String_Translate_Stack.Push(new Expression(String_Translate_Stack, Expression_Type.Ariphmetical_expression, Inst => ((Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter)) && (Inst.Peek().Token_Group != Group_of_Tokens.BooleanOperation) && (Inst.Peek().Token_Group != Group_of_Tokens.Assignment)));
                        goto case Rules_Statement.EndOfString;
                    }
                    
                    break;
                case Rules_Statement.Default:
                    if (NewElGroup == Group_of_Tokens.Digit)
                        goto case Rules_Statement.BeginDigit;
                    else if (NewElGroup == Group_of_Tokens.Function)
                        goto case Rules_Statement.FuncCallBegin;
                    else if (NewElGroup == Group_of_Tokens.Construction)
                    {
                        if (NewElement.GetID_of_Construction() == Constructions_ID.Function)
                            goto case Rules_Statement.FuncDefinitionBegin;
                        else if (NewElement.GetID_of_Construction() == Constructions_ID.If)
                            goto case Rules_Statement.IfConstructionFounded;
                        else if (NewElement.GetID_of_Construction() == Constructions_ID.While)
                            ;
                        else return false;
                    }
                    else if (NewElGroup == Group_of_Tokens.Type_Definition)
                        goto case Rules_Statement.TypeDefinitionBegin;
                    else if (NewElGroup == Group_of_Tokens.Variable)
                        ;
                    else if (NewElGroup == Group_of_Tokens.Name)
                        ;
                    else return false;
                    break;

                case Rules_Statement.EndOfString:
                    dynamic Temp;
                    if (VarDefinitionInProgress)
                    {
                        if (BoperationsInProgress == 1)
                        {
                            BoperationsInProgress--;
                            String_Translate_Stack.Push(new Expression(String_Translate_Stack, Expression_Type.Logical_expression, Inst => ((Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter)) && (Inst.Peek().Token_Group != Group_of_Tokens.Assignment)));
                        }
                        String_Translate_Stack.Push(new Variable(String_Translate_Stack));
                        VarDefinitionInProgress = false;
                    }
                    if (FunctionBodyInProgress!=null)
                    {
                        if (BracketCounter == 0)
                        {
                            Temp = String_Translate_Stack.Pop();
                            if (Temp.Token_Group==Group_of_Tokens.Variable)
                            {
                                if (!FunctionBodyInProgress.AddLocalArgument(Temp))
                                {
                                    return false;
                                }
                            }
                            FunctionBodyInProgress.AddNewFunctionBodyString(Temp);
                            TranslateCode.Add(String_Translate_Stack.Peek());
                            Build_magazine_storage.Clear();
                            Magazine_state = Rules_Statement.DefaultInStr;
                            return true;
                        }
                        else return false;
                    }
                    else if ((BracketCounter == 0)&&(String_Translate_Stack.Count==1))
                    {
                        TranslateCode.Add(String_Translate_Stack.Pop());
                        String_Translate_Stack.Clear();
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
                    else if(NewElGroup==Group_of_Tokens.EndOfString)
                    {
                        goto case Rules_Statement.EndOfString;
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

        private void CastFunctionDefinition()
        {
            while (String_Translate_Stack.Peek().Token_Group==Group_of_Tokens.Variable)
            {
                
            } 
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

        private void CastIFBrackets()
        {
            Token Temp;
            Stack<Token> Expr_creator = new Stack<Token>();
            Temp = String_Translate_Stack.Pop();
            while (Temp.Data!="(")
            {
                Expr_creator.Push(Temp);
                Temp = String_Translate_Stack.Pop();
            }
            String_Translate_Stack.Push(new Expression(Expr_creator, Expression_Type.Ariphmetical_expression, Inst => (Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter) && (Inst.Peek().Token_Group != Group_of_Tokens.BooleanOperation) && (Inst.Peek().Token_Group != Group_of_Tokens.Assignment)));

        }

        private bool CastBrackets(bool LikeBool)
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
            if (LikeBool)
            {
                return true;
            }
            else
            {
                String_Translate_Stack.Push(new Expression(Expr_creator, Expression_Type.Ariphmetical_expression, Inst => (Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter) && (Inst.Peek().Token_Group != Group_of_Tokens.BooleanOperation)&&(Inst.Peek().Token_Group!=Group_of_Tokens.Assignment)));
                BracketCounter--;
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
            Magazine_state = Rules_Statement.Default; //Test Condition
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
        Equality=9,
    }

    public class FirstParser
        /*Первый и главный парсер, преобразует входящий текст в список готовых к интерпретации токенов */
    {
        private List<AnyFunction> FuncStorage = null; //Хранилище функций.
        private List<Variable> VarStorage = null;    //Хранилище глобальных переменных.
        private List<Token> CodeStorage = null;     //Выходной код, по факту явялется выходным списком кода.
        private string Text = null;                //Базовый текст программы
        private int RowCount = 0;                 //Счетчик строк программы
        HashSet<string> Type_definitions = new HashSet<string>() { "null", "void", "int", "float", "double", "point", "char", "string", "picture", "bool" }; //Хранит в себе зарезервированные имена типов.
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
                    case PreTokenGroup.Equality:
                        i= While_delegate_function(c => getTypeChar(c) == nowcharID, nowcharID, i, CodeStorage, Input_Text, RowCount, Temp2);
                        break;
                    case PreTokenGroup.Alphabet:
                        i = While_delegate_function(CycleCondition_forAlphabet, nowcharID, i, CodeStorage, Input_Text, RowCount, Temp2);
                        break;
                    case PreTokenGroup.Numeric:
                    case PreTokenGroup.RuAlphabet:
                    case PreTokenGroup.Symbol:
                        i = While_delegate_function(c => getTypeChar(c) == nowcharID, nowcharID, i , CodeStorage, Input_Text, RowCount,Temp2);
                        break;
                    case PreTokenGroup.EndOfString:
                        i++;
                        CodeStorage.Add(new Token(nowchar.ToString(), false, Group_of_Tokens.EndOfString,RowCount,i-1,i));
                        if (!Temp2.Rule_check(CodeStorage.Last()))
                            ;
                        else
                            ;
                        RowCount++;
                        break;
                    case PreTokenGroup.HSymbols:
                    case PreTokenGroup.Delimeter:
                        Token Temp;
                        if (Temp2.Rule_check(Temp = GetToken(nowchar.ToString(), nowcharID, CodeStorage.Last(), i, i,Temp2)))
                            CodeStorage.Add(Temp);
                        else
                            ;
                        break;
                }
                i++;
            }
        }

        private bool CycleCondition_forAlphabet(char inCh)
        {
            if ((getTypeChar(inCh) == PreTokenGroup.Alphabet) || (getTypeChar(inCh) == PreTokenGroup.Numeric))
                return true;
            else return false;
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
                Temp=(GetToken(Data_former, second_cycle_condition, null, i_counter, helper_counter - 1, BldClass));
            else
                Temp=(GetToken(Data_former, second_cycle_condition, Word_list.Last(), i_counter, helper_counter - 1,BldClass));
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
                    case '*':
                    case '/':
                    case '&':
                    case '|':
                    case '>':
                    case '<':
                    case '^':
                    case '%':
                        return PreTokenGroup.Symbol;

                    case '=':
                        return PreTokenGroup.Equality;
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
                    //case ';':
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

        private Token GetToken(string inStr, PreTokenGroup preID, Token ID_Of_previous, int FValue, int SValue, Builder bld)
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
                case PreTokenGroup.Equality:
                    if (FValue == SValue)
                        return (new Token(inStr, space_resulter, Group_of_Tokens.Assignment, RowCount, FValue, SValue));
                    else if (FValue == SValue - 1)
                        return (new Boolean_operation(inStr, space_resulter, RowCount, FValue, SValue, BooleanSymbol_ID.Equality));
                    else return null;

                case PreTokenGroup.Alphabet: //Случай строки составленной из алфавитных символов
                    if (Construction_reservation.Contains(inStr.ToLower()))
                        return (new Token(inStr, space_resulter, Group_of_Tokens.Construction, RowCount, FValue, SValue));
                    else if (Type_definitions.Contains(inStr.ToLower()))
                        return (new Token(inStr, space_resulter, Group_of_Tokens.Type_Definition, RowCount, FValue, SValue));
                    else if (Function_storage.ContainsKey(inStr))
                    {
                        dynamic Tester = Function_storage[inStr];
                        Tester.ReCreateToken(inStr, space_resulter, Group_of_Tokens.Function, RowCount, FValue, SValue);  // ПОТЕНЦИАЛЬНОЕ БАГОДЕРЬМО С УКАЗАТЕЛЯМИ, ПЕРВИЧНЫЙ FUNCTION STORAGE НЕ ДОЛЖЕН МЕНЯТЬСЯ.
                        return (Tester);
                    }
                    else if(bld.CheckArgLocalName(inStr)!=null)
                    {
                        return new Variable(bld.CheckArgLocalName(inStr) as Variable, RowCount, Group_of_Tokens.Variable, FValue, SValue);
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
