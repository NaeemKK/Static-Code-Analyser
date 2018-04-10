namespace StaticAnalyser
{
    partial class FunctionsTreeView
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
            this.TreeViewTool = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // TreeViewTool
            // 
            this.TreeViewTool.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TreeViewTool.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TreeViewTool.ForeColor = System.Drawing.SystemColors.InfoText;
            this.TreeViewTool.Location = new System.Drawing.Point(-1, -2);
            this.TreeViewTool.Name = "TreeViewTool";
            this.TreeViewTool.Size = new System.Drawing.Size(750, 488);
            this.TreeViewTool.TabIndex = 0;
            // 
            // FunctionsTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(748, 484);
            this.Controls.Add(this.TreeViewTool);
            this.MaximizeBox = false;
            this.Name = "FunctionsTreeView";
            this.Text = "Functions Tree View";
            this.Load += new System.EventHandler(this.FunctionsTreeView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView TreeViewTool;
    }
}