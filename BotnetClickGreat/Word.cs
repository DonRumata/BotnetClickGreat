using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotnetClickGreat
{
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
            FRange_value=Number_range_converter(out SRange_value);
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
}
