namespace ContractorsApp.Forms
{
    partial class ContractorForm
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
            txtName = new TextBox();
            txtTaxNumber = new TextBox();
            txtREGON = new TextBox();
            dgvAddresses = new DataGridView();
            btnSave = new Button();
            btnCancel = new Button();
            labelName = new Label();
            labelTaxNumber = new Label();
            labelREGON = new Label();
            labelAddresses = new Label();
            buttonDeleteAddress = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvAddresses).BeginInit();
            SuspendLayout();
            // 
            // txtName
            // 
            txtName.Location = new Point(72, 50);
            txtName.Name = "txtName";
            txtName.Size = new Size(100, 23);
            txtName.TabIndex = 0;
            // 
            // txtTaxNumber
            // 
            txtTaxNumber.Location = new Point(178, 50);
            txtTaxNumber.Name = "txtTaxNumber";
            txtTaxNumber.Size = new Size(100, 23);
            txtTaxNumber.TabIndex = 1;
            // 
            // txtREGON
            // 
            txtREGON.Location = new Point(284, 50);
            txtREGON.Name = "txtREGON";
            txtREGON.Size = new Size(100, 23);
            txtREGON.TabIndex = 2;
            // 
            // dgvAddresses
            // 
            dgvAddresses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAddresses.Location = new Point(72, 117);
            dgvAddresses.Name = "dgvAddresses";
            dgvAddresses.Size = new Size(591, 171);
            dgvAddresses.TabIndex = 3;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(72, 294);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 4;
            btnSave.Text = "Zapisz";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += HandleSaveClick;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(153, 294);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Anuluj";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += HandleCancelClick;
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(72, 32);
            labelName.Name = "labelName";
            labelName.Size = new Size(42, 15);
            labelName.TabIndex = 6;
            labelName.Text = "Nazwa";
            // 
            // labelTaxNumber
            // 
            labelTaxNumber.AutoSize = true;
            labelTaxNumber.Location = new Point(178, 32);
            labelTaxNumber.Name = "labelTaxNumber";
            labelTaxNumber.Size = new Size(26, 15);
            labelTaxNumber.TabIndex = 7;
            labelTaxNumber.Text = "Nip";
            // 
            // labelREGON
            // 
            labelREGON.AutoSize = true;
            labelREGON.Location = new Point(284, 32);
            labelREGON.Name = "labelREGON";
            labelREGON.Size = new Size(46, 15);
            labelREGON.TabIndex = 8;
            labelREGON.Text = "REGON";
            // 
            // labelAddresses
            // 
            labelAddresses.AutoSize = true;
            labelAddresses.Location = new Point(72, 99);
            labelAddresses.Name = "labelAddresses";
            labelAddresses.Size = new Size(43, 15);
            labelAddresses.TabIndex = 9;
            labelAddresses.Text = "Adresy";
            // 
            // buttonDeleteAddress
            // 
            buttonDeleteAddress.Location = new Point(234, 294);
            buttonDeleteAddress.Name = "buttonDeleteAddress";
            buttonDeleteAddress.Size = new Size(80, 23);
            buttonDeleteAddress.TabIndex = 10;
            buttonDeleteAddress.Text = "Usuń adres";
            buttonDeleteAddress.UseVisualStyleBackColor = true;
            buttonDeleteAddress.Click += HandleDeleteAddressClick;
            // 
            // ContractorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(746, 450);
            Controls.Add(buttonDeleteAddress);
            Controls.Add(labelAddresses);
            Controls.Add(labelREGON);
            Controls.Add(labelTaxNumber);
            Controls.Add(labelName);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(dgvAddresses);
            Controls.Add(txtREGON);
            Controls.Add(txtTaxNumber);
            Controls.Add(txtName);
            Name = "ContractorForm";
            Text = "ContractorForm";
            ((System.ComponentModel.ISupportInitialize)dgvAddresses).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtName;
        private TextBox txtTaxNumber;
        private TextBox txtREGON;
        private DataGridView dgvAddresses;
        private Button btnSave;
        private Button btnCancel;
        private Label labelName;
        private Label labelTaxNumber;
        private Label labelREGON;
        private Label labelAddresses;
        private Button buttonDeleteAddress;
    }
}