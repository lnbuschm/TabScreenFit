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
            this.historySplitContainer = new System.Windows.Forms.SplitContainer();
            this.removeButton = new System.Windows.Forms.Button();
            this.moveToTopButton = new System.Windows.Forms.Button();
            this.historyListBox = new System.Windows.Forms.ListBox();
            this.fontSmallerButton = new System.Windows.Forms.Button();
            this.fontLargerButton = new System.Windows.Forms.Button();
            this.viewButton = new System.Windows.Forms.Button();
            this.openTabButton = new System.Windows.Forms.Button();
            this.panelButton = new System.Windows.Forms.Button();
            this.fontButton = new System.Windows.Forms.Button();
            this.tabSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tabTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabSplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabTextBox2 = new System.Windows.Forms.RichTextBox();
            this.tabTextBox3 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.historySplitContainer)).BeginInit();
            this.historySplitContainer.Panel1.SuspendLayout();
            this.historySplitContainer.Panel2.SuspendLayout();
            this.historySplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabSplitContainer)).BeginInit();
            this.tabSplitContainer.Panel1.SuspendLayout();
            this.tabSplitContainer.Panel2.SuspendLayout();
            this.tabSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabSplitContainer2)).BeginInit();
            this.tabSplitContainer2.Panel1.SuspendLayout();
            this.tabSplitContainer2.Panel2.SuspendLayout();
            this.tabSplitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "tabfile.txt";
            this.openFileDialog1.Filter = "Text Files (*.txt)|*.txt|DOC Files (*.doc)|*.doc|RTF Files (*.rtf)|*.rtf|All file" +
    "s (*.*)|*.*";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Select tab text file(s)";
            // 
            // historySplitContainer
            // 
            this.historySplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historySplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.historySplitContainer.Location = new System.Drawing.Point(0, 0);
            this.historySplitContainer.Name = "historySplitContainer";
            // 
            // historySplitContainer.Panel1
            // 
            this.historySplitContainer.Panel1.Controls.Add(this.removeButton);
            this.historySplitContainer.Panel1.Controls.Add(this.moveToTopButton);
            this.historySplitContainer.Panel1.Controls.Add(this.historyListBox);
            // 
            // historySplitContainer.Panel2
            // 
            this.historySplitContainer.Panel2.Controls.Add(this.fontSmallerButton);
            this.historySplitContainer.Panel2.Controls.Add(this.fontLargerButton);
            this.historySplitContainer.Panel2.Controls.Add(this.viewButton);
            this.historySplitContainer.Panel2.Controls.Add(this.openTabButton);
            this.historySplitContainer.Panel2.Controls.Add(this.panelButton);
            this.historySplitContainer.Panel2.Controls.Add(this.fontButton);
            this.historySplitContainer.Panel2.Controls.Add(this.tabSplitContainer);
            this.historySplitContainer.Size = new System.Drawing.Size(790, 418);
            this.historySplitContainer.SplitterDistance = 263;
            this.historySplitContainer.TabIndex = 0;
            this.historySplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeButton.Location = new System.Drawing.Point(85, 392);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 9;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // moveToTopButton
            // 
            this.moveToTopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.moveToTopButton.Location = new System.Drawing.Point(3, 392);
            this.moveToTopButton.Name = "moveToTopButton";
            this.moveToTopButton.Size = new System.Drawing.Size(82, 23);
            this.moveToTopButton.TabIndex = 8;
            this.moveToTopButton.Text = "Move to Top";
            this.moveToTopButton.UseVisualStyleBackColor = true;
            this.moveToTopButton.Click += new System.EventHandler(this.moveToTopButton_Click);
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
            this.historyListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.historyListBox_MouseDown);
            // 
            // fontSmallerButton
            // 
            this.fontSmallerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fontSmallerButton.Location = new System.Drawing.Point(339, 27);
            this.fontSmallerButton.Name = "fontSmallerButton";
            this.fontSmallerButton.Size = new System.Drawing.Size(23, 23);
            this.fontSmallerButton.TabIndex = 11;
            this.fontSmallerButton.Text = "-";
            this.fontSmallerButton.UseVisualStyleBackColor = true;
            this.fontSmallerButton.Click += new System.EventHandler(this.fontSmallerButton_Click);
            // 
            // fontLargerButton
            // 
            this.fontLargerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fontLargerButton.Location = new System.Drawing.Point(368, 27);
            this.fontLargerButton.Name = "fontLargerButton";
            this.fontLargerButton.Size = new System.Drawing.Size(23, 23);
            this.fontLargerButton.TabIndex = 10;
            this.fontLargerButton.Text = "+";
            this.fontLargerButton.UseVisualStyleBackColor = true;
            this.fontLargerButton.Click += new System.EventHandler(this.fontLargerButton_Click);
            // 
            // viewButton
            // 
            this.viewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.viewButton.Location = new System.Drawing.Point(397, 28);
            this.viewButton.Name = "viewButton";
            this.viewButton.Size = new System.Drawing.Size(108, 23);
            this.viewButton.TabIndex = 8;
            this.viewButton.Text = "Auto";
            this.viewButton.UseVisualStyleBackColor = true;
            this.viewButton.Click += new System.EventHandler(this.viewButton_Click);
            // 
            // openTabButton
            // 
            this.openTabButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openTabButton.AutoSize = true;
            this.openTabButton.Location = new System.Drawing.Point(178, 4);
            this.openTabButton.Name = "openTabButton";
            this.openTabButton.Size = new System.Drawing.Size(108, 23);
            this.openTabButton.TabIndex = 7;
            this.openTabButton.Text = "Open tab";
            this.openTabButton.UseVisualStyleBackColor = true;
            this.openTabButton.Click += new System.EventHandler(this.openTabButton_Click);
            // 
            // panelButton
            // 
            this.panelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButton.Location = new System.Drawing.Point(397, 4);
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
            this.fontButton.Location = new System.Drawing.Point(288, 4);
            this.fontButton.Name = "fontButton";
            this.fontButton.Size = new System.Drawing.Size(108, 23);
            this.fontButton.TabIndex = 3;
            this.fontButton.Text = "Font";
            this.fontButton.UseVisualStyleBackColor = true;
            this.fontButton.Click += new System.EventHandler(this.fontButton_Click);
            // 
            // tabSplitContainer
            // 
            this.tabSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.tabSplitContainer.Name = "tabSplitContainer";
            // 
            // tabSplitContainer.Panel1
            // 
            this.tabSplitContainer.Panel1.Controls.Add(this.tabTextBox1);
            // 
            // tabSplitContainer.Panel2
            // 
            this.tabSplitContainer.Panel2.Controls.Add(this.tabSplitContainer2);
            this.tabSplitContainer.Size = new System.Drawing.Size(523, 418);
            this.tabSplitContainer.SplitterDistance = 275;
            this.tabSplitContainer.TabIndex = 9;
            // 
            // tabTextBox1
            // 
            this.tabTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tabTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTextBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabTextBox1.Location = new System.Drawing.Point(0, 0);
            this.tabTextBox1.Name = "tabTextBox1";
            this.tabTextBox1.Size = new System.Drawing.Size(275, 418);
            this.tabTextBox1.TabIndex = 6;
            this.tabTextBox1.Text = "";
            this.tabTextBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabTextBox1_MouseDown);
            this.tabTextBox1.DoubleClick += new System.EventHandler(this.tabTextBox1_DoubleClick);
            // 
            // tabSplitContainer2
            // 
            this.tabSplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.tabSplitContainer2.Name = "tabSplitContainer2";
            // 
            // tabSplitContainer2.Panel1
            // 
            this.tabSplitContainer2.Panel1.Controls.Add(this.tabTextBox2);
            // 
            // tabSplitContainer2.Panel2
            // 
            this.tabSplitContainer2.Panel2.Controls.Add(this.tabTextBox3);
            this.tabSplitContainer2.Size = new System.Drawing.Size(244, 418);
            this.tabSplitContainer2.SplitterDistance = 127;
            this.tabSplitContainer2.TabIndex = 0;
            // 
            // tabTextBox2
            // 
            this.tabTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTextBox2.Location = new System.Drawing.Point(0, 0);
            this.tabTextBox2.Name = "tabTextBox2";
            this.tabTextBox2.Size = new System.Drawing.Size(127, 418);
            this.tabTextBox2.TabIndex = 0;
            this.tabTextBox2.Text = "";
            // 
            // tabTextBox3
            // 
            this.tabTextBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTextBox3.Location = new System.Drawing.Point(0, 0);
            this.tabTextBox3.Name = "tabTextBox3";
            this.tabTextBox3.Size = new System.Drawing.Size(113, 418);
            this.tabTextBox3.TabIndex = 0;
            this.tabTextBox3.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 418);
            this.Controls.Add(this.historySplitContainer);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Tab Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.historySplitContainer.Panel1.ResumeLayout(false);
            this.historySplitContainer.Panel2.ResumeLayout(false);
            this.historySplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.historySplitContainer)).EndInit();
            this.historySplitContainer.ResumeLayout(false);
            this.tabSplitContainer.Panel1.ResumeLayout(false);
            this.tabSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabSplitContainer)).EndInit();
            this.tabSplitContainer.ResumeLayout(false);
            this.tabSplitContainer2.Panel1.ResumeLayout(false);
            this.tabSplitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabSplitContainer2)).EndInit();
            this.tabSplitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SplitContainer historySplitContainer;
        private System.Windows.Forms.ListBox historyListBox;
        private System.Windows.Forms.Button openTabButton;
        private System.Windows.Forms.Button panelButton;
        private System.Windows.Forms.Button fontButton;
        private System.Windows.Forms.RichTextBox tabTextBox1;
        private System.Windows.Forms.Button viewButton;
        private System.Windows.Forms.SplitContainer tabSplitContainer;
        private System.Windows.Forms.Button fontSmallerButton;
        private System.Windows.Forms.Button fontLargerButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button moveToTopButton;
        private System.Windows.Forms.SplitContainer tabSplitContainer2;
        private System.Windows.Forms.RichTextBox tabTextBox2;
        private System.Windows.Forms.RichTextBox tabTextBox3;
    }
}

