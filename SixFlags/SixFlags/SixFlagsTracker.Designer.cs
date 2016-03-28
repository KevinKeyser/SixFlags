namespace SixFlags
{
    partial class SixFlagsTracker
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
            this.shiftTimes = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.shiftTracker1 = new SixFlags.ShiftTracker();
            this.shiftTracker2 = new SixFlags.ShiftTracker();
            this.shiftTimes.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // shiftTimes
            // 
            this.shiftTimes.Controls.Add(this.tabPage1);
            this.shiftTimes.Controls.Add(this.tabPage2);
            this.shiftTimes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shiftTimes.Location = new System.Drawing.Point(0, 0);
            this.shiftTimes.Name = "shiftTimes";
            this.shiftTimes.SelectedIndex = 0;
            this.shiftTimes.Size = new System.Drawing.Size(504, 446);
            this.shiftTimes.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.shiftTracker1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(496, 417);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Day";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.shiftTracker2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(496, 417);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Mid";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // shiftTracker1
            // 
            this.shiftTracker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shiftTracker1.Location = new System.Drawing.Point(3, 3);
            this.shiftTracker1.Name = "shiftTracker1";
            this.shiftTracker1.Size = new System.Drawing.Size(490, 411);
            this.shiftTracker1.TabIndex = 0;
            // 
            // shiftTracker2
            // 
            this.shiftTracker2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shiftTracker2.Location = new System.Drawing.Point(3, 3);
            this.shiftTracker2.Name = "shiftTracker2";
            this.shiftTracker2.Size = new System.Drawing.Size(490, 411);
            this.shiftTracker2.TabIndex = 0;
            // 
            // SixFlagsTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 446);
            this.Controls.Add(this.shiftTimes);
            this.Name = "SixFlagsTracker";
            this.Text = "Form1";
            this.shiftTimes.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl shiftTimes;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private ShiftTracker shiftTracker2;
        private ShiftTracker shiftTracker1;
    }
}

