using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions_Library
{
    /*На данный момент код представляет собой заглушку, а не полный и работоспособный класс, классы в дальнейшем будут расширены.
    Также необходимо написать класс автоматического исправления предупреждений или ошибок, для Premium версии */

    /*Возможно создание лога ошибок и/или предупреждений */
    public class MyAppException  //Базовый класс исключения, содержит базовые конструкторы и данные для создания ошибки или предупреждения.
    {
        protected int Row;
        protected int Column;
        protected int ID;
        protected string Message;

        public MyAppException(int row, int column, int id, string message)  //Конструктор с изначальным заполнением полей базового класса
        {
            Row = row;
            Column = column;
            ID = id;
            Message = message;
        }
        public MyAppException()  //Конструктор с заполнение полей класса по умолчанию
        {
            Row = 0;
            Column = 0;
            ID = 0;
            Message = "Undefined Error";
        }
        protected virtual void Change_all_base_data(int new_row, int new_column, int new_id, string new_message) //Позволяет изменить все базовые поля класса
        {
            Row = new_row;
            Column = new_column;
            ID = new_id;
            Message = new_message;
        }
        protected virtual int Get_all_base_data(out int row, out int column, out string message) //Позволяет получить все базовые поля класса
        {
            row = Row;
            column = Column;
            message = Message;
            return ID;
        }
    }

    class MyAppWarning:MyAppException  //Дочерний класс от MyAppException, служит для вывода и хранения предупреждений в коде.
    {
        private string Text_data;
        public MyAppWarning() : base() { Text_data = "Undefined"; } //Наследует конструктор по умолчанию базового класса, однако включает поле Text_data дочернего класса
        public MyAppWarning(int Nrow, int Ncolumn, int NID, string Nmessage, string Data_text) : base(Nrow, Ncolumn, NID, Nmessage) { Text_data = Data_text; } //Наследует конструктор базового класса, включая поле Text_data дочернего класса, все поля задаются

        public void Change_all_data(int nrow, int ncolumn, int nid, string nmessage, string ntext_data)  //Позволяет изменить все поля класса
        {
            base.Change_all_base_data(nrow, ncolumn, nid, nmessage);
            Text_data = ntext_data;
        }

        public string Get_all_data(out int brow, out int bcolumn, out int bid, out string bmessage) //Позволяет получить разделенными параметрами все поля класса
        {
            bid=Get_all_base_data(out brow, out bcolumn, out bmessage);
            return Text_data;
        }
    }

    class MyAppError:MyAppException //Дочерний класс от MyAppException, служит для вывода и хранения не Runtime ошибок в коде.
    {
        private string Gyper_confusion; //Являет собой ссылку на ошибку URL вида, на странице содержаться советы по ее устранению.
        
        public MyAppError() : base() { Gyper_confusion = ""; }  //Наследует конструктор по умолчанию базового класса, однако включает пол Gyper_confusion дочернего класса
        public MyAppError(int Nrow, int Ncolumn, int NID, string Nmessage, string NGyper_confusion) : base(Nrow, Ncolumn, NID, Nmessage) { Gyper_confusion = NGyper_confusion; } //Наследует конструктор базового класса, включая поле Gyper_confusion дочернего класса, все поля задаются

        public void Change_all_data (int nrow, int ncolumn, int nid, string nmessage, string nGyper_confusion)  //Позволяет изменить все поля класса
        {
            base.Change_all_base_data(nrow, ncolumn, nid, nmessage);
            Gyper_confusion = nGyper_confusion;
        }
        public string Get_all_data(out int brow, out int bcolumn, out int bid, out string bmessage) //Позволяет получить разделенными параметрами все поля класса
        {
            bid = Get_all_base_data(out brow, out bcolumn, out bmessage);
            return Gyper_confusion;
        }
    }

    [Serializable]
    class MyAppCritical:ApplicationException  //Дочерний класс от ApplicationException, служит для обработки Runtime ошибок и исключений, в дальнейшем данные могут обрабатываться классом MyAppError для получения более понятных данных об ошибке.
    {

    }
}
