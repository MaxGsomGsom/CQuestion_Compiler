namespace С_Question_Parser_and_Analyser
{
    partial class Form1Main
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1LoadFile = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1num = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3lec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1List = new System.Windows.Forms.TabPage();
            this.tabPage2Tree = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1List.SuspendLayout();
            this.tabPage2Tree.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1LoadFile
            // 
            this.button1LoadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1LoadFile.Location = new System.Drawing.Point(12, 453);
            this.button1LoadFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1LoadFile.Name = "button1LoadFile";
            this.button1LoadFile.Size = new System.Drawing.Size(234, 39);
            this.button1LoadFile.TabIndex = 0;
            this.button1LoadFile.Text = "Загрузить текст программы";
            this.button1LoadFile.UseVisualStyleBackColor = true;
            this.button1LoadFile.Click += new System.EventHandler(this.button1LoadFile_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1num,
            this.columnHeader2type,
            this.columnHeader3lec});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(749, 401);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1num
            // 
            this.columnHeader1num.Text = "Номер";
            this.columnHeader1num.Width = 62;
            // 
            // columnHeader2type
            // 
            this.columnHeader2type.Text = "Тип";
            this.columnHeader2type.Width = 157;
            // 
            // columnHeader3lec
            // 
            this.columnHeader3lec.Text = "Лексема";
            this.columnHeader3lec.Width = 343;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1List);
            this.tabControl1.Controls.Add(this.tabPage2Tree);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(763, 436);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1List
            // 
            this.tabPage1List.Controls.Add(this.listView1);
            this.tabPage1List.Location = new System.Drawing.Point(4, 25);
            this.tabPage1List.Name = "tabPage1List";
            this.tabPage1List.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1List.Size = new System.Drawing.Size(755, 407);
            this.tabPage1List.TabIndex = 0;
            this.tabPage1List.Text = "Список лексем";
            this.tabPage1List.UseVisualStyleBackColor = true;
            // 
            // tabPage2Tree
            // 
            this.tabPage2Tree.Controls.Add(this.treeView1);
            this.tabPage2Tree.Location = new System.Drawing.Point(4, 25);
            this.tabPage2Tree.Name = "tabPage2Tree";
            this.tabPage2Tree.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2Tree.Size = new System.Drawing.Size(755, 407);
            this.tabPage2Tree.TabIndex = 1;
            this.tabPage2Tree.Text = "Дерево лексем";
            this.tabPage2Tree.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(749, 401);
            this.treeView1.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(184, 80);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(183, 24);
            this.toolStripMenuItem1.Text = "Развернуть все";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(183, 24);
            this.toolStripMenuItem2.Text = "Свернуть все";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // Form1Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 505);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1LoadFile);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(127, 235);
            this.Name = "Form1Main";
            this.Text = "C Question";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1List.ResumeLayout(false);
            this.tabPage2Tree.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1LoadFile;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1num;
        private System.Windows.Forms.ColumnHeader columnHeader2type;
        private System.Windows.Forms.ColumnHeader columnHeader3lec;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1List;
        private System.Windows.Forms.TabPage tabPage2Tree;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}

