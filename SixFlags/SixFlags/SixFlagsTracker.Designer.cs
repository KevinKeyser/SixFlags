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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SixFlagsTracker));
            this.shiftTimes = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.fileTool = new System.Windows.Forms.ToolStripDropDownButton();
            this.fileClearTool = new System.Windows.Forms.ToolStripMenuItem();
            this.fileClearTimeSheeets = new System.Windows.Forms.ToolStripMenuItem();
            this.editTool = new System.Windows.Forms.ToolStripDropDownButton();
            this.editDepartmentTool = new System.Windows.Forms.ToolStripMenuItem();
            this.editDepartmentAddTool = new System.Windows.Forms.ToolStripMenuItem();
            this.editDepartmentEditTool = new System.Windows.Forms.ToolStripMenuItem();
            this.editDepartmentRemoveTool = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.shiftTimes.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
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
            this.shiftTimes.Size = new System.Drawing.Size(517, 419);
            this.shiftTimes.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(509, 390);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Day";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(509, 390);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Mid";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileTool,
            this.editTool});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(137, 27);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // fileTool
            // 
            this.fileTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileClearTool});
            this.fileTool.Image = ((System.Drawing.Image)(resources.GetObject("fileTool.Image")));
            this.fileTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fileTool.Name = "fileTool";
            this.fileTool.Size = new System.Drawing.Size(46, 24);
            this.fileTool.Text = "File";
            // 
            // fileClearTool
            // 
            this.fileClearTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileClearTimeSheeets});
            this.fileClearTool.Name = "fileClearTool";
            this.fileClearTool.Size = new System.Drawing.Size(181, 26);
            this.fileClearTool.Text = "Clear";
            // 
            // fileClearTimeSheeets
            // 
            this.fileClearTimeSheeets.Name = "fileClearTimeSheeets";
            this.fileClearTimeSheeets.Size = new System.Drawing.Size(181, 26);
            this.fileClearTimeSheeets.Text = "TimeSheets";
            this.fileClearTimeSheeets.Click += new System.EventHandler(this.ClearTimeSheets_Click);
            // 
            // editTool
            // 
            this.editTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.editTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editDepartmentTool});
            this.editTool.Image = ((System.Drawing.Image)(resources.GetObject("editTool.Image")));
            this.editTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editTool.Name = "editTool";
            this.editTool.Size = new System.Drawing.Size(49, 24);
            this.editTool.Text = "Edit";
            // 
            // editDepartmentTool
            // 
            this.editDepartmentTool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editDepartmentAddTool,
            this.editDepartmentEditTool,
            this.editDepartmentRemoveTool});
            this.editDepartmentTool.Name = "editDepartmentTool";
            this.editDepartmentTool.Size = new System.Drawing.Size(181, 26);
            this.editDepartmentTool.Text = "Department";
            // 
            // editDepartmentAddTool
            // 
            this.editDepartmentAddTool.Name = "editDepartmentAddTool";
            this.editDepartmentAddTool.Size = new System.Drawing.Size(181, 26);
            this.editDepartmentAddTool.Text = "Add";
            this.editDepartmentAddTool.Click += new System.EventHandler(this.AddTool_Click);
            // 
            // editDepartmentEditTool
            // 
            this.editDepartmentEditTool.Name = "editDepartmentEditTool";
            this.editDepartmentEditTool.Size = new System.Drawing.Size(181, 26);
            this.editDepartmentEditTool.Text = "Edit";
            this.editDepartmentEditTool.Click += new System.EventHandler(this.EditTool_Click);
            // 
            // editDepartmentRemoveTool
            // 
            this.editDepartmentRemoveTool.Name = "editDepartmentRemoveTool";
            this.editDepartmentRemoveTool.Size = new System.Drawing.Size(181, 26);
            this.editDepartmentRemoveTool.Text = "Remove";
            this.editDepartmentRemoveTool.Click += new System.EventHandler(this.RemoveTool_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.shiftTimes);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(517, 419);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(517, 446);
            this.toolStripContainer1.TabIndex = 4;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // SixFlagsTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 446);
            this.Controls.Add(this.toolStripContainer1);
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "SixFlagsTracker";
            this.Text = "Base Tracker";
            this.shiftTimes.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl shiftTimes;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton fileTool;
        private System.Windows.Forms.ToolStripMenuItem fileClearTool;
        private System.Windows.Forms.ToolStripMenuItem fileClearTimeSheeets;
        private System.Windows.Forms.ToolStripDropDownButton editTool;
        private System.Windows.Forms.ToolStripMenuItem editDepartmentTool;
        private System.Windows.Forms.ToolStripMenuItem editDepartmentAddTool;
        private System.Windows.Forms.ToolStripMenuItem editDepartmentEditTool;
        private System.Windows.Forms.ToolStripMenuItem editDepartmentRemoveTool;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    }
}

