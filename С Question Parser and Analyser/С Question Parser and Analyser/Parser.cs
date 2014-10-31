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
            pos = 0;
            prog = new List<Lex>();
            identifers = new List<string>();
            stringConst = new List<string>();
            numericConst = new List<double>();


        newLex:
            curLex = "";

            switch (GetNextSymb())
            {
                //незначащие разделители
                case ' ':
                case '\n':
                case '\r': goto newLex;
                //значащие разделители
                case '.':
                case '{':
                case '}':
                case '(':
                case ')':
                case ',':
                case '*':
                case '/':
                case ';': AddLex(1); goto newLex;


            }




        }

        char GetNextSymb()
        {
            curLex += inputText[pos];

            if (IsSeparator(inputText[pos]) && !IsSeparator(inputText[pos + 1])) 
            {
                AddLex(1);
                goto newLex;
            }

            pos++;
            return inputText[pos];
        }

        void AddLex(int type)
        {
            int lexID = 0;

            switch (type)
            {
                case 1:
                    {
                        lexID = separators.IndexOf(curLex);
                        break;
                    }
            }

            Lex buf = new Lex();
            buf.type = type;
            buf.number = lexID;
            prog.Add(buf);
        }

        bool IsSeparator(char c)
        {
            if (c == ' ' || c == '\n' || c == '\r' || c == ';' || c == '.' || c == '{' || c == '}' || c == '(' || c == ')' || c == ',' || c == '|' || c == '&' || c == '=' || c == '!' || c == '<' || c == '>' || c == '*' || c == '/' || c == '+' || c == '-')
            {
                return true;
            }
            else return false;
        }

    }

    //лексема
    public struct Lex
    {
        public int type;
        public int number;
    }
}
