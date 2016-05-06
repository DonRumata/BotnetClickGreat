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
        private Dictionary<string, Variable> Variable_storage;
        private Dictionary<string, User_Function> User_function_storage;
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
            Variable_storage = new Dictionary<string, Variable>();
            User_function_storage = new Dictionary<string, User_Function>();
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
            Translater.Begin_translating(First_parser.Parse_first_text(input_program_text));
        }

        public void Save_in_file() { } //Заглушка на сохранение в файлы

        public void Load_from_file() { } //Заглушка на загрузку из файла


    }

    class Variable
    {
        protected string Val_type;
        protected object Value;

        public Variable(string type, object value, string name)
        {
            Val_type = type;
            Value = value;
        }

        public object Get_value()
        {
            return Value;
        }

        public string Get_val_type()
        {
            return Val_type;
        }
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
                            /* По сути данная функция формирует десятичную дробь, если предыдущим символом была точка или запятая, а перед ней шло число
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
        
        /* Определения Hashset сверху-вниз
        Basic_types - содержит все названия/имена типов, которые возможно применять в программе.
        Basic_structure_commands - содержит все ключевые слова, имеющие отношение к базовой структуре программы.
        Ariphmetical_strings - содержит все арифметические символики, которые возможно применить при рассчетах внутри программы.
        Ariphmetical_functions - содержит все арифметические функции, которые выполняют некие арифметические расчеты и предназначены для взаимодействия с числами
        Logical_strings - содержит все логические символикиЮ которые возможно применить для логических расчетов и условий внутри программы.
        */

        public Hashtable Begin_translating(List<Word> input_list, Dictionary<string,Variable> VStorage, Dictionary<string,User_Function> UFStorage)
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
                        Ariphmetical_translation(input_list,i,)
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

        private bool Is_ariphmetical_symbol(string word_data) //Проверяет является ли слово приемлемым арифметическим символом
        {
            return Ariphmetical_strings.Contains(word_data);
        }

        private bool Is_ariphmetical_function(string word_data) //Проверяет является ли слово приемлимой арифметической функцией
        {
            return Ariphmetical_functions.Contains(word_data);
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

        private void Ariphmetical_translation(List<Word> Input_list_word, int counter, Dictionary<string,Variable> Val_storage, Dictionary<string,User_Function> User_func_storage, bool Was_equality) 

            /*Функция служит для трансляции арифметических выражений методом обратной польской записи
            сначала он формирует строку, а затем уже интерпретирует ее получая результат, важно то что в этой функции
            не идет никакой интерпретации, здесь протекает исключительно подготовка строки, трансляция выполняется другим методом.*/
        {
            List<string> Output_string = new List<string>();
            Stack<string> Operators_stack = new Stack<string>();
            string nowdata = "";
            bool row_check_result = false;
            bool cycle_stop = false;
            int priority_value = 0;
            int function_insider = 0;
            bool function_opened = false;
            bool double_combination = false;
            bool equality_left = Was_equality;
            Word Now_word = new Word();
            counter = counter - 1;
            while (!cycle_stop)
            {
                row_check_result = Row_control_check(Input_list_word, counter);
                Now_word = Input_list_word[counter];
                nowdata = Now_word.get_data();
                switch(Input_list_word[counter].Get_ID())
                {
                    case 4:
                        Output_string.Add(nowdata);
                    break;
                    case 1:
                        if ((Is_ariphmetical_symbol(nowdata))&&(nowdata!="="))
                        {
                            if (Operators_stack.Count>0)
                            {
                                if (priority_value+Priority_of_word(nowdata)<=priority_value+Priority_of_word(Operators_stack.Peek()))
                                {
                                    Output_string.Add(nowdata);
                                }
                                Operators_stack.Push(nowdata);
                            }
                        }
                        else if(nowdata=="=")
                        {
                            if(equality_left)
                            {
                                /*Нужно подумать что делать с двумя равенствами в одном выражении*/
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
                        priority_value++;
                        break;
                    case 3:
                        nowdata = Operators_stack.Pop();
                        while (nowdata!="(")
                        {
                            Output_string.Add(nowdata);
                            nowdata = Operators_stack.Pop();
                        }
                        function_insider--;
                        break;
                    case 11:
                        Variable Temp_variable_value;
                        if (Val_storage.TryGetValue(nowdata, out Temp_variable_value))
                            Temp_variable_value.Get_value();
                        else;
                            /*Здесь необходимо создавать исключение о необьявленной переменной.*/
                        break;
                    case 8:
                        function_insider++;
                        User_Function temp_interpretate;
                        if (Is_ariphmetical_function(nowdata))
                        {
                                Output_string.Add(nowdata);
                            //Нужно подумать над интерпретацией функций.
                        }
                        else if(Is_basic_function(nowdata))
                        {

                        }
                        else if (User_func_storage.TryGetValue(nowdata,out temp_interpretate)) /*Здесь необходимо будет вызывать метод интерпретации пользовательской функции, как передавать туда аргументы не знаю, возможно придется делать отдельный транслятор под юзер_функции. */
                        {
                            temp_interpretate.Interpretate(); //Нужно будет проверять возвращаемый функцией тип, в контексте арифметического транслятора он может быть исключительно числовым.
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
