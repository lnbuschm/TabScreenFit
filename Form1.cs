using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TabScreenFit
{
    // TODO:  modify accessed date to NOW when a file is re-added
    //  move to top button
    //   remove button
    //   save font, fontsize, view 
    //  add support for open with for multiple files
    //  fitting algorithm:
    //    find screensize, separate into 1/2
    public partial class Form1 : Form
    {
        String jsonFile = "";
        String jsonText = "";
        JsonTextReader jsonReader;
        JsonSerializer jsonSerializer = new JsonSerializer();
        List<HistoryEntry> history = new List<HistoryEntry>();
        Single fontSize = 8.25F;
        String fontName = "";
        String currentView = "Auto";

        public Form1(string[] args)
        {
            InitializeComponent();
            MyInitializer();
            if (args == null || args.Length == 0)
            {
                // select most recent tab to start
                if (historyListBox.Items.Count > 0)  this.historyListBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(args[0]);
                foreach(string s in args)
                {
                    if (!(Path.GetExtension(s) == ".txt"))
                    {
                        MessageBox.Show("File is not a text file.");
                    }
                    else if (!File.Exists(s))
                    {
                        MessageBox.Show("File does not exist.");
                    }
                    else
                    {
                        processFile(s);
                    }
                }

            }

        }

        private void MyInitializer()
        {
            // show 1 panel for tab view
            this.tabSplitContainer.Panel2Collapsed = true;
            this.tabSplitContainer.Panel2.Hide();
            // show side panel and load saved width
            this.splitContainer1.Panel1Collapsed = false;
            this.splitContainer1.Panel1.Show();
            this.historyListBox.Visible = true;
            int panel1Width = Properties.Settings.Default.Panel1Width;
            this.splitContainer1.SplitterDistance = panel1Width;
            // load saved font and size
            fontName = Properties.Settings.Default.Font.Name.ToString();
            fontSize = Properties.Settings.Default.FontSize;
            this.fontButton.Text = fontName;
            this.tabTextBox1.Font = new System.Drawing.Font(fontName,
                        fontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            // read tab history panel from json
            readJson();
        }

        private void readJson()
        {
            jsonFile = Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.ApplicationData), "tabscreenfit.json");
            // read file into a string and deserialize JSON to a type
            try
            {
                jsonText = File.ReadAllText(@jsonFile);
                jsonReader = new JsonTextReader(new StringReader(jsonText));
                jsonReader.SupportMultipleContent = true;
                // clear previous list, re-read entries ?????????????????? TODO  should we check if they exist?
              //  history = new List<HistoryEntry>();

                while (true)
                {
                    if (!jsonReader.Read())
                    {
                        break;
                    }
                    HistoryEntry history1 = jsonSerializer.Deserialize<HistoryEntry>(jsonReader);

                    // add to list if it doesnt already exist in list
                    if (!history.Any(item => item.fileName == history1.fileName)) {
                        history.Add(history1);
                    }
                    
                }
                // sort by descending accessed date
                history.Sort((y, x) => DateTime.Compare(x.AccessedDate, y.AccessedDate));

                //MessageBox.Show("Using history json file: " + jsonFile);
                foreach (HistoryEntry h in history)
                {
                    //MessageBox.Show("file: " + h.fileName);
                    //       historyListBox.Text += Path.GetFileNameWithoutExtension(h.toString()) + "\n";
                    this.historyListBox.Items.Add(h.ToString());

                }

            }
            catch (FileNotFoundException)
            {
                // create file on first run
                try
                {
                    File.WriteAllText(@jsonFile, "");
                    MessageBox.Show("Created history json file: " + jsonFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("History not enabled." + jsonFile + " File errror:  " + ex.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("History not enabled." + jsonFile + " File errror: " + ex.ToString());
            }
        }
        private void appendJson(string fileName)
        {
            // Create object
            HistoryEntry entry = new HistoryEntry
            {
                fileName = fileName,
                tabName = Path.GetFileNameWithoutExtension(fileName),
                CreatedDate = DateTime.Now,
                AccessedDate = DateTime.Now,
            };
            try
            {
                // serialize JSON to a string and then write string to a file
                File.AppendAllText(@jsonFile, JsonConvert.SerializeObject(entry));
            }
            catch (Exception ex)
            {
                tabTextBox1.Text = "\n\n\n\nCould not load File: " + openFileDialog1.FileName + " \n " + ex.ToString();
            }
        }

        // LNB : this function doesnt work yet
        //   we want to read json file, update a specific field,  write changes to json file
        private void updateAccessedDateJson(string fileName, DateTime newAccessed)
        {
            try
            {
                jsonText = File.ReadAllText(@jsonFile);
                string newJsonText = "";
                jsonReader = new JsonTextReader(new StringReader(jsonText));
                jsonReader.SupportMultipleContent = true;

                while (true)
                {
                    if (!jsonReader.Read())
                    {
                        break;
                    }
                    HistoryEntry history1 = jsonSerializer.Deserialize<HistoryEntry>(jsonReader);
                //    MessageBox.Show("tabName: " + history1.tabName + " fileName: " + fileName);
                    if (history1.tabName == fileName)
                    {
                        history.Remove(history1);

                        history1.AccessedDate = DateTime.Now;
                        history.Add(history1);

                        // update element in history list
                        //       var h = history.Find(file => Path.GetFileNameWithoutExtension(history1.fileName) == history1.fileName);
                        //        MessageBox.Show("Updated accessed");
                        //       var h = history.Find(file => Path.GetFileNameWithoutExtension(file.fileName) == historyListBox.SelectedItem.ToString());
                        //       string text = System.IO.File.ReadAllText(h.fileName, Encoding.Default);
                    }

                    newJsonText += "\n" + JsonConvert.SerializeObject(history1);

                }
                // write new json
                // serialize JSON to a string and then write string to a file
                File.WriteAllText(@jsonFile, newJsonText);

                // sort by descending accessed date
                history.Sort((y, x) => DateTime.Compare(x.AccessedDate, y.AccessedDate));
             //   this.historyListBox.Items.Clear();

                //MessageBox.Show("Using history json file: " + jsonFile);
                foreach (HistoryEntry h in history)
                {
                    //MessageBox.Show("file: " + h.fileName);
                    //       historyListBox.Text += Path.GetFileNameWithoutExtension(h.toString()) + "\n";
                    this.historyListBox.Items.Add(h.ToString());

                }
                updateLeftPanel();

            }
            catch (Exception ex)
            {
                MessageBox.Show("History error" + jsonFile + " File errror: " + ex.ToString());
            }
        }
        // open file, read it, show it in main panel, save json info
        private void processFile(string processFileName)
        {
            try
            {
                string text = System.IO.File.ReadAllText(@processFileName, Encoding.Default);
                this.tabTextBox1.Text = text;
                appendJson(@processFileName);
            }
            catch (Exception ex)
            {
                tabTextBox1.Text = "\n\n\n\nCould not load File: " + openFileDialog1.FileName + " \n " + ex.ToString();
            }

        }


        private void openTabButton_Click(object sender, EventArgs e)
        {
            // Show the Open File dialog. If the user chooses OK, load the 
            // tab that the user chose.
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                processFile(openFileDialog1.FileName);
            }
            // write file info to json

            // TODO: update left panel
            // TODO:  modify accessed date to NOW
            updateLeftPanel();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
        }

        // Toggle panel on/off
        private void panelButton_Click(object sender, EventArgs e)
        {
            int panel1Width = Properties.Settings.Default.Panel1Width;
            this.splitContainer1.SplitterDistance = panel1Width;
            if (!splitContainer1.Panel1Collapsed)
            {
                //    tableLayoutPanel1.ColumnCount = 1;
                splitContainer1.Panel1Collapsed = true;
                splitContainer1.Panel1.Hide();
                historyListBox.Visible = false;
            }
            else
            {
                //    tableLayoutPanel1.ColumnCount = 2;
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel1.Show();
                historyListBox.Visible = true;
            }
            Properties.Settings.Default["Panel1Width"] = this.splitContainer1.SplitterDistance;
            Properties.Settings.Default.Save();
        }

      //  int fontState = 1;
        private void fontButton_Click(object sender, EventArgs e)
        {
            switch (fontName)
            {
                case "":
                case "Lucida Console":
                    fontName = "Consolas";
                    break;
                case "DejaVu Sans Mono":
                    fontName = "Lucida Console";
                    break;
                case "Courier New":
                    fontName = "DejaVu Sans Mono";
                    break;
                case "Consolas":
                    fontName = "Courier New";
                    break;
            }
            this.tabTextBox1.Font = new System.Drawing.Font(fontName,
                        fontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
       //     fontState++;
       //     if (fontState > 4) fontState = 1;
            fontButton.Text = fontName;
            // TODO: Save current font to JSON
            //Properties.Settings.Default["SomeProperty"] = "Some Value";
            //Properties.Settings.Default.Save(); // Saves settings in application configuration file
            Properties.Settings.Default["Font"] = this.tabTextBox1.Font;
            Properties.Settings.Default.Save();
        }

        private void updateLeftPanel() // file name , not path
        {
            // reset left panel
            this.historyListBox.Items.Clear();
            // read json
            readJson();
            // sort by date

            // fill panel
        }

        private void historyListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (historyListBox.SelectedItem == null) return;
       //     updateLeftPanel(historyListBox.SelectedItem.ToString());
            // find selection in history List
            try
            {
                var h = history.Find(file => Path.GetFileNameWithoutExtension(file.fileName) == historyListBox.SelectedItem.ToString());
                string text = System.IO.File.ReadAllText(h.fileName, Encoding.Default);
                tabTextBox1.Text = text;
            }
            catch (Exception ex)
            {
                tabTextBox1.Text = "\n\n\n\n Could not load File: " + openFileDialog1.FileName + " \n " + ex.ToString();
            }

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Properties.Settings.Default["Panel1Width"] = this.splitContainer1.SplitterDistance;
            Properties.Settings.Default.Save();
        }

        private void viewButton_Click(object sender, EventArgs e)
        {
            switch (currentView)
            {
                case "Auto":
                    currentView = "1 split";
                    break;
                case "1 split":
                    currentView = "2 splits";
                    break;
                case "2 splits":
                    currentView = "3 splits";
                    break;
                case "3 splits":
                    currentView = "Auto";
                    break;
            }
            viewButton.Text = currentView;
        }

        private void fontLargerButton_Click(object sender, EventArgs e)
        {
            fontSize += 0.75F;
            this.tabTextBox1.Font = new System.Drawing.Font(fontName,
                       fontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Properties.Settings.Default["FontSize"] = fontSize;
            Properties.Settings.Default.Save();
        }

        private void fontSmallerButton_Click(object sender, EventArgs e)
        {
            fontSize -= 0.75F;
            this.tabTextBox1.Font = new System.Drawing.Font(fontName,
                       fontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Properties.Settings.Default["FontSize"] = fontSize;
            Properties.Settings.Default.Save();
        }

        private void moveToTopButton_Click(object sender, EventArgs e)
        {
            if (historyListBox.SelectedItem == null) return;
            //MessageBox.Show("Item: " + historyListBox.SelectedItem.ToString());
            updateAccessedDateJson(historyListBox.SelectedItem.ToString(), DateTime.Now);
        }
    }

}
