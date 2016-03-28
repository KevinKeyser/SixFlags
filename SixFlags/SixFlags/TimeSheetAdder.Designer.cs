namespace SixFlags
{
    partial class TimeSheetAdder
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
            this.departmentComboBox = new System.Windows.Forms.ComboBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.timeInMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.timeOutMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.departmentLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.timeInLabel = new System.Windows.Forms.Label();
            this.timeOutLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // departmentComboBox
            // 
            this.departmentComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.departmentComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.departmentComboBox.FormattingEnabled = true;
            this.departmentComboBox.Location = new System.Drawing.Point(104, 12);
            this.departmentComboBox.Name = "departmentComboBox";
            this.departmentComboBox.Size = new System.Drawing.Size(121, 24);
            this.departmentComboBox.TabIndex = 0;
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(150, 144);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 5;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(23, 144);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // timeInMaskedTextBox
            // 
            this.timeInMaskedTextBox.Location = new System.Drawing.Point(104, 70);
            this.timeInMaskedTextBox.Mask = "00:00";
            this.timeInMaskedTextBox.Name = "timeInMaskedTextBox";
            this.timeInMaskedTextBox.Size = new System.Drawing.Size(121, 22);
            this.timeInMaskedTextBox.TabIndex = 2;
            this.timeInMaskedTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // timeOutMaskedTextBox
            // 
            this.timeOutMaskedTextBox.Location = new System.Drawing.Point(104, 98);
            this.timeOutMaskedTextBox.Mask = "00:00";
            this.timeOutMaskedTextBox.Name = "timeOutMaskedTextBox";
            this.timeOutMaskedTextBox.Size = new System.Drawing.Size(121, 22);
            this.timeOutMaskedTextBox.TabIndex = 3;
            this.timeOutMaskedTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(104, 42);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(121, 22);
            this.nameTextBox.TabIndex = 1;
            // 
            // departmentLabel
            // 
            this.departmentLabel.Location = new System.Drawing.Point(12, 12);
            this.departmentLabel.Name = "departmentLabel";
            this.departmentLabel.Size = new System.Drawing.Size(86, 24);
            this.departmentLabel.TabIndex = 6;
            this.departmentLabel.Text = "Department";
            this.departmentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(12, 42);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(86, 24);
            this.nameLabel.TabIndex = 7;
            this.nameLabel.Text = "Name";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timeInLabel
            // 
            this.timeInLabel.Location = new System.Drawing.Point(12, 70);
            this.timeInLabel.Name = "timeInLabel";
            this.timeInLabel.Size = new System.Drawing.Size(86, 24);
            this.timeInLabel.TabIndex = 8;
            this.timeInLabel.Text = "Time In";
            this.timeInLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timeOutLabel
            // 
            this.timeOutLabel.Location = new System.Drawing.Point(12, 98);
            this.timeOutLabel.Name = "timeOutLabel";
            this.timeOutLabel.Size = new System.Drawing.Size(86, 24);
            this.timeOutLabel.TabIndex = 9;
            this.timeOutLabel.Text = "Time Out";
            this.timeOutLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TimeSheetAdder
            // 
            this.AcceptButton = this.submitButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(242, 179);
            this.Controls.Add(this.timeOutLabel);
            this.Controls.Add(this.timeInLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.departmentLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.timeOutMaskedTextBox);
            this.Controls.Add(this.timeInMaskedTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.departmentComboBox);
            this.Name = "TimeSheetAdder";
            this.Text = "TimeSheetAdder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox departmentComboBox;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.MaskedTextBox timeInMaskedTextBox;
        private System.Windows.Forms.MaskedTextBox timeOutMaskedTextBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label departmentLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label timeInLabel;
        private System.Windows.Forms.Label timeOutLabel;
    }
}