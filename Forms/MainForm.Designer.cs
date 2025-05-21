namespace ContractorsApp
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
            buttonAdd = new Button();
            dataGridView = new DataGridView();
            txtNameFilter = new TextBox();
            txtTaxNumberFilter = new TextBox();
            labelName = new Label();
            labelTaxNumber = new Label();
            buttonEdit = new Button();
            buttonDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            SuspendLayout();
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(89, 372);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(99, 23);
            buttonAdd.TabIndex = 0;
            buttonAdd.Text = "Dodaj";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += HandleAddContractorClick;
            // 
            // dataGridView
            // 
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Location = new Point(88, 87);
            dataGridView.Name = "dataGridView";
            dataGridView.Size = new Size(741, 279);
            dataGridView.TabIndex = 1;
            // 
            // txtNameFilter
            // 
            txtNameFilter.Location = new Point(88, 34);
            txtNameFilter.Name = "txtNameFilter";
            txtNameFilter.Size = new Size(100, 23);
            txtNameFilter.TabIndex = 2;
            // 
            // txtTaxNumberFilter
            // 
            txtTaxNumberFilter.Location = new Point(242, 34);
            txtTaxNumberFilter.Name = "txtTaxNumberFilter";
            txtTaxNumberFilter.Size = new Size(100, 23);
            txtTaxNumberFilter.TabIndex = 3;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(88, 16);
            labelName.Name = "labelName";
            labelName.Size = new Size(42, 15);
            labelName.TabIndex = 4;
            labelName.Text = "Nazwa";
            // 
            // labelTaxNumber
            // 
            labelTaxNumber.AutoSize = true;
            labelTaxNumber.Location = new Point(242, 16);
            labelTaxNumber.Name = "labelTaxNumber";
            labelTaxNumber.Size = new Size(26, 15);
            labelTaxNumber.TabIndex = 5;
            labelTaxNumber.Text = "Nip";
            // 
            // buttonEdit
            // 
            buttonEdit.Location = new Point(194, 372);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new Size(99, 23);
            buttonEdit.TabIndex = 6;
            buttonEdit.Text = "Edytuj";
            buttonEdit.UseVisualStyleBackColor = true;
            buttonEdit.Click += HandleEditContractorClick;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(299, 372);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(87, 23);
            buttonDelete.TabIndex = 7;
            buttonDelete.Text = "Usuń";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += HandleDeleteContractorClick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(917, 450);
            Controls.Add(buttonDelete);
            Controls.Add(buttonEdit);
            Controls.Add(labelTaxNumber);
            Controls.Add(labelName);
            Controls.Add(txtTaxNumberFilter);
            Controls.Add(txtNameFilter);
            Controls.Add(dataGridView);
            Controls.Add(buttonAdd);
            Name = "MainForm";
            Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonAdd;
        private DataGridView dataGridView;
        private TextBox txtNameFilter;
        private TextBox txtTaxNumberFilter;
        private Label labelName;
        private Label labelTaxNumber;
        private Button buttonEdit;
        private Button buttonDelete;
    }
}