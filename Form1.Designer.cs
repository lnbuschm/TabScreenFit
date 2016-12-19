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
            this.youtubeButton = new System.Windows.Forms.Button();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.removeButton = new System.Windows.Forms.Button();
            this.moveToTopButton = new System.Windows.Forms.Button();
            this.historyListBox = new System.Windows.Forms.ListBox();
            this.tabSplitContainer = new System.Windows.Forms.SplitContainer();
            this.tabSplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabTextBox1 = new TabScreenFit.SyncTextBox();
            this.tabTextBox2 = new TabScreenFit.SyncTextBox();
            this.tabTextBox3 = new TabScreenFit.SyncTextBox();
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
            this.historySplitContainer.Panel1.Controls.Add(this.youtubeButton);
            this.historySplitContainer.Panel1.Controls.Add(this.searchBox);
            this.historySplitContainer.Panel1.Controls.Add(this.removeButton);
            this.historySplitContainer.Panel1.Controls.Add(this.moveToTopButton);
            this.historySplitContainer.Panel1.Controls.Add(this.historyListBox);
            // 
            // historySplitContainer.Panel2
            // 
            this.historySplitContainer.Panel2.Controls.Add(this.tabSplitContainer);
            this.historySplitContainer.Size = new System.Drawing.Size(790, 418);
            this.historySplitContainer.SplitterDistance = 263;
            this.historySplitContainer.TabIndex = 0;
            this.historySplitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // youtubeButton
            // 
            this.youtubeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.youtubeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.youtubeButton.Location = new System.Drawing.Point(98, 366);
            this.youtubeButton.Name = "youtubeButton";
            this.youtubeButton.Size = new System.Drawing.Size(59, 22);
            this.youtubeButton.TabIndex = 11;
            this.youtubeButton.Text = "YouTube!";
            this.youtubeButton.UseVisualStyleBackColor = true;
            this.youtubeButton.Click += new System.EventHandler(this.youtubeButton_Click);
            // 
            // searchBox
            // 
            this.searchBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.searchBox.Location = new System.Drawing.Point(7, 366);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(85, 20);
            this.searchBox.TabIndex = 10;
            this.searchBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.searchBox_MouseClick);
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeButton.Location = new System.Drawing.Point(85, 392);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 22);
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
            this.moveToTopButton.Size = new System.Drawing.Size(82, 22);
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
            // tabSplitContainer2
            // 
            this.tabSplitContainer2.AllowDrop = true;
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
            // tabTextBox1
            // 
            this.tabTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tabTextBox1.Buddies = null;
            this.tabTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTextBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabTextBox1.Location = new System.Drawing.Point(0, 0);
            this.tabTextBox1.Name = "tabTextBox1";
            this.tabTextBox1.Size = new System.Drawing.Size(275, 418);
            this.tabTextBox1.TabIndex = 6;
            this.tabTextBox1.Text = "";
            this.tabTextBox1.WordWrap = false;
            this.tabTextBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabTextBox1_MouseDown);
            this.tabTextBox1.DoubleClick += new System.EventHandler(this.tabTextBox1_DoubleClick);
            // 
            // tabTextBox2
            // 
            this.tabTextBox2.Buddies = null;
            this.tabTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTextBox2.Location = new System.Drawing.Point(0, 0);
            this.tabTextBox2.Name = "tabTextBox2";
            this.tabTextBox2.Size = new System.Drawing.Size(127, 418);
            this.tabTextBox2.TabIndex = 0;
            this.tabTextBox2.Text = "";
            this.tabTextBox2.WordWrap = false;
            // 
            // tabTextBox3
            // 
            this.tabTextBox3.Buddies = null;
            this.tabTextBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTextBox3.Location = new System.Drawing.Point(0, 0);
            this.tabTextBox3.Name = "tabTextBox3";
            this.tabTextBox3.Size = new System.Drawing.Size(113, 418);
            this.tabTextBox3.TabIndex = 0;
            this.tabTextBox3.Text = "";
            this.tabTextBox3.WordWrap = false;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 418);
            this.Controls.Add(this.historySplitContainer);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Tab Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.historySplitContainer.Panel1.ResumeLayout(false);
            this.historySplitContainer.Panel1.PerformLayout();
            this.historySplitContainer.Panel2.ResumeLayout(false);
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
        private System.Windows.Forms.SplitContainer tabSplitContainer;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button moveToTopButton;
        private System.Windows.Forms.SplitContainer tabSplitContainer2;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Button youtubeButton;
        private SyncTextBox tabTextBox1;
        private SyncTextBox tabTextBox2;
        private SyncTextBox tabTextBox3;
    }
}

