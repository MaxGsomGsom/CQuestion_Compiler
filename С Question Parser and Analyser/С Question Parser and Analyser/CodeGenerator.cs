using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace С_Question_Parser_and_Analyser
{

    //надо бы сделать чтобы переменные с одинаковыми названиями переименовывались (это в анализаторе) 
    //и проверялось объявлена ли переменная вообще
    ////обработка сложения строк не реализована в stringmath
    ////сравнение строк не реализовано equal можно через цикл
    class CodeGenerator
    {
        ProgTextStore prog;
        string asmText = "";
        int labelNum = 0;
        string assingFlag = ""; //нужно для работы gets
        string continueFlag = "";
        string breakFlag = "";
        string returnFlag = "";

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


        //функции, генерирующие ассемблерный код для каждой вершины - нетерминала
        void ProgF(Tree<Lex> tree)
        {
            int childPos = 0;

            childPos = GoDeep(childPos, tree); //оно идет по всем дочерним вершинам, начиная с заданной и до первой терминальной

            childPos += 3;

            childPos = GoDeep(childPos, tree);

        }

        void UsingF(Tree<Lex> tree)
        {
            //добавление библиотек, вместо систем добавляются стандартные сишные

            if (prog.identifers[tree.Children[1].Children[tree.Children[1].Children.Count - 1].Value.number] == "System")
                asmText += "#include <stdlib.h>\n#include <stdio.h>\n\n";
        }


        void LibFuncNameF(Tree<Lex> tree)
        {

            //берем только последний идентификатор, так как клссов нет
            //сделано распознавание чтения с клавы, вывод в консоль, конвертация строк и чисел

            if (prog.identifers[tree.Children[tree.Children.Count - 1].Value.number] == "WriteLine")
            {
                asmText += "call puts;\n";
            }
            else if (prog.identifers[tree.Children[tree.Children.Count - 1].Value.number] == "ReadLine")
            {
                if (assingFlag != "")
                {
                    asmText += "push " + assingFlag + ";\n";
                }
                else
                {
                    asmText += "push _str_buf;\n";
                }
                asmText += "call gets;\n";
                asmText += "add esp, 4;\n";

            }
            else if (prog.identifers[tree.Children[tree.Children.Count - 1].Value.number] == "ToString")
            {
                asmText = asmText.Remove(asmText.Length - 10);
                asmText += "push 10;\n";
                if (assingFlag != "")
                {
                    asmText += "push " + assingFlag + ";\n";
                }
                else
                {
                    asmText += "push _str_buf;\n";
                }
                asmText += "push eax;\n";
                asmText += "call _itoa;\n";
                asmText += "add esp, 8;\n";

            }
            else if (prog.identifers[tree.Children[tree.Children.Count - 1].Value.number] == "ToInt32")
            {
                asmText += "call atoi;\n";
            }
            else asmText += "call " + prog.identifers[tree.Children[tree.Children.Count - 1].Value.number] + ";\n";
        }

        void ClassF(Tree<Lex> tree)
        {
            //классов нет,  фиг с ними

            GoDeep(3, tree);

        }



        void FuncF(Tree<Lex> tree)
        {
            int childPos = 0;
            labelNum++;
            string returnFlagOld = returnFlag;
            returnFlag = "labelRET" + labelNum;

            asmText += prog.reservedWords[tree.Children[childPos].Value.number];
            childPos++;
            asmText += " " + prog.identifers[tree.Children[childPos].Value.number] + " (";
            childPos += 2;
            childPos = GoDeep(childPos, tree);
            asmText += ") {\n";

            //объявление всех переменных и строк
            strgs str = new strgs();
            str.intStr = "int";
            str.strStr = "char *_str_buf = new char[255],";
            str.strConst = "";
            str = FindVars(tree, str);
            if (str.intStr != "int") asmText += str.intStr.Remove(str.intStr.Length - 1) + ";\n";
            if (str.strStr != "char") asmText += str.strStr.Remove(str.strStr.Length - 1) + ";\n";
            if (str.strConst != "") asmText += str.strConst;

            asmText += "_asm {\n";
            //asmText += "pusha;\n";
            childPos += 2;
            childPos = GoDeep(childPos, tree);
            asmText += returnFlag + ":\n"; //для return
            //asmText += "popa;\n";
            asmText += "}\n}\n\n";

            returnFlag = returnFlagOld;
        }

        //находит переменные ниже по дереву и объявляет их вначале функции
        //объявляет строки вначале
        strgs FindVars(Tree<Lex> tree, strgs str)
        {
            if (tree.Value.type == LexType.str)
            {
                str.strConst += "char *_str_" + tree.Value.number + " = \"" + prog.stringConst[tree.Value.number] + "\";\n";
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
                        else str.intStr += " " + prog.identifers[tree.Children[i].Value.number];
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
                                str.strStr += " *" + prog.identifers[tree.Children[1].Children[0].Value.number] + " = new char[255]";
                            }
                            else str.strStr += " *" + prog.identifers[tree.Children[i].Value.number] + " = new char[255]";
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
                    if (breakFlag != "") asmText += "jmp " + breakFlag + ";\n";
                }
                else if (tree.Children[0].Value.number == 8)
                {
                    //continue
                    if (continueFlag != "") asmText += "jmp " + continueFlag + ";\n";
                }
                else if (tree.Children[0].Value.number == 9)
                {
                    //return
                    if (tree.Children[1].Value.type == LexType.id) asmText += "mov eax, " + prog.identifers[tree.Children[1].Value.number] + ";\n";
                    asmText += "jmp " + returnFlag + ";\n";
                }
            }

        }

        void FuncCallF(Tree<Lex> tree)
        {

            GoDeep(2, tree);//параметры в стек

            GoDeep(0, tree);//вызов функции

            if (tree.Children[2].Value.type == LexType.unTerm)
            {
                int paramNum = 0;
                for (int i = 0; i < tree.Children[2].Children.Count; i++)
                {
                    if (tree.Children[2].Children[i].Value.type == LexType.unTerm) paramNum++;
                }
                asmText += "add esp, " + paramNum * 4 + ";\n";
            }

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
            assingFlag = prog.identifers[tree.Children[0].Value.number];

            GoDeep(2, tree);
            asmText += "mov " + prog.identifers[tree.Children[0].Value.number] + ", eax;\n";

            assingFlag = "";
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
            int childNum = 2;
            //даже предусмотрена ситуация если в заголовке цикла пустые блоки
            labelNum += 3;
            int l1 = labelNum, l2 = labelNum + 1, l3 = labelNum + 2;

            string continueFlagOld = continueFlag;
            string breakFlagOld = breakFlag;

            continueFlag = "labelFOR" + l3;
            breakFlag = "labelFOR" + l2;


            if (tree.Children[2].Value.type == LexType.unTerm)
            {
                GoDeep(2, tree);//объявление переменной цикла
                childNum += 2;
            }
            else childNum++;
            asmText += "labelFOR" + l1 + ":\n";
            GoDeep(childNum, tree);//условие
            childNum += 2;

            asmText += "cmp eax, 0;\n";
            asmText += "je labelFOR" + l2 + ";\n";

            GoDeep(tree.Children.Count - 1, tree);//тело

            if (tree.Children.Count - 3 == childNum)
            {
                asmText += "labelFOR" + l3 + ":\n"; //метка для continue
                GoDeep(childNum, tree);//инкремент в конце цикла

            }
            asmText += "jmp labelFOR" + l1 + ";\n";
            asmText += "labelFOR" + l2 + ":\n";

            continueFlag = continueFlagOld;
            breakFlag = breakFlagOld;
        }


        void IfOperatorF(Tree<Lex> tree)
        {
            labelNum += 2;
            int l1 = labelNum, l2 = labelNum + 1;
            GoDeep(2, tree);//условие

            asmText += "cmp eax, 0;\n";
            asmText += "je labelIF" + l1 + ";\n";

            GoDeep(4, tree);//если 1
            if (tree.Children.Count > 6) asmText += "jmp labelIF" + l2 + ";\n";
            asmText += "labelIF" + l1 + ":\n";

            if (tree.Children.Count == 7)
            {
                GoDeep(6, tree); //если 0
                asmText += "labelIF" + l2 + ":\n";
            }
        }

        void IfForBodyF(Tree<Lex> tree)
        {
            if (tree.Children[0].Value.type == LexType.unTerm) GoDeep(0, tree);
            else GoDeep(1, tree);

        }

        void MathF(Tree<Lex> tree)
        {
            GoDeep(0, tree);

            for (int i = 2; i < tree.Children.Count; i += 2)
            {
                asmText += "push eax;\n";
                GoDeep(i, tree);
                asmText += "pop ebx;\n";
                asmText += "or eax, ebx;\n"; // ИЛИ
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
                asmText += "and eax, ebx;\n"; // И
            }
        }


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
                asmText += "je labelCMP" + l1 + ";\n";
                if (tree.Children[i - 1].Value.number == 10) asmText += "mov eax, 0;\n"; // ==
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
            labelNum += 2;
            int l1 = labelNum, l2 = labelNum + 1;

            GoDeep(0, tree);

            for (int i = 2; i < tree.Children.Count; i += 2)
            {
                asmText += "push eax;\n";
                GoDeep(i, tree);
                asmText += "pop ebx;\n";
                asmText += "cmp eax, ebx;\n";
                if (tree.Children[i - 1].Value.number == 12) asmText += "jg labelCMP" + l1 + ";\n"; // <
                else if (tree.Children[i - 1].Value.number == 13) asmText += "jl labelCMP" + l1 + ";\n"; // >
                else if (tree.Children[i - 1].Value.number == 14) asmText += "jle labelCMP" + l1 + ";\n"; // >=
                else if (tree.Children[i - 1].Value.number == 15) asmText += "jge labelCMP" + l1 + ";\n"; // <=

                asmText += "mov eax, 0;\n";
                asmText += "jmp labelCMP" + l2 + ";\n";
                asmText += "labelCMP" + l1 + ":\n";
                asmText += "mov eax, 1;\n";
                asmText += "labelCMP" + l2 + ":\n";
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
                if (tree.Children[i - 1].Value.number == 19) asmText += "mov ecx, eax;\nmov eax, ebx;\nmov edx, 0;\nidiv ecx;\n";
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
                //отрицание !
                else if (tree.Children[0].Value.number == 20)
                {


                    labelNum += 2;
                    int l1 = labelNum, l2 = labelNum + 1;


                    asmText += "cmp eax, 0;\n";
                    asmText += "je labelCMP" + l1 + ";\n";
                    asmText += "mov eax, 0;\n";
                    asmText += "jmp labelCMP" + l2 + ";\n";
                    asmText += "labelCMP" + l1 + ":\n";
                    asmText += "mov eax, 1;\n";
                    asmText += "labelCMP" + l2 + ":\n";



                }
            }

        }

        void StringMathF(Tree<Lex> tree)
        {
            asmText += "mov eax, _str_" + tree.Children[0].Value.number + ";\n";

        }

        void IncrF(Tree<Lex> tree)
        {

            if (tree.Children[1].Value.number == 21) asmText += "inc " + prog.identifers[tree.Children[0].Value.number] + ";\n";
            else if (tree.Children[1].Value.number == 22) asmText += "inc " + prog.identifers[tree.Children[0].Value.number] + ";\n";

        }





    }
}
