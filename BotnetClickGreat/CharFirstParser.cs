using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotnetClickGreat
{
    class CharFirstParser
    {

        public List<Word> Parse_first_text(string Input_text)
        {
            var List_of_Words = new List<Word>();  //Хранит в себе данные о токенах|словах
            bool Quiter = false, Helper = false, Default_trigger = false;  //Служит для окончания списка, помогает контролировать конец строки для правильной записи внутри Word переменной
            int i = 0, Previous_char_ID = -1, Helper1 = -1, Is_Parenthesis = 0, Row_Counter=0;  //I - счетчик, Previous_char_ID - самая важная переменная, указывает на ID предыдущего символа.
            char nowchar;  //Значение текущего символа
            string help_str = ""; //Вспомогательная строковая переменная
            //Word word_creator = new Word();  //Служит для создания токена, основываясь на поступающих данных
            //HashSet<char> Another_char_set = Hashset_creator(); //Множество Char символов соответствующих ID другие символы. НЕ ИСПОЛЬЗУЕТСЯ!
            while (i!=Input_text.Length)
            {
                nowchar = Input_text[i];
                /*if (nowchar == ' ')) //Проверка символа на пробел
                {
                    if (nowchar == ' ')
                        Previous_char_ID = 0;
                    else
                        Previous_char_ID = 11;
                    word_creator.Plus_data(nowchar);
                }*/
                if ((Arifm_check(nowchar)) == 0)  //Проверка символа на принадлежность к арифметическим операциям
                {
                    if (Numeric_check(nowchar) == 0)  //Если символ был не арифметической операцией, то проверка на принадлежность к цифрам
                    {
                        if ((Alphabet_check(nowchar)) == 0)
                        {
                            Helper = Previous_char_ID == 0;
                            switch (nowchar)   //Отдельно создает заготовки токенов под различные возможные значения,
                            {
                                case '[':
                                    Previous_char_ID = 5;
                                    List_of_Words.Add(new Word("[", Helper, 5, Row_Counter, i, i));
                                    break;
                                case ']':
                                    Previous_char_ID = 6;
                                    List_of_Words.Add(new Word("]", Helper, 6, Row_Counter, i, i));
                                    break;
                                case ';':
                                    List_of_Words.Add(new Word(";", Helper, 7, Row_Counter, i, i));
                                    Previous_char_ID = 7;
                                    break;
                                case '(':
                                    Is_Parenthesis++;
                                    List_of_Words.Add(new Word("(", Helper, 2, Row_Counter, i, i));
                                    Previous_char_ID = 2;
                                    break;
                                case ')':
                                    Is_Parenthesis--;
                                    List_of_Words.Add(new Word(")", Helper, 3, Row_Counter, i, i));
                                    Previous_char_ID = 3;
                                    break;
                                case ',':
                                case '.':
                                    List_of_Words.Add(new Word(nowchar.ToString(), Helper, 9, Row_Counter, i, i));
                                    Previous_char_ID = 9;
                                    break;
                                case '\r':
                                case '\n':
                                    Row_Counter++;
                                    Previous_char_ID = 10;
                                    break;
                                case ' ':
                                    Previous_char_ID = 0;
                                    break;
                                default:
                                    Helper1 = i;
                                    Default_trigger = true;
                                    help_str+=nowchar;
                                    Previous_char_ID = -2;
                                    break;
                            }
                            if (Default_trigger)   //Костыль и потенциальное БАГОДЕРЬМО, нужно придумать как легким путем, без прохождения switch лишний раз, создавать токен под id определяемое положением default.
                            {
                                if (Previous_char_ID!=-2)
                                {
                                    Default_trigger = false;
                                    List_of_Words.Add(new Word(help_str, false, -2, Row_Counter, Helper1, i - 1));
                                    help_str = "";
                                    Helper1 = -1;
                                }
                            }
                        }
                        else
                        {
                            i = While_delegate_function(Alphabet_check, 8, i, Previous_char_ID, List_of_Words,Input_text,Row_Counter); //Формирует из букв слово, которое затем записывается как отдельный токен, даже в том случае если перед словом шел не пробел, однако это указывается отдельно
                            Previous_char_ID = 8;
                        }
                    }
                    else
                    {
                        i = While_delegate_function(Numeric_check, 4, i, Previous_char_ID, List_of_Words,Input_text,Row_Counter);  //Формирует из цифр число, которое затем записывается как отдельный токен, даже в том случае если перед числом шел не пробел, однако это указывается отдельно
                        Previous_char_ID = 4;
                    }
                }
                else
                {
                    i = While_delegate_function(Arifm_check, 1, i, Previous_char_ID, List_of_Words,Input_text,Row_Counter);  //Формирует из арифметических символов строку, которая затем записывается как отдельный токен, даже в том случае если перед ней не шел пробел, однако это указывается отдельно
                    Previous_char_ID = 1;
                }
                i++;
            }
            return List_of_Words;
        }

        private int Arifm_check(char nowchar_f)  //Проверяет входящий Char символ на принадлежность к арифметическим действиям|символам
        {
            if ((nowchar_f == '*') || (nowchar_f == '/') || (nowchar_f == '+') || (nowchar_f == '-') || (nowchar_f == '^') || (nowchar_f == '=') || (nowchar_f == '%'))
                return 1;
            else
                return 0;
        }
        /* Гипотетически служила для проверок внутри switch на принадлежность скобок, входила в Arifm_check, функция непотимальна и в ней нет необходимости.
        unsafe private int Parenthesis_choose(int helper, Word creator, int* is_parenthesis, bool Was_space)  //ПОТЕНЦИАЛЬНОЕ БАГОДЕРЬМО Выстраивает значение контрольной переменной для открытия|закрытия круглых скобок, а также формирует ID и Bool составляющие токена
        {
            creator.change_bool(Was_space);
            if (helper == 1)  //Проверка был ли символ арифметической операцией
            {
                creator.change_id(helper);
            }
            else if (helper == 2) //Проверка был ли символ скобкой
            {
                creator.change_id(helper);
                *is_parenthesis += 1;
            }
            else  //Проверка был ли символ закрывающейся скобкой
            {
                creator.change_id(helper);
                *is_parenthesis -= 1;
            }
            return helper;
        }
        */

        private int Numeric_check(char Nowchar)   //Проверяет является ли текущий символ цифрой.
        {
            if (char.IsDigit(Nowchar))
                return 4;
            else
                return 0;
        }

        /* По сути является копией конструктора Word с устаревшими данными, поскольку она де факт копия конструктора, в ней нет необходимости.
        private Word Word_checkncreator(string Else_word, bool Was_space, int id) //Формирует токен согласно поступившим значениям
        {
            Word result = new Word();
            result.change_id(id);
            result.change_data(Else_word);
            result.change_bool(true, true, Was_space);
            return result;
        }
        */

        /*Также не нужна и не используется, за неоптимальностью и отсутствием необходимости проверять все прочие символы согласно чарсету, и отсутствии самого чарсета.
        private int Another_check(char Nowchar, HashSet<char> Charset)  //Проверяет является ли текущий символ другим символом, основывается на contains hashset, также проверяет еще несколько других символов.
        {
            if (Charset.Contains(Nowchar))
                return 5;
            else return 0;
        }
        */


        private int Alphabet_check(char Nowchar)  //Проверяет является ли текущий символ, кириллицей или латиницей.
        {
            if (((Nowchar > 96) && (Nowchar < 123)) || ((Nowchar > 64) && (Nowchar < 91)))
                return 8;
            else
                if ((Nowchar > 191) && (Nowchar < 256))
                return 11;
            else
                return 0;
        }

        private int While_delegate_function(Func<char, int> Cycle_condition, int second_cycle_condition, int i_counter, int previous_ID, List<Word> Word_list, string input_text, int row_count)  //Делегирует функцию, для сокращения кода похожих циклов While кода.
        {
            int helper_counter = i_counter;
            string Data_former = "";
            while (Cycle_condition(input_text[helper_counter]) == second_cycle_condition)  //Проходит циклом по тексту и формирует строку, пока входящая функция удовлетворяет второму условию
            {
                Data_former += input_text[helper_counter];
                helper_counter++;
            }
            Word_list.Add(new Word(Data_former,previous_ID==0,second_cycle_condition,row_count,i_counter,helper_counter)); //Формирует и добавляет заготовку токена в список
            return helper_counter;
        }
    }

    class LexicalSintaParser
    {
        public Hashtable Start_parse_List(List<Word> Word_List) //Функция начинающая парсинг листа слов, подготовленных первоначальным парсером
        {
            Hashtable trulala = new Hashtable();
            return trulala;
        }
    }
}
