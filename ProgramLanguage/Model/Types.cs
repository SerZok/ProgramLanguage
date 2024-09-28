using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramLanguage.Model
{
    public static class Types
    {
        /// <summary>
        /// Ключевые слова (int, if, while)
        /// </summary>
        public struct tElemKey(string name, string lex)
        {
            public string Name { get; set; } = name; // Имя ключевого слова (например, "int", "if").
            public string Lex { get; set; } = lex;  // Лексема (представление ключевого слова в исходном коде).
        }

        /// <summary>
        /// Структура для хрананеия информации
        /// </summary>
        public struct tElSp
        {
            public int Info { get; set; }
            //tElSp* Next;
        }

        /// <summary>
        /// Информация о метках (ярлыках), мб goto
        /// </summary>
        public struct tElemLbl
        {
            public tElSp telSp;
            public string Lex { get; set; }
            public int Index { get; set; }
        }

        /// <summary>
        /// Идентификаторы (переменные)
        /// 
        /// </summary>
        public struct tElemIde
        {
            public string Lex { get; set; }  // Лексема идентификатора (например, имя переменной).
            public int Cat { get; set; }     // Категория идентификатора (например, переменная, функция, тип данных).
            public int Tip { get; set; }     // Тип идентификатора (например, целый тип, вещественный и т.д.).
            public int Size { get; set; }    // Размер идентификатора (например, размер переменной в байтах).
            public int Addr { get; set; }    // Адрес идентификатора в памяти (если необходимо для кодогенерации).
        }

        /// <summary>
        /// Токен
        /// </summary>
        /// <param name="code"></param>
        /// <param name="attr"></param>
        /// <param name="name"></param>
        public struct tToken(int code, int attr, string name) 
        {
            public int Code { get; set; } = code; // Код токена (идентификатор типа токена).
            public int Attr { get; set; } = attr; // Атрибут токена (например, индекс в таблице символов).
            public string Name { get; set; } = name; // Имя токена (тип лексемы, например "int", "id").
            public int LexBeg { get; set; } // Начальная позиция лексемы в исходном коде.
            public int LexEnd { get; set; } // Конечная позиция лексемы в исходном коде.
            public override readonly string ToString()
            {
                //<код_токена, атрибут>
                return $"<{Attr}, {Name}>";
            }
        }
    }
}
