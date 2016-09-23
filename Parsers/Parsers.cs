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
        public int Num_of_rows = 0;
        protected string Program_text = "";
        private List<AnyFunction> Function_storage = null;
        private List<Variable> Variable_storage = null;
        private List<Token> TokenListText=null;
        //private Dictionary<int, Message> Output_data;



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

        private void Initialize_translater()
        {
            /*Инициализирует транслятор и словари и списки привязанные к трансляции, проверяет их существование.*/
            Function_storage = new List<AnyFunction>();
            Variable_storage = new List<Variable>();
            TokenListText = new List<Token>();
            if ((Function_storage != null) && (Variable_storage != null) && (TokenListText != null))
            {
                
            }
        }

        private void Initialize_interpretators() //Будет инициазилировать интерпретатор пока что в стадии заглушки
        {

        }

        public void Translate_program(string input_program_text)
        {

            Program_text = input_program_text;
            FirstParser Parser = new FirstParser();
            Parser.First_Parse(Function_storage, Variable_storage, input_program_text);
            //Translater.Begin_translating(First_parser.Parse_first_text(input_program_text), Variable_storage, User_function_storage);
        }

        public void Save_in_file() { } //Заглушка на сохранение в файлы

        public void Load_from_file() { } //Заглушка на загрузку из файла


    }


    class Message : MyAppException
    {
        protected string message = "";

    }
}
