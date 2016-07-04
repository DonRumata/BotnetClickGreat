using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tokens_Library;
using Exceptions_Library;



namespace Parsers
{

    public class MainProgram
    /* Основной класс который хранит главные атрибуты программы
    - Количество открытых/закрытых begin/end
    - Местоположение последнего открытого begin
    - Общее количество строк
    - Номер анализируемой строки(внешне данные не нужны)
    - Dictionary объявленных в программе переменных(string type, object value)
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
        protected string Program_text;
        protected List<string> First_translation_result;
        protected Dictionary<string, AnyFunction> Program_interpretation = new Dictionary<string, AnyFunction>();
        private Dictionary<int, Global_Variable> Variable_storage;
        private Dictionary<int, User_Function> User_function_storage;
        private Dictionary<int, Message> Output_data;



        /* Функции и методы */
        /*
        public delegate void Anyfunction();

        public void Translation(string Input_text) //Нужно подумать переносить ли сюда все из Form1.
        {
            var d=Anyfunction.CreateDelegate(typeof(MainProgram).GetMethod("save_in_file").GetType(), typeof(MainProgram).GetMethod("save_in_file"));
            Anyfunction p = new Anyfunction(Save_in_file);
        }
        */

        public MainProgram()
            /*Конструктор, выполняет роль инициализатора, инициализирует и проверяет инициализацию всех переменных.*/
        {
            Num_of_right_BeginEnds = 0;
            Num_of_last_open_begin = 0;
            Num_of_rows = 0;
            Num_of_analyse_string = 0;
            Program_text = "";
        }

        private bool Initialize_translaters()
        {
            /*Инициализирует транслятор и словари и списки привязанные к трансляции, проверяет их существование.*/
            First_translation_result = new List<string>();
            Variable_storage = new Dictionary<int, Global_Variable>();
            User_function_storage = new Dictionary<int, User_Function>();
            Program_interpretation = new Dictionary<string, AnyFunction>();
            if ((First_translation_result != null) && (Variable_storage != null) && (User_function_storage != null) && (Program_interpretation != null))
                return true;
            else
                return false;
        }

        private void Initialize_interpretators() //Будет инициазилировать интерпретатор пока что в стадии заглушки
        {

        }

        public void Translate_program(string input_program_text)
        {

            Program_text = input_program_text;
            First_char_parser First_parser=new First_char_parser();
            Translation_word_parser Translater = new Translation_word_parser();
            Translater.Begin_translating(First_parser.Parse_first_text(input_program_text),Variable_storage,User_function_storage);
        }

        public void Save_in_file() { } //Заглушка на сохранение в файлы

        public void Load_from_file() { } //Заглушка на загрузку из файла


    }


    class Message:MyAppException
    {
        protected string message = "";

    }

    class Word  //Класс необходимый для формирования списка токенов, представляет собой по факту описание токена.
    {
        private string Data, Number_range;
        private bool Space_check;
        private int ID,Row;
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

        public int Get_ID()
        {
            return this.ID;
        }

        public int Get_row_count()
        {
            return this.Row;
        }
        public void change_data(string newdata)  //Метод для изменения поля Data
        {
            this.Data = newdata;
        }
        /* ДАННЫЕ ФУНКЦИИ НИГДЕ НЕ ИСПОЛЬЗУЮТСЯ!
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
        */
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
                                        List_of_Words.Add(new Word(",", Helper, 15, Row_Counter, i, i));
                                        Previous_char_ID = 15;
                                        break;
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
                        if ((Previous_char_ID == 9) && (List_of_Words[List_of_Words.Count - 2].Get_ID() == 4))
                        {
                            int help_counter = List_of_Words.Count - 2;
                            /* По сути данная функция формирует десятичную дробь, если предыдущим символом была точка, а перед ней шло число
                              В общем и целом позволяет избежать дальнейших исправлений списка. */
                            List_of_Words[help_counter].change_data(List_of_Words[help_counter + 1].get_data() + List_of_Words[help_counter + 2].get_data());
                            List_of_Words.RemoveRange(help_counter + 1, 1); //ПОТЕНЦИАЛЬНОЕ БАГОДЕРЬМО, НУЖНО ОТТРАССИРОВАТЬ И ИСПРАВИТЬ ЗНАЧЕНИЯ!
                        }
                        else if ((Previous_char_ID ==11)||(Previous_char_ID==8))
                        {
                            /*Если число идет после кириллицы или латиницы без пробела, то формирует из этого единую строку и помечает эту строку как возможное имя переменной*/
                            List_of_Words[List_of_Words.Count - 1].change_data(List_of_Words[List_of_Words.Count - 1].get_data() + List_of_Words.Last().get_data());
                            List_of_Words.RemoveAt(List_of_Words.Count);
                        }
                            
                        Previous_char_ID = 4;
                    }
                }
                else
                {
                    i = While_delegate_function(Arifm_check, 1, i, Previous_char_ID, List_of_Words,Input_text,Row_Counter);  //Формирует из арифметических символов строку, которая затем записывается как отдельный токен, даже в том случае если перед ней не шел пробел, однако это указывается отдельно
                    if (List_of_Words.Last().get_data() == "--")  //Отдельно меняет -- на + для избежания дальнейших ошибок в компиляции.
                        List_of_Words.Last().change_data("+");
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
            while ((helper_counter!=input_text.Length)&&(Cycle_condition(input_text[helper_counter]) == second_cycle_condition))  //Проходит циклом по тексту и формирует строку, пока входящая функция удовлетворяет второму условию
            {
                Data_former += input_text[helper_counter];
                helper_counter++;
            }
            Word_list.Add(new Word(Data_former,previous_ID==0,second_cycle_condition,row_count,i_counter,helper_counter-1)); //Формирует и добавляет заготовку токена в список
            return helper_counter-1;
        }
    }

    class Translation_word_parser 
    /*Основной парсер трансляции, принимает на вход список слов, подготовленных First_char_parser и превращает их в список токенов готовых к интерпретации. 
    Семантический анализ проводится построково отдельно уже с готовыми токенами!
    */
    {
        //ПОТЕНЦИАЛЬНОЕ БАГОДЕРЬМО, НУЖНО ПРОВЕРИТЬ ЗАДАНИЕ ЗНАЧЕНИЙ!
        HashSet<string> Basic_types = new HashSet<string>() { "int", "float", "double", "point", "var", "picture" };
        HashSet<string> Basic_structure_commands = new HashSet<string>() { "if", "while", "for", "function", "{", "}", "begin", "end", "program", "repeat", "until", "do" };
        HashSet<string> Ariphmetical_strings = new HashSet<string>() { "=", "-", "+", "*", "/", "+=", "-=", "*=", "/=", "%", "^", "=+", "=-", "=*", "=/" };
        HashSet<string> Ariphmetical_functions = new HashSet<string>() { "mod", "div", "round", "sqr", "sqrt", "log", "integr", "sin", "cos", "tan", "abs", "acos", "asin", "atan", "fabs", "pow" };
        HashSet<string> Logical_strings = new HashSet<string>() { ">", "<", "==", "<=", "!=", ">=", "><" };
        //static Dictionary<string, User_Function> User_function_storage = new Dictionary<string, User_Function>();
        
        /* Определения Hashset сверху-вниз
        Basic_types - содержит все названия/имена типов, которые возможно применять в программе.
        Basic_structure_commands - содержит все ключевые слова, имеющие отношение к базовой структуре программы.
        Ariphmetical_strings - содержит все арифметические символики, которые возможно применить при рассчетах внутри программы.
        Ariphmetical_functions - содержит все арифметические функции, которые выполняют некие арифметические расчеты и предназначены для взаимодействия с числами
        Logical_strings - содержит все логические символикиЮ которые возможно применить для логических расчетов и условий внутри программы.
        User_function_storage - хранит в себе все транслированные пользовательские функции.
        */

        public Hashtable Begin_translating(List<Word> input_list, Dictionary<int,Global_Variable> VStorage, Dictionary<string,User_Function> UFStorage)
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
                        Ariphmetical_translation(input_list, i, VStorage, UFStorage, false);
                        break;
                }
                i++;
            }
            return Result;
        }

        private bool Row_control_check(List<Word> input_list_word, int count)  //Проводит проверку на соответствие строк предыдущего и следующего слова.
        {
            return (input_list_word[count + 1].Get_row_count() == input_list_word[count].Get_row_count());
        }

        private bool Row_control_check(List<Word> input_list_word, int count, int Range) //Проводит проверку на соответствие строк диапазона слов, задаваемого range.
        {
            bool NFailed = true;
            int end_cycler = count + Range - 1;
            while ((count!=end_cycler)&&(NFailed))
            {
                NFailed = (input_list_word[count].Get_row_count() == input_list_word[count + 1].Get_row_count());
                count++;
            }
            return NFailed;
        }

        private int Priority_of_word(string Word_data)
        {
            switch (Word_data)
            {
                case "(":
                    return 0;
                case ")":
                    return 1;
                case "+=":
                case "+":
                    return 2;
                case "-=":
                case "-":
                    return 3;
                case "*=":
                case "*":
                    return 4;
                case "/=":
                    goto case "/";
                case "/":
                    return 4;
                case "^":
                    return 5;
                case "%":
                    return 6;
                case "--":
                    goto case "+";
                default:
                    return 7; //Возможно необходимо будет исправление.
            }
        }

        private bool Is_definition_ariphmetical_allowed(string data) //Проверяет удовлетворяет ли строка требованиям к допустимым арифметическим символикам при обьявлении переменной.
        {
            char[] oh_shi = data.ToCharArray(); 
            return (((oh_shi[0] == '=') || (oh_shi[1] == '=')) && (Is_ariphmetical_symbol(data)));
        }

        private bool Is_basic_function(string word_data)
        {
            return false;
            //return typeof(MainProgram).GetMethod("get_variable_storage", args);
        }

        private bool Is_variable(string word_data, Dictionary<string,Variable> VStorage) //Проверяет содержится ли строка в пространстве имен пользовательских переменных
        {
            return VStorage.ContainsKey(word_data);
        }

        private bool Is_user_function(string word_data, Dictionary<string,User_Function> User_function_storage) //Проверяет содержится ли строка в пространстве имен пользовательских функций
        {
            return User_function_storage.ContainsKey(word_data);
        }

        private bool Is_prodigy_function(string word_data)
        {
            return false;
        }

        private bool Is_fraction(string word_data)
        {
            return false;
        }

        private bool is_ariphm_delimeter(string word_data)
        {
            return word_data == ",";
        }

        private bool Is_delimeter(string word_data)
        {
            return word_data == "=";
        }

        private bool Is_structure_function(string word_data)
        {
            return Basic_structure_commands.Contains(word_data);
        }

        private bool Is_logical_symbol(string word_data)
        {
            return Logical_strings.Contains(word_data);
        }

        private bool Is_type_definition(string word_data)
        {
            return Basic_types.Contains(word_data);
        }

        private bool Is_normal_for_name(string word_data)
        {
            return false;
        }

        private bool Is_ariphmetical_symbol(string word_data) //Проверяет является ли слово приемлемым арифметическим символом
        {
            return Ariphmetical_strings.Contains(word_data);
        }

        private bool Is_ariphmetical_function(string word_data) //Проверяет является ли слово приемлимой арифметической функцией
        {
            return Ariphmetical_functions.Contains(word_data);
        }
        
        private bool Is_any_function(string data)
        {
            return ((Is_ariphmetical_function(data)) || (Is_basic_function(data)) || (Is_prodigy_function(data)));
        }

        private bool equality_with_the_row_check(int equaler, List<Word> Word_list, int counter, out int err_code)
        {
            if (equaler != Word_list[counter].Get_ID())
            {
                err_code = 1;
                return false;
            }
            else if (Word_list[counter].Get_row_count() != Word_list[counter + 1].Get_row_count())
            {
                err_code = 2;
                return false;
            }
            else
            {
                err_code = 0;
                return true;
            }
        }

        private void Type_calc()
        {

        }

        private string Get_type_from_UF(string Func_name, Dictionary<string, User_Function> Ustorage)
        {
            User_Function Temp_USERFUNCTION;
            if (Ustorage.TryGetValue(Func_name, out Temp_USERFUNCTION))
                return Temp_USERFUNCTION.GetType();
            else
                throw new StackOverflowException();//Заменить на создание собственного Exception о невозможности определить тип.
        }

        private string Get_type_from_Any_function(string Func_name,Dictionary<string,BuiltIn_Function> Fstorage)
        {
            BuiltIn_Function Temp_BUILTINFUNCTION;
            if (Fstorage.TryGetValue(Func_name, out Temp_BUILTINFUNCTION))
                return Temp_BUILTINFUNCTION.GetType();
            else
                throw new StackOverflowException();//Заменить на создание собственного Exception о невозможности определить тип.
        }

        private string Type_getter(string Input_word, int ID)
        {
            int IFINT = 0;
            float IFFLOAT = 0;
            double IFDOUBLE = 0;
            Dictionary<string, User_Function> temps = new Dictionary<string, User_Function>(); //ЗАМЕНИТЬ НА ПРИВЯЗКУ К ГЛОБАЛЬНОЙ!
            Dictionary<string, BuiltIn_Function> temps2 = new Dictionary<string, BuiltIn_Function>(); //ЗАМЕНИТЬ НА ПРИВЯЗКУ К ГЛОБАЛЬНОЙ!
            switch (ID)
            {
                case 4:
                    if (int.TryParse(Input_word, out IFINT))
                        return "int";
                    else if (float.TryParse(Input_word, out IFFLOAT))
                        return "float";
                    else if (double.TryParse(Input_word, out IFDOUBLE))
                        return "double";
                    else return "anytype";
                case 8:
                    if (Is_any_function(Input_word))
                        return BuiltIn_Function
            }
        }

        private string Type_getter(Word Input_word)
        {
            int IFINT = 0;
            float IFFLOAT = 0;
            double IFDOUBLE = 0;
            Dictionary<string, User_Function> temps = new Dictionary<string, User_Function>(); //ЗАМЕНИТЬ НА ПРИВЯЗКУ К ГЛОБАЛЬНОЙ!
            Dictionary<string, BuiltIn_Function> temps2 = new Dictionary<string, BuiltIn_Function>(); //ЗАМЕНИТЬ НА ПРИВЯЗКУ К ГЛОБАЛЬНОЙ!
            string nowdata = Input_word.get_data();
            
            switch (Input_word.Get_ID())
            {
                case 4:
                    if (int.TryParse(Input_word.get_data(), out IFINT))
                        return "int";
                    else if (float.TryParse(Input_word.get_data(), out IFFLOAT))
                        return "float";
                    else if (double.TryParse(Input_word.get_data(), out IFDOUBLE))
                        return "double";
                    else
                        return "anytype"; //Создавать ошибку о неизвестном числовом типе.
                case 8:
                    if (Is_type_definition(nowdata))
                        return nowdata;
                    else if (Is_any_function(nowdata))
                        return "";
                    else if (Is_user_function(nowdata, temps))  //зАМЕНИТЬ TEMPS НА ПРИВЯЗКУ К ГЛОБАЛЬНОЙ!
                        return Get_type_from_UF(nowdata, temps);
                    else
                        return "anytype"; //Создавать ошибку о неизвестном типе функции.
                default:
                    return "anytype";  //Создавать ошибку о неизвестном типе функции.
            }
        }

        private bool equality_with_the_row_check(string equaler, List<Word> Word_list,int counter)
        {
            return ((Word_list[counter].Get_row_count() == Word_list[counter + 1].Get_row_count()) && (Word_list[counter + 1].get_data() == equaler));
        }

        private bool equality_with_the_row_check(Func<string,bool> equaler, List<Word> Word_list, int counter)
        {
            return ((Word_list[counter].Get_row_count() == Word_list[counter + 1].Get_row_count()) && (equaler(Word_list[counter+1].get_data())));
        }

        private bool equality_with_the_row_check(Func<string,bool> equaler, List<Word> Word_list, int counter, bool space_check, out int num_of_error)
        {
            if (!equaler(Word_list[counter + 1].get_data()))
                num_of_error = 1;
            else if ((!Word_list[counter + 1].get_space_check())&&(space_check))
                num_of_error = 2;
            else
                num_of_error = 0;
            return ((Word_list[counter].Get_row_count() == Word_list[counter + 1].Get_row_count()) && (equaler(Word_list[counter + 1].get_data()))&&(Word_list[counter+1].get_space_check()));
        }

        /* Небольшие эксперименты над функциями contains и функциями
        для определения типа.
        private bool contains (int[] input_array, int value_cont)
        {
            return input_array.Contains(value_cont);
        }

        private bool IsContainsOperator(int Word_ID)
        {
            return (new int[] {1,2,3,4 }.Contains(Word_ID))
        }
        */

        private void If_construction_translation(List<Word> List_word, int counter, Dictionary<int, Variable> Var_storage, HashSet<Argument> Args, Dictionary<string, User_Function> UF_storage)
        {
            int error_num = -1;
            bool cycle_stop = false;
            if (equality_with_the_row_check("(", List_word, counter - 1))
            {
                counter++;
                while (!cycle_stop)
                {
                    switch (List_word[counter].Get_ID())
                    {
                        case 4:
                            break;
                    }
                    /*switch (Input_list[counter].Get_ID())
                    {
                        case 4:
                            Output_string.Add(nowdata);
                            break;
                        case 1:
                            if ((Is_ariphmetical_symbol(nowdata)) && (nowdata != "="))
                            {
                                if (Operators_stack.Count > 0)
                                {
                                    if (Priority_of_word(nowdata) <= Priority_of_word(Operators_stack.Peek()))
                                    {
                                        Output_string.Add(nowdata);
                                    }
                                }
                                Operators_stack.Push(nowdata);
                            }
                            else if (nowdata == "=")
                            {
                                if (equality_left)
                                {
                                    /*Нужно подумать что делать с двумя равенствами в одном выражении, как вариант добавлять некую пометку в строку, о том что тут должно быть второе присваивание, возможно знак равенства.*//*
                                }
                                else
                                {
                                    cycle_stop = true; /*Здесь необходимо заканчивать трансляцию и вызывать интерпретацию, поскольку после правостороннего =
                                необходимо вычислять значение выражения слева.*//*
                                }
                            }
                            break;
                        case 2:
                            Operators_stack.Push(Now_word.get_data());
                            break;
                        case 3:
                            nowdata = Operators_stack.Pop();
                            while (nowdata != "(")
                            {
                                Output_string.Add(nowdata);
                                nowdata = Operators_stack.Pop();
                            }
                            break;
                        case 11:
                            Variable Temp_variable_value;
                            /*if (Val_storage.TryGetValue(nowdata, out Temp_variable_value))
                                Temp_variable_value.Get_value();
                            else;*/
                            /*Здесь необходимо создавать исключение о необьявленной переменной.*//*
                            break;
                        case 8:
                            if (Is_ariphmetical_function(nowdata))
                            {
                                Output_string.Add(nowdata);
                                //Нужно подумать над интерпретацией функций.
                            }
                            else if (Is_basic_function(nowdata))
                            {

                            }
                            else if (Is_prodigy_function(nowdata))
                            {
                                //Здесь необходимо проверять и завершать работу транслятора арифметических выражений.
                            }
                            else if (Is_structure_function(nowdata))
                            {
                                //Здесь необходимо проверять и завершать работу транслятора арифметических выражений.
                            }
                            else if (Is_type_definition(nowdata))
                            {

                            }
                            else
                            {
                                /*Возможно формирование исключения*//*
                            }
                            break;
                        case 9:

                            break;
                    }*/
                }
            }
            else
                ;//Создавать сообщение об ошибке, "ожидалось "("    

            
        }

        private void cycle_construction_translator()
        {

        }

        private bool UFunction_args_definition_translater(List<Word> List_word, int counter, int function_id, out int count)
            /*Функция транслирует и проверяет форму и семантику записи */
        {
            int error_num = -1;
            bool delimeter = true;
            bool was_error = false;
            HashSet<Argument> Args_hst = new HashSet<Argument>();
            if (List_word[counter].Get_ID() == 2)
                counter++;
            while ((List_word[counter].Get_ID() != 3)&&(!was_error))
            {
                if (equality_with_the_row_check(Is_type_definition,List_word,counter,true, out error_num))
                    switch (error_num)
                    {
                        case 0: /*Создавать ошибку на тему row_count, несоответствие строки*/
                            break;
                        case 1: /*Создавать ошибку на тему аргумента неизвестного типа*/
                            if ((!delimeter)&&(List_word[counter].get_data()==","))
                            {
                                delimeter = true;
                                if (Row_control_check(List_word, counter))
                                    goto case 2;
                            }
                            else
                            {
                                //Создавать ошибку на тему аргумента неизвестного типа.
                            }
                            break;
                        case 2:
                            counter++;
                            //Тут нужно добавить проверку на соответствие имени, возможному вводу и символам.
                            Args_hst.Add(new Argument(List_word[counter - 1].get_data(), List_word[counter].get_data(), function_id));
                            if (Row_control_check(List_word, counter))
                            {
                                counter++;
                                delimeter = false;
                            }
                            else
                            {
                                //Создавать ошибку на row_count, несоответствие строки.
                            }
                            break;
                        default:/*Создавать ошибку об ошибке трансляции аргументов*/
                            break;
                    }
            }
            if (!was_error)
            {
                count = counter;  //ПОТЕНЦИАЛЬНОЕ БАГОДЕРЬМО, нужно выяснить нужна ли out переменная.
                return true;
            }
            else
            {
                count = counter;
                return false;
            }
        }

        private void Function_body_translator(List<Word> List_word, int counter, Dictionary<int, Variable> Var_storage, HashSet<Argument> Args, Dictionary<string,User_Function> UF_storage)
            /*Функция осуществляет трансляцию тела функции, приводя ее к виду RPN, при вызове функции осуществляется интерпретация RPN строки*/
        {
            bool quiter = false;
            while (!quiter)
            {

            }
        }

        private void Function_definition_translator(List<Word> Input_list_word, int counter, Dictionary<int,Variable> Var_storage, Dictionary<string,User_Function> Uf_storage)
            /*Функция осуществляет трансляцию определения функции и записывает ее в список User_function, */
        {
            string return_type = "";
            string Func_name = "";
            int FuncID=Uf_storage.Count+1;
            int error_num = -1;
            if (equality_with_the_row_check(Is_type_definition, Input_list_word, counter, true,out error_num))
            {
                counter++;
                return_type = Input_list_word[counter].get_data();
            }
            else
            {
                counter++;
                switch (error_num)
                {
                    case 0:/*Создавать ошибку на тему row_count, несоответствие строки*/
                        break;
                    case 1:
                        return_type = "void";
                        Func_name = Input_list_word[counter].get_data();
                        UFunction_args_definition_translater(Input_list_word, counter, FuncID, out counter);
                        break;
                    case 2:/*Создавать ошибку на тему space_check, отсутствие пробела*/
                        break;
                    default:
                        /*Создавать ошибку об ошибке трансляции.*/
                        break;
                }
            }

        }
        
        
        private void Non_local_Args_translator(string Call_function_name, List<Word> Input_list_word, int counter, Dictionary<int,Variable> Var_storage, Dictionary<string,User_Function> Uf_storage)
            /*Пока что преобразовывается в заглушку, ибо приоритет меньше чем у транслятора обьявления функций*/
        {
            if (Is_any_function(Call_function_name))
            {
                
            }
        }

        private void RPN_translation(List<Word> Input_list, int counter, Dictionary<int,Global_Variable> Var_storage, Dictionary<string,User_Function> User_func_storage)
            /*Универсальная функция трансляции через метод обратной польской записи, преобразует входящий текст в RPN строку*/
        {
            List<string> Output_string = new List<string>();
            Stack<string> Operators_stack = new Stack<string>();
            string nowdata = "";
            bool row_check_result = false;
            bool cycle_stop = false;
            Word Now_word = new Word();
            while (!cycle_stop)
            {
                row_check_result = Row_control_check(Input_list, counter);
                Now_word = Input_list[counter];
                nowdata = Now_word.get_data();
                switch (Input_list[counter].Get_ID())
                {
                    case 4:
                        Output_string.Add(nowdata);
                        break;
                }
            }
        }

        private void Ariphmetical_translation(List<Word> Input_list, int counter, Dictionary<int,Global_Variable> Val_storage, Dictionary<string,User_Function> User_func_storage, bool Was_equality) 

            /*Функция служит для трансляции арифметических выражений методом обратной польской записи
            сначала он формирует строку, а затем уже интерпретирует ее получая результат, ВАЖНО то что в этой функции
            не идет никакой интерпретации, здесь протекает исключительно подготовка строки, интерпретация выполняется другим методом.*/
        {
            List<string> Output_string = new List<string>();
            Stack<string> Operators_stack = new Stack<string>();
            string nowdata = "";
            bool row_check_result = false;
            bool cycle_stop = false;
            bool equality_left = Was_equality;
            Word Now_word = new Word();
            counter = counter - 1;
            while (!cycle_stop)
            {
                row_check_result = Row_control_check(Input_list, counter);
                Now_word = Input_list[counter];
                nowdata = Now_word.get_data();
                switch(Input_list[counter].Get_ID())
                {
                    case 4:
                        Output_string.Add(nowdata);
                    break;
                    case 1:
                        if ((Is_ariphmetical_symbol(nowdata))&&(nowdata!="="))
                        {
                            if (Operators_stack.Count>0)
                            {
                                if (Priority_of_word(nowdata)<=Priority_of_word(Operators_stack.Peek()))
                                {
                                    Output_string.Add(nowdata);
                                }
                            }
                            Operators_stack.Push(nowdata);
                        }
                        else if (nowdata=="=")
                        {
                            if(equality_left)
                            {
                                /*Нужно подумать что делать с двумя равенствами в одном выражении, как вариант добавлять некую пометку в строку, о том что тут должно быть второе присваивание, возможно знак равенства.*/
                            }
                            else
                            {
                                cycle_stop = true; /*Здесь необходимо заканчивать трансляцию и вызывать интерпретацию, поскольку после правостороннего =
                                необходимо вычислять значение выражения слева.*/
                            }
                        }
                        break;
                    case 2:
                        Operators_stack.Push(Now_word.get_data());
                        break;
                    case 3:
                        nowdata = Operators_stack.Pop();
                        while (nowdata!="(")
                        {
                            Output_string.Add(nowdata);
                            nowdata = Operators_stack.Pop();
                        }
                        break;
                    case 11:
                        Variable Temp_variable_value;
                        /*if (Val_storage.TryGetValue(nowdata, out Temp_variable_value))
                            Temp_variable_value.Get_value();
                        else;*/
                            /*Здесь необходимо создавать исключение о необьявленной переменной.*/
                        break;
                    case 8:
                        if (Is_ariphmetical_function(nowdata))
                        {
                                Output_string.Add(nowdata);
                            //Нужно подумать над интерпретацией функций.
                        }
                        else if(Is_basic_function(nowdata))
                        {

                        }
                        else if(Is_prodigy_function(nowdata))
                        {
                            //Здесь необходимо проверять и завершать работу транслятора арифметических выражений.
                        }
                        else if(Is_structure_function(nowdata))
                        {
                            //Здесь необходимо проверять и завершать работу транслятора арифметических выражений.
                        }
                        else if(Is_type_definition(nowdata))
                        {
                            
                        }
                        else
                        {
                            /*Возможно формирование исключения*/
                        }
                        break;
                    case 9:

                        break;
                }
                counter++;                    
            }
            while (Operators_stack.Count>0)
                Output_string.Add(Operators_stack.Pop());
        }
    }

    
}
