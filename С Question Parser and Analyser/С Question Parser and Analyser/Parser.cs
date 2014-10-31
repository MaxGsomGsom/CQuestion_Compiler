using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace С_Question_Parser_and_Analyser
{
    public class Parser
    {
        //тип тексемы 0
        static string[] reservedWordsBuf = {"using", "namespace", "class", "void",
                                            "string", "bool", "int", "break", 
                                            "continue", "return", "for", "if", 
                                            "else", "true", "false"};
        static List<string> reservedWords = new List<string>(reservedWordsBuf);
        //тип тексемы 1
        static string[] separatorsBuf = {";", ".", "{", "}", "(", ")", ",", 
                                         "=", "||", "&&", "==", "!=", 
                                         "<", ">", ">=", "<=", "+", 
                                         "-", "*", "/", "!", "++", "--"};
        static List<string> separators = new List<string>(separatorsBuf);
        //тип лексемы 2
        List<string> identifers;
        //тип лексемы 3
        List<string> stringConst;
        //тип лексемы 4
        List<double> numericConst;

        //все лексемы
        List<Lex> prog = new List<Lex>();

        int pos;
        string inputText;
        string curLex;

        public Parser(string inputText)
        {
            this.inputText = inputText;
        }

        public void Parse()
        {
            pos = -1;
            prog = new List<Lex>();
            identifers = new List<string>();
            stringConst = new List<string>();
            numericConst = new List<double>();
            curLex = "";


            while (pos<inputText.Length-1)
            {
                pos++;

                if (IsEmptyChar(false)) continue;

                curLex += inputText[pos];


                if (IsSeparatorChar(false) && !IsSeparatorChar(true))
                {
                    AddLex(1);
                    continue;
                }
                if (!IsSeparatorChar(false) && !IsEmptyChar(false) && (IsSeparatorChar(true) || IsEmptyChar(true)))
                {
                    if (IsItReservedWord()) AddLex(0);
                    else AddLex(2);
                    continue;
                }

            }



        }

        void AddLex(int type)
        {
            int lexID = 0;

            switch (type)
            {
                case 0:
                    {
                        lexID = reservedWords.IndexOf(curLex);
                        break;
                    }
                case 1:
                    {
                        lexID = separators.IndexOf(curLex);
                        break;
                    }
                case 2:
                    {
                        if (identifers.IndexOf(curLex) == -1) identifers.Add(curLex);
                        lexID = identifers.IndexOf(curLex);
                        break;
                    }
            }

            Lex buf = new Lex();
            buf.type = type;
            buf.number = lexID;
            prog.Add(buf);
            curLex = "";
        }

        bool IsSeparatorChar(bool next)
        {
            char c;
            if (next) c = inputText[pos + 1];
            else c = inputText[pos];
            if (c == ';' || c == '.' || c == '{' || c == '}' || c == '(' || c == ')' || c == ',' || c == '|' || c == '&' || c == '=' || c == '!' || c == '<' || c == '>' || c == '*' || c == '/' || c == '+' || c == '-') return true;
            return false;
        }

        bool IsItReservedWord()
        {
            if (curLex == "using" || curLex == "namespace" || curLex == "class" || curLex == "void" || curLex == "string" || curLex == "bool" || curLex == "int" || curLex == "break" || curLex == "continue" || curLex == "return" || curLex == "for" || curLex == "if" || curLex == "else" || curLex == "true" || curLex == "false") return true;
            return false;
        }

        bool IsEmptyChar(bool next)
        {
            char c;
            if (next) c = inputText[pos + 1];
            else c = inputText[pos];
            if (c == ' ' || c == '\n' || c == '\r') return true;
            return false;
        }

        public List<string[]> GetLexes()
        {
            List<string[]> output = new List<string[]>();

            for (int i = 0; i < prog.Count; i++)
            {
                string num = (i+1).ToString();
                string type = "";
                string lex = "";
                if (prog[i].number!=-1)
                switch (prog[i].type)
                {
                    case 0:
                        {
                            type = "Зарезервированное слово";
                            lex = reservedWords[prog[i].number];
                            break;
                        }
                    case 1:
                        {
                            type = "Значащий разделитель";
                            lex = separators[prog[i].number];
                            break;
                        }
                    case 2:
                        {
                            type = "Идентификатор";
                            lex = identifers[prog[i].number];
                            break;
                        }
                }

                string[] outLex = { num, type, lex };
                output.Add(outLex);
            }
            return output;
        }

    }

    //лексема
    public struct Lex
    {
        public int type;
        public int number;
    }
}
