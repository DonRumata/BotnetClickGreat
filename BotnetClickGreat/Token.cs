using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotnetClickGreat
{
    class Token //Базовый класс токена, содержит в себе лишь общие значения для всех токенов.
    {
        protected string Data { get; set; } //Текстовое значение слова содержащегося в токене.
        protected int ID { get; set; } //ID конкретного токена.
        protected string Number_range;
        protected int Row_count { get; set; }

        public Token(string data, int First_numR, int Second_numR, int row_counter, int id) //Конструктор базового класса задающий базовые значения извне
        {
            this.Data = data;
            this.ID = id;
            this.Row_count = row_counter;
            Number_range_former(First_numR, Second_numR);
        }

        public Token() //Конструктор базового класса по умолчанию.
        {
            this.Data = "";
            this.ID = 0;
            this.Row_count = 0;
            this.Number_range = "UNDEFINED";
        }

        private int Number_range_converter(out int Last_value)  //Возвращает в виде Int значений диапазон номеров символов для текущего слова
        {
            int help_value = Number_range.IndexOf('-');
            Last_value = int.Parse(Number_range.Substring(help_value));
            return int.Parse(Number_range.Substring(0, help_value));
        }

        private void Number_range_former(int First_char_num, int Last_char_num)  //Формирует строку хранящую диапазон номеров символов для текущего слова
        {
            this.Number_range = (First_char_num.ToString() + "-" + Last_char_num.ToString());
        }
    }

    class Ariphmetical:Token
    {
        public Ariphmetical(string ndata, int nFirst_numR, int nSecond_numR, int nrow_counter, int nid) : base(ndata, nFirst_numR, nSecond_numR, nrow_counter, nid) { }
        public Ariphmetical() : base() { }
        public object Interpretation_function(string first_value, string second_value)
        {
            double FValue = Convert.ToDouble(first_value);
            double SValue = Convert.ToDouble(first_value);
            if (Data.Length==1)
            {
                switch (Data)
                {
                    case "+":
                        break;
                    case "-":
                        break;
                    case "*":
                        break;
                    case "/":
                        break;
                    case "%":
                        break;
                    case "^":
                        break;
                }
            }
            else
            {
                switch (Data)
                {
                    case "+=":
                        break;
                    case "-=":
                        break;
                    case "*=":
                        break;
                    case "/=":
                        break;
                }
            }
           /* switch (Data)
            {
                case "+":
                    break;
                case "-":
                    break;
                case "+=":
                    break;
                case: "-=":
                    break;
                case "*":
                    break;
                case "/":
                    break;
                case "*=":
                    break;
                case "/=":
                    break;
                case "%":
                    break;
                case "^":
                    break;
            }*/
            return "trulala";
        }
    }

    //public MyAppWarning(int Nrow, int Ncolumn, int NID, string Nmessage, string Data_text) : base(Nrow, Ncolumn, NID, Nmessage) { Text_data = Data_text; }
}
