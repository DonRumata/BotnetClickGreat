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
        protected int Num_of_right_BeginEnds = 0;
        protected int Num_of_last_open_begin = 0;
        public int Num_of_rows = 0;
        private int Num_of_analyse_string = 0;
        protected string Program_text = "";

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
            First_char_parser First_parser = new First_char_parser();
            Translation_word_parser Translater = new Translation_word_parser();
            Translater.Begin_translating(First_parser.Parse_first_text(input_program_text), Variable_storage, User_function_storage);
        }

        public void Save_in_file() { } //Заглушка на сохранение в файлы

        public void Load_from_file() { } //Заглушка на загрузку из файла


    }


    class Message : MyAppException
    {
        protected string message = "";

    }

    class Word  //Класс необходимый для формирования списка токенов, представляет собой по факту описание токена.
    {

        public enum ETypeChar       //Типы предтокенов
        {
            space = 0,
            arifm = 1,
            lBracket = 2,
            rBracket = 3,
            digit = 4,
            lSBracket = 5,
            rSBracket = 6,
            dotComma = 7,
            alpha = 8,
            dot = 9,
            newLine = 10,
            alphaRus = 11,
            lFBracket = 12,
            rFBracket = 13,
            logical = 14,
            comma = 15,
            unknown = -1,
            other = -2,

        }

        private string Data;           //сам предтокен
        private Tuple<int, int> Range; //расположение предтокена в строке (номер первого символа, номер последнего символа)
        private bool Space_check;       //флаг наличия пробела до предтокена
        private ETypeChar ID;                 //Номер типа токена
        private int Row;                //Номер строки

        public Word(string data, bool space, ETypeChar id, int row, int FRange_value, int SRange_value) //Конструктор для создания класса Word, сразу со всеми первоначальными данными
        {
            this.Data = data;
            this.Space_check = space;
            this.ID = id;
            this.Row = row;
            this.Range = Tuple.Create(FRange_value, SRange_value);
        }

        public Word()  //Вариант конструктора для простого выделения памяти и создания класса с значениями по умолчанию.
        {
            this.Data = "";
            this.Space_check = false;
            this.ID = 0;
            this.Row = 0;
            this.Range = null;
        }

        public bool get_space_check()
        {
            return this.Space_check;
        }

        public ETypeChar Get_ID()
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
        public string get_all_data(out ETypeChar id, out int row, out int FRange_value, out int SRange_value) //Позволяет получить все данные о слове.
        {
            id = this.ID;
            row = this.Row;
            FRange_value = Range.Item1;
            SRange_value = Range.Item2;
            return this.Data;
        }
        public string get_Prime_data(out ETypeChar id) //Позволяет ID и Data слова
        {
            id = this.ID;
            return this.Data;
        }
        public string get_data() //Позволяет получить Data значение слова
        {
            return this.Data;
        }

    }
    class First_char_parser
    /* Первичный парсер кода превращает код введенный пользователем в список слов класса Word, выходной результата данного класса это список слов с небольшими помогающими анализаторам пометками. */
    {




        private Word.ETypeChar getTypeChar(char c)
        {
            Word.ETypeChar res = Alphabet_check(c);
            if (res != Word.ETypeChar.unknown) return res;
            switch (c)   //Отдельно создает заготовки токенов под различные возможные значения,
            {
                case '\r':
                case '\n':
                    return Word.ETypeChar.newLine;
                case ' ':
                    return Word.ETypeChar.space;
                case '(':
                    return Word.ETypeChar.lBracket;
                case ')':
                    return Word.ETypeChar.rBracket;
                case '[':
                    return Word.ETypeChar.lSBracket;
                case ']':
                    return Word.ETypeChar.rSBracket;
                case ';':
                    return Word.ETypeChar.dotComma;
                case ',':
                    return Word.ETypeChar.comma;
                case '.':
                    return Word.ETypeChar.dot;
                case '{':
                    return Word.ETypeChar.lFBracket;
                case '}':
                    return Word.ETypeChar.rFBracket;
                case '*':
                case '/':
                case '+':
                case '-':
                case '^':
                case '%':
                    return Word.ETypeChar.arifm;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return Word.ETypeChar.digit;
                case '|':
                case '&':
                case '>':
                case '<':
                case '=':
                    return Word.ETypeChar.logical;
                default:
                    return Word.ETypeChar.other;
            }

        }

        public List<Word> Parse_first_text(string Input_text)
        {
            var List_of_Words = new List<Word>();  //Хранит в себе данные о токенах|словах
            int i = 0, Is_Parenthesis = 0, Row_Counter = 0;  //I - счетчик, Previous_char_ID - самая важная переменная, указывает на ID предыдущего символа.
            Word.ETypeChar Previous_char_ID = Word.ETypeChar.unknown;

            //Word word_creator = new Word();  //Служит для создания токена, основываясь на поступающих данных
            //HashSet<char> Another_char_set = Hashset_creator(); //Множество Char символов соответствующих ID другие символы. НЕ ИСПОЛЬЗУЕТСЯ!
            while (i != Input_text.Length)
            {
                char nowchar = Input_text[i]; //Значение текущего символа
                Word.ETypeChar nowCharId = getTypeChar(nowchar);
                switch (nowCharId)
                {
                    case Word.ETypeChar.arifm:
                    case Word.ETypeChar.digit:
                    case Word.ETypeChar.alpha:
                    case Word.ETypeChar.alphaRus:
                    case Word.ETypeChar.logical:
                    case Word.ETypeChar.other:
                        i = While_delegate_function(c => getTypeChar(c) == nowCharId, nowCharId, i, Previous_char_ID, List_of_Words, Input_text, Row_Counter);  //Формирует из арифметических символов строку, которая затем записывается как отдельный токен, даже в том случае если перед ней не шел пробел, однако это указывается отдельно
                        break;
                }

                if (nowCharId == Word.ETypeChar.arifm)  //Проверка символа на принадлежность к арифметическим операциям
                {
                    if (List_of_Words.Last().get_data() == "--")  //Отдельно меняет -- на + для избежания дальнейших ошибок в компиляции.
                        List_of_Words.Last().change_data("+");

                }
                else if (nowCharId == Word.ETypeChar.digit)  //Если символ был не арифметической операцией, то проверка на принадлежность к цифрам
                {

                    if ((Previous_char_ID == Word.ETypeChar.dot) && (List_of_Words.Count > 1) && (List_of_Words[List_of_Words.Count - 2].Get_ID() == Word.ETypeChar.digit))
                    {
                        int help_counter = List_of_Words.Count - 2;
                        /* По сути данная функция формирует десятичную дробь, если предыдущим символом была точка, а перед ней шло число
                          В общем и целом позволяет избежать дальнейших исправлений списка. */
                        List_of_Words[help_counter].change_data(List_of_Words[help_counter + 1].get_data() + List_of_Words[help_counter + 2].get_data());
                        List_of_Words.RemoveRange(help_counter + 1, 1); //ПОТЕНЦИАЛЬНОЕ БАГОДЕРЬМО, НУЖНО ОТТРАССИРОВАТЬ И ИСПРАВИТЬ ЗНАЧЕНИЯ!
                    }
                    else if ((Previous_char_ID == Word.ETypeChar.alphaRus) || (Previous_char_ID == Word.ETypeChar.alpha))
                    {
                        /*Если число идет после кириллицы или латиницы без пробела, то формирует из этого единую строку и помечает эту строку как возможное имя переменной*/
                        List_of_Words[List_of_Words.Count - 1].change_data(List_of_Words[List_of_Words.Count - 1].get_data() + List_of_Words.Last().get_data());
                        List_of_Words.RemoveAt(List_of_Words.Count);
                    }

                }
                else if ((nowCharId != Word.ETypeChar.alpha)&& (nowCharId != Word.ETypeChar.alphaRus) && (nowCharId != Word.ETypeChar.logical))
                {
                    bool Helper = Previous_char_ID == Word.ETypeChar.space;


                    //TODO слово, после которого нет символов не обработается
                    switch (nowCharId)
                    {
                        case Word.ETypeChar.lBracket: ++Is_Parenthesis; break;
                        case Word.ETypeChar.rBracket: --Is_Parenthesis; break;
                    }
                    switch (nowCharId)   //Отдельно создает заготовки токенов под различные возможные значения,
                    {
                        case Word.ETypeChar.newLine:
                            Row_Counter++;
                            break;
                        case Word.ETypeChar.lBracket:
                        case Word.ETypeChar.rBracket:
                        case Word.ETypeChar.lSBracket:
                        case Word.ETypeChar.rSBracket:
                        case Word.ETypeChar.lFBracket:
                        case Word.ETypeChar.rFBracket:
                        case Word.ETypeChar.dotComma:
                        case Word.ETypeChar.dot:
                        case Word.ETypeChar.comma:
                            List_of_Words.Add(new Word(nowchar.ToString(), Helper, nowCharId, Row_Counter, i, i));
                            break;
                    }



                }


                Previous_char_ID = nowCharId;
                i++;
            }
            return List_of_Words;
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


        private Word.ETypeChar Alphabet_check(char nowchar_f)  //Проверяет является ли текущий символ, кириллицей или латиницей.
        {
            if (((nowchar_f >= 'a') && (nowchar_f <= 'z')) || ((nowchar_f >= 'A') && (nowchar_f <= 'Z')))
                return Word.ETypeChar.alpha;
            else
                if ((nowchar_f >= 'А') && ((nowchar_f >= 'Я')) || (nowchar_f >= 'а') && ((nowchar_f >= 'я')) || (nowchar_f >= 'ё') || (nowchar_f >= 'Ё'))  //Потенциальное БАГОДЕРЬМО, не удовлетворяет значениям кириллицы в кодировке юникода, нужно сделать отдельную проверку для кириллицы
                return Word.ETypeChar.alphaRus;
            else
                return Word.ETypeChar.unknown;
        }

        private int While_delegate_function(Func<char, bool> Cycle_condition, Word.ETypeChar second_cycle_condition, int i_counter, Word.ETypeChar previous_ID, List<Word> Word_list, string input_text, int row_count)
        /* Делегирует функцию, для сокращения кода похожих циклов While в коде*/
        {
            int helper_counter = i_counter;
            string Data_former = "";
            while ((helper_counter != input_text.Length) && (Cycle_condition(input_text[helper_counter])))  //Проходит циклом по тексту и формирует строку, пока входящая функция удовлетворяет второму условию
            {
                Data_former += input_text[helper_counter];
                helper_counter++;
            }
            Word_list.Add(new Word(Data_former, previous_ID == 0, second_cycle_condition, row_count, i_counter, helper_counter - 1)); //Формирует и добавляет заготовку токена в список
            return helper_counter - 1;
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

        public Hashtable Begin_translating(List<Word> input_list, Dictionary<int, Global_Variable> VStorage, Dictionary<string, User_Function> UFStorage)
        {
            int i = 0;
            Word.ETypeChar Word_ID = 0;
            string Word_data = "";
            bool quiter = false;
            Word Word_copyr = new Word();
            Hashtable Result = new Hashtable();
            Dictionary<string, Variable> Variable_storage = new Dictionary<string, Variable>();
            Dictionary<string, AnyFunction> Function_storage = new Dictionary<string, AnyFunction>();


            while (i != input_list.Count)
            {
                Word_data = input_list[i].get_Prime_data(out Word_ID);
                switch (Word_ID)
                {
                    case Word.ETypeChar.arifm:
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
            while ((count != end_cycler) && (NFailed))
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
                case "--":
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

        private bool Is_variable(string word_data, Dictionary<string, Variable> VStorage) //Проверяет содержится ли строка в пространстве имен пользовательских переменных
        {
            return VStorage.ContainsKey(word_data);  //ЗАМЕНИТЬ VSTORAGE НА УНИВЕРСАЛЬНОЕ ХРАНИЛИЩЕ ИЛИ ПРИВЯЗКУ К ГЛОБАЛЬНОЙ!
        }

        private bool Is_user_function(string word_data, Dictionary<string, User_Function> User_function_storage) //Проверяет содержится ли строка в пространстве имен пользовательских функций
        {
            return User_function_storage.ContainsKey(word_data);
        }

        private bool Is_prodigy_function(string word_data)
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
            return Enumerable.Concat(Ariphmetical_functions, Basic_structure_commands).Contains(data);
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

        private string Type_compability(string first_value, string second_value)
        {
            if (first_value ==)
                return "";
        }

        private string Type_calc(List<string> input_string)
        /*Функция частичной интерпретации, из входящей строки типов считает выходящий тип, рассчитывает арифметические и логические действия над типами*/
        {
            string resulter = "";
            Stack<string> temp_stack = new Stack<string>();
            bool cycle_stop = false;
            int counter = 0;
            string nowdata;
            while (!cycle_stop)
            {
                nowdata = input_string[counter];
                if (Is_ariphmetical_symbol(nowdata))
                {
                    string first_value = temp_stack.Pop();
                    string second_value = temp_stack.Pop();
                    switch (nowdata)
                    {
                        case "+":

                            break;
                        case "-":
                            break;
                        case "+=":
                            break;
                        case "-=":
                            break;
                        case "*":
                            break;
                        case "*=":
                            break;
                        case "/":
                            break;
                        case "/=":
                            break;
                        case "%":
                            break;
                        case "^":
                            break;
                    }
                }
                else
                {
                    temp_stack.Push(nowdata);
                }
            }
        }

        private string Get_type_from_UF(string Func_name, Dictionary<string, User_Function> Ustorage, out bool ISERROR)
        {
            User_Function Temp_USERFUNCTION;
            ISERROR = false;
            if (Ustorage.TryGetValue(Func_name, out Temp_USERFUNCTION))
                return Temp_USERFUNCTION.GetType();
            else
            {
                ISERROR = true;
                return "anytype";  //Ошибка о невозможности определить тип.
            }
        }

        private string Get_type_from_Any_function(string Func_name, Dictionary<string, BuiltIn_Function> Fstorage, out bool INISERROR)
        {
            BuiltIn_Function Temp_BUILTINFUNCTION;
            INISERROR = false;
            if (Fstorage.TryGetValue(Func_name, out Temp_BUILTINFUNCTION))
                return Temp_BUILTINFUNCTION.GetType();
            else
            {
                INISERROR = true;
                return "anytype";//Ошибка о невозможности определить тип.
            }
        }

        private string Type_getter_from_int(string Input_word, out bool INISERROR)
        {
            int IFINT = 0;
            float IFFLOAT = 0;
            double IFDOUBLE = 0;
            INISERROR = false;
            if (int.TryParse(Input_word, out IFINT))
                return "int";
            else if (float.TryParse(Input_word, out IFFLOAT))
                return "float";
            else if (double.TryParse(Input_word, out IFDOUBLE))
                return "double";
            else
            {
                INISERROR = true;
                return "anytype";  //Создавать ошибку на неизвестный числовой тип
            }
        }

        private string Type_getter_from_func(string input_word, out bool ISERROR)
        {
            ISERROR = false;
            Dictionary<string, User_Function> temps = new Dictionary<string, User_Function>(); //ЗАМЕНИТЬ НА ПРИВЯЗКУ К ГЛОБАЛЬНОЙ!
            Dictionary<string, BuiltIn_Function> temps2 = new Dictionary<string, BuiltIn_Function>(); //ЗАМЕНИТЬ НА ПРИВЯЗКУ К ГЛОБАЛЬНОЙ!
            if (Is_type_definition(input_word))
                return input_word;
            else if (Is_any_function(input_word))
                return "";
            else if (Is_user_function(input_word, temps))  //зАМЕНИТЬ TEMPS НА ПРИВЯЗКУ К ГЛОБАЛЬНОЙ!
                return Get_type_from_UF(input_word, temps, out ISERROR);
            else
            {
                ISERROR = true;
                return "anytype"; //Создавать ошибку о неизвестном типе функции.
            }
        }

        private string Type_getter(Word Input_word, out bool ISERROR)
        {
            int IFINT = 0;
            float IFFLOAT = 0;
            double IFDOUBLE = 0;
            ISERROR = false;
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
                    {
                        ISERROR = true;
                        return "anytype"; //Создавать ошибку о неизвестном числовом типе.
                    }
                case 8:
                    if (Is_type_definition(nowdata))
                        return nowdata;
                    else if (Is_any_function(nowdata))
                        return "";
                    else if (Is_user_function(nowdata, temps))  //зАМЕНИТЬ TEMPS НА ПРИВЯЗКУ К ГЛОБАЛЬНОЙ!
                        return Get_type_from_UF(nowdata, temps, out ISERROR);
                    else
                    {
                        ISERROR = true;
                        return "anytype"; //Создавать ошибку о неизвестном типе функции.
                    }
                default:
                    {
                        ISERROR = true;
                        return "anytype";  //Создавать ошибку о неизвестном типе.
                    }
            }
        }

        private string Type_getter_until_symbol(List<Word> Input_list, int counter, out bool ISERROR, string symbol)
        {
            int IFINT = 0;
            float IFFLOAT = 0;
            double IFDOUBLE = 0;
            bool cycle_stop = false;
            ISERROR = false;
            string nowdata = "";
            Word temp_word = new Word();
            List<Word> Output_string = new List<Word>();
            Stack<Word> Operators_stack = new Stack<Word>();
            Dictionary<string, User_Function> temps = new Dictionary<string, User_Function>();
            Dictionary<string, BuiltIn_Function> temps2 = new Dictionary<string, BuiltIn_Function>();
            Dictionary<string, Variable> temps3 = new Dictionary<string, Variable>();
            while ((!cycle_stop) || (nowdata != symbol))
            {
                nowdata = Input_list[counter].get_data();
                temp_word = Input_list[counter];
                int nowid = Input_list[counter].Get_ID();
                switch (nowid)
                {
                    case 4:
                        temp_word.change_data(Type_getter_from_int(nowdata, out cycle_stop));
                        Output_string.Add(temp_word);
                        break;
                    case 2:
                        Operators_stack.Push(Input_list[counter]);
                        break;
                    case 3:
                        temp_word = Operators_stack.Pop();
                        while (temp_word.get_data() != "(")
                        {
                            Output_string.Add(temp_word);
                            temp_word = Operators_stack.Pop();
                        }
                        break;
                    case 1:
                        if ((Is_ariphmetical_symbol(nowdata)) && (nowdata != "="))
                        {
                            if (Operators_stack.Count > 0)
                            {
                                if (Priority_of_word(nowdata) <= Priority_of_word(Operators_stack.Peek().get_data()))
                                {
                                    Output_string.Add(temp_word);
                                }
                            }
                            Operators_stack.Push(temp_word);
                        }
                        else if (nowdata == "=")
                        {

                        }
                        break;
                    case 8:
                        if (Is_variable(nowdata, temps3))
                        {
                            temp_word.change_data(temps3[nowdata].Get_val_type());
                            Output_string.Add(temp_word);
                        }
                        else if (Is_any_function(nowdata))
                        {
                            temp_word.change_data(Get_type_from_Any_function(nowdata, temps2, out cycle_stop));
                            Output_string.Add(temp_word);
                        }
                        else if (Is_type_definition(nowdata))
                        {

                        }
                        else
                        {

                        }
                        break;
                }
                counter++;
            }
            if (nowdata == symbol)
            {
                while (Operators_stack.Count > 0)
                    Output_string.Add(Operators_stack.Pop());
            }
            Type_calc
        }

        private bool equality_with_the_row_check(string equaler, List<Word> Word_list, int counter)
        {
            return ((Word_list[counter].Get_row_count() == Word_list[counter + 1].Get_row_count()) && (Word_list[counter + 1].get_data() == equaler));
        }

        private bool equality_with_the_row_check(Func<string, bool> equaler, List<Word> Word_list, int counter)
        {
            return ((Word_list[counter].Get_row_count() == Word_list[counter + 1].Get_row_count()) && (equaler(Word_list[counter + 1].get_data())));
        }

        private bool equality_with_the_row_check(Func<string, bool> equaler, List<Word> Word_list, int counter, bool space_check, out int num_of_error)
        {
            if (!equaler(Word_list[counter + 1].get_data()))
                num_of_error = 1;
            else if ((!Word_list[counter + 1].get_space_check()) && (space_check))
                num_of_error = 2;
            else
                num_of_error = 0;
            return ((Word_list[counter].Get_row_count() == Word_list[counter + 1].Get_row_count()) && (equaler(Word_list[counter + 1].get_data())) && (Word_list[counter + 1].get_space_check()));
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

        private bool Call_Function_translation(List<Word> Word_List, int counter, out int Err_num)
        /*Проверяет правильность вызова функции, проверка типа аргументов.*/
        {
            bool result = false;
            bool cycle_stop = false;
            string nowdata = "";
            string function_name = Word_List[counter].get_data();
            int now_arg_num = -1;
            Dictionary<string, User_Function> Temps1 = new Dictionary<string, User_Function>();  //ЗАМЕНИТЬ НА АРГУМЕНТ ИЛИ НА ГЛОБАЛЬНУЮ.
            Dictionary<string, BuiltIn_Function> Temps2 = new Dictionary<string, BuiltIn_Function>(); //ЗАМЕНИТЬ НА АРГУМЕНТ ИЛИ НА ГЛОБАЛЬНУЮ.
            Dictionary<string, Variable> Temps3 = new Dictionary<string, Variable>(); //ЗАМЕНИТЬ НА АРГУМЕНТ ИЛИ НА ГЛОБАЛЬНУЮ.
            if (equality_with_the_row_check("(", Word_List, counter))
            {
                counter++;
                while (!cycle_stop)
                {
                    if (Row_control_check(Word_List, counter))
                    {
                        counter++;
                        nowdata = Word_List[counter].get_data();
                        now_arg_num++;
                        switch (Word_List[counter].Get_ID())
                        {
                            case 4:
                                if (Type_getter_until_symbol(Word_List, counter, out cycle_stop, ",") == Temps2["nowdata"].Get_arg_type(now_arg_num))
                                {

                                }
                                break;
                            case 8:
                                if (Is_variable(nowdata, Temps3)) //ЗАМЕНИТЬ TEMPS3!
                                {
                                    if (Temps3[nowdata].Get_val_type() == Temps2["nowdata"].Get_arg_type(now_arg_num))
                                    {

                                    }
                                }
                                else if (Is_any_function(nowdata))
                                {
                                    if (Temps2[nowdata].GetType() == Temps2["nowdata"].Get_arg_type(now_arg_num))
                                    {
                                        if (Call_Function_translation(Word_List, counter, out Err_num))
                                        {

                                        }
                                    }
                                }
                                else if (Is_user_function(nowdata, Temps1))
                                {
                                    if (Temps1[nowdata].GetType() == Temps2["nowdata"].Get_arg_type(now_arg_num))
                                    {

                                    }
                                }
                                else
                                {
                                    Err_num = 1;
                                    cycle_stop = true;
                                }
                                break;
                        }
                    }
                }
            }
            return result;
        }

        private void If_construction_translation(List<Word> List_word, int counter, Dictionary<int, Variable> Var_storage, HashSet<Argument> Args, Dictionary<string, User_Function> UF_storage)
        /*Функция занимается трансляцией конструкции IF, основывается на калькуляторе типов, который работает через RPN.
         Выходным результатом является строка типов с действиями.*/
        {
            int error_num = -1;
            int construct_completer = 1;
            Dictionary<string, BuiltIn_Function> Temps = new Dictionary<string, BuiltIn_Function>(); //ЗАМЕНИТЬ НА АРГУМЕНТ ИЛИ НА ГЛОБАЛЬНУЮ.
            List<string> Output_string = new List<string>();
            Stack<string> Operators_stack = new Stack<string>();
            string nowdata;
            bool cycle_stop = false;
            if (equality_with_the_row_check("(", List_word, counter - 1))
            {
                counter++;
                nowdata = List_word[counter].get_data();
                while ((!cycle_stop) || (construct_completer == 0))
                {
                    switch (List_word[counter].Get_ID())
                    {
                        case 4:
                            Output_string.Add(Type_getter_from_int(nowdata, out cycle_stop));
                            break;
                        case 2:
                            construct_completer++;
                            Operators_stack.Push(nowdata);
                            break;
                        case 3:
                            nowdata = Operators_stack.Pop();
                            construct_completer--;
                            while (nowdata != "(")
                            {
                                Output_string.Add(nowdata);
                                nowdata = Operators_stack.Pop();
                            }
                            break;
                        case 8:
                            if (Is_any_function(nowdata))
                            {
                                if ((nowdata = Get_type_from_Any_function(nowdata, Temps, out cycle_stop)) == "anytype")
                                {
                                    //Создавать ошибку о невозможности прочитать тип.
                                }
                                else
                                {
                                    Output_string.Add(nowdata);
                                }
                            }
                            else if (Is_structure_function(nowdata))
                            {
                                //Создавать ошибку, невозможно создавать структуру, внутри условия структуры.
                            }
                            else if (Is_type_definition(nowdata))
                            {

                            }
                            else if (Is_user_function(nowdata, UF_storage))
                            {
                                if ((nowdata = Get_type_from_UF(nowdata, UF_storage, out cycle_stop)) == "anytype")
                                {
                                    //Создавать ошибку о невозможности прочитать тип.
                                }
                                else
                                {
                                    Output_string.Add(nowdata);
                                }
                            }
                            else
                            {
                                /*Возможно формирование исключения*/
                            }
                            break;
                    }
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
            while ((List_word[counter].Get_ID() != 3) && (!was_error))
            {
                if (equality_with_the_row_check(Is_type_definition, List_word, counter, true, out error_num))
                    switch (error_num)
                    {
                        case 0: /*Создавать ошибку на тему row_count, несоответствие строки*/
                            break;
                        case 1: /*Создавать ошибку на тему аргумента неизвестного типа*/
                            if ((!delimeter) && (List_word[counter].get_data() == ","))
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

        private void Function_body_translator(List<Word> List_word, int counter, Dictionary<int, Variable> Var_storage, HashSet<Argument> Args, Dictionary<string, User_Function> UF_storage)
        /*Функция осуществляет трансляцию тела функции, приводя ее к виду RPN, при вызове функции осуществляется интерпретация RPN строки*/
        {
            bool quiter = false;
            while (!quiter)
            {

            }
        }

        private void Function_definition_translator(List<Word> Input_list_word, int counter, Dictionary<int, Variable> Var_storage, Dictionary<string, User_Function> Uf_storage)
        /*Функция осуществляет трансляцию определения функции и записывает ее в список User_function, */
        {
            string return_type = "";
            string Func_name = "";
            int FuncID = Uf_storage.Count + 1;
            int error_num = -1;
            if (equality_with_the_row_check(Is_type_definition, Input_list_word, counter, true, out error_num))
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


        private void Non_local_Args_translator(string Call_function_name, List<Word> Input_list_word, int counter, Dictionary<int, Variable> Var_storage, Dictionary<string, User_Function> Uf_storage)
        /*Пока что преобразовывается в заглушку, ибо приоритет меньше чем у транслятора обьявления функций*/
        {
            if (Is_any_function(Call_function_name))
            {

            }
        }

        private void RPN_translation(List<Word> Input_list, int counter, Dictionary<int, Global_Variable> Var_storage, Dictionary<string, User_Function> User_func_storage)
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

        private void Ariphmetical_translation(List<Word> Input_list, int counter, Dictionary<int, Global_Variable> Val_storage, Dictionary<string, User_Function> User_func_storage, bool Was_equality)

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
                switch (Input_list[counter].Get_ID())
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
                        /*Здесь необходимо создавать исключение о необьявленной переменной.*/
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
                            /*Возможно формирование исключения*/
                        }
                        break;
                    case 9:

                        break;
                }
                counter++;
            }
            while (Operators_stack.Count > 0)
                Output_string.Add(Operators_stack.Pop());
        }
    }


}
