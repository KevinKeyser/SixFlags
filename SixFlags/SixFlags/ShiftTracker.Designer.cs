namespace SixFlags
{
    partial class ShiftTracker
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mobileButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mobileButton
            // 
            this.mobileButton.Location = new System.Drawing.Point(44, 45);
            this.mobileButton.Name = "mobileButton";
            this.mobileButton.Size = new System.Drawing.Size(107, 59);
            this.mobileButton.TabIndex = 0;
            this.mobileButton.Text = "Mobile";
            this.mobileButton.UseVisualStyleBackColor = true;
            this.mobileButton.Click += new System.EventHandler(this.mobileButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(201, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 59);
            this.button1.TabIndex = 1;
            this.button1.Text = "Mobile";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(353, 45);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(107, 59);
            this.button2.TabIndex = 2;
            this.button2.Text = "Mobile";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // ShiftTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mobileButton);
            this.Name = "ShiftTracker";
            this.Size = new System.Drawing.Size(582, 395);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button mobileButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
