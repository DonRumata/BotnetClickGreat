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
    class FirstParser
    {
        private List<AnyFunction> FuncStorage = null;
        private List<Variable> VarStorage = null;
        private string Text = null;

        public void PARSETEXT(List<AnyFunction>InFunc, List<Variable>InVar, string Input_Text)
        {
            FuncStorage = InFunc;
            VarStorage = InVar;
            Text = Input_Text;
            char nowchar;
            int i = 0;
            while (i!=Input_Text.Length)
            {
                nowchar = Input_Text[i];
                
            }
           
            
        }

        private int While_delegate_function(Func<char, bool> Cycle_condition, ETypeChar second_cycle_condition, int i_counter, ETypeChar previous_ID, List<Token> Word_list, string input_text, int row_count)
        /* Делегирует функцию, для сокращения кода похожих циклов While в коде*/
        {
            int helper_counter = i_counter;
            string Data_former = "";
            while ((helper_counter != input_text.Length) && (Cycle_condition(input_text[helper_counter])))  //Проходит циклом по тексту и формирует строку, пока входящая функция удовлетворяет второму условию
            {
                Data_former += input_text[helper_counter];
                helper_counter++;
            }
            Word_list.Add(new Token(Data_former, previous_ID == 0, second_cycle_condition, row_count, i_counter, helper_counter - 1)); //Формирует и добавляет заготовку токена в список
            return helper_counter - 1;
        }

        private ETypeChar Alphabet_check(char nowchar_f)  //Проверяет является ли текущий символ, кириллицей или латиницей.
        {
            if (((nowchar_f >= 'a') && (nowchar_f <= 'z')) || ((nowchar_f >= 'A') && (nowchar_f <= 'Z')))
                return ETypeChar.alpha;
            else
                if ((nowchar_f >= 'А') && ((nowchar_f >= 'Я')) || (nowchar_f >= 'а') && ((nowchar_f >= 'я')) || (nowchar_f >= 'ё') || (nowchar_f >= 'Ё'))  //Потенциальное БАГОДЕРЬМО, не удовлетворяет значениям кириллицы в кодировке юникода, нужно сделать отдельную проверку для кириллицы
                return ETypeChar.alphaRus;
            else
                return ETypeChar.unknown;
        }

        private ETypeChar getTypeChar(char c)
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
                    return 
            }

        private void GetToken()
        {

            switch (nowCharId)
            {
                case ETypeChar.arifm:
                case ETypeChar.digit:
                case ETypeChar.alpha:
                case ETypeChar.alphaRus:
                case ETypeChar.logical:
                case ETypeChar.other:
                    i = While_delegate_function(c => getTypeChar(c) == nowCharId, nowCharId, i, Previous_char_ID, List_of_Words, Input_text, Row_Counter);  //Формирует из арифметических символов строку, которая затем записывается как отдельный токен, даже в том случае если перед ней не шел пробел, однако это указывается отдельно
                    break;
            }

        }
    }
}
