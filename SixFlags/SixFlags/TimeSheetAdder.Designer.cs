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
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.departmentLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.timeInLabel = new System.Windows.Forms.Label();
            this.timeOutLabel = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.ComboBox();
            this.startDateLabel = new System.Windows.Forms.Label();
            this.timeInPicker = new System.Windows.Forms.DateTimePicker();
            this.timeOutPicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // departmentComboBox
            // 
            this.departmentComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.departmentComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.departmentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.departmentComboBox.FormattingEnabled = true;
            this.departmentComboBox.Location = new System.Drawing.Point(104, 42);
            this.departmentComboBox.Name = "departmentComboBox";
            this.departmentComboBox.Size = new System.Drawing.Size(121, 24);
            this.departmentComboBox.TabIndex = 1;
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(150, 163);
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
            this.cancelButton.Location = new System.Drawing.Point(15, 163);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.TabStop = false;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(104, 72);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(121, 22);
            this.nameTextBox.TabIndex = 2;
            // 
            // departmentLabel
            // 
            this.departmentLabel.Location = new System.Drawing.Point(12, 42);
            this.departmentLabel.Name = "departmentLabel";
            this.departmentLabel.Size = new System.Drawing.Size(86, 24);
            this.departmentLabel.TabIndex = 6;
            this.departmentLabel.Text = "Department";
            this.departmentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(12, 72);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(86, 24);
            this.nameLabel.TabIndex = 7;
            this.nameLabel.Text = "Name";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timeInLabel
            // 
            this.timeInLabel.Location = new System.Drawing.Point(12, 100);
            this.timeInLabel.Name = "timeInLabel";
            this.timeInLabel.Size = new System.Drawing.Size(86, 24);
            this.timeInLabel.TabIndex = 8;
            this.timeInLabel.Text = "Time In";
            this.timeInLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timeOutLabel
            // 
            this.timeOutLabel.Location = new System.Drawing.Point(26, 127);
            this.timeOutLabel.Name = "timeOutLabel";
            this.timeOutLabel.Size = new System.Drawing.Size(72, 24);
            this.timeOutLabel.TabIndex = 9;
            this.timeOutLabel.Text = "Time Out";
            this.timeOutLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dateTimePicker.FormatString = "d";
            this.dateTimePicker.FormattingEnabled = true;
            this.dateTimePicker.Location = new System.Drawing.Point(104, 12);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(121, 24);
            this.dateTimePicker.TabIndex = 0;
            // 
            // startDateLabel
            // 
            this.startDateLabel.Location = new System.Drawing.Point(12, 12);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(86, 24);
            this.startDateLabel.TabIndex = 11;
            this.startDateLabel.Text = "Start Date";
            this.startDateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timeInPicker
            // 
            this.timeInPicker.CustomFormat = "HH:mm";
            this.timeInPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timeInPicker.Location = new System.Drawing.Point(104, 99);
            this.timeInPicker.Name = "timeInPicker";
            this.timeInPicker.ShowUpDown = true;
            this.timeInPicker.Size = new System.Drawing.Size(121, 22);
            this.timeInPicker.TabIndex = 3;
            // 
            // timeOutPicker
            // 
            this.timeOutPicker.CustomFormat = "HH:mm";
            this.timeOutPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.timeOutPicker.Location = new System.Drawing.Point(104, 127);
            this.timeOutPicker.Name = "timeOutPicker";
            this.timeOutPicker.ShowUpDown = true;
            this.timeOutPicker.Size = new System.Drawing.Size(121, 22);
            this.timeOutPicker.TabIndex = 4;
            // 
            // TimeSheetAdder
            // 
            this.AcceptButton = this.submitButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(242, 198);
            this.Controls.Add(this.timeOutPicker);
            this.Controls.Add(this.timeInPicker);
            this.Controls.Add(this.startDateLabel);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.timeOutLabel);
            this.Controls.Add(this.timeInLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.departmentLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.departmentComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TimeSheetAdder";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TimeSheetAdder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox departmentComboBox;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label departmentLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label timeInLabel;
        private System.Windows.Forms.Label timeOutLabel;
        private System.Windows.Forms.ComboBox dateTimePicker;
        private System.Windows.Forms.Label startDateLabel;
        private System.Windows.Forms.DateTimePicker timeInPicker;
        private System.Windows.Forms.DateTimePicker timeOutPicker;
    }
}