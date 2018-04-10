namespace StaticAnalyser
{
    partial class AnalysisOptionsList
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
            this.AnalysisOptionsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.BtnAnalysisOptionsSelcted = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AnalysisOptionsCheckedListBox
            // 
            this.AnalysisOptionsCheckedListBox.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.AnalysisOptionsCheckedListBox.CheckOnClick = true;
            this.AnalysisOptionsCheckedListBox.ForeColor = System.Drawing.SystemColors.InfoText;
            this.AnalysisOptionsCheckedListBox.FormattingEnabled = true;
            this.AnalysisOptionsCheckedListBox.Items.AddRange(new object[] {
            "Functions Tree View",
            "No. Of Lines/Statements in a Function",
            "Highlight Nested Functions Calls",
            "Floating Point Operations",
            "Include System Header Files",
            "Detailed View Of Called Functions Within Function",
            "Single/Nested For Loops"});
            this.AnalysisOptionsCheckedListBox.Location = new System.Drawing.Point(-1, 0);
            this.AnalysisOptionsCheckedListBox.Name = "AnalysisOptionsCheckedListBox";
            this.AnalysisOptionsCheckedListBox.Size = new System.Drawing.Size(285, 124);
            this.AnalysisOptionsCheckedListBox.TabIndex = 0;
            // 
            // BtnAnalysisOptionsSelcted
            // 
            this.BtnAnalysisOptionsSelcted.Location = new System.Drawing.Point(99, 148);
            this.BtnAnalysisOptionsSelcted.Name = "BtnAnalysisOptionsSelcted";
            this.BtnAnalysisOptionsSelcted.Size = new System.Drawing.Size(75, 23);
            this.BtnAnalysisOptionsSelcted.TabIndex = 1;
            this.BtnAnalysisOptionsSelcted.Text = "OK";
            this.BtnAnalysisOptionsSelcted.UseVisualStyleBackColor = true;
            this.BtnAnalysisOptionsSelcted.Click += new System.EventHandler(this.BtnAnalysisOptionsSelcted_Click);
            // 
            // AnalysisOptionsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(283, 183);
            this.Controls.Add(this.BtnAnalysisOptionsSelcted);
            this.Controls.Add(this.AnalysisOptionsCheckedListBox);
            this.Name = "AnalysisOptionsList";
            this.Text = "AnalysisOptionsList";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox AnalysisOptionsCheckedListBox;
        private System.Windows.Forms.Button BtnAnalysisOptionsSelcted;

    }
}