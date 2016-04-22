using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsers
{

    class Word  //Класс необходимый для формирования списка токенов, представляет собой по факту описание токена.
    {
        private string Data, Number_range;
        private bool Space_check;
        private int ID, Row;
        public Word(string data, bool space, int id, int row, int FRange_value, int SRange_value) //Конструктор для создания класса Word, сразу со всеми первоначальными данными
        {
            this.Data = data;
            this.Space_check = space;
            this.ID = id;
            this.Row = row;
            this.Number_range_former(FRange_value, SRange_value);
        }
        public Word()  //Вариант конструктора для простого выделения памяти и создания класса с значениями по умолчанию.
        {
            this.Data = "";
            this.Space_check = false;
            this.ID = 0;
            this.Row = 0;
            this.Number_range = "Undefined";
        }

        public bool get_space_check()
        {
            return this.Space_check;
        }

        public int Get_row_count()
        {
            return this.Row;
        }

        public void change_data(string newdata)  //Метод для изменения поля Data
        {
            this.Data = newdata;
        }
        public void Plus_data(char char_plus)  //Метод для дополнения поля Data символом char
        {
            this.Data += char_plus;
        }
        public void change_id(int id)  //Метод для изменения поля ID
        {
            this.ID = id;
        }
        public void change_bool(bool space)  //Метод для изменения bool полей класса
        {
            this.Space_check = space;
        }
        public string get_all_data(out int id, out int row, out int FRange_value, out int SRange_value) //Позволяет получить все данные о слове.
        {
            id = this.ID;
            row = this.Row;
            FRange_value = Number_range_converter(out SRange_value);
            return this.Data;
        }
        public string get_Prime_data(out int id) //Позволяет ID и Data слова
        {
            id = this.ID;
            return this.Data;
        }
        public string get_data() //Позволяет получить Data значение слова
        {
            return this.Data;
        }

        public void Number_range_former(int First_char_num, int Last_char_num)  //Формирует строку хранящую диапазон номеров символов для текущего слова
        {
            this.Number_range = (First_char_num.ToString() + "-" + Last_char_num.ToString());
        }

        public int Number_range_converter(out int Last_value)  //Возвращает в виде Int значений диапазон номеров символов для текущего слова
        {
            int help_value = Number_range.IndexOf('-');
            Last_value = int.Parse(Number_range.Substring(help_value));
            return int.Parse(Number_range.Substring(0, help_value));
        }
    }
    class First_char_parser
        /* Первичный парсер кода превращает код введенный пользователем в список слов класса Word, выходной результата данного класса это список слов с небольшими помогающими анализаторам пометками. */
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
                            if ((Logical_check(nowchar))==0)
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
                                    case '{':
                                        List_of_Words.Add(new Word(nowchar.ToString(), Helper, 12, Row_Counter, i, i));
                                        Previous_char_ID = 12;
                                        break;
                                    case '}':
                                        List_of_Words.Add(new Word(nowchar.ToString(), Helper, 13, Row_Counter, i, i));
                                        Previous_char_ID = 13;
                                        break;
                                    case ' ':
                                        Previous_char_ID = 0;
                                        break;
                                    default:
                                        Helper1 = i;
                                        Default_trigger = true;
                                        help_str += nowchar;
                                        Previous_char_ID = -2;
                                        break;
                                }
                                if (Default_trigger)   //Костыль и потенциальное БАГОДЕРЬМО, нужно придумать как легким путем, без прохождения switch лишний раз, создавать токен под id определяемое положением default.
                                {
                                    if (Previous_char_ID != -2)
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
                                i = While_delegate_function(Logical_check, 14, i, Previous_char_ID, List_of_Words, Input_text, Row_Counter); //Формирует из символов сравнения слово, которое затем записывается как отдельный токен, даже в том случае если перед словом шел не пробел, однако это указывается отдельно
                                Previous_char_ID = 14;
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
            if ((nowchar_f == '*') || (nowchar_f == '/') || (nowchar_f == '+') || (nowchar_f == '-') || (nowchar_f == '^') || (nowchar_f == '%'))
                return 1;
            else
                return 0;
        }
        private int Logical_check(char nowchar_f) //Проверяет входящий CHar символ на принадлежность к символам сравнений
        {
            if ((nowchar_f == '|') || (nowchar_f == '&') || (nowchar_f == '>') || (nowchar_f == '<') || (nowchar_f == '='))
                return 14;
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

        private int Numeric_check(char nowchar_f)   //Проверяет является ли текущий символ цифрой.
        {
            if (char.IsDigit(nowchar_f))
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


        private int Alphabet_check(char nowchar_f)  //Проверяет является ли текущий символ, кириллицей или латиницей.
        {
            if (((nowchar_f > 96) && (nowchar_f < 123)) || ((nowchar_f > 64) && (nowchar_f < 91)))
                return 8;
            else
                if ((nowchar_f > 191) && (nowchar_f < 256))  //Потенциальное БАГОДЕРЬМО, не удовлетворяет значениям кириллицы в кодировке юникода, нужно сделать отдельную проверку для кириллицы
                return 11;
            else
                return 0;
        }

        private int While_delegate_function(Func<char, int> Cycle_condition, int second_cycle_condition, int i_counter, int previous_ID, List<Word> Word_list, string input_text, int row_count)
            /* Делегирует функцию, для сокращения кода похожих циклов While в коде*/
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

    class Translation_word_parser 
    /*Основной парсер трансляции, принимает на вход список слов, подготовленных First_char_parser и превращает их в список токенов готовых к интерпретации. 
    Семантический анализ проводится построково отдельно уже с готовыми токенами!
    */
    {
        public Hashtable Begin_translating(List<Word> input_list)
        {
            int i = 0;
            int Word_ID = 0;
            string Word_data = "";
            bool quiter = false;
            Word Word_copyr = new Word();
            Hashtable Result = new Hashtable();
            while (i!=input_list.Count)
            {
                Word_data = input_list[i].get_Prime_data(out Word_ID);
                switch (Word_ID)
                {
                    case 1:
                        
                        break;
                }
            }
            return Result;
        }

        private bool Row_control_check(List<Word> input_list_word, int count)  //Проводит проверку на соответствие строк предыдущего и следующего слова.
        {
            if (input_list_word[count + 1].Get_row_count() != input_list_word[count].Get_row_count())
                return false;
            else
                return true;
        }

        private void Ariphmetical_translation(List<Word> Input_list_word, int counter,string word_data)
        {
            bool row_check_result = false;
            bool cycle_stop = false;
            string previous_word_data= Input_list_word[counter - 1].get_data();
            string next_word_data = Input_list_word[counter + 1].get_data();
            while (!cycle_stop)
            {
                row_check_result = Row_control_check(Input_list_word, counter);
                switch (word_data)
                {
                    case "==":

                        break;
                    case "=":

                        break;
                    case "+=":
                    case "+":
                        break;
                    case "-=":
                    case "-":
                        break;
                    case "*=":
                    case "*":
                        break;
                    case "/=":
                    case "/":
                        break;
                    case "^":
                        break;
                    case "%":
                        break;
                    case "--":
                        goto case "+";
                    default:
                        break;
                }
            }
            
        }
    }

    class MainProgram
        /* Основной класс который хранит главные атрибуты программы
        - Количество открытых/закрытых begin/end
        - Местоположение последнего открытого begin
        - Общее количество строк
        - Номер анализируемой строки(внешне данные не нужны)
        - Hashtable объявленных в программе переменных(string type, object value)
        - т.д. 
        Содержит функции вызова трансляции и интерпретации кода.
        Содержит функции сохранения и загрузки кода из файла.
        Содержит функции вызова шифрования.

        КЛАСС И ОПИСАНИЕ НЕ ЗАКОНЧЕННО! */
    {
        /* Переменные и атрибуты*/
        protected int Num_of_right_BeginEnds;
        protected int Num_of_last_open_begin;
        public int Num_of_rows;
        private int Num_of_analyse_string;
        protected Hashtable Variable_storage = new Hashtable();
        
        /* Функции и методы */

        public void Translation(string Input_text) //Нужно подумать переносить ли сюда все из Form1.
        {

        }

        public void Save_in_file() { } //Заглушка на сохранение в файлы

        public void Load_from_file() { } //Заглушка на загрузку из файла
    }
}
