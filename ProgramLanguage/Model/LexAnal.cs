using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProgramLanguage.Model.LexAnal;
using static ProgramLanguage.Model.Types;

namespace ProgramLanguage.Model
{
    public class LexAnal : ReactiveObject
    {
        public LexAnal()
        {
            InitDataType();
        }

        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged("Code");
            }
        }

        private bool _isError = false;
        public bool IsError { get => _isError; private set => _isError = value; }
        private char _charError;
        public char CharError { get => _charError; private set => _charError = value; }

        /// <summary>
        /// Конечные состояния (столбцы в _TP)
        /// </summary>
        public tToken[] SostAndToken =
        [
            //code, attr, name
            new (10, 0, ";"),             // {-1}
            new (11, 0, ","),             // {-2}
            new (12, 0, "["),             // {-3}
            new (13, 0, "]"),             // {-4}
            new (14, 0, "+"),             // {-5}
            new (15, 0, "-"),             // {-6}
            new (16, 0, "*"),             // {-7}
            new (17, 0, "/"),             // {-8}
            new (18, 0, "as"),            // {-9}
            new (19, 0, "rel"),           // {-10}
            new (20, 0, "rel"),           // {-11}
            new (21, 0, "rel"),           // {-12}
            new (22, 0, "rel"),           // {-13}
            new (23, 0, "rel"),           // {-14}
            new (24, 0, "!"),             // {-15}
            new (25, 0, "rel"),           // {-16}
            new (26, 0, "+"),             // {-17}
            new (27, 0, "*"),             // {-18}
            new (28, 0, "lbl float e"),   // {-19}
            new (29, 0, "lbl int"),       // {-20}
            new (30, 0, "lbl float"),     // {-21}
            new (31, 0, "id"),            // {-22}
            new (35, 0, "("),             // {-23}
            new (36, 0, ")"),             // {-24}
        ];

        private List<char> Letters = new List<char> { '_', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private List<char> Numbers = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// Таблица состояний ( переходы между состояниями автомата )
        /// Отрицательные значения - конечное состояние
        /// </summary>
        private readonly int[,] _TP = new int[,]
        {
            //Строки  - символ в коде   (функция NomStrOfTP - определяет какую строчку брать)
            //Столбцы - состояния       (массив SostAndToken [id+1] - определяет конечное состояние)
            //Крч.            0      1       2       3       4       5       6       7       8       9       10      11      12      13      14      15      16      17 
            /*1     ;   */  {-1,    -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*2     ,   */	{-2,    -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*3     [   */	{-3,    -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*4     ]   */	{-4,    -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*5     +   */	{-5,    -9,     -11,    -13,    -15,    -28,    -28,     13,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*6     -   */	{-6,    -9,     -11,    -13,    -15,    -28,    -28,     13,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*7     *   */	{-7,    -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*8     /   */	{-8,    -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*9     =   */	{1,     -10,    -12,    -14,    -16,    -28,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*10    <   */	{2,     -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*11    >   */	{3,     -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*12    !   */	{4,     -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*13    |   */	{5,     -9,     -11,    -13,    -15,    -17,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*14    &   */	{6,     -9,     -11,    -13,    -15,    -28,    -18,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*15    e   */	{-28,   -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,      7,     11,    -27,    -28,    -19,      7,    -28,    -25},
            /*16    .   */	{16,    -9,     -11,    -13,    -15,    -28,    -28,    -28,     17,    -22,      8,     11,    -27,    -28,    -19,    -21,     17,    -25},
            /*17 letter */	{9,     -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,      9,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*18  num   */	{10,    -9,     -11,    -13,    -15,    -28,    -28,     14,     15,      9,     10,     11,    -27,     14,     14,     15,    -28,    -25},
            /*19    #   */	{11,    -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,    -26,    -27,    -28,    -19,    -21,    -28,    -25},
            /*20  space */	{12,    -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,     12,    -28,    -19,    -21,    -28,    -25},
            /*21   tab  */	{12,    -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,     12,    -28,    -19,    -21,    -28,    -25},
            /*22    if  */	{12,    -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,     12,    -28,    -19,    -21,    -28,    -25},
            /*23   /r/n  */	{12,    -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,     12,    -28,    -19,    -21,    -28,    -25},
            /*24    (   */	{-23,   -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*25    )   */	{-24,   -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25},
            /*26  other */	{-28,   -9,     -11,    -13,    -15,    -28,    -28,    -28,    -28,    -22,    -20,     11,    -27,    -28,    -19,    -21,    -28,    -25}

            /*
             * -28 - ошибка
            */
        };

        private List<tElemLbl> Lbls = new List<tElemLbl>();
        private List<tElemIde> Idents = new List<tElemIde>();
        private List<tElemKey> ElemKeys = new List<tElemKey>{
               //name           lex
            new ("and",         "*"),
            new ("beginBlock",  "beginBlock"),		//{1}
	        new ("end",         "end" ),			//{3}
	        new ("endBlock",    "endBlock"),		//{4}
	        new ("endp",        "endp" ),		    //{5}
	        new ("false",       "false"),           //{6}
            new ("if",          "if" ),				//{7}
	        new ("or",          "+"),               //{8}
            new ("startp",      "startp" ),	        //{9}
	        new ("true",        "true"),
            new ("z",           "zzz" )				//{10}
        };

        private int KolLbl = -1;
        private int KolId = 2; //id Идентефикаторов

        private int countLines = 1;
        public int CountLines
        {
            get => countLines;
            set => this.RaiseAndSetIfChanged(ref countLines, value);
        }

        private int countChar = 1;
        public int CountChar
        {
            get => countLines;
            set => this.RaiseAndSetIfChanged(ref countLines, value);
        }

        /// <summary>
        /// Первый символ
        /// </summary>
        private int f = 0;

        /// <summary>
        /// Текущий символ
        /// </summary>
        private int r = 0;
        public void InitDataType()
        {
            Idents.Clear();
            Idents.Add(new tElemIde { Lex = "int", Cat = 1, Tip = 1, Size = 4, Addr = -1 });
            Idents.Add(new tElemIde { Lex = "float", Cat = 1, Tip = 2, Size = 4, Addr = -1 });
            Idents.Add(new tElemIde { Lex = "bool", Cat = 1, Tip = 3, Size = 1, Addr = -1 });

            KolLbl = -1;
            KolId = 2;
        }
        private int BinarySearchLexKey(string lex)
        {
            return ElemKeys.FindIndex(x => x.Lex == lex);
        }
        private int SearchLexLbl(string lex)
        {
            var lblIndex = Lbls.FindIndex(l => l.Lex == lex);
            if (lblIndex == -1)
            {
                KolLbl++;
                Lbls.Add(new tElemLbl { Lex = lex, Index = -1 });
                return KolLbl;
            }
            return lblIndex;
        }

        /// <summary>
        /// Поиск переменной по лексеме, если нет, то создает новую
        /// </summary>
        /// <param name="lex"></param>
        /// <returns></returns>
        private int SearchLexId(string lex)
        {
            var idIndex = Idents.FindIndex(i => i.Lex == lex);
            if (idIndex == -1) //Если не нашел лексему, то создаем переменную
            {
                KolId++;
                Idents.Add(new tElemIde { Lex = lex, Cat = 0, Tip = 0, Size = 0, Addr = -1 });
                return KolId;
            }
            return idIndex;
        }
        public void CountPosition()
        {
            if (Code[r] == '\n')
            {
                countLines++;
                countChar = 1;   // Обнуляем счётчик символов для новой строки
            }
            else
            {
                countChar++; // Увеличиваем счётчик символов для любой другой ситуации
            }
        }

        /// <summary>
        /// Список токенов
        /// </summary>
        private List<tToken> tokens = new List<tToken>();

        /// <summary>
        /// Поиск символа в таблице состояний _TP (Номер в строке)
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        private int NomStrOfTP(char sym)
        {
            char charUp = Char.ToUpper(sym);
            if (Letters.Contains(charUp)) return 17;
            if (Numbers.Contains(charUp)) return 18;

            return sym switch
            {
                ';' => 1,
                ',' => 2,
                '[' => 3,
                ']' => 4,
                '+' => 5,
                '-' => 6,
                '*' => 7,
                '/' => 8,
                '=' => 9,
                '<' => 10,
                '>' => 11,
                '!' => 12,
                '|' => 13,
                '&' => 14,
                '.' => 16,
                '#' => 19,
                ' ' => 20,
                '\t' => 21,
                '\n' => 22,
                '\r' => 23,
                '(' => 24,
                ')' => 25,
                _ => 26
            };
        }
        public List<tToken> Scanner()
        {
            tokens.Clear(); // Очищаем список токенов
            r = 0;          // Сбрасываем индекс символа
            f = 0;
            InitDataType();

            CountChar = 0;
            CountLines = 0;
            IsError = false;

            while (r <= Code.Length) // Проходим по всему коду
            {
                tToken token = NextToken(); // Получаем следующий токен

                if (token.Code != 0) // Если токен найден, добавляем в список
                {
                    tokens.Add(token);
                }
                if (token.Code == -1)
                {
                    //Сообщить о плохом токене
                    IsError = true;
                    break;
                }
            }

            return tokens; // Возвращаем список токенов
        }
        private tToken NextToken()
        {
            int state = 0;
            int startPos = r;
            string lex = "";
            tToken result = new tToken();
            bool tokenFound = false;

            while (!tokenFound && r < Code.Length)
            {
                // Определяем строку таблицы переходов в зависимости от символа
                int strTP = NomStrOfTP(Code[r]);
                state = _TP[strTP - 1, state]; // Получаем новое состояние

                if (state >= 0)
                {
                    CountPosition(); // Обновляем позицию
                    r++; // Переходим к следующему символу
                }
                if (state < 0)
                {
                    state = -state; // Конечное состояние, найден токен

                    if (state == 27) // Пробелы или новая строка
                    {
                        f = r;
                        state = 0;
                        continue; // Игнорируем и продолжаем
                    }
                    else if (state == 26) // Комментарии
                    {
                        if (r == Code.Length - 1)
                        {
                            f = r;
                            return result; // Завершаем на последнем символе
                        }
                        else
                        {
                            countChar++;
                            f = r + 1;
                            r = f + 1;
                            state = 0;
                            continue;
                        }
                    }

                    // Обработка идентификатора или ключевого слова
                    if (state == 22) // Идентификатор
                    {
                        lex = Code.Substring(startPos, r - startPos);
                        int keyIndex = BinarySearchLexKey(lex);

                        if (keyIndex > 0) // Найдено ключевое слово.
                        {
                            result.Code = keyIndex;
                            result.Name = ElemKeys[keyIndex].Name;
                            result.Attr = 0;
                            result.LexBeg = f;
                            result.LexEnd = r - 1;
                        }
                        else  //Создает Переменную ?
                        {
                            int idIndex = SearchLexId(lex);
                            result.Attr = idIndex;
                            result.Code = SostAndToken[state - 1].Code;
                            result.Name = SostAndToken[state - 1].Name;
                            result.LexBeg = f;
                            result.LexEnd = r - 1;
                        }

                        tokenFound = true;
                        break;
                    }

                    // Обработка чисел и операторов
                    else if (state == 19 || state == 20 || state == 21) // Числа
                    {
                        lex = Code.Substring(startPos, r - startPos);
                        int numIndex = SearchLexLbl(lex); // Найти метку числа

                        result.Code = SostAndToken[state - 1].Code;
                        result.Attr = numIndex;
                        result.Name = SostAndToken[state - 1].Name;
                        result.LexBeg = startPos;
                        result.LexEnd = r - 1;
                        tokenFound = true;
                        break;
                    }

                    //// Обработка операторов и других символов
                    if (state > 0 && state <= SostAndToken.Length)
                    {
                        result.Code = SostAndToken[state - 1].Code;
                        result.Name = SostAndToken[state - 1].Name;
                        result.Attr = SostAndToken[state - 1].Attr;
                        result.LexBeg = startPos;
                        result.LexEnd = r - 1;
                        tokenFound = true;
                        break;
                    }
                    else
                    {
                        CharError = Code[r];
                        return new tToken(-1, -1, "error");
                    }
                }
            }
            r++;
            return result; // Возвращаем результат токена
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
