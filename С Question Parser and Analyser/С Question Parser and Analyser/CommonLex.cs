using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace С_Question_Parser_and_Analyser
{
    //лексема
    public struct Lex
    {
        public LexType type;
        public int number;
    }

    public class ProgTextStore
    {
        //тип тексемы 0
        string[] reservedWordsBuf = {"using", "namespace", "class", "void",
                                            "string", "bool", "int", "break", 
                                            "continue", "return", "for", "if", 
                                            "else", "true", "false"};
        public List<string> reservedWords;
        //тип тексемы 1
        string[] separatorsBuf = {";", ".", "{", "}", "(", ")", ",", 
                                         "=", "||", "&&", "==", "!=", 
                                         "<", ">", ">=", "<=", "+", 
                                         "-", "*", "/", "!", "++", "--"};
        public List<string> separators;
        //тип лексемы 2
        public List<string> identifers;
        //тип лексемы 3
        public List<string> stringConst;
        //тип лексемы 4
        public List<double> numericConst;
        //тип лексемы 5 - нетерминалы
        string[] unTermBuf = { "<<Prog>>", "<<Using>>", "<<LibFuncName>>", "<<Class>>", 
                                 "<<Func>>", "<<Params>>", "<<Operator>>", "<<FuncCall>>",
                                 "<<ParamsIn>>", "<<VarAssign>>", "<<VarDelcar>>",
                                 "<<ForOperator>>", "<<IfOperator>>", "<<IfForBody>>", 
                                 "<<Math>>", "<<And>>", "<<Equal>>", "<<Compare>>", "<<Plus>>", 
                                 "<<Mult>>", "<<Unar>>", "<<StringMath>>", "<<Incr>>"};
        public List<string> unTerm;

        //все лексемы
        public List<Lex> lexList;
        public Tree<Lex> lexTree;

        public ProgTextStore()
        {
            reservedWords = new List<string>(reservedWordsBuf);
            separators = new List<string>(separatorsBuf);
            identifers = new List<string>();
            stringConst = new List<string>();
            numericConst = new List<double>();
            lexList = new List<Lex>();
            lexTree = new Tree<Lex>();
            unTerm = new List<string>(unTermBuf);
        }

    }




    public enum LexType
    {
        reserv,
        separ,
        id,
        str,
        num,
        unTerm
    }

}
