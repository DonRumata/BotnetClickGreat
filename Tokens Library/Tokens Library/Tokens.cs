using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokens_Library
{
   /* public class Token //Базовый класс токена, содержит в себе лишь общие значения для всех токенов.
    {

        protected string Data;           //сам предтокен
        protected Tuple<int, int> Range; //расположение предтокена в строке (номер первого символа, номер последнего символа)
        protected bool Space_check;       //флаг наличия пробела до предтокена
        protected ETypeChar ID;                 //Номер типа токена
        protected int Row;                //Номер строки

        public Token(string data, bool space, ETypeChar id, int row, int FRange_value, int SRange_value) //Конструктор для создания класса Word, сразу со всеми первоначальными данными
        {
            this.Data = data;
            this.Space_check = space;
            this.ID = id;
            this.Row = row;
            this.Range = Tuple.Create(FRange_value, SRange_value);
        }

        public Token()  //Вариант конструктора для простого выделения памяти и создания класса с значениями по умолчанию.
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
        *//*
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
    } */

    /*class Ariphmetical : Token  //Дочерний класс токен, описывающий все взаимодействия с арифметическими символами
    {
        /*Свойства и атрибуты, наследуемые от базового класса.
        protected string Data { get; set; } //Текстовое значение слова содержащегося в токене.
        protected int ID { get; set; } //ID конкретного токена.
        protected string Number_range; //Хранит диапазон номеров символов для текущего токена.
        protected int Row_count { get; set; } //Хранит номер строки в тексте, для текущего токена.
        */
        /*
        public Ariphmetical(string ndata, int NRow, int NFRange_value, int NSRange_value, ETypeChar NID, bool spacer) : base(ndata,spacer,NID,NRow,NFRange_value,NSRange_value) { } //Наследуется от базового, конструктор, вызывается при переводе первичной строки слов, в токены
        public Ariphmetical() : base() { } //Наследуется от базового, конструктор с базовыми значениями по умолчанию
        public double Interpretation_function(string first_value, string second_value) //Функция для исполнения интерпретатором, вшита непосредственно в токен для более быстрой интерпретации.
        {
            double FValue = Convert.ToDouble(first_value);
            double SValue = Convert.ToDouble(first_value);
            double Result = 0;
            switch (Data)
            {
                case "+=":
                case "+":
                    Result = FValue + SValue;
                    break;
                case "-=":
                case "-":
                    Result = FValue - SValue;
                    break;
                case "*=":
                case "*":
                    Result = FValue * SValue;
                    break;
                case "/=":
                case "/":
                    Result = FValue / SValue;
                    break;
                case "%":
                    Result = FValue / 100;
                    break;
                case "^":
                    Result = Math.Round(Math.Pow(FValue, SValue), 4);
                    break;
            }
            return Result;
        }
    } */

    class Basic_ariphmetical_commands : Token //Дочерний класс токен, описывающий все арифметические команды вроде синусов, косинусов, логарифмов и интегралов ОСТОРОЖНО, СОДЕРЖИТ МАТАН!!
    {
        /*Свойства и атрибуты, наследуемые от базового класса.
        protected string Data { get; set; } //Текстовое значение слова содержащегося в токене.
        protected int ID { get; set; } //ID конкретного токена.
        protected string Number_range; //Хранит диапазон номеров символов для текущего токена.
        protected int Row_count { get; set; } //Хранит номер строки в тексте, для текущего токена.
        */

        public Basic_ariphmetical_commands(string ndata, int nFirst_numR, int nSecond_numR, int nrow_counter, int nid) : base(ndata, nFirst_numR, nSecond_numR, nrow_counter, nid) { } //Наследуется от базового, конструктор, вызывается при переводе первичных слов в токены
        public Basic_ariphmetical_commands() : base() { } //Наследуется от базового, конструктор для создания класса со значениями по умолчанию
        public double interpretation_function(double first_value, double second_value) //Функция для исполнения интерпретатором, вшита непосредственно в токен для более быстрой интерпретации.
        {
            double Result = 0;
            switch (Data.ToLower())
            {
                case "mod":
                    Result = first_value % second_value;
                    break;
                case "div":
                    Result = (int)(first_value / second_value);
                    break;
                case "round":
                    Result = (int)first_value;
                    break;
                case "sqr":
                    Result = (int)first_value ^ 2;
                    break;
                case "sqrt":
                    Result = Math.Round(Math.Sqrt(first_value), 4);
                    break;
                case "log":
                    Result = Math.Log(first_value);
                    break;
                case "integr":
                    break;
                case "acos":
                    Result = Math.Acos(first_value);
                    break;
                case "asin":
                    Result = Math.Asin(first_value);
                    break;
                case "atan":
                    Result = Math.Atan(first_value);
                    break;
                case "sin":
                    Result = Math.Sin(first_value);
                    break;
                case "cos":
                    Result = Math.Cos(first_value);
                    break;
                case "tan":
                    Result = Math.Tan(first_value);
                    break;
                case "pow":
                    Result = Math.Pow(first_value, second_value);
                    break;
            }
            return Result;
        }

    }

    class Logical : Token
    {
        /*Свойства и атрибуты, наследуемые от базового класса.
        protected string Data { get; set; } //Текстовое значение слова содержащегося в токене.
        protected int ID { get; set; } //ID конкретного токена.
        protected string Number_range; //Хранит диапазон номеров символов для текущего токена.
        protected int Row_count { get; set; } //Хранит номер строки в тексте, для текущего токена.
        */

        public Logical(string ndata, int nFirst_numR, int nSecond_numR, int nrow_counter, int nid) : base(ndata, nFirst_numR, nSecond_numR, nrow_counter, nid) { } //Наследуется от базового, конструктор, вызывается при переводе первичных слов в токены
        public Logical() : base() { } //Наследуется от базового, конструктор для создания класса со значениями по умолчанию

        public bool interpretation_function(object first_value, object second_value) //Функция для исполнения интерпретатором, вшита непосредственно в токен для более быстрой интерпретации.
        {
            /* Тут большая проблема интерпретации, для интерпретации логических элементов, необходимо определять входящий тип,
            для более менее приемлемого кода необходим конвертер из object, в какой-либо из типов, а затем проверки на совместимость сравниваемых типов.
            Геморой довольно большой и писать его без конвертера в object непосредственно во время трансляции, не представляется возможным.
            */
            return false;
        }

    }

    class Basic_structure_WhileCycle : Token
    {
        /*Свойства и атрибуты, наследуемые от базового класса.
        protected string Data { get; set; } //Текстовое значение слова содержащегося в токене.
        protected int ID { get; set; } //ID конкретного токена.
        protected string Number_range; //Хранит диапазон номеров символов для текущего токена.
        protected int Row_count { get; set; } //Хранит номер строки в тексте, для текущего токена.
        */
        protected int Iterator { get; set; }
        private bool Condition { get; set; }
    }


    class Basic_condition_structure:Token
    {

    }


    /*double Nth_root(double number, double power)
    {
        double eps = 0.000001;
        double prev_y, next_y;

        next_y = number;
        do
        {
            prev_y = next_y;
            next_y = (prev_y * (power - 1) + number / pow(prev_y, power - 1)) / power;
        } while (fabs(next_y - prev_y) > eps);
        return next_y;
    } */

    //public MyAppWarning(int Nrow, int Ncolumn, int NID, string Nmessage, string Data_text) : base(Nrow, Ncolumn, NID, Nmessage) { Text_data = Data_text; }
}
