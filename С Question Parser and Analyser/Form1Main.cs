using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace С_Question_Parser_and_Analyser
{

    public partial class Form1Main : Form
    {
        LexParser pars;
        public Form1Main()
        {
            InitializeComponent();
        }

        private void button1LoadFile_Click(object sender, EventArgs e)
        {
        

            //вывод лексем
            pars = new LexParser(textBox1text.Text);
            pars.Parse();

            listView1.Items.Clear();

            List<string[]> lexes = pars.GetLexesToOut();

            for (int i = 0; i < lexes.Count; i++)
            {
                listView1.Items.Add(new ListViewItem(lexes[i]));
            }

            //построение дерева
            SyntaxAnalyzer synAn = new SyntaxAnalyzer(pars.LexesStore);
            synAn.Analyze();

            //вывод ассемблера
            CodeGenerator gen = new CodeGenerator(synAn.LexesStore);
            textBox1code.Text = gen.ExploreTree().Replace("\n", "\r\n");

            //вывод дерева
            synAn.OptimizeTree();  //оптимизация ломает генератор кода, поэтому выполняется вконце

            treeView1.Nodes.Clear();

            treeView1.BeginUpdate();
            ViewLexTree(synAn.LexesStore.lexTree, synAn.LexesStore, treeView1.Nodes);
            treeView1.EndUpdate();

        }

        void ViewLexTree(Tree<Lex> lexTree, ProgTextStore progText, TreeNodeCollection nodeCollection)
        {
            string lex = "root";

            switch (lexTree.Value.type)
            {
                case LexType.reserv:
                    {
                        //type = "Зарезерв. слово";
                        lex = progText.reservedWords[lexTree.Value.number];
                        break;
                    }
                case LexType.separ:
                    {
                        //type = "Разделитель";
                        lex = progText.separators[lexTree.Value.number];
                        break;
                    }
                case LexType.id:
                    {
                        //type = "Идентификатор";
                        lex = progText.identifers[lexTree.Value.number];
                        break;
                    }
                case LexType.str:
                    {
                        //type = "Строка";
                        lex = progText.stringConst[lexTree.Value.number];
                        break;
                    }
                case LexType.num:
                    {
                        //type = "Число";
                        lex = progText.numericConst[lexTree.Value.number].ToString();
                        break;
                    }
                case LexType.unTerm:
                    {
                        //type = "нетерминал";
                        lex = progText.unTerm[lexTree.Value.number].ToString();
                        break;
                    }
            }

            nodeCollection.Add(lex);

            for (int i = 0; i < lexTree.Children.Count; i++)
            {
                ViewLexTree(lexTree.Children[i], progText, nodeCollection[nodeCollection.Count - 1].Nodes);
            }

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void button1open_Click(object sender, EventArgs e)
        {

            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.ShowDialog();
            if (openDialog.FileName == "") return;

            textBox1text.Text = File.ReadAllText(openDialog.FileName, Encoding.GetEncoding(1251));
        }

    }
}
