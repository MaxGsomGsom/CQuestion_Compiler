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
            this.button1LoadFile = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1num = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3lec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // button1LoadFile
            // 
            this.button1LoadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1LoadFile.Location = new System.Drawing.Point(9, 368);
            this.button1LoadFile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1LoadFile.Name = "button1LoadFile";
            this.button1LoadFile.Size = new System.Drawing.Size(114, 32);
            this.button1LoadFile.TabIndex = 0;
            this.button1LoadFile.Text = "Загрузить файл";
            this.button1LoadFile.UseVisualStyleBackColor = true;
            this.button1LoadFile.Click += new System.EventHandler(this.button1LoadFile_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1num,
            this.columnHeader2type,
            this.columnHeader3lec});
            this.listView1.Location = new System.Drawing.Point(9, 10);
            this.listView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(572, 354);
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
            // Form1Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 410);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.button1LoadFile);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MinimumSize = new System.Drawing.Size(100, 200);
            this.Name = "Form1Main";
            this.Text = "C Question";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1LoadFile;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1num;
        private System.Windows.Forms.ColumnHeader columnHeader2type;
        private System.Windows.Forms.ColumnHeader columnHeader3lec;
    }
}

