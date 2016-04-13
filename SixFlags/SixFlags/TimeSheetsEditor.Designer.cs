namespace SixFlags
{
    partial class TimeSheetsEditor
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
            this.timeSheetListBox = new System.Windows.Forms.ListBox();
            this.addButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.sendLunchButton = new System.Windows.Forms.Button();
            this.sendBreakButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.endShiftButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timeSheetListBox
            // 
            this.timeSheetListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.timeSheetListBox.FormattingEnabled = true;
            this.timeSheetListBox.ItemHeight = 16;
            this.timeSheetListBox.Location = new System.Drawing.Point(0, 0);
            this.timeSheetListBox.Name = "timeSheetListBox";
            this.timeSheetListBox.Size = new System.Drawing.Size(232, 191);
            this.timeSheetListBox.TabIndex = 0;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(252, 12);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(140, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // editButton
            // 
            this.editButton.Location = new System.Drawing.Point(252, 41);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(140, 23);
            this.editButton.TabIndex = 2;
            this.editButton.Text = "Edit";
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(252, 70);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(140, 23);
            this.deleteButton.TabIndex = 3;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // sendLunchButton
            // 
            this.sendLunchButton.Location = new System.Drawing.Point(252, 99);
            this.sendLunchButton.Name = "sendLunchButton";
            this.sendLunchButton.Size = new System.Drawing.Size(140, 23);
            this.sendLunchButton.TabIndex = 4;
            this.sendLunchButton.Text = "Send On Lunch";
            this.sendLunchButton.UseVisualStyleBackColor = true;
            this.sendLunchButton.Click += new System.EventHandler(this.sendLunchButton_Click);
            // 
            // sendBreakButton
            // 
            this.sendBreakButton.Location = new System.Drawing.Point(252, 128);
            this.sendBreakButton.Name = "sendBreakButton";
            this.sendBreakButton.Size = new System.Drawing.Size(140, 23);
            this.sendBreakButton.TabIndex = 5;
            this.sendBreakButton.Text = "Send On Break";
            this.sendBreakButton.UseVisualStyleBackColor = true;
            this.sendBreakButton.Click += new System.EventHandler(this.sendBreakButton_Click);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // endShiftButton
            // 
            this.endShiftButton.Location = new System.Drawing.Point(252, 157);
            this.endShiftButton.Name = "endShiftButton";
            this.endShiftButton.Size = new System.Drawing.Size(140, 23);
            this.endShiftButton.TabIndex = 6;
            this.endShiftButton.Text = "End Shift";
            this.endShiftButton.UseVisualStyleBackColor = true;
            this.endShiftButton.Click += new System.EventHandler(this.endShiftButton_Click);
            // 
            // TimeSheetsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 191);
            this.Controls.Add(this.endShiftButton);
            this.Controls.Add(this.sendBreakButton);
            this.Controls.Add(this.sendLunchButton);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.timeSheetListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TimeSheetsEditor";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TimeSheetEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox timeSheetListBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button sendLunchButton;
        private System.Windows.Forms.Button sendBreakButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button endShiftButton;
    }
}