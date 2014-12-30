using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace С_Question_Parser_and_Analyser
{
    class CodeGenerator
    {
        ProgTextStore prog;
        string asmText = "";

        public CodeGenerator(ProgTextStore prog)
        {
            this.prog = prog;
        }


        public string ExploreTree()
        {
            try
            {
                ProgF(prog.lexTree);
            }
            catch { }
            return asmText;

        }

        //проход по незначащим термам - потомкам вершины дерева
        int GoDeep(int childPos, Tree<Lex> tree)
        {
            while (tree.Children[childPos].Value.type == LexType.unTerm)
            {
                switch (tree.Children[childPos].Value.number)
                {
                    case 1: UsingF(tree.Children[childPos]); break;
                    case 2: LibFuncNameF(tree.Children[childPos]); break;
                    case 3: ClassF(tree.Children[childPos]); break;
                    case 4: FuncF(tree.Children[childPos]); break;
                    case 5: ParamsF(tree.Children[childPos]); break;
                    case 6: OperatorF(tree.Children[childPos]); break;
                    case 7: FuncCallF(tree.Children[childPos]); break;
                    case 8: ParamsInF(tree.Children[childPos]); break;
                    case 9: VarAssignF(tree.Children[childPos]); break;
                    case 10: VarDelcarF(tree.Children[childPos]); break;



                    case 14: MathF(tree.Children[childPos]); break;
                    case 15: AndF(tree.Children[childPos]); break;
                    case 16: EqualF(tree.Children[childPos]); break;
                    case 17: CompareF(tree.Children[childPos]); break;
                    case 18: PlusF(tree.Children[childPos]); break;
                    case 19: MultF(tree.Children[childPos]); break;
                    case 20: UnarF(tree.Children[childPos]); break;
                }

                if (childPos == tree.Children.Count - 1) break;
                childPos++;

            }
            return childPos;
        }


        //функции, генерирующие ассемблерный код для каждой вершины - незначащего терма
        void ProgF(Tree<Lex> tree)
        {
            int childPos = 0;

            childPos = GoDeep(childPos, tree);

            childPos += 3;

            childPos = GoDeep(childPos, tree);

        }

        void UsingF(Tree<Lex> tree)
        {

        }

        void LibFuncNameF(Tree<Lex> tree)
        {
            //for (int childPos = 0; childPos < tree.Children.Count; childPos++)
            //{
            //    if (tree.Children[childPos].Value.type == LexType.id) asmText += prog.identifers[tree.Children[childPos].Value.number];
            //    else asmText += prog.separators[tree.Children[childPos].Value.number];
            //}

            //идентификатор функции, продумать для функций ввода вывода
            asmText += prog.identifers[tree.Children[tree.Children.Count - 1].Value.number];
        }

        void ClassF(Tree<Lex> tree)
        {
            int childPos = 1;

            //asmText += "public class " + prog.identifers[tree.Children[childPos].Value.number] + " {\n";

            childPos += 2;

            childPos = GoDeep(childPos, tree);

            //asmText += "}\n\n";
        }


        struct strgs
        {
            public string intStr;
            public string strStr;
        };

        void FuncF(Tree<Lex> tree)
        {
            int childPos = 0;

            asmText += /*"public " +*/ prog.reservedWords[tree.Children[childPos].Value.number];
            childPos++;
            asmText += " " + prog.identifers[tree.Children[childPos].Value.number] + " (";
            childPos += 2;
            childPos = GoDeep(childPos, tree);
            asmText += ") {\n";

            //объявление всех переменных
            strgs str = new strgs();
            str.intStr = "int";
            str.strStr = "string";
            str = FindVars(tree, str);
            if (str.intStr != "int") asmText += str.intStr.Remove(str.intStr.Length - 1) + ";\n";
            if (str.strStr != "string") asmText += str.strStr.Remove(str.strStr.Length - 1) + ";\n";

            asmText += "_asm {\n";
            childPos += 2;
            childPos = GoDeep(childPos, tree);
            asmText += "}\n}\n\n";
        }

        //находит переменные ниже по дереву и объявляет их вначале функции, переменные с одинаковыми названиями переименовывает
        strgs FindVars(Tree<Lex> tree, strgs str)
        {
            if (tree.Value.type == LexType.unTerm && tree.Value.number == 10)
            {
                if (tree.Children[0].Value.number == 6)
                {
                    for (int i = 1; i < tree.Children.Count; i += 2)
                    {
                        if (tree.Children[1].Value.type == LexType.unTerm)
                        {
                            str.intStr += " " + prog.identifers[tree.Children[1].Children[0].Value.number];
                        }
                        else str.intStr += " " + prog.identifers[tree.Children[1].Value.number];
                        str.intStr += ",";
                    }
                }

                else
                {
                    if (tree.Children[0].Value.number == 4)
                    {
                        for (int i = 1; i < tree.Children.Count; i += 2)
                        {
                            if (tree.Children[1].Value.type == LexType.unTerm)
                            {
                                str.strStr += " " + prog.identifers[tree.Children[1].Children[0].Value.number];
                            }
                            else str.strStr += " " + prog.identifers[tree.Children[1].Value.number];
                            str.strStr += ",";
                        }
                    }
                }
            }

            for (int i = 0; i < tree.Children.Count; i++)
            {
                str = FindVars(tree.Children[i], str);
            }
            return str;
        }

        void ParamsF(Tree<Lex> tree)
        {
            for (int childPos = 0; childPos < tree.Children.Count; childPos += 3)
            {
                asmText += prog.reservedWords[tree.Children[childPos].Value.number] + " ";
                asmText += prog.identifers[tree.Children[childPos + 1].Value.number];
                if (childPos + 2 != tree.Children.Count) asmText += ", ";
            }
        }

        void OperatorF(Tree<Lex> tree)
        {
            if (tree.Children[0].Value.type == LexType.unTerm)
            {
                GoDeep(0, tree);
            }
            else
            {
                if (tree.Children[0].Value.number == 7)
                {
                    //break
                }
                else if (tree.Children[0].Value.number == 8)
                {
                    //continue
                }
                else if (tree.Children[0].Value.number == 9)
                {
                    //return
                }
            }

        }

        void FuncCallF(Tree<Lex> tree)
        {

            asmText += "pusha;\n";

            GoDeep(2, tree);

            asmText += "call ";
            GoDeep(0, tree);//идентификатор функции
            asmText += ";\n";

            if (tree.Children[2].Value.type == LexType.unTerm)
            {
                int paramNum = (tree.Children[2].Children.Count + 1) / 3;
                asmText += "add esp, " + paramNum * 4 + ";\n";
            }

            asmText += "popa;\n";
        }

        void ParamsInF(Tree<Lex> tree)
        {
            //помещаем каждый параметр в стек, после его подсчета
            for (int childPos = 0; childPos < tree.Children.Count; childPos++)
            {
                if (tree.Children[childPos].Value.type == LexType.unTerm)
                {
                    GoDeep(childPos, tree);
                    asmText += "push eax;\n";
                }
            }

        }

        void VarAssignF(Tree<Lex> tree)
        {
            GoDeep(2, tree);
            asmText += "mov " + prog.identifers[tree.Children[0].Value.number] + ", eax;\n";
        }

        void VarDelcarF(Tree<Lex> tree)
        {
            //нужно пропускать объявление и только присваивать
            for (int i = 1; i < tree.Children.Count; i++)
            {
                if (tree.Children[i].Value.type == LexType.unTerm)
                {
                    GoDeep(i, tree);
                }
            }
        }

        void ForOperatorF()
        {

        }

        void IfOperatorF()
        {

        }

        void IfForBodyF()
        {

        }

        void MathF(Tree<Lex> tree)
        {
            GoDeep(0, tree);

            for (int i = 2; i < tree.Children.Count; i += 2)
            {
                asmText += "push eax;\n";
                GoDeep(i, tree);
                asmText += "pop ebx;\n";
                asmText += "or eax, ebx;\n"; //хз как реализовать ИЛИ
            }
        }

        void AndF(Tree<Lex> tree)
        {
            GoDeep(0, tree);

            for (int i = 2; i < tree.Children.Count; i += 2)
            {
                asmText += "push eax;\n";
                GoDeep(i, tree);
                asmText += "pop ebx;\n";
                asmText += "and eax, ebx;\n"; //хз как реализовать И
            }
        }

        void EqualF(Tree<Lex> tree)
        {
            GoDeep(0, tree);

            for (int i = 2; i < tree.Children.Count; i += 2)
            {
                asmText += "push eax;\n";
                GoDeep(i, tree);
                asmText += "pop ebx;\n";
                asmText += "cmp eax, ebx;\n"; //хз как реализовать сравнение и сравнение с отрицание
                if (tree.Children[i - 1].Value.number == 11) asmText += "not eax;\n";
            }
        }

        void CompareF(Tree<Lex> tree)
        {
            GoDeep(0, tree);

            for (int i = 2; i < tree.Children.Count; i += 2)
            {
                asmText += "push eax;\n";
                GoDeep(i, tree);
                asmText += "pop ebx;\n";
                asmText += "cmp eax, ebx;\n"; //хз как реализовать сравнение больше меньше
                if (tree.Children[i - 1].Value.number == 11) asmText += "not eax;\n";
            }
        }

        void PlusF(Tree<Lex> tree)
        {
            GoDeep(0, tree);

            for (int i = 2; i < tree.Children.Count; i += 2)
            {
                //asmText += "mov ebx, eax;\n";
                asmText += "push eax;\n";
                GoDeep(i, tree);
                asmText += "pop ebx;\n";
                if (tree.Children[i - 1].Value.number == 16) asmText += "add eax, ebx;\n";
                if (tree.Children[i - 1].Value.number == 17) asmText += "sub eax, ebx;\nneg eax;\n";
            }
        }

        void MultF(Tree<Lex> tree)
        {
            GoDeep(0, tree);

            for (int i = 2; i < tree.Children.Count; i += 2)
            {
                asmText += "push eax;\n";
                GoDeep(i, tree);
                asmText += "pop ebx;\n";
                if (tree.Children[i - 1].Value.number == 18) asmText += "imul ebx;\n";
                if (tree.Children[i - 1].Value.number == 19) asmText += "idiv ebx;\n"; 
            }
        }

        void UnarF(Tree<Lex> tree)
        {
            int childPos = 0;

            if (tree.Children[0].Value.type == LexType.separ && (tree.Children[0].Value.number == 17 || tree.Children[0].Value.number == 20))
            {
                childPos++;
            }

            //unTerm
            if (tree.Children[childPos].Value.type == LexType.unTerm) GoDeep(childPos, tree);
            //number
            else if (tree.Children[childPos].Value.type == LexType.num) asmText += "mov eax, " + prog.numericConst[tree.Children[childPos].Value.number] + ";\n";
            //true, false
            else if (tree.Children[childPos].Value.type == LexType.reserv)
            {
                if (tree.Children[childPos].Value.number == 13) asmText += "mov eax, 1;\n";
                if (tree.Children[childPos].Value.number == 14) asmText += "mov eax, 0;\n";
            }
            //unTerm
            else if (tree.Children[childPos].Value.type == LexType.separ) asmText += GoDeep(childPos + 1, tree);
            //id 
            else if (tree.Children[childPos].Value.type == LexType.id) asmText += "mov eax, " + prog.identifers[tree.Children[childPos].Value.number] + ";\n";

            //- or !
            if (tree.Children[0].Value.type == LexType.separ)
            {
                if (tree.Children[0].Value.number == 17) asmText += "neg eax;\n";
                else if (tree.Children[0].Value.number == 20) asmText += "not eax;\n";
            }

        }

        void StringMathF()
        {

        }

        void IncrF()
        {

        }










        //писать функции на ассемблере в вижуал си
        //создать структуру для хранения значений идентификаторов типов данных или сразу в ассемблер фигачить
        //решить, делать триады или сразу в ассебмлер, почитать про перевод разных конструкций в ассемблер
        //обход синтаксического дерева рекурсивной функцией и генерация ассемблерного кода

        //функции пишем на языке ассемблера, встроенные функции просто вызываем, основная часть проги - открытие консоли,вывод данных, ввод данных происходит через вызов встроеных функций
    }
}
