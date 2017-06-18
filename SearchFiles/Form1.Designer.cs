namespace SearchFiles
{
    partial class Search
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Start = new System.Windows.Forms.Button();
            this.Directory = new System.Windows.Forms.TextBox();
            this.Pattern = new System.Windows.Forms.TextBox();
            this.SearchText = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.Tree = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.TimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.FileLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(686, 177);
            this.Start.Name = "StartBtn";
            this.Start.Size = new System.Drawing.Size(121, 23);
            this.Start.TabIndex = 0;
            this.Start.Text = "Начать поиск";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click_1);
            // 
            // Directory
            // 
            this.Directory.Location = new System.Drawing.Point(451, 78);
            this.Directory.Name = "Folder";
            this.Directory.Size = new System.Drawing.Size(229, 20);
            this.Directory.TabIndex = 1;
            // 
            // Pattern
            // 
            this.Pattern.Location = new System.Drawing.Point(451, 125);
            this.Pattern.Name = "Pattern";
            this.Pattern.Size = new System.Drawing.Size(229, 20);
            this.Pattern.TabIndex = 2;
            // 
            // SearchText
            // 
            this.SearchText.Location = new System.Drawing.Point(451, 180);
            this.SearchText.Name = "SearchText";
            this.SearchText.Size = new System.Drawing.Size(229, 20);
            this.SearchText.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(686, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Выбрать папку";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Directory_Click_1);
            // 
            // Tree
            // 
            this.Tree.Location = new System.Drawing.Point(12, 12);
            this.Tree.Name = "Tree";
            this.Tree.Size = new System.Drawing.Size(390, 393);
            this.Tree.TabIndex = 5;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.TimeLabel,
                this.FileLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 408);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(819, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // TimeLabel
            // 
            this.TimeLabel.Name = "TimeLabel2";
            this.TimeLabel.Size = new System.Drawing.Size(49, 17);
            this.TimeLabel.Text = "00:00:00";
            // 
            // FileLabel
            // 
            this.FileLabel.Name = "FileLabel";
            this.FileLabel.Size = new System.Drawing.Size(64, 17);
            this.FileLabel.Text = "Ожидание";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(448, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Стартовая директория";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(448, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Шаблон имени файла";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(448, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Текст в файле";
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 430);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.Tree);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SearchText);
            this.Controls.Add(this.Pattern);
            this.Controls.Add(this.Directory);
            this.Controls.Add(this.Start);
            this.MaximizeBox = false;
            this.Name = "Search";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Поиск файлов";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}