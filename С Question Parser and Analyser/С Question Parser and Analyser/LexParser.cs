using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using System.Windows.Forms;



namespace С_Question_Parser_and_Analyser
{

 
    public class LexParser
    {
        ProgTextStore outProgText = new ProgTextStore();

        int pos;
        string inputText;
        string curLex;

        public LexParser(string inputText)
        {
            this.inputText = inputText;
        }

        public void Parse()
        {
            pos = -1;

            bool minusInNum = false;

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
                    AddLex(LexType.separ);
                    continue;
                }
                else if (inputText[pos] == '=' || inputText[pos] == '>' || inputText[pos] == '<' || inputText[pos] == '!')
                {
                    curLex += inputText[pos];
                    if (inputText[pos + 1] == '=') AddNextSymb();
                    AddLex(LexType.separ);
                    continue;
                }
                else if (inputText[pos] == '-' || inputText[pos] == '+')
                {
                    curLex += inputText[pos];
                    if (IsSymbLetter(pos - 1) && inputText[pos + 1] == inputText[pos])
                    {
                        AddNextSymb();
                        AddLex(LexType.separ);
                    }
                    else if (!IsSymbDigit(pos + 1) || inputText[pos] == '+') AddLex(LexType.separ);
                    else minusInNum = true;
                    continue;
                }
                else if (inputText[pos] == '|' || inputText[pos] == '&')
                {
                    curLex += inputText[pos];
                    if (inputText[pos + 1] == inputText[pos])
                    {
                        AddNextSymb();
                        AddLex(LexType.separ);
                        continue;
                    }
                    else { errorText = "Неверная логическая операция"; break; }; //error
                }
                //числовые константы
                else if (IsSymbDigit(pos))
                {
                    if (inputText[pos - 1] == '-' && minusInNum) curLex += inputText[pos - 1];
                    curLex += inputText[pos];
                    while (IsSymbDigit(pos + 1)) AddNextSymb();

                    if (inputText[pos + 1] == '.')
                    {
                        AddNextSymb();
                        while (IsSymbDigit(pos + 1)) AddNextSymb();

                        if (!IsSymbEmpty(pos + 1) && !IsSymbSeparator(pos + 1)) { errorText = "Неверное число"; break; }  //error

                        AddLex(LexType.num);
                        continue;
                    }
                    else if (!IsSymbEmpty(pos + 1) && !IsSymbSeparator(pos + 1)) { errorText = "Неверное число"; break; }  //error
                    AddLex(LexType.num);
                    minusInNum = false;
                    continue;
                }
                //идентификаторы и ключевые слова
                else if (IsSymbLetter(pos))
                {
                    curLex += inputText[pos];
                    while (IsSymbLetter(pos + 1) || IsSymbDigit(pos+1)) AddNextSymb(); 

                    if (outProgText.reservedWords.IndexOf(curLex) == -1) AddLex(LexType.id);
                    else AddLex(LexType.reserv);
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
                        AddLex(LexType.separ);
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

                    AddLex(LexType.str);
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
                MessageBox.Show("Ошибка в строке " + stringNum + "\n" + errorText, "Ошибка лексического разбора", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        char AddNextSymb()
        {
            pos++;
            curLex += inputText[pos];
            return inputText[pos];
        }

        void AddLex(LexType type)
        {
            int lexID = 0;

            switch (type)
            {
                case LexType.reserv:
                    {
                        lexID = outProgText.reservedWords.IndexOf(curLex);
                        break;
                    }
                case LexType.separ:
                    {
                        lexID = outProgText.separators.IndexOf(curLex);
                        break;
                    }
                case LexType.id:
                    {
                        if (outProgText.identifers.IndexOf(curLex) == -1) outProgText.identifers.Add(curLex);
                        lexID = outProgText.identifers.IndexOf(curLex);
                        break;
                    }
                case LexType.str:
                    {
                        if (outProgText.stringConst.IndexOf(curLex) == -1) outProgText.stringConst.Add(curLex);
                        lexID = outProgText.stringConst.IndexOf(curLex);
                        break;
                    }
                case LexType.num:
                    {
                        double curLexD = double.Parse(curLex, System.Globalization.CultureInfo.InvariantCulture);
                        if (outProgText.numericConst.IndexOf(curLexD) == -1) outProgText.numericConst.Add(curLexD);
                        lexID = outProgText.numericConst.IndexOf(curLexD);
                        break;
                    }
            }

            Lex buf = new Lex();
            buf.type = type;
            buf.number = lexID;
            outProgText.lexList.Add(buf);
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
            if (c == ' ' || c == '\n' || c == '\r' || c == '\t') return true;
            return false;
        }

        bool IsSymbDigit(int pos)
        {
            char c = inputText[pos];
            if (c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9'
                || c == '0') return true;
            return false;
        }

        public List<string[]> GetLexesToOut()
        {
            List<string[]> output = new List<string[]>();

            for (int i = 0; i < outProgText.lexList.Count; i++)
                {
                    string num = (i + 1).ToString();
                    string type = "";
                    string lex = "";
                    if (outProgText.lexList[i].number != -1)
                        switch (outProgText.lexList[i].type)
                        {
                            case LexType.reserv:
                                {
                                    type = "Зарезерв. слово";
                                    lex = outProgText.reservedWords[outProgText.lexList[i].number];
                                    break;
                                }
                            case LexType.separ:
                                {
                                    type = "Разделитель";
                                    lex = outProgText.separators[outProgText.lexList[i].number];
                                    break;
                                }
                            case LexType.id:
                                {
                                    type = "Идентификатор";
                                    lex = outProgText.identifers[outProgText.lexList[i].number];
                                    break;
                                }
                            case LexType.str:
                                {
                                    type = "Строка";
                                    lex = outProgText.stringConst[outProgText.lexList[i].number];
                                    break;
                                }
                            case LexType.num:
                                {
                                    type = "Число";
                                    lex = outProgText.numericConst[outProgText.lexList[i].number].ToString();
                                    break;
                                }
                        }

                    string[] outLex = { num, type, lex };
                    output.Add(outLex);
                } 

            return output;
        }

        public ProgTextStore LexesStore
        {
            get
            {
                return outProgText;
            }
        }
    }

}
