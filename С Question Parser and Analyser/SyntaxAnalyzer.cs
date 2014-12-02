using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Forms;

//отладить на разных примерах, добавить ошибки

namespace С_Question_Parser_and_Analyser
{
    class SyntaxAnalyzer
    {
        ProgTextStore progStore;
        int lexPos = 0;
        Tree<Lex> curNode;
        bool errorShowed = false;

        public SyntaxAnalyzer(ProgTextStore progStore)
        {
            this.progStore = progStore;
            Lex root = new Lex();
            root.type = LexType.unTerm;
            root.number = 0;
            curNode = new Tree<Lex>(root);
        }

        public void Analyze()
        {

            curNode = curNode.Root;
            lexPos = 0;

            errorShowed = false;

            ProgF();

            curNode = curNode.Root;
            progStore.lexTree = curNode;

        }

        public ProgTextStore LexesStore
        {
            get { return progStore; }
        }

        Lex GetCurLex()
        {
            if (lexPos == progStore.lexList.Count)
            {
                Lex l = new Lex();
                l.type = LexType.reserv;
                l.number = -1;
                return l;
            }
            return progStore.lexList[lexPos];
        }

        Lex GetNextLex()
        {
            if (lexPos == progStore.lexList.Count)
            {
                Lex l = new Lex();
                l.type = LexType.reserv;
                l.number = -1;
                return l;
            }
            return progStore.lexList[lexPos+1];
        }

        void AddLexNode()
        {
            curNode.Add(new Tree<Lex>(GetCurLex()));
            lexPos++;
        }

        void AddEmptyNode(int unTerm)
        {
            Lex p = new Lex();
            p.type = LexType.unTerm;
            p.number = unTerm;

            curNode.Add(new Tree<Lex>(p));
        }

        void GoUpTree()
        {
            curNode = curNode.Parent;
        }

        void GoDeepTree()
        {
            curNode = curNode.Children[curNode.Children.Count - 1];
        }

        bool IsTrueLexNum(int[] m)
        {
            for (int i = 0; i < m.Length; i++)
            {
                if (GetCurLex().number == m[i])
                {
                    return true;
                }
            }
            return false;
        }

        void ShowError(string text)
        {
            if (!errorShowed)
            {
                MessageBox.Show("Ошибка в лексеме " + (lexPos+1) + "\nОжидалась лексема: "+ text, "Ошибка синтаксического разбора", MessageBoxButtons.OK, MessageBoxIcon.Error);
                errorShowed = true;
            }
        }


        void ProgF()
        {

            while (GetCurLex().type == LexType.reserv && GetCurLex().number == 0) 
            {
                AddEmptyNode(1);
                UsingF();
            }

            if (GetCurLex().type == LexType.reserv && GetCurLex().number == 1)
            {
                AddLexNode();
            }
            else ShowError("namespace");

            if (GetCurLex().type == LexType.id)
            {
                AddLexNode();
            }
            else ShowError("идентификатор");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 2)
            {
                AddLexNode();
            }
            else ShowError("{");

            while (GetCurLex().type == LexType.reserv && GetCurLex().number == 2)
            {
                AddEmptyNode(3);
                ClassF();
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 3)
            {
                AddLexNode();
            }
            else ShowError("}");

        }

        void UsingF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && GetCurLex().number == 0)
            {
                AddLexNode();
            }
            else ShowError("using");

            if (GetCurLex().type == LexType.id)
            {
                AddEmptyNode(2);
                LibFuncNameF();
            }
            else ShowError("идентификатор");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
            {
                AddLexNode();
            }
            else ShowError(";");

            GoUpTree();
        }

        void LibFuncNameF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id)
            {
                AddLexNode();
            }
            else ShowError("идентификатор");

            while (GetCurLex().type == LexType.separ && GetCurLex().number == 1)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id)
                {
                    AddLexNode();
                }
                else ShowError("идентификатор");
            }

            GoUpTree();

        }

        void ClassF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && GetCurLex().number == 2)
            {
                AddLexNode();
            }
            else ShowError("class");

            if (GetCurLex().type == LexType.id)
            {
                AddLexNode();
            }
            else ShowError("идентификатор");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 2)
            {
                AddLexNode();
            }
            else ShowError("{");

            while (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6 }))
            {
                AddEmptyNode(4);
                FuncF();
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 3)
            {
                AddLexNode();
            }
            else ShowError("}");

            GoUpTree();
        }

        void FuncF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6}))
            {
                AddLexNode();
            }
            else ShowError("тип функции");

            if (GetCurLex().type == LexType.id)
            {
                AddLexNode();
            }
            else ShowError("идентификатор");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 4)
            {
                AddLexNode();
            }
            else ShowError("(");

            if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6}))
            {
                AddEmptyNode(5);
                ParamsF();
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 5)
            {
                AddLexNode();
            }
            else ShowError(")");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 2)
            {
                AddLexNode();
            }
            else ShowError("{");

            while (GetCurLex().type == LexType.id || 
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] {3,4,5,6,7,8,9,10,11})))
            {
                AddEmptyNode(6);
                OperatorF();
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 3)
            {
                AddLexNode();
            }
            else ShowError("}");

            GoUpTree();
        }

        void ParamsF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6 }))
            {
                AddLexNode();
            }
            else ShowError("тип параметра");

            if (GetCurLex().type == LexType.id)
            {
                AddLexNode();
            }
            else ShowError("идентификатор");

            while (GetCurLex().type == LexType.separ && GetCurLex().number == 6)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6 }))
                {
                    AddLexNode();
                }
                else ShowError("тип параметра");

                if (GetCurLex().type == LexType.id)
                {
                    AddLexNode();
                }
                else ShowError("идентификатор");
            }

            GoUpTree();
        }

        void OperatorF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6 }))
            {
                AddEmptyNode(10);
                VarDelcarF();

                if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
                {
                    AddLexNode();
                }
                else ShowError(";");
            }
            else if (GetCurLex().type == LexType.reserv && GetCurLex().number == 10)
            {
                AddEmptyNode(11);
                ForOperatorF();
            }
            else if (GetCurLex().type == LexType.reserv && GetCurLex().number == 11)
            {
                AddEmptyNode(12);
                IfOperatorF();
            }
            else if (GetCurLex().type == LexType.reserv && GetCurLex().number == 7)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
                {
                    AddLexNode();
                }
                else ShowError(";");
            }
            else if (GetCurLex().type == LexType.reserv && GetCurLex().number == 8)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
                {
                    AddLexNode();
                }
                else ShowError(":");
            }
            else if (GetCurLex().type == LexType.reserv && GetCurLex().number == 9)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id)
                {
                    AddLexNode();
                }

                if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
                {
                    AddLexNode();
                }
                else ShowError(";");
            }
            else if (GetCurLex().type == LexType.id)
            {
                if (GetNextLex().type == LexType.separ && (GetNextLex().number == 1 || GetNextLex().number == 4))
                {
                    AddEmptyNode(7);
                    FuncCallF();

                    if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
                    {
                        AddLexNode();
                    }
                    else ShowError(";");
                }
                else if (GetNextLex().type == LexType.separ && GetNextLex().number == 7)
                {
                    AddEmptyNode(9);
                    VarAssignF();

                    if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
                    {
                        AddLexNode();
                    }
                    else ShowError(";");
                }
                else if (GetNextLex().type == LexType.separ && (GetNextLex().number == 21 || GetNextLex().number == 22))
                {
                    AddEmptyNode(22);
                    IncrF();

                    if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
                    {
                        AddLexNode();
                    }
                    else ShowError(";");
                }
                else ShowError("оператор");
            }
            else MessageBox.Show("оператор");

            GoUpTree();
        }

        void FuncCallF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id)
            {
                AddEmptyNode(2);
                LibFuncNameF();
            }
            else ShowError("идентификатор");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 4)
            {
                AddLexNode();
            }
            else ShowError("(");

            if (GetCurLex().type == LexType.str || GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode(8);
                ParamsInF();
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 5)
            {
                AddLexNode();
            }
            else ShowError(")");

            GoUpTree();
        }

        void ParamsInF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode(14);
                MathF();
            }
            else if (GetCurLex().type == LexType.str)
            {
                AddEmptyNode(21);
                StringMathF();
            }
            else ShowError("идентификатор или выражение");

            while (GetCurLex().type == LexType.separ && GetCurLex().number == 6)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode(14);
                    MathF();
                }
                else if (GetCurLex().type == LexType.str)
                {
                    AddEmptyNode(21);
                    StringMathF();
                }
                else ShowError("идентификатор или выражение");
            }

            GoUpTree();

        }

        void VarAssignF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id)
            {
                AddLexNode();
            }
            else ShowError("идентификатор");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 7)
            {
                AddLexNode();
            }
            else ShowError("=");

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode(14);
                MathF();
            }
            else if (GetCurLex().type == LexType.str)
            {
                AddEmptyNode(21);
                StringMathF();
            }
            else ShowError("идентификатор или выражение");

            GoUpTree();
        }

        void VarDelcarF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6 }))
            {
                AddLexNode();
            }
            else ShowError("тип переменной");

            if (GetCurLex().type == LexType.id)
            {
                if (GetNextLex().type == LexType.separ && GetNextLex().number == 7)
                {
                    AddEmptyNode(9);
                    VarAssignF();
                }
                else
                {
                    AddLexNode();
                }
            }
            else ShowError("идентификатор");

            while (GetCurLex().type == LexType.separ && GetCurLex().number == 6)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id)
                {
                    if (GetNextLex().type == LexType.separ && GetNextLex().number == 7)
                    {
                        AddEmptyNode(9);
                        VarAssignF();
                    }
                    else
                    {
                        AddLexNode();
                    }
                }
                else ShowError("идентификатор");
            }

            GoUpTree();
        }

        void ForOperatorF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && GetCurLex().number == 10)
            {
                AddLexNode();
            }
            else ShowError("for");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 4)
            {
                AddLexNode();
            }
            else ShowError("(");

            if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6 }))
            {
                AddEmptyNode(10);
                VarDelcarF();
            }
            else if (GetCurLex().type == LexType.id)
            {
                if (GetNextLex().type == LexType.separ && GetNextLex().number == 7)
                {
                    AddEmptyNode(9);
                    VarAssignF();
                }
                else if (GetNextLex().type == LexType.separ && (GetNextLex().number == 21 || GetNextLex().number == 22))
                {
                    AddEmptyNode(22);
                    IncrF();
                }
                else ShowError("выражение");
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
            {
                AddLexNode();
            }
            else ShowError(";");

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode(14);
                MathF();
            }
            else ShowError("выражение");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
            {
                AddLexNode();
            }
            else ShowError(";");

            if (GetCurLex().type == LexType.id)
            {
                if (GetNextLex().type == LexType.separ && GetNextLex().number == 7)
                {
                    AddEmptyNode(9);
                    VarAssignF();
                }
                else if (GetNextLex().type == LexType.separ && (GetNextLex().number == 21 || GetNextLex().number == 22))
                {
                    AddEmptyNode(22);
                    IncrF();
                }
                else ShowError("выражение");
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 5)
            {
                AddLexNode();
            }
            else ShowError(")");

            if (GetCurLex().type == LexType.id ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11 }))
                || (GetCurLex().type == LexType.separ && GetCurLex().number == 2))
            {
                AddEmptyNode(13);
                IfForBodyF();
            }
            else ShowError("оператор или блок");

            GoUpTree();
        }

        void IfOperatorF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && GetCurLex().number == 11)
            {
                AddLexNode();
            }
            else ShowError("if");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 4)
            {
                AddLexNode();
            }
            else ShowError("(");

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode(14);
                MathF();
            }
            else ShowError("выражение");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 5)
            {
                AddLexNode();
            }
            else ShowError(")");


            if (GetCurLex().type == LexType.id ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11 }))
                || (GetCurLex().type == LexType.separ && GetCurLex().number == 2))
            {
                AddEmptyNode(13);
                IfForBodyF();
            }
            else ShowError("оператор или блок");

            if (GetCurLex().type == LexType.reserv && GetCurLex().number == 12)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11 }))
                    || (GetCurLex().type == LexType.separ && GetCurLex().number == 2))
                {
                    AddEmptyNode(13);
                    IfForBodyF();
                }
                else ShowError("оператор или блок");
            }

            GoUpTree();
        }

        void IfForBodyF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11 })))
            {
                AddEmptyNode(6);
                OperatorF();
            }
            else if (GetCurLex().type == LexType.separ && GetCurLex().number == 2)
            {
                AddLexNode();

                while (GetCurLex().type == LexType.id ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11 })))
                {
                    AddEmptyNode(6);
                    OperatorF();
                }

                if (GetCurLex().type == LexType.separ && GetCurLex().number == 3)
                {
                    AddLexNode();
                }
                else ShowError("}");
            }
            else ShowError("оператор или блок");

            GoUpTree();
        }

        void MathF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode(15);
                AndF();
            }
            else ShowError("выражение");

            while (GetCurLex().type == LexType.separ && GetCurLex().number == 8)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode(15);
                    AndF();
                }
                else ShowError("выражение");
            }

            GoUpTree();
        }

        void AndF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode(16);
                EqualF();
            }
            else ShowError("выражение");

            while (GetCurLex().type == LexType.separ && GetCurLex().number == 9)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode(16);
                    EqualF();
                }
                else ShowError("выражение");
            }

            GoUpTree();
        }

        void EqualF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode(17);
                CompareF();
            }
            else ShowError("выражение");

            while (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] {11,10}))
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode(17);
                    CompareF();
                }
                else ShowError("выражение");
            }

            GoUpTree();
        }

        void CompareF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode(18);
                PlusF();
            }
            else ShowError("выражение");

            while (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 12,13,14,15 }))
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode(18);
                    PlusF();
                }
                else ShowError("выражение");
            }

            GoUpTree();
        }

        void PlusF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode(19);
                MultF();
            }
            else ShowError("выражение");

            while (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 16,17 }))
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode(19);
                    MultF();
                }
                else ShowError("выражение");
            }

            GoUpTree();
        }

        void MultF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode(20);
                UnarF();
            }
            else ShowError("выражение");

            while (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 18, 19 }))
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode(20);
                    UnarF();
                }
                else ShowError("выражение");
            }

            GoUpTree();
        }

        void UnarF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.separ && (GetCurLex().number == 17 || GetCurLex().number == 20))
            {
                AddLexNode();
            }

            if (GetCurLex().type == LexType.id)
            {
                if (GetNextLex().type == LexType.separ && (GetNextLex().number == 21 || GetNextLex().number == 22))
                {
                    AddEmptyNode(22);
                    IncrF();
                }
                else if (GetNextLex().type == LexType.separ && (GetNextLex().number == 1 || GetNextLex().number == 4))
                {
                    AddEmptyNode(7);
                    FuncCallF();
                }
                else
                {
                    AddLexNode();
                }
            }
            else if (GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.reserv && (GetCurLex().number == 13 || GetCurLex().number == 14)))
            {
                AddLexNode();
            }
            else if (GetCurLex().type == LexType.separ && GetCurLex().number == 4)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode(14);
                    MathF();
                }
                else ShowError("выражение");

                if (GetCurLex().type == LexType.separ && GetCurLex().number == 5)
                {
                    AddLexNode();
                }
                else ShowError(")");
            }
            else ShowError("выражение");

            GoUpTree();
        }

        void StringMathF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.str)
            {
                AddLexNode();
            }
            else ShowError("строковая константа");

            while (GetCurLex().type == LexType.separ && GetCurLex().number == 16)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.str)
                {
                    AddLexNode();
                }
                else ShowError("строковая константа");
            }

            GoUpTree();
        }

        void IncrF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id)
            {
                AddLexNode();
            }
            else ShowError("идентификатор");

            if (GetCurLex().type == LexType.separ && (GetCurLex().number == 21 || GetCurLex().number == 22))
            {
                AddLexNode();
            }
            else ShowError("++ или --");

            GoUpTree();
        }

        Tree<Lex> OptimizeTreeRecursive(Tree<Lex> lexTree)
        {

            for (int i = 0; i < lexTree.Children.Count; i++)
            {
                OptimizeTreeRecursive(lexTree.Children[i]);
            }

            if (lexTree.Parent != null && lexTree.Value.type == LexType.unTerm && lexTree.Parent.Children.Count == 1)
            {
                for (int i = 0; i < lexTree.Children.Count; i++)
                {
                    lexTree.Parent.Add(lexTree.Children[i]);
                }
                lexTree.Parent.RemoveChild(lexTree);
            }

            return lexTree;

        }

        public void OptimizeTree()
        {
            progStore.lexTree = OptimizeTreeRecursive(progStore.lexTree);
        }
    }

    
}
