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
        Parser pars;
        public Form1Main()
        {
            InitializeComponent();
        }

        private void button1LoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.ShowDialog();

            string inputText = File.ReadAllText(openDialog.FileName, Encoding.GetEncoding(1251));

            pars = new Parser(inputText);
            pars.Parse();

        }
    }
}
