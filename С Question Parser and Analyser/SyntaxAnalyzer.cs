using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace С_Question_Parser_and_Analyser
{
    class SyntaxAnalyzer
    {
        static char[,] relations = { { ' ' } };
        Stack<Lex> lexStack = new Stack<Lex>();
        List<Lex> lexList;

        public SyntaxAnalyzer(List<Lex> lexList)
        {
            this.lexList = lexList;
        }

        public void Analyze() { }
        
    }
}
