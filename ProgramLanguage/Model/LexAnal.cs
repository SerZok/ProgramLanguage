using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProgramLanguage.Model.LexAnal;

namespace ProgramLanguage.Model
{
    public class LexAnal: INotifyPropertyChanged
    {
        public LexAnal()
        {

        }
        public string Scanner()
        {
            var res = Code;

            //res += tToken;
            return res; //<код_токена, атрибут>
        }

        public tToken[] SostAndToken = new tToken[]
        {
            new (10, 0, ";"),             // {-1}
            new (11, 0, ","),             // {-2}
            new (12, 0, "["),             // {-3}
            new (13, 0, "]"),             // {-4}
            new (14, 0, "+"),             // {-5}
            new (15, 0, "+"),             // {-6}
            new (16, 0, "*"),             // {-7}
            new (17, 0, "*"),             // {-8}
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
        };

        private string _code;
        public string Code { get => _code;
            set
            {
                _code = value;
                OnPropertyChanged("Code");
            }
        }

        private List<char> _letters;
        private List<int> _ints;
        private int[][] _TP;
        private struct tElemKey
        {
            private string Name { get; set; }
            private string Lex { get; set; }
        }
        private struct tElSp
        {
            private int Info { get; set; }
            
        }
        private struct tElemLbl
        {
            private string Lex { get; set; }
            private int Index { get; set; }
        }
        private struct tElemIde
        {
            private string Lex { get; set; }
            private int Cat { get; set; }
            private int Tip { get; set; }
            private int Size {  get; set; }
            private int Addr {  get; set; }
        }
        public struct tToken
        {
            public int Code { get; set; }
            public int Attr { get; set; } //указывает на запись в таблице идентификаторов, соответствующую данному токену.
            public string Name { get; set; }
            public int LexBeg {  get; set; }
            public int LexEnd { get; set; }

            public tToken(int code, int attr, string name)
            {
                Code = code;
                Attr = attr;
                Name = name;
            }
            
            public override string ToString()
            {
                //<код_токена, атрибут>
                return $"<{Code}, {Attr}>";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
