using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.ShowDialog();
            if (openDialog.FileName == "") return;

            string inputText = File.ReadAllText(openDialog.FileName, Encoding.GetEncoding(1251));

            pars = new LexParser(inputText);
            pars.Parse();

            listView1.Items.Clear();

            List<string[]> lexes = pars.GetLexesToOut();

            for (int i = 0; i < lexes.Count; i++)
            {
                listView1.Items.Add(new ListViewItem(lexes[i]));
            }
        }
    }
}
