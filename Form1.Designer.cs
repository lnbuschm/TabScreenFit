namespace TabScreenFit
{
    partial class Form1
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.historyListBox = new System.Windows.Forms.ListBox();
            this.showButton = new System.Windows.Forms.Button();
            this.panelButton = new System.Windows.Forms.Button();
            this.fontButton = new System.Windows.Forms.Button();
            this.tabTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "tab";
            this.openFileDialog1.Filter = "Text Files (*.txt)|*.txt|DOC Files (*.doc)|*.doc|RTF Files (*.rtf)|*.rtf|All file" +
    "s (*.*)|*.*";
            this.openFileDialog1.Title = "Select a text file";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.historyListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.showButton);
            this.splitContainer1.Panel2.Controls.Add(this.panelButton);
            this.splitContainer1.Panel2.Controls.Add(this.fontButton);
            this.splitContainer1.Panel2.Controls.Add(this.tabTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(790, 418);
            this.splitContainer1.SplitterDistance = 263;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // historyListBox
            // 
            this.historyListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyListBox.FormattingEnabled = true;
            this.historyListBox.Location = new System.Drawing.Point(0, 0);
            this.historyListBox.Margin = new System.Windows.Forms.Padding(1);
            this.historyListBox.Name = "historyListBox";
            this.historyListBox.Size = new System.Drawing.Size(263, 418);
            this.historyListBox.TabIndex = 7;
            this.historyListBox.SelectedIndexChanged += new System.EventHandler(this.historyListBox_SelectedIndexChanged);
            // 
            // showButton
            // 
            this.showButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.showButton.AutoSize = true;
            this.showButton.Location = new System.Drawing.Point(288, 0);
            this.showButton.Name = "showButton";
            this.showButton.Size = new System.Drawing.Size(108, 23);
            this.showButton.TabIndex = 7;
            this.showButton.Text = "Open tab";
            this.showButton.UseVisualStyleBackColor = true;
            this.showButton.Click += new System.EventHandler(this.showButton_Click);
            // 
            // panelButton
            // 
            this.panelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButton.Location = new System.Drawing.Point(397, 0);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(108, 23);
            this.panelButton.TabIndex = 2;
            this.panelButton.Text = "Toggle Panel";
            this.panelButton.UseVisualStyleBackColor = true;
            this.panelButton.Click += new System.EventHandler(this.panelButton_Click);
            // 
            // fontButton
            // 
            this.fontButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fontButton.AutoSize = true;
            this.fontButton.Location = new System.Drawing.Point(178, 0);
            this.fontButton.Name = "fontButton";
            this.fontButton.Size = new System.Drawing.Size(108, 23);
            this.fontButton.TabIndex = 3;
            this.fontButton.Text = "Font";
            this.fontButton.UseVisualStyleBackColor = true;
            this.fontButton.Click += new System.EventHandler(this.fontButton_Click);
            // 
            // tabTextBox1
            // 
            this.tabTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tabTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTextBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabTextBox1.Location = new System.Drawing.Point(0, 0);
            this.tabTextBox1.Name = "tabTextBox1";
            this.tabTextBox1.Size = new System.Drawing.Size(523, 418);
            this.tabTextBox1.TabIndex = 6;
            this.tabTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 418);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Tab Viewer";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox historyListBox;
        private System.Windows.Forms.Button showButton;
        private System.Windows.Forms.Button panelButton;
        private System.Windows.Forms.Button fontButton;
        private System.Windows.Forms.RichTextBox tabTextBox1;
    }
}

