﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tokens_Library;
using Exceptions_Library;
using Parsers;


namespace Parsers
{

    public class SemanticalParser
    {
        private List<ETypeTable> IntegerTypePriorityList = new List<ETypeTable>() { ETypeTable.Byte, ETypeTable.Short, ETypeTable.Int, ETypeTable.Long, ETypeTable.Float, ETypeTable.Double };

        public ETypeTable String_semantical_check(dynamic ExpressionForCheck)
        {
            Queue<Token> TempStacker = new Queue<Token>();
            Group_of_Tokens TempSwitcher;
            TempSwitcher = ExpressionForCheck.get_group_of_token();

            switch (TempSwitcher)
            {
                case Group_of_Tokens.AriphmeticalExpression:
                    TempStacker = new Queue<Token>(ExpressionForCheck.RPN_Expression_data);
                    ExpressionForCheck.SemanticalWasChecked();
                    return BaseRPN_SemanticalParse(TempStacker, TempSwitcher);
                case Group_of_Tokens.Variable:
                    ETypeTable TempResulter;
                    TempStacker = new Queue<Token>(ExpressionForCheck.RPNValue.RPN_Expression_data);
                    if (!ExpressionForCheck.RPNValue.Check_Semantical)
                        ExpressionForCheck.RPNValue.SetExpressionResultType(BaseRPN_SemanticalParse(TempStacker, TempSwitcher));
                    if (TwoType_AutoReassignment_Ability_check(ExpressionForCheck.GetTypeOfToken(), ExpressionForCheck.RPNValue.ExpressionTypeResult))
                        return ExpressionForCheck.GetTypeOfToken();
                    else return ETypeTable.ERR;
            }
            return ETypeTable.NULL;
        }

        private ETypeTable BaseRPN_SemanticalParse(Queue<Token> InTempStacker, Group_of_Tokens InTempSwitcher)
        {
            Token NowItem;
            Stack<ETypeTable> ResulterTest = new Stack<ETypeTable>();
            ETypeTable Resulter = ETypeTable.NonStarted;
            Stack<Token> TempSolutionStack = new Stack<Token>();
            while (InTempStacker.Count != 0)
            {
                NowItem = InTempStacker.Dequeue();
                InTempSwitcher = NowItem.get_group_of_token();
                switch (InTempSwitcher)
                {
                    case Group_of_Tokens.Digit:
                    case Group_of_Tokens.Function:
                        TempSolutionStack.Push(NowItem);
                        break;
                    case Group_of_Tokens.Ariphmetical:
                        if (Resulter == ETypeTable.NonStarted)
                            Resulter = CountTypes(TempSolutionStack.Pop().GetTypeOfToken(), TempSolutionStack.Pop().GetTypeOfToken(), NowItem);
                        else
                            Resulter = CountTypes(TempSolutionStack.Pop().GetTypeOfToken(), Resulter, NowItem);
                        break;
                    case Group_of_Tokens.AriphmeticalExpression:
                        break;
                    case Group_of_Tokens.BooleanOperation:
                        if (TempSolutionStack.Count == 2)
                            ResulterTest.Push(TwoTypeBoolOperation_AbilityCheck(TempSolutionStack.Pop().GetTypeOfToken(), TempSolutionStack.Pop().GetTypeOfToken(), NowItem));
                        else if (TempSolutionStack.Count > 0)
                            ResulterTest.Push(TwoTypeBoolOperation_AbilityCheck(TempSolutionStack.Pop().GetTypeOfToken(), ResulterTest.Pop(), NowItem));
                        else if (ResulterTest.Count == 2)
                            ResulterTest.Push(TwoTypeBoolOperation_AbilityCheck(ResulterTest.Pop(), ResulterTest.Pop(), NowItem));
                        else
                            ResulterTest.Push(ETypeTable.ERR);
                        Resulter = ResulterTest.Peek();
                        break;
                        
                }
            }
            return Resulter;
        }

        private ETypeTable TwoTypeBoolOperation_AbilityCheck(ETypeTable FirstOperandType, ETypeTable SecondOperantType, Token BoolOperation)
        {
            if (TwoType_AutoReassignment_Ability_check(FirstOperandType, SecondOperantType))
            {
                return ETypeTable.Boolean;
            }
            else
                return ETypeTable.ERR;
        }

        private bool TwoType_AutoReassignment_Ability_check(ETypeTable ResultingType, ETypeTable TypeForReassignment)
        {
            ETypeGroup ResGroupType=Typecial.GetTypeGroup(ResultingType);
            ETypeGroup InGroupType = Typecial.GetTypeGroup(TypeForReassignment);
            if(ResGroupType==InGroupType)
            {
                if (ResultingType >= TypeForReassignment)
                    return true;
                else
                {
                    switch (ResGroupType)
                    {
                        case ETypeGroup.IntegerGroup:
                            //создание предупреждения о потере данных и автоматическое приведение
                            return true;
                        case ETypeGroup.StringGroup:
                            if (ResultingType == ETypeTable.String)
                            {
                                return true;
                            }
                            else
                                return false;
                        default:
                            return false;
                    }
                }
            }
            else
            {
                switch(ResGroupType)
                {
                    case ETypeGroup.IntegerGroup:
                        switch(TypeForReassignment)
                        {
                            case ETypeTable.String:
                                break;
                            case ETypeTable.Overflowed:
                                break;
                            case ETypeTable.Point:
                                break;
                            default:
                                break;
                        }
                        break;
                    case ETypeGroup.StringGroup:
                        if (ResultingType==ETypeTable.String)
                        {
                            //Вызов метода tostring.
                        }
                        else
                        {
                            switch(ResultingType)
                            {

                            }
                        }
                        break;
                }
                return false;
            }
        }

        private ETypeTable Integer_IntegerTypicalCount(ETypeTable FirstType, ETypeTable SecondType, Token Operation)
        {
            switch ((Operation as Ariphmetical).AriphmeticalID)
            {
                case AriphmeticalSymbol_ID.Division:
                    if ((FirstType == ETypeTable.Double) || (SecondType == ETypeTable.Double))
                        return ETypeTable.Double;
                    else
                        return ETypeTable.Float;
                default:
                    if (FirstType > SecondType)
                        return FirstType;
                    else return SecondType;
            }
        }

        private ETypeTable Integer_StringTypicalCount(Token Operation)
        {
            switch((Operation as Ariphmetical).AriphmeticalID)
            {
                case AriphmeticalSymbol_ID.Multiplication:
                    return ETypeTable.String;
                default: return ETypeTable.ERR;
            }
        }

        private ETypeTable String_StringTypicalCount(ETypeTable FirstT, ETypeTable SecondT, Token Operation)
        {
            switch ((Operation as Ariphmetical).AriphmeticalID)
            {
                case AriphmeticalSymbol_ID.Plus:
                    return Integer_IntegerTypicalCount(FirstT, SecondT,Operation);
                default: return ETypeTable.ERR;
            }
        }

        private ETypeTable CountTypes(ETypeTable FirstValue, ETypeTable SecondValue, Token Operand)
        {
            ETypeTable ResultType;
            switch(SecondValue)
            {
                case ETypeTable.Byte:
                case ETypeTable.Short:
                case ETypeTable.Int:
                case ETypeTable.Long:
                case ETypeTable.Float:
                case ETypeTable.Double:
                    if (Typecial.GetTypeGroup(FirstValue) == ETypeGroup.IntegerGroup)
                        return Integer_IntegerTypicalCount(FirstValue, SecondValue,Operand);
                    else if (Typecial.GetTypeGroup(FirstValue) == ETypeGroup.StringGroup)
                    {
                        return Integer_StringTypicalCount(Operand);
                    }
                    else
                    {
                        switch (FirstValue)
                        {
                            case ETypeTable.Overflowed:
                            case ETypeTable.Point:
                                return FirstValue;
                            default: return ETypeTable.ERR;
                        }
                    }
                case ETypeTable.String:
                case ETypeTable.Char:
                    if (Typecial.GetTypeGroup(FirstValue) == ETypeGroup.IntegerGroup)
                        return Integer_StringTypicalCount(Operand);
                    else if (Typecial.GetTypeGroup(FirstValue) == ETypeGroup.StringGroup)
                        return String_StringTypicalCount(FirstValue, SecondValue, Operand);
                    else
                        switch(FirstValue)
                        {

                        }
                        break;
            }
            return ETypeTable.NULL;
        }
    }

    public enum Rules_Statement
    {
        NoN=-3,
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
        AfterVarGet=37,
        AfterVarGetArgs=38,
        DefaultInIFBody=39,
        EmptyEndOfString=40,
    }
    public enum Which_builder
    {
        Digit=1,
    }

    public class Builder
    {
        //private int PriorityIncreaser = 0;
        private SemanticalParser Semantical = new SemanticalParser();
        private Rules_Statement Magazine_state;
        private Stack<If_Condition_construction> IFBuild_magazine_storage = new Stack<If_Condition_construction>();
        private List<Token> Build_magazine_storage=new List<Token>();
        private List<Token> Strange_names=new List<Token>();
        private Stack<Token> String_Translate_Stack=new Stack<Token>();
        private List<Token> TranslateCode = new List<Token>();
        private int Comma_counter = 0;
        private int StructureBracketCounter = 0;
        private int InlineBracketCounter = 0;
        //private int VarAssignmentInProgress = 0;
        //private Variable VarCallInProgress = null;
        private int BoperationsInProgress = 0;
        private bool VarDefinitionInProgress = false;
        private AnyFunction FunctionBodyInProgress = null;
        private bool IFConstructionInProgress = false;
        private int IfBodyProgression = 0;
        private If_Condition_construction NowBodyInProgression = null;
        private int SetVarInProgress = 0;
        private bool IgnoreCase = false;

        private bool AddMethodToQueue_OfVariable(string VarName, bool WhichOneMethod, Token AnyExpression)
        {
            if (FunctionBodyInProgress!=null)
            {
                return FunctionBodyInProgress.AddMethodToArgument(VarName, AnyExpression, WhichOneMethod);
            }
            else
            {
                return false;
            }
        }

        private bool ChangeLastMethodTypeInQueue(Token InVariable, Token InExpression)
        {
            if (FunctionBodyInProgress != null)
            {
                return FunctionBodyInProgress.ChangeMethodOfArgument(InVariable.Data, InExpression);
            }
            else
                return false;
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
            NewElement.BaseSetPriority();
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
                            StructureBracketCounter++;
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.DefaultInIFConstruction;
                            return true;
                        }
                        else return false;
                    else return false;

                case Rules_Statement.VarCallFounded:
                    if (NewElGroup==Group_of_Tokens.Assignment)
                    {
                        SetVarInProgress++;
                        String_Translate_Stack.Push(new Token(String_Translate_Stack.Pop(), Group_of_Tokens.VariableMethodCall));
                        AddMethodToQueue_OfVariable(String_Translate_Stack.Peek().Data, true, null);
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.DefaultInStr;
                    }
                    else
                    {
                        Token TempValue=String_Translate_Stack.Pop();
                        String_Translate_Stack.Push(new Token(TempValue, Group_of_Tokens.VariableMethodCall));
                        AddMethodToQueue_OfVariable(String_Translate_Stack.Peek().Data, true, null);
                        switch (NewElGroup)
                        {
                            case Group_of_Tokens.Ariphmetical:
                                String_Translate_Stack.Push(NewElement);
                                Magazine_state = Rules_Statement.FormAriphmExpression;
                                return true;
                            case Group_of_Tokens.BooleanOperation:
                                String_Translate_Stack.Push(NewElement);
                                Magazine_state = Rules_Statement.AfterBoolOperation;
                                return true;
                            case Group_of_Tokens.Delimeter:
                                if ((NewElement as Delimeter).DelimeterID==Delimeters_ID.LBracket)
                                {
                                    
                                }
                                else if(((NewElement as Delimeter).DelimeterID==Delimeters_ID.RBracket)&&(StructureBracketCounter>0))
                                {
                                    String_Translate_Stack.Push(new Expression(String_Translate_Stack, inst => ((inst.Count > 0) && (inst.Peek().Token_Group != Group_of_Tokens.Delimeter)),false));
                                    CastArgsBrackets(Expression_Type.Ariphmetical_expression, false);
                                    if ((IFConstructionInProgress) && (StructureBracketCounter != 0))
                                        Magazine_state = Rules_Statement.FormAriphmExpression;
                                    else
                                        Magazine_state = Rules_Statement.EndOfString;
                                }
                                break;
                        }
                    }
                    break;

                case Rules_Statement.AfterVarGet:
                    if (NewElGroup == Group_of_Tokens.Ariphmetical)
                    {
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.FormAriphmExpression;
                    }
                    else if (NewElGroup==Group_of_Tokens.BooleanOperation)
                    {
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.AfterBoolOperation;
                    }
                    else if(NewElGroup==Group_of_Tokens.Delimeter)
                    {
                        if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.LBracket)
                        {

                        }
                        else if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.RBracket)
                        {
                            CastBrackets(false);
                            if (IFConstructionInProgress)
                                if (StructureBracketCounter > 0)
                                {
                                    Magazine_state = Rules_Statement.AfterArifmExpr;
                                    return true;
                                }
                                else Magazine_state = Rules_Statement.EndOfString;
                            else
                            {
                                Magazine_state = Rules_Statement.FormAriphmExpression;
                                return true;
                            }
                        }
                    }
                    else if(NewElGroup==Group_of_Tokens.Assignment)
                    {
                        SetVarInProgress++;
                        String_Translate_Stack.Push(NewElement);
                        VarDefinitionInProgress = true;
                        Magazine_state = Rules_Statement.AfterVarDefEquality;
                        return true;
                    }
                    else if (NewElGroup==Group_of_Tokens.EndOfString)
                    {
                        goto case Rules_Statement.EndOfString;
                    }

                    break;


                case Rules_Statement.DefaultInIFConstruction:
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
                        case Group_of_Tokens.Variable:
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.VarCallFounded;
                            return true;
                        case Group_of_Tokens.Name:

                            break;
                        case Group_of_Tokens.Delimeter:
                            if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.LBracket)
                            {
                                String_Translate_Stack.Push(NewElement);
                                InlineBracketCounter++;
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
                        String_Translate_Stack.Push(new Token(NewElement, Group_of_Tokens.VariableMethodCall));
                        AddMethodToQueue_OfVariable(NewElement.Data, true, null);
                        Magazine_state = Rules_Statement.AfterVarGet;
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
                        Changer = (NewElement as Boolean_operation).BooleanID;
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
                        else goto case Rules_Statement.EndOfString;
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
                                InlineBracketCounter++;
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
                        case Group_of_Tokens.Variable:
                            String_Translate_Stack.Push(new Token(NewElement, Group_of_Tokens.VariableMethodCall));
                            AddMethodToQueue_OfVariable(NewElement.Data, true, null);
                            Magazine_state = Rules_Statement.AfterVarGet;
                            break;
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
                            Magazine_state = Rules_Statement.AfterArifmExpr;
                            return true;
                        case Group_of_Tokens.Delimeter: //Если разделитель
                            Changer = (NewElement as Delimeter).get_group_of_token();
                            if (Changer == Delimeters_ID.LBracket) //Разделитель открывающаяся скобка
                            {
                                String_Translate_Stack.Push(NewElement);
                                InlineBracketCounter++;
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
                        case Group_of_Tokens.Variable:
                            String_Translate_Stack.Push(new Token(NewElement, Group_of_Tokens.VariableMethodCall));
                            AddMethodToQueue_OfVariable(NewElement.Data, true, null);
                            Magazine_state = Rules_Statement.AfterVarGet;
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
                            Magazine_state = Rules_Statement.VarCallFounded;
                            return true;

                        case Group_of_Tokens.Name:

                            break;
                        case Group_of_Tokens.Delimeter:
                            if (InStrChanger.DelimeterID==Delimeters_ID.LBracket)
                            {
                                String_Translate_Stack.Push(NewElement);
                                InlineBracketCounter++;
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
                                    IFConstructionInProgress = true;
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
                    {
                        String_Translate_Stack.Push(NewElement);
                        BoperationsInProgress++;
                        Magazine_state = Rules_Statement.AfterBoolOperation;
                    }
                        
                    else if (NewElGroup == Group_of_Tokens.Delimeter)
                    {
                        if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.Equality)
                            goto case Rules_Statement.Equality;
                        else if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.RBracket)
                        {
                            if ((InlineBracketCounter == 0) && (StructureBracketCounter > 0))
                            {
                                String_Translate_Stack.Push(new Expression(String_Translate_Stack, inst => ((inst.Count > 0) && (inst.Peek().Token_Group != Group_of_Tokens.Delimeter)), false));
                                CastArgsBrackets(Expression_Type.Ariphmetical_expression, false);
                            }
                            else
                                CastBrackets(false);
                            Magazine_state = Rules_Statement.AfterArifmExpr;
                        }
                    }
                    else if (NewElGroup == Group_of_Tokens.EndOfString)
                    {
                        if (!IFConstructionInProgress)
                            String_Translate_Stack.Push(new Expression(String_Translate_Stack, Inst => ((Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter)) && (Inst.Peek().Token_Group != Group_of_Tokens.Assignment)&&(Inst.Peek().Token_Group!=Group_of_Tokens.NoN), true));
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
                        goto case Rules_Statement.VarCallFounded;
                    else if (NewElGroup == Group_of_Tokens.Name)
                        ;
                    else return false;
                    break;

                case Rules_Statement.DefaultInIFBody:
                    switch(NewElGroup)
                    {
                        case Group_of_Tokens.Delimeter:
                            if (NewElement.get_group_of_token() == Delimeters_ID.LBrace)
                            {
                                NowBodyInProgression.IncreaseBraceCount();
                                IgnoreCase = true;
                                String_Translate_Stack.Push(NewElement);
                                return true;
                            }
                            else if (NewElement.get_group_of_token() == Delimeters_ID.Semicolon)
                            {

                            }
                            else if (NewElement.get_group_of_token() == Delimeters_ID.LBracket)
                            {

                            }
                            else if (NewElement.get_group_of_token() == Delimeters_ID.RBrace)
                            {
                                if (NowBodyInProgression.BraceCount > 0)
                                {
                                    NowBodyInProgression.DecreaseBraceCount();
                                    String_Translate_Stack.Pop();
                                    if(NowBodyInProgression.BraceCount==0)
                                    {
                                        if (NowBodyInProgression.ThenInTranslation == false)
                                        {
                                            IfBodyProgression--;
                                            String_Translate_Stack.Pop();
                                            String_Translate_Stack.Push(NowBodyInProgression);
                                            Magazine_state = Rules_Statement.EndOfString;
                                        }
                                        else
                                            return true;
                                    }
                                    else if(NowBodyInProgression.BraceCount<0)
                                    {

                                    }
                                    return true;
                                }
                            }
                            else
                            return false;
                            break;
                        case Group_of_Tokens.Digit:
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.WhichNextDigit;
                            break;
                        case Group_of_Tokens.EndOfString:
                            if (((String_Translate_Stack.Peek().Token_Group==Group_of_Tokens.Delimeter)&&(String_Translate_Stack.Peek().get_group_of_token()==Delimeters_ID.LBrace))||(IgnoreCase))
                            {
                                IgnoreCase = false;
                                return true;
                            }
                            break;
                        case Group_of_Tokens.Construction:
                            if(NewElement.GetID_of_Construction()==Constructions_ID.Else)
                            {
                                if ((NowBodyInProgression.ThenInTranslation == true) && (NowBodyInProgression.BraceCount == 0))
                                {
                                    NowBodyInProgression.ThenInTranslation = false;
                                    IgnoreCase = true;
                                    return true;
                                }
                                else
                                    return false;
                            }
                            else
                            {
                                return false;
                            }
                        case Group_of_Tokens.Help_symbol:
                            if (NewElement.get_group_of_token() == Help_SymbolsID.Apostrophe)
                            {

                            }
                            else if (NewElement.get_group_of_token() == Help_SymbolsID.Quotes)
                            {

                            }
                            else
                                return false;
                            break;
                        case Group_of_Tokens.Function:
                            String_Translate_Stack.Push(NewElement);
                            Build_magazine_storage.Add(NewElement);
                            Magazine_state = Rules_Statement.FuncNameFounded;
                            break;
                        case Group_of_Tokens.Variable:
                            String_Translate_Stack.Push(new Token(NewElement, Group_of_Tokens.VariableMethodCall));
                            AddMethodToQueue_OfVariable(NewElement.Data, true, null);
                            Magazine_state = Rules_Statement.AfterVarGet;
                            break;
                        case Group_of_Tokens.Name:
                            break;
                    }
                    break;

                case Rules_Statement.EndOfString:
                    dynamic Temp;
                    bool StateSetter = true;
                    bool SemanticalIsChecked = false;
                    if(SetVarInProgress>0)
                    {
                        Temp=(new Expression(String_Translate_Stack, Inst => ((Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter)) && (Inst.Peek().Token_Group != Group_of_Tokens.Assignment),true));
                        String_Translate_Stack.Pop();
                        ChangeLastMethodTypeInQueue(String_Translate_Stack.Peek(), Temp);
                        SetVarInProgress--;
                        VarDefinitionInProgress = false;
                    }
                    if (IFConstructionInProgress)
                    {
                        String_Translate_Stack.Push(new If_Condition_construction(String_Translate_Stack));
                        IFConstructionInProgress = false;
                        IfBodyProgression++;
                        NowBodyInProgression = String_Translate_Stack.Peek() as If_Condition_construction;
                        IFBuild_magazine_storage.Push(NowBodyInProgression);    
                        Magazine_state = Rules_Statement.DefaultInIFBody;
                        StateSetter = false;
                    }
                    if (VarDefinitionInProgress)
                    {
                        if (BoperationsInProgress == 1)
                            BoperationsInProgress--;
                        String_Translate_Stack.Push(new Variable(String_Translate_Stack));
                        Semantical.String_semantical_check(String_Translate_Stack.Peek());
                        SemanticalIsChecked = true;
                        VarDefinitionInProgress = false;
                    }
                    if(IfBodyProgression>0)
                    {
                        if ((String_Translate_Stack.Peek().Token_Group != Group_of_Tokens.Delimeter) && (StateSetter))
                        {
                            NowBodyInProgression.AddMethodToBody(String_Translate_Stack.Pop());
                            IFBuild_magazine_storage.Pop();
                            IFBuild_magazine_storage.Push(NowBodyInProgression);
                        }   
                        Magazine_state = Rules_Statement.DefaultInIFBody;
                        return true;
                    }
                    else if (FunctionBodyInProgress != null)
                    {
                        if (IfBodyProgression == 0)
                        {
                            if (InlineBracketCounter == 0)
                            {
                                Temp = String_Translate_Stack.Pop();
                                if (Temp.Token_Group == Group_of_Tokens.Variable)
                                {
                                    if (!FunctionBodyInProgress.AddLocalArgument(Temp))
                                    {
                                        return false;
                                    }
                                }
                                if (!SemanticalIsChecked)
                                    Semantical.String_semantical_check(Temp);
                                FunctionBodyInProgress.AddNewFunctionBodyString(Temp);
                                TranslateCode.Add(String_Translate_Stack.Peek());
                                Build_magazine_storage.Clear();
                                Magazine_state = Rules_Statement.DefaultInStr;
                                return true;
                            }
                            else return false;
                        }
                        else
                            return true;
                    }
                    else if ((InlineBracketCounter == 0) && (String_Translate_Stack.Count == 1) && (StructureBracketCounter == 0))
                    {
                        Semantical.String_semantical_check(String_Translate_Stack.Peek());
                        TranslateCode.Add(String_Translate_Stack.Pop());
                        String_Translate_Stack.Clear();
                        Magazine_state = Rules_Statement.Default;
                        return true;
                    }
                    else
                    {
                        String_Translate_Stack.Push(new Expression(String_Translate_Stack, Inst => (Inst.Count > 0), true, SemanticalCheck:true,SemanticalParser:Semantical));
                        Semantical.String_semantical_check(String_Translate_Stack.Peek());
                    }
                        return false;


                case Rules_Statement.Equality:
                    break;


                case Rules_Statement.FuncNameFounded: //Состояние при котором строка начинается с вызова имени функции.
                    if (NewElGroup == Group_of_Tokens.Delimeter)
                        if (NewElement.get_group_of_token() == Delimeters_ID.LBracket)
                        {
                            Comma_counter = 0;
                            String_Translate_Stack.Push(NewElement);
                            StructureBracketCounter++;
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
                            if(NewElement.get_group_of_token()==Delimeters_ID.LBracket)
                            {
                                InlineBracketCounter++;
                                String_Translate_Stack.Push(NewElement);
                                Magazine_state = Rules_Statement.LBracketDefaultInStr;
                            }
                            break;
                        case Group_of_Tokens.Name:
                            break;
                        case Group_of_Tokens.Variable:
                            String_Translate_Stack.Push(new Token(NewElement,Group_of_Tokens.VariableMethodCall));
                            AddMethodToQueue_OfVariable(NewElement.Data, true, null);
                            Magazine_state = Rules_Statement.AfterVarGetArgs;
                            break;
                    }
                    break;

                case Rules_Statement.AfterVarGetArgs:
                    if (NewElGroup==Group_of_Tokens.Ariphmetical)
                    {
                        String_Translate_Stack.Push(NewElement);
                        Magazine_state = Rules_Statement.ArgsAfterAriphmetical;
                        return true;
                    }
                    else if(NewElGroup==Group_of_Tokens.BooleanOperation)
                    {
                        String_Translate_Stack.Push(NewElement);
                    }
                    else if(NewElGroup==Group_of_Tokens.Delimeter)
                    {
                        if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.RBracket)
                        {
                            if (InlineBracketCounter > 0)
                            {
                                CastBrackets(false);
                                return true;
                            }
                            else
                            {
                                Comma_counter++;
                                String_Translate_Stack.Push(new Expression(String_Translate_Stack, inst => ((inst.Count > 0) && (inst.Peek().Token_Group != Group_of_Tokens.Delimeter)),true));
                                (Build_magazine_storage.Last() as AnyFunction).Args[Comma_counter - 1].RPNValue = String_Translate_Stack.Pop();
                                CastArgsBrackets(Expression_Type.Ariphmetical_expression,true);
                                return true;
                            }
                        }
                        else
                            return false;
                    }
                    else if(NewElGroup==Group_of_Tokens.Help_symbol)
                    {
                        if ((NewElement as HelpSymbol).SymbolID==Help_SymbolsID.Comma)
                        {
                            Comma_counter++;
                            String_Translate_Stack.Push(new Expression(String_Translate_Stack, inst => ((inst.Count > 0) && (inst.Peek().Token_Group != Group_of_Tokens.Delimeter)),false));
                            (Build_magazine_storage.Last() as AnyFunction).AddArgument(String_Translate_Stack.Peek(), Comma_counter - 1, Semantical.String_semantical_check(String_Translate_Stack.Pop()));
                            Magazine_state = Rules_Statement.ArgsOnCall;
                            return true;
                        }
                    }
                    break;

                case Rules_Statement.WhichNextDigitArgs:
                    if (NewElGroup == Group_of_Tokens.Delimeter)
                        if (NewElement.get_group_of_token() == Delimeters_ID.RBracket)
                        {
                            if (InlineBracketCounter>0)
                            {
                                CastBrackets(false);
                            }
                            else
                            {
                                Comma_counter++;
                                if(Build_magazine_storage.Count==0)
                                {
                                    String_Translate_Stack.Push(new Expression(String_Translate_Stack, inst => ((inst.Count > 0) && (inst.Peek().Token_Group != Group_of_Tokens.Delimeter)),false));
                                    CastArgsBrackets(Expression_Type.Ariphmetical_expression,false);
                                }
                                else if (Comma_counter > (Build_magazine_storage.Last() as AnyFunction).Args.Count)
                                    return false;
                                else
                                {
                                    String_Translate_Stack.Push(new Expression(String_Translate_Stack, inst => ((inst.Count > 0) && (inst.Peek().Token_Group != Group_of_Tokens.Delimeter)),true));
                                    (Build_magazine_storage.Last() as AnyFunction).AddArgument(String_Translate_Stack.Peek(), Comma_counter - 1, Semantical.String_semantical_check(String_Translate_Stack.Pop()));
                                    CastArgsBrackets(Expression_Type.Ariphmetical_expression,true);
                                }
                            }
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
                                String_Translate_Stack.Push(new Expression(String_Translate_Stack, inst => ((inst.Count > 0) && (inst.Peek().Token_Group != Group_of_Tokens.Delimeter)),false));
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
                    else if(NewElGroup==Group_of_Tokens.Delimeter)
                    {
                        if ((NewElement as Delimeter).DelimeterID == Delimeters_ID.LBracket)
                        {
                            InlineBracketCounter++;
                            String_Translate_Stack.Push(NewElement);
                            Magazine_state = Rules_Statement.ArgsOnCall;
                            return true;
                        }
                        else
                            return false;
                    }
                    else if(NewElGroup==Group_of_Tokens.Variable)
                    {
                        String_Translate_Stack.Push(new Token(NewElement, Group_of_Tokens.VariableMethodCall));
                        AddMethodToQueue_OfVariable(NewElement.Data, true, null);
                        Magazine_state = Rules_Statement.AfterVarGetArgs;
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

        private void CastArgsBrackets(Expression_Type Type_OF_expr, bool NotJustCastBracket)
        {
            Token TempValue=String_Translate_Stack.Pop();
            if (NotJustCastBracket)
            {
                dynamic TypeConnection = Build_magazine_storage.Last();
                Comma_counter -= TypeConnection.Args.Count;
                Build_magazine_storage.RemoveAt(Build_magazine_storage.Count - 1);
                Comma_counter = ReCountPreviousComm();
            }
            else
            {
                String_Translate_Stack.Pop();
                String_Translate_Stack.Push(TempValue);
            }
            //switch (Temp.Token_Group)
            //{
            //    case Group_of_Tokens.Ariphmetical:
            //        if (TypeConnection.GetDelegateMethodType() == 1)
            //        {
            //            String_Translate_Stack.Push(Build_magazine_storage.Last());
            //            String_Translate_Stack.Push(new Expression(String_Translate_Stack, Expression_Type.Ariphmetical_expression, Inst => (Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter) && (Inst.Peek().Token_Group != Group_of_Tokens.BooleanOperation)));
            //            Comma_counter -= TypeConnection.Args.Count;
            //            Magazine_state = Rules_Statement.WhichNextDigitArgs;
            //        }

            //        break;
            //    case Group_of_Tokens.Delimeter:
            //        if (TypeConnection.GetDelegateMethodType()==1)
            //        {
            //            String_Translate_Stack.Push(TypeConnection);
            //            Magazine_state=Rules_Statement.WhichNextDigitArgs;
            //        }
            //        break;
            //    case Group_of_Tokens.Help_symbol:
            //        if (Temp.get_group_of_token()==Help_SymbolsID.Comma)
            //        {
            //            if(Build_magazine_storage.Count>1)
            //            {
            //                String_Translate_Stack.Pop();
            //                if (Build_magazine_storage[Build_magazine_storage.Count-2]==String_Translate_Stack.Peek())
            //                {

            //                }
            //            }
            //        }
            //        break;
            //}
            StructureBracketCounter--;
            if(Build_magazine_storage.Count==0)
            {
                if(VarDefinitionInProgress)
                {
                    Magazine_state = Rules_Statement.AfterVarDefEquality;
                }
                else if(IFConstructionInProgress)
                {
                    Magazine_state = Rules_Statement.AfterArifmExpr;
                }
                else if(IfBodyProgression>0)
                {
                    Magazine_state = Rules_Statement.AfterArifmExpr;
                }
            }
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
            String_Translate_Stack.Push(new Expression(Expr_creator, Inst => (Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter) && (Inst.Peek().Token_Group != Group_of_Tokens.BooleanOperation) && (Inst.Peek().Token_Group != Group_of_Tokens.Assignment),false));

        }

        private Rules_Statement IFDefaultBodyConstructionRECLAIMER(Constructions_ID IDOFConstruction)
        {
            switch (IDOFConstruction)
            {
                case Constructions_ID.Else:
                    break;
                case Constructions_ID.If:
                    break;
                case Constructions_ID.For:
                    break;
                case Constructions_ID.While:
                    break;
                case Constructions_ID.Repeat:
                    break;
                default: break;
            }
            return Rules_Statement.Default;
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
                if (Expr_creator.Count==1)
                {
                    String_Translate_Stack.Push(Expr_creator.Pop());
                }
                else
                {
                    String_Translate_Stack.Push(new Expression(Expr_creator, Inst => (Inst.Count > 0) && (Inst.Peek().Token_Group != Group_of_Tokens.Delimeter) && (Inst.Peek().Token_Group != Group_of_Tokens.BooleanOperation) && (Inst.Peek().Token_Group != Group_of_Tokens.Assignment), false));
                }
                InlineBracketCounter--;
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
        HashSet<string> Construction_reservation = new HashSet<string>() { "if", "while", "for", "function", "procedure", "do", "repeat", "until", "begin", "end", "else" };  //Хранит в себе имена зарезервированных конструкций


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
            Temps.Add(new Variable(ETypeTable.Int));
            Function_storage.Add("testfunc", new Built_InFunction<int>(TestFunc,Temps,ETypeTable.Int));
            Function_storage.Add("testfuncs", new Built_InFunction<Token>(TestFunc2, Temps2, ETypeTable.NULL));
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
                        i = Cycle_forNumeric(Input_Text, i, CodeStorage, RowCount, Temp2);
                        break;
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
                        if (Temp2.Rule_check(Temp = GetToken(nowchar.ToString(), nowcharID, CodeStorage.Last(), i, i,Temp2,Input_Text)))
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

        private bool CycleCondition_forNumeric(char inCh, int PreTokTypeRes)
        {
            if (getTypeChar(inCh) == PreTokenGroup.Numeric)
                return true;
            else if (GetHelpSymbolID(inCh)==Help_SymbolsID.Dot)
            {
                if (PreTokTypeRes > 0)
                    return false;
                PreTokTypeRes++;
                return true;
            }
            else
            {
                return false;
            }
        }

        private int Cycle_forNumeric(string input_text, int i_counter, List<Token> Word_list, int row_count, Builder BldClass)
        {
            int helper_counter = i_counter;
            byte FractureCounter = 0;
            string Data_former = "";
            //    public Digit(string Ndata, bool Nspace, Group_of_Tokens Nid, int Nrow, int NFRange_value, int NSRange_value, bool Fractured):base(Ndata,Nspace,Nid,Nrow,NFRange_value,NSRange_value)
            //{
            //    Is_fracture = Fractured;
            //}
            Token Temp;
            while(CycleCondition_forNumeric(input_text[helper_counter],FractureCounter))
            {
                Data_former += input_text[helper_counter];
                helper_counter++;
            }
            if (Word_list.Count == 0)
                Temp = new Digit(Data_former, true, Group_of_Tokens.Digit, RowCount, i_counter, helper_counter - 1, FractureCounter > 0);
            else
                Temp = new Digit(Data_former, Word_list.Last().Data==" ", Group_of_Tokens.Digit, RowCount, i_counter, helper_counter - 1, FractureCounter > 0);
            if (BldClass.Rule_check(Temp))
                Word_list.Add(Temp);//Сформировав строку, вызывает метод определения и составления токена для слова, после чего добавляет его в хранилище кода программы.
            return helper_counter - 1;
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
                Temp=(GetToken(Data_former, second_cycle_condition, null, i_counter, helper_counter - 1, BldClass,input_text));
            else
                Temp=(GetToken(Data_former, second_cycle_condition, Word_list.Last(), i_counter, helper_counter - 1,BldClass,input_text));
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

        private Token GetToken(string inStr, PreTokenGroup preID, Token ID_Of_previous, int FValue, int SValue, Builder bld, string BigInString)
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

        private Help_SymbolsID GetHelpSymbolID(char InChar)
        {
            switch (InChar)
            {
                case '"': return Help_SymbolsID.Quotes;
                case '\'': return Help_SymbolsID.Apostrophe;
                case ',': return Help_SymbolsID.Comma;
                case '.': return Help_SymbolsID.Dot;
                case '[': return Help_SymbolsID.LSqrBracket;
                case ']': return Help_SymbolsID.RSqrBracket;
                default: return Help_SymbolsID.NaN;
            }
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
