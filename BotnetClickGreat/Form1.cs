using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parsers;

namespace BotnetClickGreat
{
    public partial class Form1 : Form
    {
        public static MainProgram User_program;

        private void Initialize_program_shell() //Данная функция инициализирует глобальные переменные, а также проводит валидацию DLL и обеспечивает инициализацию связи клиент/сервер приложения.
        {
            User_program = new MainProgram();
        }

        public Form1()
        {
            InitializeComponent();
        }
        //ПЕРЕНЕСЕНО!!!!
        /*Весь код был перенесен в класс CharFirstParser, необходимо решить дальнейшую проблему взаимодействия различных парсеров, и нужно ли прикручивать к ним наследование.
        Есть мысль наладить их взаимодействие через интерфейс, между тремя различными парсерами, в таком случае, один будет запускать следующий и т.д. Пока не дойдет до последнего, а дальше массовый Return
        Подобная структура не очень оптимальна в плане стековой памяти, возможен Overflow, нужно попробовать, получается что-то вроде рекурсии, тройной вызов, чревато ошибками. */

        private void button1_Click(object sender, EventArgs e)   //Запускает трансляцию введенного пользователем скрипта.
        {
            Initialize_program_shell();
            User_program.Translate_program(textBox1.Text);
        }
        /*
var List_of_Words = new List<Word>();  //Хранит в себе данные о токенах|словах
bool Quiter = false;  //Служит для окончания списка, помогает контролировать конец строки для правильной записи внутри Word переменной
int i = 0, Previous_char_ID = -1, Helper1 = -1, Is_Parenthesis = 0;  //I - счетчик, Previous_char_ID - самая важная переменная, указывает на ID предыдущего символа.
char nowchar;  //Значение текущего символа
string help_str = ""; //Вспомогательная строковая переменная
Word word_creator = new Word();  //Служит для создания токена, основываясь на поступающих данных
HashSet<char> Another_char_set = Hashset_creator(); //Множество Char символов соответствующих ID другие символы. НЕ ИСПОЛЬЗУЕТСЯ!
while (!Quiter)
{
   nowchar = textBox1.Text[i];
   if ((nowchar == '\n') || (nowchar == ' ') || (nowchar =='\r')) //Проверка символа на пробел или конец строки|перевод каретки
   {
       if (nowchar == ' ')
           Previous_char_ID = 0;
       else
           Previous_char_ID = 11;
       word_creator.Plus_data(nowchar);
   }
   else if((Helper1=Arifm_check(nowchar))==0)  //Проверка символа на принадлежность к арифметическим операциям
   {
       if (Numeric_check(nowchar)==0)  //Если символ был не арифметической операцией, то проверка на принадлежность к цифрам
       {
           if ((Helper1=Alphabet_check(nowchar))==0)
           {
               switch (nowchar)
               {
                   case '[':
                       Previous_char_ID = 5;
                       break;
                   case ']':
                       Previous_char_ID = 6;
                       break;
                   case ';':
                       Previous_char_ID = 7;
                       break;
                   case '(':
                       Is_Parenthesis++;
                       Previous_char_ID = 2;
                       break;
                   case ')':
                       Is_Parenthesis--;
                       Previous_char_ID = 3;
                       break;
                   case ',':
                   case '.':Previous_char_ID = 9;
                       break;
                   case '\r':
                   case '\n':Previous_char_ID = 10;
                       break;
                   default: Previous_char_ID = -2;
                       break;
               }
           }
           else
           {
               i=While_delegate_function(Alphabet_check, 8, i, Previous_char_ID, List_of_Words); //Формирует из букв слово, которое затем записывается как отдельный токен, даже в том случае если перед словом шел не пробел, однако это указывается отдельно
               Previous_char_ID = 8;
           }                            
       }
       else
       {
           i=While_delegate_function(Numeric_check, 4, i, Previous_char_ID, List_of_Words);  //Формирует из цифр число, которое затем записывается как отдельный токен, даже в том случае если перед числом шел не пробел, однако это указывается отдельно
           Previous_char_ID = 4;
       }
   }
   else if(word_creator.get_data()=="")  //Если операция оказалось арифметической, проверка на содержание поля data класса, данная часть выполняется если перед этим в поле data класса ничего не было записано
   {
       word_creator.Plus_data(nowchar);
           unsafe
           {
               Previous_char_ID = Parenthesis_choose(Helper1, word_creator, &Is_Parenthesis, (Previous_char_ID == 0) || (Previous_char_ID == 11));  //ПОТЕНЦИАЛЬНОЕ БАГОДЕРЬМО
           }

   }
   else  //Если перед этим в поле data класса уже была некая запись
   {
       word_creator.change_id(Previous_char_ID);
       List_of_Words.Add(word_creator);
       word_creator.Plus_data(nowchar);
       unsafe
       {
           Previous_char_ID = Parenthesis_choose(Helper1, word_creator, &Is_Parenthesis,((Previous_char_ID==0)||(Previous_char_ID==11)));
       }
   }
}
}
*/
        /*
                //ПЕРЕНЕСЕНО
                private int Arifm_check(char nowchar_f)  //Проверяет входящий Char символ на принадлежность к арифметическим действиям|символам  //ПЕРЕНЕСЕНО!!!!
                {
                    if ((nowchar_f == '*') || (nowchar_f == '/') || (nowchar_f == '+') || (nowchar_f == '-') || (nowchar_f == '^') || (nowchar_f == '=')||(nowchar_f=='%'))
                        return 1;
                    else
                        return 0;
                }

                //ПЕРЕНЕСЕНО!!!!
                unsafe private int Parenthesis_choose(int helper, Word creator, int* is_parenthesis, bool Was_space)  //ПОТЕНЦИАЛЬНОЕ БАГОДЕРЬМО Выстраивает значение контрольной переменной для открытия|закрытия круглых скобок, а также формирует ID и Bool составляющие токена
                {
                    creator.change_bool(Was_space);
                    if (helper==1)  //Проверка был ли символ арифметической операцией
                    {
                        creator.change_id(helper);
                        creator.change_bool(true, true);
                    }
                    else if(helper==2) //Проверка был ли символ скобкой
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

                //ПЕРЕНЕСЕНО!!!
                private int Numeric_check(char Nowchar)   //Проверяет является ли текущий символ цифрой.
                {
                    if (char.IsDigit(Nowchar))
                        return 4;
                    else
                        return 0;
                }

                //ПЕРЕНЕСЕНО!!!
                private Word Word_checkncreator (string Else_word, bool Was_space, int id) //Формирует токен согласно поступившим значениям
                {
                    Word result = new Word();
                    result.change_id(id);
                    result.change_data(Else_word);
                    result.change_bool(true, true, Was_space);
                    return result;
                }

                //ПЕРЕНЕСЕНО!!!
                private int Another_check(char Nowchar, HashSet<char> Charset)  //Проверяет является ли текущий символ другим символом, основывается на contains hashset, также проверяет еще несколько других символов.
                {
                    if (Charset.Contains(Nowchar))
                        return 5;
                    else return 0;
                }

                //Заглушка, неиспользуемая функция.
                private HashSet<char> Hashset_creator()  //Задает значения hashset-а
                {
                    HashSet<char> result = new HashSet<char>();
                    result.Add('#');
                    result.Add('$');
                    result.Add('&');
                    result.Add('@');
                    result.Add('!');
                    result.Add('?');
                    result.Add('/');
                    result.Add('|');
                    result.Add('\\');
                    result.Add('{');
                    result.Add('}');
                    return result;
                }

                //ПЕРЕНЕСЕНО!!!
                private int Alphabet_check(char Nowchar)  //Проверяет является ли текущий символ, кириллицей или латиницей.
                {
                    if (((Nowchar > 96) && (Nowchar < 123))||((Nowchar>64)&&(Nowchar<91)))
                        return 8;
                    else
                        if ((Nowchar > 191) && (Nowchar < 256))
                        return 11;
                    else
                        return 0;
                }

                //ПЕРЕНЕСЕНО!!!
                private int While_delegate_function(Func<char, int> Cycle_condition, int second_cycle_condition, int i_counter, int previous_ID, List<Word> Word_list)  //Делегирует 2 функции, для сокращения кода похожих циклов While кода.
                {
                    int helper_counter = i_counter;
                    string Data_former = "";
                    while (Cycle_condition(textBox1.Text[helper_counter])==second_cycle_condition)  //Проходит циклом по тексту и формирует строку, пока входящая функция удовлетворяет второму условию
                    {
                        Data_former += textBox1.Text[helper_counter];
                        helper_counter++;
                    }
                    Word_list.Add(new Word(Data_former, true, true, previous_ID == 0, second_cycle_condition)); //Формирует и добавляет заготовку токена в список
                    return helper_counter;
                }
                */
    }
}
