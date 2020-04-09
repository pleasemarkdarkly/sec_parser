namespace FileList
{
    partial class GoogleBasedParser
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkIterate = new System.Windows.Forms.CheckBox();
            this.textFolder = new System.Windows.Forms.TextBox();
            this.btnSetFolder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnProcess = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.textOutPut = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkSEC = new System.Windows.Forms.CheckBox();
            this.checkDocs = new System.Windows.Forms.CheckBox();
            this.checkEmail = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textCriteria = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkListSamples = new System.Windows.Forms.CheckedListBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkIterate);
            this.panel1.Controls.Add(this.textFolder);
            this.panel1.Controls.Add(this.btnSetFolder);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(737, 40);
            this.panel1.TabIndex = 0;
            // 
            // checkIterate
            // 
            this.checkIterate.AutoSize = true;
            this.checkIterate.Checked = true;
            this.checkIterate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIterate.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkIterate.Location = new System.Drawing.Point(70, 20);
            this.checkIterate.Name = "checkIterate";
            this.checkIterate.Size = new System.Drawing.Size(612, 17);
            this.checkIterate.TabIndex = 4;
            this.checkIterate.Text = "Search folder by folder (works best for 5000+ docs)";
            this.checkIterate.UseVisualStyleBackColor = true;
            // 
            // textFolder
            // 
            this.textFolder.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::FileList.Properties.Settings.Default, "DefaultFolder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.textFolder.Location = new System.Drawing.Point(70, 0);
            this.textFolder.Name = "textFolder";
            this.textFolder.Size = new System.Drawing.Size(612, 20);
            this.textFolder.TabIndex = 2;
            this.textFolder.Text = global::FileList.Properties.Settings.Default.DefaultFolder;
            // 
            // btnSetFolder
            // 
            this.btnSetFolder.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSetFolder.Location = new System.Drawing.Point(682, 0);
            this.btnSetFolder.Name = "btnSetFolder";
            this.btnSetFolder.Size = new System.Drawing.Size(55, 40);
            this.btnSetFolder.TabIndex = 1;
            this.btnSetFolder.Text = "Choose Folder";
            this.btnSetFolder.UseVisualStyleBackColor = true;
            this.btnSetFolder.Click += new System.EventHandler(this.btnSetFolder_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "folder:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnProcess
            // 
            this.btnProcess.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnProcess.Location = new System.Drawing.Point(682, 0);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(55, 31);
            this.btnProcess.TabIndex = 3;
            this.btnProcess.Text = "Search!";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // textOutPut
            // 
            this.textOutPut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textOutPut.Location = new System.Drawing.Point(3, 3);
            this.textOutPut.Multiline = true;
            this.textOutPut.Name = "textOutPut";
            this.textOutPut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textOutPut.Size = new System.Drawing.Size(723, 312);
            this.textOutPut.TabIndex = 1;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 442);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(737, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkSEC);
            this.panel2.Controls.Add(this.checkDocs);
            this.panel2.Controls.Add(this.checkEmail);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.btnProcess);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 67);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(737, 31);
            this.panel2.TabIndex = 4;
            // 
            // checkSEC
            // 
            this.checkSEC.AutoSize = true;
            this.checkSEC.Checked = true;
            this.checkSEC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSEC.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkSEC.Location = new System.Drawing.Point(400, 0);
            this.checkSEC.Name = "checkSEC";
            this.checkSEC.Size = new System.Drawing.Size(108, 31);
            this.checkSEC.TabIndex = 3;
            this.checkSEC.Text = "Public SEC filings";
            this.checkSEC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkSEC.UseVisualStyleBackColor = true;
            // 
            // checkDocs
            // 
            this.checkDocs.AutoSize = true;
            this.checkDocs.Checked = true;
            this.checkDocs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkDocs.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkDocs.Location = new System.Drawing.Point(216, 0);
            this.checkDocs.Name = "checkDocs";
            this.checkDocs.Size = new System.Drawing.Size(184, 31);
            this.checkDocs.TabIndex = 2;
            this.checkDocs.Text = "Documents (specify folder above)";
            this.checkDocs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkDocs.UseVisualStyleBackColor = true;
            // 
            // checkEmail
            // 
            this.checkEmail.AutoSize = true;
            this.checkEmail.Checked = true;
            this.checkEmail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkEmail.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkEmail.Location = new System.Drawing.Point(70, 0);
            this.checkEmail.Name = "checkEmail";
            this.checkEmail.Size = new System.Drawing.Size(146, 31);
            this.checkEmail.TabIndex = 1;
            this.checkEmail.Text = "E-Mail (local outlook only)";
            this.checkEmail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkEmail.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 31);
            this.label2.TabIndex = 0;
            this.label2.Text = "Types of searches:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.textCriteria);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(737, 27);
            this.panel3.TabIndex = 5;
            // 
            // textCriteria
            // 
            this.textCriteria.Dock = System.Windows.Forms.DockStyle.Top;
            this.textCriteria.Location = new System.Drawing.Point(70, 0);
            this.textCriteria.Name = "textCriteria";
            this.textCriteria.Size = new System.Drawing.Size(667, 20);
            this.textCriteria.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 27);
            this.label3.TabIndex = 1;
            this.label3.Text = "Criteria:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 98);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(737, 344);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textOutPut);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(729, 318);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Output";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkListSamples);
            this.tabPage2.Controls.Add(this.panel4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(729, 318);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Search criteria quick reference";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkListSamples
            // 
            this.checkListSamples.CheckOnClick = true;
            this.checkListSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkListSamples.FormattingEnabled = true;
            this.checkListSamples.Items.AddRange(new object[] {
            "\"Jan Wallace\" and \"Kyleen Cane\"",
            "\"Michael Cane\" or \"Kyleen Cane\" or \"Cane Clark\"",
            "\"Petition\" near \"relief\"",
            "\"wallace\" near \"cane\" and (\"kyleen\" or \"michael\") and \"davi skin\""});
            this.checkListSamples.Location = new System.Drawing.Point(3, 27);
            this.checkListSamples.Name = "checkListSamples";
            this.checkListSamples.Size = new System.Drawing.Size(723, 274);
            this.checkListSamples.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.button1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(723, 24);
            this.panel4.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(205, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Examples of search criteria with operators:";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(598, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 24);
            this.button1.TabIndex = 2;
            this.button1.Text = "Copy sample criteria";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // GoogleBasedParser
            // 
            this.AcceptButton = this.btnProcess;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 465);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Name = "GoogleBasedParser";
            this.Text = "Google-based E-Mail + Docs + SEC (sql based) search tool with Excel output";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textFolder;
        private System.Windows.Forms.Button btnSetFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.TextBox textOutPut;
        private System.Windows.Forms.CheckBox checkIterate;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkDocs;
        private System.Windows.Forms.CheckBox checkEmail;
        private System.Windows.Forms.CheckBox checkSEC;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textCriteria;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckedListBox checkListSamples;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
    }
}