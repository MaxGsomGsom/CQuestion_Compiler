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
        int labelNum = 0;

        struct strgs
        {
            public string intStr;
            public string strStr;
            public string strConst;
        };

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
                    case 11: ForOperatorF(tree.Children[childPos]); break;
                    case 12: IfOperatorF(tree.Children[childPos]); break;
                    case 13: IfForBodyF(tree.Children[childPos]); break;
                    case 14: MathF(tree.Children[childPos]); break;
                    case 15: AndF(tree.Children[childPos]); break;
                    case 16: EqualF(tree.Children[childPos]); break;
                    case 17: CompareF(tree.Children[childPos]); break;
                    case 18: PlusF(tree.Children[childPos]); break;
                    case 19: MultF(tree.Children[childPos]); break;
                    case 20: UnarF(tree.Children[childPos]); break;
                    case 21: StringMathF(tree.Children[childPos]); break;
                    case 22: IncrF(tree.Children[childPos]); break;
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
            //тут типа добавление библиотек, сделать чтобы вместо систем добавлялись стандартные сишные
        }

        void LibFuncNameF(Tree<Lex> tree)
        {

            //берем только последний идентификатор, так как клссов нет
            //нужно сделать распознавание принтф, сканф и ещё функцию конвертации строки в число
            asmText += prog.identifers[tree.Children[tree.Children.Count - 1].Value.number];
        }

        void ClassF(Tree<Lex> tree)
        {
            //классов нет, хер с ними

            GoDeep(3, tree);

        }



        void FuncF(Tree<Lex> tree)
        {
            int childPos = 0;

            asmText += prog.reservedWords[tree.Children[childPos].Value.number];
            childPos++;
            asmText += " " + prog.identifers[tree.Children[childPos].Value.number] + " (";
            childPos += 2;
            childPos = GoDeep(childPos, tree);
            asmText += ") {\n";

            //объявление всех переменных и строк
            strgs str = new strgs();
            str.intStr = "int";
            str.strStr = "string";
            str.strConst = "";
            str = FindVars(tree, str);
            if (str.intStr != "int") asmText += str.intStr.Remove(str.intStr.Length - 1) + ";\n"; //сделать распознавание и замену майн
            if (str.strStr != "string") asmText += str.strStr.Remove(str.strStr.Length - 1) + ";\n";
            if (str.strConst != "") asmText += str.strConst;

            asmText += "_asm {\n";
            childPos += 2;
            childPos = GoDeep(childPos, tree);
            asmText += "}\n}\n\n";
        }

        //находит переменные ниже по дереву и объявляет их вначале функции
        //объявляет строки вначале
        //сделать чтобы переменные с одинаковыми названиями переименовывались (это в анализаторе)
        strgs FindVars(Tree<Lex> tree, strgs str)
        {
            if (tree.Value.type == LexType.str)
            {
                str.strConst += "char *str_" + tree.Value.number + " = \"" + prog.stringConst[tree.Value.number] + "\";\n";
            }


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
                    //break сделать
                }
                else if (tree.Children[0].Value.number == 8)
                {
                    //continue сделать
                }
                else if (tree.Children[0].Value.number == 9)
                {
                    //return сделать
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
            //объявление пропускается и идет только присваивание
            for (int i = 1; i < tree.Children.Count; i++)
            {
                if (tree.Children[i].Value.type == LexType.unTerm)
                {
                    GoDeep(i, tree);
                }
            }
        }

        void ForOperatorF(Tree<Lex> tree)
        {
            //не предусмотрена ситуация если в заголовке цикла пустые блоки
            labelNum += 2;
            int l1 = labelNum, l2 = labelNum+1;

            GoDeep(2, tree);//объявление переменной цикла

            asmText += "labelFOR" + l1 + ":\n";
            GoDeep(4, tree);//условие
            asmText += "cmp eax, 0;\n";
            asmText += "je labelFOR" + l2 + ";\n";

            GoDeep(8, tree);//тело

            GoDeep(6, tree);//инкремент в конце цикла
            asmText += "jmp labelFOR" + l1 + ";\n";
            asmText += "labelFOR" + l2 + ":\n";
        }


        void IfOperatorF(Tree<Lex> tree)
        {
            labelNum+=2;
            int l1 = labelNum, l2 = labelNum + 1;
            GoDeep(2, tree);//условие

            asmText += "cmp eax, 0;\n";
            asmText += "je labelIF" + l1 + ";\n";

            GoDeep(4, tree);//если 1
            if (tree.Children.Count > 6) asmText += "jmp labelIF" + l2 + ";\n";
            asmText += "labelIF" + l1 + ":\n";

            if (tree.Children.Count > 6) GoDeep(6, tree);//если 0
            if (tree.Children.Count > 6) asmText += "labelIF" + l2 + ":\n";
        }

        void IfForBodyF(Tree<Lex> tree)
        {
            for (int i = 0; i < tree.Children.Count; i++)
            {
                if (tree.Children[i].Value.type == LexType.unTerm)
                {
                    GoDeep(i, tree);
                }
            }
        }

        void MathF(Tree<Lex> tree)
        {
            GoDeep(0, tree);

            for (int i = 2; i < tree.Children.Count; i += 2)
            {
                asmText += "push eax;\n";
                GoDeep(i, tree);
                asmText += "pop ebx;\n";
                asmText += "or eax, ebx;\n"; //нужно реализовать по шаблону ==
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
                asmText += "and eax, ebx;\n"; //нужно реализовать по шаблону ==
            }
        }

        //сравнение строк не реализовано
        void EqualF(Tree<Lex> tree)
        {
            labelNum += 2;
            int l1 = labelNum, l2 = labelNum + 1;

            GoDeep(0, tree);

            for (int i = 2; i < tree.Children.Count; i += 2)
            {
                asmText += "push eax;\n";
                GoDeep(i, tree);
                asmText += "pop ebx;\n";
                asmText += "cmp eax, ebx;\n"; 
                asmText += "je labelCMP"+l1+";\n";
                if (tree.Children[i-1].Value.number == 10) asmText += "mov eax, 0;\n"; // ==
                else asmText += "mov eax, 1;\n"; // !=
                asmText += "jmp labelCMP" + l2 + ";\n";
                asmText += "labelCMP" + l1 + ":\n";
                if (tree.Children[i - 1].Value.number == 10) asmText += "mov eax, 1;\n";
                else asmText += "mov eax, 0;\n";
                asmText += "labelCMP" + l2 + ":\n";
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
                asmText += "cmp eax, ebx;\n"; //нужно реализовать по шаблону ==
                if (tree.Children[i - 1].Value.number == 11) asmText += "not eax;\n";
            }
        }


        void PlusF(Tree<Lex> tree)
        {
            GoDeep(0, tree);

            for (int i = 2; i < tree.Children.Count; i += 2)
            {
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
                else if (tree.Children[0].Value.number == 20) asmText += "not eax;\n"; //сделать правильное логическое отрицание
            }

        }

        void StringMathF(Tree<Lex> tree)
        {
            asmText += "mov eax, str_" + tree.Children[0].Value.number + ";\n";
            //тут добавить обработку сложения строк и строки с числом
        }

        void IncrF(Tree<Lex> tree)
        {

            if (tree.Children[1].Value.number == 21) asmText += "inc " + prog.identifers[tree.Children[0].Value.number] + ";\n";
            else if (tree.Children[1].Value.number == 22) asmText += "inc " + prog.identifers[tree.Children[0].Value.number] + ";\n";

        }





    }
}
