using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Windows.Forms;

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

            string errorText = "Неизвестная ошибка";

            while (pos < (inputText.Length - 1))
            {
                curLex = "";
                pos++;

                //разделители
                if (IsSymbEmpty(pos)) continue;
                else if (inputText[pos] == '(' || inputText[pos] == ')' || inputText[pos] == ';' || inputText[pos] == '{'
                    || inputText[pos] == '}' || inputText[pos] == ',' || inputText[pos] == '.' || inputText[pos] == '*')
                {
                    curLex += inputText[pos];
                    AddLex(1);
                    continue;
                }
                else if (inputText[pos] == '=' || inputText[pos] == '>' || inputText[pos] == '<' || inputText[pos] == '!')
                {
                    curLex += inputText[pos];
                    if (inputText[pos + 1] == '=') AddNextSymb();
                    AddLex(1);
                    continue;
                }
                else if (inputText[pos] == '-' || inputText[pos] == '+')
                {
                    curLex += inputText[pos];
                    if (inputText[pos + 1] == inputText[pos]) AddNextSymb();
                    if (!IsSymbDigit(pos + 1)) AddLex(1);
                    continue;
                }
                else if (inputText[pos] == '|' || inputText[pos] == '&')
                {
                    curLex += inputText[pos];
                    if (inputText[pos + 1] == inputText[pos])
                    {
                        AddNextSymb();
                        AddLex(1);
                        continue;
                    }
                    else { errorText = "Неверная логическая операция"; break; }; //error
                }
                //числовые константы
                else if (IsSymbDigit(pos))
                {
                    if (inputText[pos - 1] == '-') curLex += inputText[pos - 1];
                    curLex += inputText[pos];
                    while (IsSymbDigit(pos + 1)) AddNextSymb();

                    if (inputText[pos + 1] == '.')
                    {
                        AddNextSymb();
                        while (IsSymbDigit(pos + 1)) AddNextSymb();

                        if (!IsSymbEmpty(pos + 1) && !IsSymbSeparator(pos + 1)) { errorText = "Неверное число"; break; }  //error

                        AddLex(4);
                        continue;
                    }
                    else if (!IsSymbEmpty(pos + 1) && !IsSymbSeparator(pos + 1)) { errorText = "Неверное число"; break; }  //error
                    AddLex(4);
                    continue;
                }
                //идентификаторы и ключевые слова
                else if (IsSymbLetter(pos))
                {
                    curLex += inputText[pos];
                    while (IsSymbLetter(pos + 1) || IsSymbDigit(pos + 1)) AddNextSymb();

                    if (reservedWords.IndexOf(curLex) == -1) AddLex(2);
                    else AddLex(0);
                }
                //комментарии
                else if (inputText[pos] == '/')
                {
                    if (inputText[pos + 1] == '*')
                    {
                        pos++;
                        int posTemp = pos;
                        while (posTemp < (inputText.Length - 1))
                        {
                            if (inputText[posTemp++] == '*')
                            {
                                if (inputText[posTemp++] == '/') 
                                { 
                                    pos = posTemp; 
                                    break; 
                                }
                            }
                        }
                        if (posTemp >= (inputText.Length - 1)) { errorText = "Комментарий не закрыт"; break; } //error
                    }
                    else
                    {
                        curLex += inputText[pos];
                        AddLex(1);
                    }
                    continue;
                }
                //строковые константы
                else if (inputText[pos] == '"')
                {
                    bool err = false;
                    while (pos < (inputText.Length - 1))
                    {

                        if (inputText[pos++] == '\\')
                        {
                            AddNextSymb();
                            continue;
                        }
                        else if (inputText[pos] == '"') break;
                        else if (inputText[pos] == '\n')
                        {
                            err = true;
                            break;
                        }
                        else curLex += inputText[pos];
                    }
                    if (err) { errorText = "В строке отсутсвует закрывающая кавычка"; break; }; //error

                    AddLex(3);
                    continue;

                }
                else { errorText = "Неизвестный символ"; break; };

            }


            if (pos >= (inputText.Length - 1)) return;
            else
            {
                int stringNum = 1;
                for (int i = 0; i < pos; i++)
                {
                    if (inputText[i]=='\n') stringNum++;
                }
                MessageBox.Show("Ошибка в строке " + stringNum + "\n" + errorText);
            }

        }


        char AddNextSymb()
        {
            pos++;
            curLex += inputText[pos];
            return inputText[pos];
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
                case 3:
                    {
                        if (stringConst.IndexOf(curLex) == -1) stringConst.Add(curLex);
                        lexID = stringConst.IndexOf(curLex);
                        break;
                    }
                case 4:
                    {
                        double curLexD = double.Parse(curLex, System.Globalization.CultureInfo.InvariantCulture);
                        if (numericConst.IndexOf(curLexD) == -1) numericConst.Add(curLexD);
                        lexID = numericConst.IndexOf(curLexD);
                        break;
                    }
            }

            Lex buf = new Lex();
            buf.type = type;
            buf.number = lexID;
            prog.Add(buf);
            curLex = "";
        }

        bool IsSymbLetter(int pos)
        {
            char c = inputText[pos];
            if (c == 'a' || c == 'b' || c == 'c' || c == 'd' || c == 'e' || c == 'f' || c == 'g' || c == 'h' || c == 'i'
                || c == 'j' || c == 'k' || c == 'l' || c == 'm' || c == 'n' || c == 'o' || c == 'p' || c == 'q' || c == 'r'
                || c == 's' || c == 't' || c == 'u' || c == 'v' || c == 'w' || c == 'x' || c == 'y' || c == 'z' || c == '_'
                || c == 'A' || c == 'B' || c == 'C' || c == 'D' || c == 'E' || c == 'F' || c == 'G' || c == 'H' || c == 'I'
                || c == 'J' || c == 'K' || c == 'L' || c == 'M' || c == 'N' || c == 'O' || c == 'P' || c == 'Q' || c == 'R'
                || c == 'S' || c == 'T' || c == 'U' || c == 'V' || c == 'W' || c == 'X' || c == 'Y' || c == 'Z') return true;
            return false;
        }

        bool IsSymbSeparator(int pos)
        {
            char c = inputText[pos];
            if (c == ';' || c == '.' || c == '{' || c == '}' || c == '(' || c == ')' || c == ',' || c == '|' || c == '&'
                || c == '=' || c == '!' || c == '<' || c == '>' || c == '*' || c == '/' || c == '+' || c == '-') return true;
            return false;
        }


        bool IsSymbEmpty(int pos)
        {
            char c = inputText[pos];
            if (c == ' ' || c == '\n' || c == '\r') return true;
            return false;
        }

        bool IsSymbDigit(int pos)
        {
            char c = inputText[pos];
            if (c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9'
                || c == '0') return true;
            return false;
        }

        public List<string[]> GetLexes()
        {
            List<string[]> output = new List<string[]>();

            for (int i = 0; i < prog.Count; i++)
            {
                string num = (i + 1).ToString();
                string type = "";
                string lex = "";
                if (prog[i].number != -1)
                    switch (prog[i].type)
                    {
                        case 0:
                            {
                                type = "Зарезерв. слово";
                                lex = reservedWords[prog[i].number];
                                break;
                            }
                        case 1:
                            {
                                type = "Разделитель";
                                lex = separators[prog[i].number];
                                break;
                            }
                        case 2:
                            {
                                type = "Идентификатор";
                                lex = identifers[prog[i].number];
                                break;
                            }
                        case 3:
                            {
                                type = "Строка";
                                lex = stringConst[prog[i].number];
                                break;
                            }
                        case 4:
                            {
                                type = "Число";
                                lex = numericConst[prog[i].number].ToString();
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
