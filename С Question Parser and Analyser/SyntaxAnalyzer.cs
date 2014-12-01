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

        public SyntaxAnalyzer(ProgTextStore progStore)
        {
            this.progStore = progStore;
            Lex root = new Lex();
            root.type = LexType.reserv;
            root.number = -1;
            curNode = new Tree<Lex>(root);
        }

        public void Analyze()
        {

            curNode = curNode.Root;
            lexPos = 0;

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

        void AddEmptyNode()
        {
            Lex p = new Lex();
            p.type = LexType.reserv;
            p.number = -1;

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



        void ProgF()
        {

            while (GetCurLex().type == LexType.reserv && GetCurLex().number == 0) 
            {
                AddEmptyNode();
                UsingF();
            }

            if (GetCurLex().type == LexType.reserv && GetCurLex().number == 1)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.id)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 2)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            while (GetCurLex().type == LexType.reserv && GetCurLex().number == 2)
            {
                AddEmptyNode();
                ClassF();
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 3)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

        }

        void UsingF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && GetCurLex().number == 0)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.id)
            {
                AddEmptyNode();
                LibFuncNameF();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            GoUpTree();
        }

        void LibFuncNameF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            while (GetCurLex().type == LexType.separ && GetCurLex().number == 1)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id)
                {
                    AddLexNode();
                }
                else MessageBox.Show("Error");
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
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.id)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 2)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            while (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6 }))
            {
                AddEmptyNode();
                FuncF();
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 3)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            GoUpTree();
        }

        void FuncF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6}))
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.id)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 4)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6}))
            {
                AddEmptyNode();
                ParamsF();
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 5)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 2)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            while (GetCurLex().type == LexType.id || 
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] {3,4,5,6,7,8,9,10,11})))
            {
                AddEmptyNode();
                OperatorF();
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 3)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            GoUpTree();
        }

        void ParamsF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6 }))
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.id)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            while (GetCurLex().type == LexType.separ && GetCurLex().number == 6)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6 }))
                {
                    AddLexNode();
                }
                else MessageBox.Show("Error");

                if (GetCurLex().type == LexType.id)
                {
                    AddLexNode();
                }
                else MessageBox.Show("Error");
            }

            GoUpTree();
        }

        void OperatorF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6 }))
            {
                AddEmptyNode();
                VarDelcarF();

                if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
                {
                    AddLexNode();
                }
                else MessageBox.Show("Error");
            }
            else if (GetCurLex().type == LexType.reserv && GetCurLex().number == 10)
            {
                AddEmptyNode();
                ForOperatorF();
            }
            else if (GetCurLex().type == LexType.reserv && GetCurLex().number == 11)
            {
                AddEmptyNode();
                IfOperatorF();
            }
            else if (GetCurLex().type == LexType.reserv && GetCurLex().number == 7)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
                {
                    AddLexNode();
                }
                else MessageBox.Show("Error");
            }
            else if (GetCurLex().type == LexType.reserv && GetCurLex().number == 8)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
                {
                    AddLexNode();
                }
                else MessageBox.Show("Error");
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
                else MessageBox.Show("Error");
            }
            else if (GetCurLex().type == LexType.id)
            {
                if (GetNextLex().type == LexType.separ && (GetNextLex().number == 1 || GetNextLex().number == 4))
                {
                    AddEmptyNode();
                    FuncCallF();

                    if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
                    {
                        AddLexNode();
                    }
                    else MessageBox.Show("Error");
                }
                else if (GetNextLex().type == LexType.separ && GetNextLex().number == 7)
                {
                    AddEmptyNode();
                    VarAssignF();

                    if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
                    {
                        AddLexNode();
                    }
                    else MessageBox.Show("Error");
                }
                else if (GetNextLex().type == LexType.separ && (GetNextLex().number == 21 || GetNextLex().number == 22))
                {
                    AddEmptyNode();
                    IncrF();

                    if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
                    {
                        AddLexNode();
                    }
                    else MessageBox.Show("Error");
                }
                else MessageBox.Show("Error");
            }
            else MessageBox.Show("Ожидался оператор");

            GoUpTree();
        }

        void FuncCallF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id)
            {
                AddEmptyNode();
                LibFuncNameF();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 4)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.str || GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode();
                ParamsInF();
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 5)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            GoUpTree();
        }

        void ParamsInF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode();
                MathF();
            }
            else if (GetCurLex().type == LexType.str)
            {
                AddEmptyNode();
                StringMathF();
            }
            else MessageBox.Show("Error");

            while (GetCurLex().type == LexType.separ && GetCurLex().number == 6)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode();
                    MathF();
                }
                else if (GetCurLex().type == LexType.str)
                {
                    AddEmptyNode();
                    StringMathF();
                }
                else MessageBox.Show("Error");
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
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 7)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode();
                MathF();
            }
            else if (GetCurLex().type == LexType.str)
            {
                AddEmptyNode();
                StringMathF();
            }
            else MessageBox.Show("Error");

            GoUpTree();
        }

        void VarDelcarF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6 }))
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.id)
            {
                if (GetNextLex().type == LexType.separ && GetNextLex().number == 7)
                {
                    AddEmptyNode();
                    VarAssignF();
                }
                else
                {
                    AddLexNode();
                }
            }
            else MessageBox.Show("Error");

            while (GetNextLex().type == LexType.separ && GetNextLex().number == 6)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id)
                {
                    if (GetNextLex().type == LexType.separ && GetNextLex().number == 7)
                    {
                        AddEmptyNode();
                        VarAssignF();
                    }
                    else
                    {
                        AddLexNode();
                    }
                }
                else MessageBox.Show("Error");
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
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 4)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6 }))
            {
                AddEmptyNode();
                VarDelcarF();
            }
            else if (GetCurLex().type == LexType.id)
            {
                if (GetNextLex().type == LexType.separ && GetNextLex().number == 7)
                {
                    AddEmptyNode();
                    VarAssignF();
                }
                else if (GetNextLex().type == LexType.separ && (GetNextLex().number == 21 || GetNextLex().number == 22))
                {
                    AddEmptyNode();
                    IncrF();
                }
                else MessageBox.Show("Error");
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode();
                MathF();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 0)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.id)
            {
                if (GetNextLex().type == LexType.separ && GetNextLex().number == 7)
                {
                    AddEmptyNode();
                    VarAssignF();
                }
                else if (GetNextLex().type == LexType.separ && (GetNextLex().number == 21 || GetNextLex().number == 22))
                {
                    AddEmptyNode();
                    IncrF();
                }
                else MessageBox.Show("Error");
            }

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 5)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.id ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11 })))
            {
                AddEmptyNode();
                IfForBodyF();
            }
            else MessageBox.Show("Error");

            GoUpTree();
        }

        void IfOperatorF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.reserv && GetCurLex().number == 11)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 4)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode();
                MathF();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.separ && GetCurLex().number == 5)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.id ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11 })))
            {
                AddEmptyNode();
                IfForBodyF();
            }
            else MessageBox.Show("Error");

            if (GetCurLex().type == LexType.reserv && GetCurLex().number == 12)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11 })))
                {
                    AddEmptyNode();
                    IfForBodyF();
                }
                else MessageBox.Show("Error");
            }

            GoUpTree();
        }

        void IfForBodyF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11 })))
            {
                AddEmptyNode();
                OperatorF();
            }
            else if (GetCurLex().type == LexType.separ && GetCurLex().number == 2)
            {
                AddLexNode();

                while (GetCurLex().type == LexType.id ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11 })))
                {
                    AddEmptyNode();
                    OperatorF();
                }

                if (GetCurLex().type == LexType.separ && GetCurLex().number == 3)
                {
                    AddLexNode();
                }
                else MessageBox.Show("Error");
            }
            else MessageBox.Show("Error");

            GoUpTree();
        }

        void MathF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
            {
                AddEmptyNode();
                AndF();
            }
            else MessageBox.Show("Error");

            while (GetCurLex().type == LexType.separ && GetCurLex().number == 8)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode();
                    AndF();
                }
                else MessageBox.Show("Error");
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
                AddEmptyNode();
                EqualF();
            }
            else MessageBox.Show("Error");

            while (GetCurLex().type == LexType.separ && GetCurLex().number == 9)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode();
                    EqualF();
                }
                else MessageBox.Show("Error");
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
                AddEmptyNode();
                CompareF();
            }
            else MessageBox.Show("Error");

            while (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] {11,10}))
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode();
                    CompareF();
                }
                else MessageBox.Show("Error");
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
                AddEmptyNode();
                PlusF();
            }
            else MessageBox.Show("Error");

            while (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 12,13,14,15 }))
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode();
                    PlusF();
                }
                else MessageBox.Show("Error");
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
                AddEmptyNode();
                MultF();
            }
            else MessageBox.Show("Error");

            while (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 16,17 }))
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode();
                    MultF();
                }
                else MessageBox.Show("Error");
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
                AddEmptyNode();
                UnarF();
            }
            else MessageBox.Show("Error");

            while (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 18, 19 }))
            {
                AddLexNode();

                if (GetCurLex().type == LexType.id || GetCurLex().type == LexType.num ||
                (GetCurLex().type == LexType.separ && IsTrueLexNum(new int[] { 17, 20, 4 })) ||
                (GetCurLex().type == LexType.reserv && IsTrueLexNum(new int[] { 13, 14 })))
                {
                    AddEmptyNode();
                    UnarF();
                }
                else MessageBox.Show("Error");
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
                    AddEmptyNode();
                    IncrF();
                }
                else if (GetNextLex().type == LexType.separ && (GetNextLex().number == 1 || GetNextLex().number == 4))
                {
                    AddEmptyNode();
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
                    AddEmptyNode();
                    CompareF();
                }
                else MessageBox.Show("Error");

                if (GetCurLex().type == LexType.separ && GetCurLex().number == 5)
                {
                    AddLexNode();
                }
                else MessageBox.Show("Error");
            }
            else MessageBox.Show("Error");

            GoUpTree();
        }

        void StringMathF()
        {
            GoDeepTree();

            if (GetCurLex().type == LexType.str)
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            while (GetCurLex().type == LexType.separ && GetCurLex().number == 16)
            {
                AddLexNode();

                if (GetCurLex().type == LexType.str)
                {
                    AddLexNode();
                }
                else MessageBox.Show("Error");
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
            else MessageBox.Show("Error");

            if (GetNextLex().type == LexType.separ && (GetNextLex().number == 21 || GetNextLex().number == 22))
            {
                AddLexNode();
            }
            else MessageBox.Show("Error");

            GoUpTree();
        }
    }
}
