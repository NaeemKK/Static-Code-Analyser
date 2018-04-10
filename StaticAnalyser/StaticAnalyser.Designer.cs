namespace StaticAnalyser
{
    partial class StaticAnalyser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StaticAnalyser));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.FileFolderPath = new System.Windows.Forms.TextBox();
            this.BtnBrowseFolder = new System.Windows.Forms.Button();
            this.BtnBrowseFile = new System.Windows.Forms.Button();
            this.BtnStartStaticAnalysis = new System.Windows.Forms.Button();
            this.BtnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.PreprocessorDirectiveLabel = new System.Windows.Forms.Label();
            this.DefinedCheckBox = new System.Windows.Forms.CheckBox();
            this.UnDefCheckBox = new System.Windows.Forms.CheckBox();
            this.AskForOptionForPreprocessorDirectives = new System.Windows.Forms.Label();
            this.DefMacrosRichTextBox = new System.Windows.Forms.RichTextBox();
            this.UnDefMacroRichTextBox = new System.Windows.Forms.RichTextBox();
            this.AnalysisProgressBar = new System.Windows.Forms.ProgressBar();
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // FileFolderPath
            // 
            this.FileFolderPath.Enabled = false;
            this.FileFolderPath.Location = new System.Drawing.Point(56, 26);
            this.FileFolderPath.Name = "FileFolderPath";
            this.FileFolderPath.Size = new System.Drawing.Size(549, 20);
            this.FileFolderPath.TabIndex = 0;
            // 
            // BtnBrowseFolder
            // 
            this.BtnBrowseFolder.Location = new System.Drawing.Point(611, 13);
            this.BtnBrowseFolder.Name = "BtnBrowseFolder";
            this.BtnBrowseFolder.Size = new System.Drawing.Size(84, 22);
            this.BtnBrowseFolder.TabIndex = 1;
            this.BtnBrowseFolder.Text = "Browse Folder";
            this.BtnBrowseFolder.UseVisualStyleBackColor = true;
            this.BtnBrowseFolder.Click += new System.EventHandler(this.BtnBrowseFolder_Click);
            // 
            // BtnBrowseFile
            // 
            this.BtnBrowseFile.Location = new System.Drawing.Point(611, 41);
            this.BtnBrowseFile.Name = "BtnBrowseFile";
            this.BtnBrowseFile.Size = new System.Drawing.Size(84, 22);
            this.BtnBrowseFile.TabIndex = 2;
            this.BtnBrowseFile.Text = "Browse File";
            this.BtnBrowseFile.UseVisualStyleBackColor = true;
            this.BtnBrowseFile.Click += new System.EventHandler(this.BtnBrowseFile_Click);
            // 
            // BtnStartStaticAnalysis
            // 
            this.BtnStartStaticAnalysis.Location = new System.Drawing.Point(611, 103);
            this.BtnStartStaticAnalysis.Name = "BtnStartStaticAnalysis";
            this.BtnStartStaticAnalysis.Size = new System.Drawing.Size(111, 22);
            this.BtnStartStaticAnalysis.TabIndex = 3;
            this.BtnStartStaticAnalysis.Text = "Start Analysis";
            this.BtnStartStaticAnalysis.UseVisualStyleBackColor = true;
            this.BtnStartStaticAnalysis.Click += new System.EventHandler(this.BtnStartStaticAnalysis_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(662, 276);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 22);
            this.BtnClose.TabIndex = 4;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(21, 242);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Static Analyser";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(21, 255);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Developer : Naeem Khan";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(21, 268);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Email : naeemkhan058@gmail.com";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(21, 281);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Cell # +92-343-5250235";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(21, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Path";
            // 
            // PreprocessorDirectiveLabel
            // 
            this.PreprocessorDirectiveLabel.AutoSize = true;
            this.PreprocessorDirectiveLabel.ForeColor = System.Drawing.SystemColors.InfoText;
            this.PreprocessorDirectiveLabel.Location = new System.Drawing.Point(21, 86);
            this.PreprocessorDirectiveLabel.Name = "PreprocessorDirectiveLabel";
            this.PreprocessorDirectiveLabel.Size = new System.Drawing.Size(159, 39);
            this.PreprocessorDirectiveLabel.TabIndex = 11;
            this.PreprocessorDirectiveLabel.Text = "Preprocessor Directives(If any)\r\ne.g. A in ( #ifdef A,#ifndef A etc)\r\nWrite one o" +
                "n each line.";
            // 
            // DefinedCheckBox
            // 
            this.DefinedCheckBox.AutoSize = true;
            this.DefinedCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.DefinedCheckBox.Location = new System.Drawing.Point(186, 86);
            this.DefinedCheckBox.Name = "DefinedCheckBox";
            this.DefinedCheckBox.Size = new System.Drawing.Size(63, 17);
            this.DefinedCheckBox.TabIndex = 13;
            this.DefinedCheckBox.Text = "Defined";
            this.DefinedCheckBox.UseVisualStyleBackColor = true;
            // 
            // UnDefCheckBox
            // 
            this.UnDefCheckBox.AutoSize = true;
            this.UnDefCheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.UnDefCheckBox.Location = new System.Drawing.Point(186, 121);
            this.UnDefCheckBox.Name = "UnDefCheckBox";
            this.UnDefCheckBox.Size = new System.Drawing.Size(77, 17);
            this.UnDefCheckBox.TabIndex = 14;
            this.UnDefCheckBox.Text = "UnDefined";
            this.UnDefCheckBox.UseVisualStyleBackColor = true;
            // 
            // AskForOptionForPreprocessorDirectives
            // 
            this.AskForOptionForPreprocessorDirectives.AutoSize = true;
            this.AskForOptionForPreprocessorDirectives.ForeColor = System.Drawing.SystemColors.ControlText;
            this.AskForOptionForPreprocessorDirectives.Location = new System.Drawing.Point(183, 70);
            this.AskForOptionForPreprocessorDirectives.Name = "AskForOptionForPreprocessorDirectives";
            this.AskForOptionForPreprocessorDirectives.Size = new System.Drawing.Size(52, 13);
            this.AskForOptionForPreprocessorDirectives.TabIndex = 15;
            this.AskForOptionForPreprocessorDirectives.Text = "Are these";
            // 
            // DefMacrosRichTextBox
            // 
            this.DefMacrosRichTextBox.Location = new System.Drawing.Point(266, 68);
            this.DefMacrosRichTextBox.Name = "DefMacrosRichTextBox";
            this.DefMacrosRichTextBox.Size = new System.Drawing.Size(191, 35);
            this.DefMacrosRichTextBox.TabIndex = 16;
            this.DefMacrosRichTextBox.Text = "";
            this.DefMacrosRichTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DefMacrosRichTextBox_KeyDown);
            // 
            // UnDefMacroRichTextBox
            // 
            this.UnDefMacroRichTextBox.Location = new System.Drawing.Point(266, 109);
            this.UnDefMacroRichTextBox.Name = "UnDefMacroRichTextBox";
            this.UnDefMacroRichTextBox.Size = new System.Drawing.Size(191, 35);
            this.UnDefMacroRichTextBox.TabIndex = 17;
            this.UnDefMacroRichTextBox.Text = "";
            this.UnDefMacroRichTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UnDefMacrosRichTextBox_KeyDown);
            // 
            // AnalysisProgressBar
            // 
            this.AnalysisProgressBar.Location = new System.Drawing.Point(589, 103);
            this.AnalysisProgressBar.Name = "AnalysisProgressBar";
            this.AnalysisProgressBar.Size = new System.Drawing.Size(155, 22);
            this.AnalysisProgressBar.TabIndex = 18;
            this.AnalysisProgressBar.Visible = false;
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ProgressLabel.Location = new System.Drawing.Point(586, 131);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(48, 13);
            this.ProgressLabel.TabIndex = 19;
            this.ProgressLabel.Text = "Progress";
            this.ProgressLabel.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::StaticAnalyser.Properties.Resources.analysis;
            this.pictureBox1.Location = new System.Drawing.Point(24, 154);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(170, 98);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // StaticAnalyser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(770, 305);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.UnDefMacroRichTextBox);
            this.Controls.Add(this.DefMacrosRichTextBox);
            this.Controls.Add(this.AskForOptionForPreprocessorDirectives);
            this.Controls.Add(this.UnDefCheckBox);
            this.Controls.Add(this.DefinedCheckBox);
            this.Controls.Add(this.PreprocessorDirectiveLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnBrowseFile);
            this.Controls.Add(this.BtnBrowseFolder);
            this.Controls.Add(this.FileFolderPath);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.AnalysisProgressBar);
            this.Controls.Add(this.BtnStartStaticAnalysis);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "StaticAnalyser";
            this.Text = "Static Analyser";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox FileFolderPath;
        private System.Windows.Forms.Button BtnBrowseFolder;
        private System.Windows.Forms.Button BtnBrowseFile;
        private System.Windows.Forms.Button BtnStartStaticAnalysis;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label PreprocessorDirectiveLabel;
        private System.Windows.Forms.CheckBox DefinedCheckBox;
        private System.Windows.Forms.CheckBox UnDefCheckBox;
        private System.Windows.Forms.Label AskForOptionForPreprocessorDirectives;
        private System.Windows.Forms.RichTextBox DefMacrosRichTextBox;
        private System.Windows.Forms.RichTextBox UnDefMacroRichTextBox;
        private System.Windows.Forms.ProgressBar AnalysisProgressBar;
        private System.Windows.Forms.Label ProgressLabel;
    }
}

