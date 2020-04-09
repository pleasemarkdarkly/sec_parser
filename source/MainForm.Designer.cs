namespace SECcrawler.UI
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabDataStores = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabpMasterIndexes = new System.Windows.Forms.TabPage();
            this.dataGridMasterIndexes = new System.Windows.Forms.DataGridView();
            this.tblFilesfilesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripdataStore = new System.Windows.Forms.ToolStrip();
            this.toolPopulateIndexList = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonProcessIndexes = new System.Windows.Forms.ToolStripButton();
            this.tabCompanies = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tblSECCompanyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripCompanies = new System.Windows.Forms.ToolStrip();
            this.tabPageForms = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tblSECFormsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.relativePathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastProcessedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSourceSecCrawler = new System.Windows.Forms.BindingSource(this.components);
            this.secCrawlerDataDataSet = new SECcrawler.UI.SecCrawlerDataDataSet();
            this.companyCIKDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.companyNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.companyCIKDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.companyNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.formTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.formDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.formPartialURLDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tblFiles_filesTableAdapter = new SECcrawler.UI.SecCrawlerDataDataSetTableAdapters.tblFiles_filesTableAdapter();
            this.tblSEC_CompanyTableAdapter = new SECcrawler.UI.SecCrawlerDataDataSetTableAdapters.tblSEC_CompanyTableAdapter();
            this.tblSEC_FormsTableAdapter = new SECcrawler.UI.SecCrawlerDataDataSetTableAdapters.tblSEC_FormsTableAdapter();
            this.tabControlMain.SuspendLayout();
            this.tabDataStores.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabpMasterIndexes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMasterIndexes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblFilesfilesBindingSource)).BeginInit();
            this.toolStripdataStore.SuspendLayout();
            this.tabCompanies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblSECCompanyBindingSource)).BeginInit();
            this.tabPageForms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblSECFormsBindingSource)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSecCrawler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.secCrawlerDataDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabDataStores);
            this.tabControlMain.Controls.Add(this.tabCompanies);
            this.tabControlMain.Controls.Add(this.tabPageForms);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(752, 444);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabDataStores
            // 
            this.tabDataStores.Controls.Add(this.tabControl2);
            this.tabDataStores.Controls.Add(this.toolStripdataStore);
            this.tabDataStores.Location = new System.Drawing.Point(4, 22);
            this.tabDataStores.Name = "tabDataStores";
            this.tabDataStores.Padding = new System.Windows.Forms.Padding(3);
            this.tabDataStores.Size = new System.Drawing.Size(744, 418);
            this.tabDataStores.TabIndex = 0;
            this.tabDataStores.Text = "Data Store";
            this.tabDataStores.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabpMasterIndexes);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 28);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(738, 387);
            this.tabControl2.TabIndex = 2;
            // 
            // tabpMasterIndexes
            // 
            this.tabpMasterIndexes.Controls.Add(this.dataGridMasterIndexes);
            this.tabpMasterIndexes.Location = new System.Drawing.Point(4, 22);
            this.tabpMasterIndexes.Name = "tabpMasterIndexes";
            this.tabpMasterIndexes.Padding = new System.Windows.Forms.Padding(3);
            this.tabpMasterIndexes.Size = new System.Drawing.Size(730, 361);
            this.tabpMasterIndexes.TabIndex = 0;
            this.tabpMasterIndexes.Text = "Master indexes";
            this.tabpMasterIndexes.UseVisualStyleBackColor = true;
            // 
            // dataGridMasterIndexes
            // 
            this.dataGridMasterIndexes.AutoGenerateColumns = false;
            this.dataGridMasterIndexes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridMasterIndexes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.relativePathDataGridViewTextBoxColumn,
            this.lastProcessedDataGridViewTextBoxColumn});
            this.dataGridMasterIndexes.DataSource = this.tblFilesfilesBindingSource;
            this.dataGridMasterIndexes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridMasterIndexes.Location = new System.Drawing.Point(3, 3);
            this.dataGridMasterIndexes.Name = "dataGridMasterIndexes";
            this.dataGridMasterIndexes.Size = new System.Drawing.Size(724, 355);
            this.dataGridMasterIndexes.TabIndex = 1;
            // 
            // tblFilesfilesBindingSource
            // 
            this.tblFilesfilesBindingSource.DataMember = "tblFiles_files";
            this.tblFilesfilesBindingSource.DataSource = this.bindingSourceSecCrawler;
            // 
            // toolStripdataStore
            // 
            this.toolStripdataStore.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolPopulateIndexList,
            this.toolStripButtonProcessIndexes});
            this.toolStripdataStore.Location = new System.Drawing.Point(3, 3);
            this.toolStripdataStore.Name = "toolStripdataStore";
            this.toolStripdataStore.Size = new System.Drawing.Size(738, 25);
            this.toolStripdataStore.TabIndex = 0;
            this.toolStripdataStore.Text = "toolStrip1";
            // 
            // toolPopulateIndexList
            // 
            this.toolPopulateIndexList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolPopulateIndexList.Image = ((System.Drawing.Image)(resources.GetObject("toolPopulateIndexList.Image")));
            this.toolPopulateIndexList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolPopulateIndexList.Name = "toolPopulateIndexList";
            this.toolPopulateIndexList.Size = new System.Drawing.Size(107, 22);
            this.toolPopulateIndexList.Text = "Populate index list";
            this.toolPopulateIndexList.Click += new System.EventHandler(this.toolPopulateIndexList_Click);
            // 
            // toolStripButtonProcessIndexes
            // 
            this.toolStripButtonProcessIndexes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonProcessIndexes.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonProcessIndexes.Image")));
            this.toolStripButtonProcessIndexes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonProcessIndexes.Name = "toolStripButtonProcessIndexes";
            this.toolStripButtonProcessIndexes.Size = new System.Drawing.Size(178, 22);
            this.toolStripButtonProcessIndexes.Text = "Process all unprocessed indexes";
            this.toolStripButtonProcessIndexes.Click += new System.EventHandler(this.toolStripButtonProcessIndexes_Click);
            // 
            // tabCompanies
            // 
            this.tabCompanies.Controls.Add(this.dataGridView1);
            this.tabCompanies.Controls.Add(this.toolStripCompanies);
            this.tabCompanies.Location = new System.Drawing.Point(4, 22);
            this.tabCompanies.Name = "tabCompanies";
            this.tabCompanies.Padding = new System.Windows.Forms.Padding(3);
            this.tabCompanies.Size = new System.Drawing.Size(744, 418);
            this.tabCompanies.TabIndex = 1;
            this.tabCompanies.Text = "Companies";
            this.tabCompanies.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.companyCIKDataGridViewTextBoxColumn,
            this.companyNameDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.tblSECCompanyBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 28);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(738, 387);
            this.dataGridView1.TabIndex = 1;
            // 
            // tblSECCompanyBindingSource
            // 
            this.tblSECCompanyBindingSource.DataMember = "tblSEC_Company";
            this.tblSECCompanyBindingSource.DataSource = this.bindingSourceSecCrawler;
            // 
            // toolStripCompanies
            // 
            this.toolStripCompanies.Location = new System.Drawing.Point(3, 3);
            this.toolStripCompanies.Name = "toolStripCompanies";
            this.toolStripCompanies.Size = new System.Drawing.Size(738, 25);
            this.toolStripCompanies.TabIndex = 0;
            this.toolStripCompanies.Text = "toolStripCompanies";
            // 
            // tabPageForms
            // 
            this.tabPageForms.Controls.Add(this.dataGridView2);
            this.tabPageForms.Controls.Add(this.toolStrip1);
            this.tabPageForms.Location = new System.Drawing.Point(4, 22);
            this.tabPageForms.Name = "tabPageForms";
            this.tabPageForms.Size = new System.Drawing.Size(744, 418);
            this.tabPageForms.TabIndex = 2;
            this.tabPageForms.Text = "Forms";
            this.tabPageForms.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.companyCIKDataGridViewTextBoxColumn1,
            this.companyNameDataGridViewTextBoxColumn1,
            this.formTypeDataGridViewTextBoxColumn,
            this.formDateDataGridViewTextBoxColumn,
            this.formPartialURLDataGridViewTextBoxColumn});
            this.dataGridView2.DataSource = this.tblSECFormsBindingSource;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(0, 25);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(744, 393);
            this.dataGridView2.TabIndex = 1;
            // 
            // tblSECFormsBindingSource
            // 
            this.tblSECFormsBindingSource.DataMember = "tblSEC_Forms";
            this.tblSECFormsBindingSource.DataSource = this.bindingSourceSecCrawler;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(744, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 444);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(752, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(500, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // relativePathDataGridViewTextBoxColumn
            // 
            this.relativePathDataGridViewTextBoxColumn.DataPropertyName = "RelativePath";
            this.relativePathDataGridViewTextBoxColumn.HeaderText = "RelativePath";
            this.relativePathDataGridViewTextBoxColumn.Name = "relativePathDataGridViewTextBoxColumn";
            // 
            // lastProcessedDataGridViewTextBoxColumn
            // 
            this.lastProcessedDataGridViewTextBoxColumn.DataPropertyName = "LastProcessed";
            this.lastProcessedDataGridViewTextBoxColumn.HeaderText = "LastProcessed";
            this.lastProcessedDataGridViewTextBoxColumn.Name = "lastProcessedDataGridViewTextBoxColumn";
            // 
            // bindingSourceSecCrawler
            // 
            this.bindingSourceSecCrawler.DataSource = this.secCrawlerDataDataSet;
            this.bindingSourceSecCrawler.Position = 0;
            // 
            // secCrawlerDataDataSet
            // 
            this.secCrawlerDataDataSet.DataSetName = "SecCrawlerDataDataSet";
            this.secCrawlerDataDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // companyCIKDataGridViewTextBoxColumn
            // 
            this.companyCIKDataGridViewTextBoxColumn.DataPropertyName = "CompanyCIK";
            this.companyCIKDataGridViewTextBoxColumn.HeaderText = "CompanyCIK";
            this.companyCIKDataGridViewTextBoxColumn.Name = "companyCIKDataGridViewTextBoxColumn";
            // 
            // companyNameDataGridViewTextBoxColumn
            // 
            this.companyNameDataGridViewTextBoxColumn.DataPropertyName = "CompanyName";
            this.companyNameDataGridViewTextBoxColumn.HeaderText = "CompanyName";
            this.companyNameDataGridViewTextBoxColumn.Name = "companyNameDataGridViewTextBoxColumn";
            // 
            // companyCIKDataGridViewTextBoxColumn1
            // 
            this.companyCIKDataGridViewTextBoxColumn1.DataPropertyName = "CompanyCIK";
            this.companyCIKDataGridViewTextBoxColumn1.HeaderText = "CompanyCIK";
            this.companyCIKDataGridViewTextBoxColumn1.Name = "companyCIKDataGridViewTextBoxColumn1";
            // 
            // companyNameDataGridViewTextBoxColumn1
            // 
            this.companyNameDataGridViewTextBoxColumn1.DataPropertyName = "CompanyName";
            this.companyNameDataGridViewTextBoxColumn1.HeaderText = "CompanyName";
            this.companyNameDataGridViewTextBoxColumn1.Name = "companyNameDataGridViewTextBoxColumn1";
            // 
            // formTypeDataGridViewTextBoxColumn
            // 
            this.formTypeDataGridViewTextBoxColumn.DataPropertyName = "FormType";
            this.formTypeDataGridViewTextBoxColumn.HeaderText = "FormType";
            this.formTypeDataGridViewTextBoxColumn.Name = "formTypeDataGridViewTextBoxColumn";
            // 
            // formDateDataGridViewTextBoxColumn
            // 
            this.formDateDataGridViewTextBoxColumn.DataPropertyName = "FormDate";
            this.formDateDataGridViewTextBoxColumn.HeaderText = "FormDate";
            this.formDateDataGridViewTextBoxColumn.Name = "formDateDataGridViewTextBoxColumn";
            // 
            // formPartialURLDataGridViewTextBoxColumn
            // 
            this.formPartialURLDataGridViewTextBoxColumn.DataPropertyName = "FormPartialURL";
            this.formPartialURLDataGridViewTextBoxColumn.HeaderText = "FormPartialURL";
            this.formPartialURLDataGridViewTextBoxColumn.Name = "formPartialURLDataGridViewTextBoxColumn";
            // 
            // tblFiles_filesTableAdapter
            // 
            this.tblFiles_filesTableAdapter.ClearBeforeFill = true;
            // 
            // tblSEC_CompanyTableAdapter
            // 
            this.tblSEC_CompanyTableAdapter.ClearBeforeFill = true;
            // 
            // tblSEC_FormsTableAdapter
            // 
            this.tblSEC_FormsTableAdapter.ClearBeforeFill = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 466);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "S.E.C. Crawler";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabDataStores.ResumeLayout(false);
            this.tabDataStores.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabpMasterIndexes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridMasterIndexes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblFilesfilesBindingSource)).EndInit();
            this.toolStripdataStore.ResumeLayout(false);
            this.toolStripdataStore.PerformLayout();
            this.tabCompanies.ResumeLayout(false);
            this.tabCompanies.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblSECCompanyBindingSource)).EndInit();
            this.tabPageForms.ResumeLayout(false);
            this.tabPageForms.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblSECFormsBindingSource)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSecCrawler)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.secCrawlerDataDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabDataStores;
        private System.Windows.Forms.TabPage tabCompanies;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStripdataStore;
        private System.Windows.Forms.ToolStripButton toolPopulateIndexList;
        private System.Windows.Forms.BindingSource bindingSourceSecCrawler;
        private SecCrawlerDataDataSet secCrawlerDataDataSet;
        private System.Windows.Forms.DataGridView dataGridMasterIndexes;
        private System.Windows.Forms.BindingSource tblFilesfilesBindingSource;
        private SECcrawler.UI.SecCrawlerDataDataSetTableAdapters.tblFiles_filesTableAdapter tblFiles_filesTableAdapter;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabpMasterIndexes;
        private System.Windows.Forms.DataGridViewTextBoxColumn fileIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn relativePathDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastProcessedDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStrip toolStripCompanies;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource tblSECCompanyBindingSource;
        private SECcrawler.UI.SecCrawlerDataDataSetTableAdapters.tblSEC_CompanyTableAdapter tblSEC_CompanyTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyCIKDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabPage tabPageForms;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.BindingSource tblSECFormsBindingSource;
        private SECcrawler.UI.SecCrawlerDataDataSetTableAdapters.tblSEC_FormsTableAdapter tblSEC_FormsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyCIKDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn formTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn formDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn formPartialURLDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripButton toolStripButtonProcessIndexes;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
    }
}