﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheCodeKing.ActiveButtons.Controls;


namespace TabScreenFit
{

    //  fitting algorithm:
    //    find screensize, separate into 1/2
    //   - locate blocks on the screen that are available
    //  TODO:  need to add designator to displayed tab name for duplicate names but non duplicate filename
    //  TODO?  Save current view history entry 
    //11/28 - TODO add search highlighting in history
    //  add a search youtube button
    //   add a debug window overlay
    //  check for duplicates on open and if not from the same file+path , change tabname to add (1) etc.
    //  TODO : modify         private void scrollToLine(int lineNum ) so 
    //                         that when lines are too high and "Auto" is set, reduce number of tab panels
    public partial class Form1 : Form
    {
        string jsonFile = "";
        string jsonText = "";
        JsonTextReader jsonReader;
        JsonSerializer jsonSerializer = new JsonSerializer();
        List<HistoryEntry> history = new List<HistoryEntry>();
        Single defaultFontSize = 8.25F;
        string defaultFontName = "Courier New";
        Single fontSize = 8.25F;
        string fontName = "";
        string currentView = "Auto"; // default currentview set in currentViewSM()
        string currentFilename = "";
        //  int currentHistoryEntryIndex;
        double linesPerScreen;
        RECT tabTextBox1Rect;
        int VISIBLE_TAB_PANELS;
        bool searchInProgress = false;
        bool historyListBoxClicked = false;
        ActiveButton titleTogglePanelButton;
        ActiveButton titleFontButton;
        ActiveButton titleFontLargerButton;
        ActiveButton titleFontSmallerButton;
        ActiveButton titleViewButton;
        ActiveButton titleOpenButton;


        public Form1(string[] args)
        {
            InitializeComponent();
            MyInitializer();
            if (historyListBox.Items.Count > 0) this.historyListBox.SelectedIndex = 0;
            // check if program is opened using "open with.." on a txt file
            if (!(args == null || args.Length == 0))
            {
                //  MessageBox.Show(args[0]);
                foreach (string s in args)
                {
                    if (!(Path.GetExtension(s) == ".txt"))
                    {
                        MessageBox.Show("File is not a text file.");
                        if (historyListBox.Items.Count > 0) this.historyListBox.SelectedIndex = 0;

                    }
                    else if (!File.Exists(s))
                    {
                        MessageBox.Show("File does not exist.");
                        if (historyListBox.Items.Count > 0) this.historyListBox.SelectedIndex = 0;

                    }
                    else
                    {
                        this.historyListBox.SelectedIndex = 1;
                        //   MessageBox.Show("Processing " + s);
                        bool duplicateFound = false;
                        foreach (HistoryEntry h in history)
                        {
                            if (h.fileName == s)
                            {
                                //    MessageBox.Show("Duplicate found");
                                duplicateFound = true;
                                openTab(s);
                                updateAccessedDateJson(Path.GetFileNameWithoutExtension(s), DateTime.Now);
                                break;
                            }
                        }
                        if (!duplicateFound) processNewFile(s);


                    }
                }
                
             //   this.historyListBox.SelectedIndex = 0;

                updateLeftPanel();
                // select most recent tab to start
              //  if (historyListBox.Items.Count > 0) this.historyListBox.SelectedIndex = 0;
            }
            else
            {
                if (historyListBox.Items.Count > 0) this.historyListBox.SelectedIndex = 0;

            }

        }
        Control[] syncedCtrls;
        private void MyInitializer()
        {
            tabTextBox1Rect = new RECT();

            // add the required controls into scroll sync
            syncedCtrls = new Control[] { tabTextBox1, tabTextBox2, tabTextBox3 };
            foreach (SyncTextBox ctr in syncedCtrls)
            {
                ctr.Buddies = syncedCtrls;
            }

            // Set window location
            if (Properties.Settings.Default.WindowLocation != null)
            {
                this.Location = Properties.Settings.Default.WindowLocation;
                // MessageBox.Show("WindowLocation = " + Location);
            }

            // Set window size
            if (Properties.Settings.Default.WindowSize != null)
            {
                this.Size = Properties.Settings.Default.WindowSize;
            }

            currentViewSM();

            // show side panel and load saved width
            this.historySplitContainer.Panel1Collapsed = false;
            this.historySplitContainer.Panel1.Show();
            this.historyListBox.Visible = true;

            int HistoryPanelWidth = Properties.Settings.Default.HistoryPanelWidth;
            //       MessageBox.Show("panelwidth = " + HistoryPanelWidth);
            this.historySplitContainer.SplitterDistance = HistoryPanelWidth;


            // load saved font and size
            //     fontName = Properties.Settings.Default.Font.Name.ToString();
            //     fontSize = Properties.Settings.Default.FontSize;
            //     this.fontButton.Text = fontName;
            updateFontInTabbox();

            // read tab history panel from json
            readJson();

            AddButton(ref titleTogglePanelButton, "Toggle Left Panel On/Off", panelButton_Click);
            AddButton(ref titleFontButton, "x             Font              x", fontButton_Click);
            AddButton(ref titleFontLargerButton, "+", fontLargerButton_Click);
            AddButton(ref titleFontSmallerButton, "-", fontSmallerButton_Click);
            AddButton(ref titleViewButton, "x  View  x", viewButton_Click);
            AddButton(ref titleOpenButton, "Open file (.txt)", openTabButton_Click);
            //     MessageBox.Show("myInitializer()");
        }

        /// <summary>
        /// 	Helper method for adding items to the menu bar.
        /// </summary>
        /// <param name = "text"></param>
        /// <param name = "handler"></param>
        private void AddButton(ref ActiveButton  button, string text, EventHandler handler)
        {
            // get an instance of IActiveMenu used to attach
            // buttons to the form
            IActiveMenu menu = ActiveMenu.GetInstance(this);

            // define a new button

            button = new ActiveButton();
            button.Text = text;
            if (text == "x             Font              x")
            {
                menu.ToolTip.SetToolTip(button, "Font");

            }
            else if (text == "x  View  x")
            {
                menu.ToolTip.SetToolTip(button, "View");
            }
            else
            {
                menu.ToolTip.SetToolTip(button, button.Text);

            }
            // button.BackColor = colorSwitch.BackColor;
            button.Click += handler;

            // add the button to the menu
            menu.Items.Add(button);

         //   errorLabel.Text = "";
         //   buttonText.Text = (buttonInt++).ToString();
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
                    if (!history.Any(item => item.fileName == history1.fileName))
                    {
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
                fontSize = defaultFontSize,
                fontName = defaultFontName,
                view = "Auto",
                yScroll = 0,
            };
            try
            {
                // serialize JSON to a string and then write string to a file
                File.AppendAllText(@jsonFile, JsonConvert.SerializeObject(entry));
            }
            catch (Exception ex)
            {
                tabTextBox1.Text = "\n\n\n\nappendJson() Could not load File: " + openFileDialog1.FileName + " \n " + ex.ToString();
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
                        var itemToRemove = history.Single(h => h.fileName == history1.fileName);
                        history.Remove(itemToRemove);
                        //   MessageBox.Show("Found entry?: " + history.Remove(itemToRemove));
                        //    MessageBox.Show("Updated tabName: " + history1.tabName + " fileName: " + fileName);

                        history1.AccessedDate = newAccessed;
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

        //   we want to read json file, update a specific field,  write changes to json file
        private void updateFontsizeJson(string fileName, Single newFontsize)
        {
            try
            {
                jsonText = File.ReadAllText(@jsonFile);
                string newJsonText = "";
                jsonReader = new JsonTextReader(new StringReader(jsonText));
                jsonReader.SupportMultipleContent = true;

                while (true)
                {
                    if (!jsonReader.Read()) break;
                    HistoryEntry history1 = jsonSerializer.Deserialize<HistoryEntry>(jsonReader);
                    if (history1.fileName == fileName)
                    {
                        var itemToRemove = history.Single(h => h.fileName == history1.fileName);
                        history.Remove(itemToRemove);

                        history1.fontSize = newFontsize;
                        history.Add(history1);
                    }

                    newJsonText += "\n" + JsonConvert.SerializeObject(history1);

                }
                // write new json
                // serialize JSON to a string and then write string to a file
                File.WriteAllText(@jsonFile, newJsonText);

            }
            catch (Exception ex)
            {
                MessageBox.Show("History error" + jsonFile + " File errror: " + ex.ToString());
            }
        }

        //   we want to read json file, update a specific field,  write changes to json file
        private void updateViewJson(string fileName, string view)
        {
            try
            {
                jsonText = File.ReadAllText(@jsonFile);
                string newJsonText = "";
                jsonReader = new JsonTextReader(new StringReader(jsonText));
                jsonReader.SupportMultipleContent = true;

                while (true)
                {
                    if (!jsonReader.Read()) break;
                    HistoryEntry history1 = jsonSerializer.Deserialize<HistoryEntry>(jsonReader);
                    if (history1.fileName == fileName)
                    {
                        var itemToRemove = history.Single(h => h.fileName == history1.fileName);
                        history.Remove(itemToRemove);

                        history1.view = view;
                        history.Add(history1);
                    }

                    newJsonText += "\n" + JsonConvert.SerializeObject(history1);

                }
                // write new json
                // serialize JSON to a string and then write string to a file
                File.WriteAllText(@jsonFile, newJsonText);

            }
            catch (Exception ex)
            {
                MessageBox.Show("History error" + jsonFile + " File errror: " + ex.ToString());
            }
        }

        private void updateYscrollJson(string fileName, int newYscroll)
        {
            try
            {
                // tabTextBox1.ScrollToCaret
                jsonText = File.ReadAllText(@jsonFile);
                string newJsonText = "";
                jsonReader = new JsonTextReader(new StringReader(jsonText));
                jsonReader.SupportMultipleContent = true;

                while (true)
                {
                    if (!jsonReader.Read()) break;
                    HistoryEntry history1 = jsonSerializer.Deserialize<HistoryEntry>(jsonReader);
                    if (history1.fileName == fileName)
                    {
                        var itemToRemove = history.Single(h => h.fileName == history1.fileName);
                        history.Remove(itemToRemove);
                        //      MessageBox.Show("Updated Y scroll to: " + newYscroll);
                        history1.yScroll = newYscroll;
                        //      tabTextBox1.SelectionStart = newYscroll

                        history.Add(history1);
                    }

                    newJsonText += "\n" + JsonConvert.SerializeObject(history1);

                }
                // write new json
                // serialize JSON to a string and then write string to a file
                File.WriteAllText(@jsonFile, newJsonText);

            }
            catch (Exception ex)
            {
                MessageBox.Show("History error" + jsonFile + " File errror: " + ex.ToString());
            }
        }


        private void updateFontnameJson(string fileName, string newFontName)
        {
            try
            {
                jsonText = File.ReadAllText(@jsonFile);
                string newJsonText = "";
                jsonReader = new JsonTextReader(new StringReader(jsonText));
                jsonReader.SupportMultipleContent = true;

                while (true)
                {
                    if (!jsonReader.Read()) break;
                    HistoryEntry history1 = jsonSerializer.Deserialize<HistoryEntry>(jsonReader);
                    if (history1.fileName == fileName)
                    {
                        var itemToRemove = history.Single(h => h.fileName == history1.fileName);
                        history.Remove(itemToRemove);

                        history1.fontName = newFontName;
                        history.Add(history1);
                    }

                    newJsonText += "\n" + JsonConvert.SerializeObject(history1);

                }
                // write new json
                // serialize JSON to a string and then write string to a file
                File.WriteAllText(@jsonFile, newJsonText);

            }
            catch (Exception ex)
            {
                MessageBox.Show("History error" + jsonFile + " File errror: " + ex.ToString());
            }
        }

        // open file, read it, show it in main panel, save json info
        private void processNewFile(string processFileName)
        {
            try
            {
                string text = System.IO.File.ReadAllText(@processFileName, Encoding.Default);
                setupVisibleTabPanels(text, 0);
                currentFilename = processFileName;
                //MessageBox.Show("process filename: " + processFileName);
                appendJson(@processFileName);
            }
            catch (Exception ex)
            {
                tabTextBox1.Text = "\n\n\n\nprocessNewFile() Could not load File: " + openFileDialog1.FileName + " \n " + ex.ToString();
            }

        }


        private void openTab(string openFilename)
        {
            bool duplicateFound = false;
            // check if file already exists
            foreach (HistoryEntry h in history)
            {
                if (h.fileName == openFilename)
                {
                    //    MessageBox.Show("Duplicate found");
                    updateAccessedDateJson(Path.GetFileNameWithoutExtension(openFilename), DateTime.Now);
                    duplicateFound = true;
                    break;
                }
            }
            //       foreach (ListBox.ObjectCollection item in historyListBox.Items) {
            //           if (item.)
            //       }
            // if not , process file normally
            if (!duplicateFound) processNewFile(openFilename);
            else historyListBox.SelectedIndex = 0;

            updateLeftPanel();
            currentViewSM();
        }

        private void openTabButton_Click(object sender, EventArgs e)
        {
            // Show the Open File dialog. If the user chooses OK, load the 
            // tab that the user chose.
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string openFilename in openFileDialog1.FileNames.Reverse())
                {
                    openTab(openFilename);
                }
                updateLeftPanel();

            }
            // write file info to json

            // TODO: update left panel
            // TODO:  modify accessed date to NOW
        }

        // Toggle panel on/off
        private void panelButton_Click(object sender, EventArgs e)
        {
            int HistoryPanelWidth = Properties.Settings.Default.HistoryPanelWidth;
            this.historySplitContainer.SplitterDistance = HistoryPanelWidth;
            if (!historySplitContainer.Panel1Collapsed)
            {
                //    tableLayoutPanel1.ColumnCount = 1;
                historySplitContainer.Panel1Collapsed = true;
                historySplitContainer.Panel1.Hide();
                historyListBox.Visible = false;
            }
            else
            {
                //    tableLayoutPanel1.ColumnCount = 2;
                historySplitContainer.Panel1Collapsed = false;
                historySplitContainer.Panel1.Show();
                historyListBox.Visible = true;
            }
            Properties.Settings.Default.HistoryPanelWidth = this.historySplitContainer.SplitterDistance;

            //      Properties.Settings.Default["HistoryPanelWidth"] = this.splitContainer1.SplitterDistance;
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
            updateFontInTabbox();
            //     fontState++;
            //     if (fontState > 4) fontState = 1;
            //fontButton.Text = fontName;
            titleFontButton.Text = fontName.PadRight(16);
            // TODO: Save current font to JSON
            //Properties.Settings.Default["SomeProperty"] = "Some Value";
            //Properties.Settings.Default.Save(); // Saves settings in application configuration file
            //         Properties.Settings.Default["Font"] = this.tabTextBox1.Font;
            //         Properties.Settings.Default.Save();
            if (currentFilename != "") updateFontnameJson(currentFilename, fontName);

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

        private void setupVisibleTabPanels(string text, int yScroll)
        {
            //  int linesPerScreen = Convert.ToInt32(((double)tabTextBox1.Height / (double)(tabTextBox1.Font.Height + 1)));

            //   string[] textHighlighted = Regex.Split(text, "\r\n|\r|\n");



            if (VISIBLE_TAB_PANELS == 1)
            {
                this.tabTextBox1.Text = text;
                this.tabTextBox2.Text = text;
                this.tabTextBox3.Text = text;
            }
            else if (VISIBLE_TAB_PANELS == 2)
            {
                this.tabTextBox1.Text = text;
                this.tabTextBox2.Text = text;
                this.tabTextBox3.Text = text;
            }
            else if (VISIBLE_TAB_PANELS == 3)
            {
                this.tabTextBox1.Text = text;
                this.tabTextBox2.Text = text;
                this.tabTextBox3.Text = text;
            }
            int lineToScrollTo = yScroll + Convert.ToInt16(linesPerScreen) - 3; // - 3;  // -3 works
            int firstCharOfLineIndex = tabTextBox1.GetFirstCharIndexFromLine(lineToScrollTo);

            if (firstCharOfLineIndex > 0)
            {
                int currentLine = tabTextBox1.GetLineFromCharIndex(firstCharOfLineIndex);
                int firstCharOfNextLineIndex = tabTextBox1.GetFirstCharIndexFromLine(lineToScrollTo + 1);
                // if line is blank, choose a  line that has some text
                while (firstCharOfNextLineIndex == -1)
                {
                    MessageBox.Show("Highlighted line: " + linesPerScreen +
" \n firstCharOfLineIndex: " + firstCharOfLineIndex + " \n firstCharOfNextLineIndex: " + firstCharOfNextLineIndex + "\n currentLine " + currentLine);

                    lineToScrollTo--;
                    firstCharOfLineIndex = tabTextBox1.GetFirstCharIndexFromLine(lineToScrollTo);
                    currentLine = tabTextBox1.GetLineFromCharIndex(firstCharOfLineIndex);
                    firstCharOfNextLineIndex = tabTextBox1.GetFirstCharIndexFromLine(lineToScrollTo + 1);
                    //       firstCharOfNextLineIndex = firstCharOfLineIndex;
                }

                this.tabTextBox1.Select(firstCharOfLineIndex, firstCharOfNextLineIndex - firstCharOfLineIndex); // currentLine);
                this.tabTextBox1.SelectionBackColor = Color.Yellow;
                this.tabTextBox1.Select(0, 0);
                if ((VISIBLE_TAB_PANELS == 2) || (VISIBLE_TAB_PANELS == 3))
                {
                    //             MessageBox.Show("Highlighted line: " + linesPerScreen +
                    //           " \n firstCharOfLineIndex: " + firstCharOfLineIndex + "\n currentLine " + currentLine);
                    this.tabTextBox2.Select(firstCharOfLineIndex, firstCharOfNextLineIndex - firstCharOfLineIndex); // currentLine);
                    this.tabTextBox2.SelectionBackColor = Color.Yellow;
                    this.tabTextBox2.Select(0, 0);
                    //      tabTextBox2.AppendText("");
                }
                else if (VISIBLE_TAB_PANELS == 3)
                {
                    this.tabTextBox2.Select(firstCharOfLineIndex, firstCharOfNextLineIndex - firstCharOfLineIndex); // currentLine);
                    this.tabTextBox2.SelectionBackColor = Color.Yellow;
                    this.tabTextBox2.Select(0, 0);
                    this.tabTextBox3.Select(firstCharOfLineIndex, firstCharOfNextLineIndex - firstCharOfLineIndex); // currentLine);
                    this.tabTextBox3.SelectionBackColor = Color.Yellow;
                    this.tabTextBox3.Select(0, 0);
                }
            }


            int currentHistoryEntryIndex = history.FindIndex(h => h.tabName.Equals(Path.GetFileNameWithoutExtension(currentFilename), StringComparison.Ordinal));
            scrollToLine(history.ElementAt(currentHistoryEntryIndex).yScroll);
        }

        List<string> searchHistoryItems = new List<string>();
        private void historyListBox_MouseDown(object sender, MouseEventArgs e)
        {
            updateYscrollInJson();
            //      MessageBox.Show("Selected index: " + historyListBox.SelectedIndex +
            //                       "\n text: " + historyListBox.SelectedItem.ToString());
            if (searchInProgress)
            {
                historyListBoxClicked = true;
                for (int i = 0; i < historyListBox.Items.Count; i++)
                {
                    if (historyListBox.GetSelected(i))
                    {
                        searchHistoryItems.Remove(historyListBox.Items[i].ToString());
                    }
                    historyListBox.SetSelected(i, false);
                }
                //   MessageBox.Show("search history size: " + searchHistoryItems.Count);
                historyListBox.SelectionMode = SelectionMode.One;
                displayItem(searchHistoryItems.ElementAt(0));
            }
            else
            {
                historyListBoxClicked = false;
            }
            searchInProgress = false;


        }
        // update y scroll in json before changing selection
        private void updateYscrollInJson()
        {
            //          if (tabTextBox1.GetPositionFromCharIndex(0).Y == 0) return;
            double pixelsPerLine = tabTextBox1.Height * 1.0 / linesPerScreen;
            //    int linesToScroll = Convert.ToInt32((-1.0 * tabTextBox1.GetPositionFromCharIndex(0).Y/ pixelsPerLine));// * linesPerScreen);// / tabTextBox1.Height;
            // works ?        int linesToScroll = Convert.ToInt32((-1.0 * tabTextBox1.GetPositionFromCharIndex(0).Y/ pixelsPerLine));// * linesPerScreen);// / tabTextBox1.Height;
            int linesToScroll = Convert.ToInt32((-1.0 * tabTextBox1.GetPositionFromCharIndex(0).Y / (double)(tabTextBox1.Font.Height + 1)));// * linesPerScreen);// / tabTextBox1.Height;
                                                                                                                                            //    MessageBox.Show("updating yscrol");

            // works ok        int linesToScroll = Convert.ToInt32(Math.Floor((-1.0 * tabTextBox1.GetPositionFromCharIndex(0).Y / (double)tabTextBox1.Font.Height)));// * linesPerScreen);// / tabTextBox1.Height;

            //     int lineToScroll = Convert.ToInt32((-1.0 * tabTextBox1.GetPositionFromCharIndex(0).Y / (double)tabTextBox1.Font.Height));// * linesPerScreen);// / tabTextBox1.Height;
            //    int lineToScroll = Convert.ToInt32(((double) -tabTextBox1.GetPositionFromCharIndex(0).Y / tabTextBox1.Font.Height) * linesPerScreen);// / tabTextBox1.Height;
            //  int scroll1 = tabTextBox1.GetCharFromPosition(lineToScroll);
            int scroll1 = tabTextBox1.GetFirstCharIndexFromLine(linesToScroll);
            if (currentFilename != "") updateYscrollJson(currentFilename, linesToScroll);
            //           MessageBox.Show("Y scroll: " + lineToScroll.ToString()
            //           + "\n -tabTextBox1.GetPositionFromCharIndex(0).Y=" + tabTextBox1.GetPositionFromCharIndex(0).Y
            //           + "\n  this.tabTextBox1.Font.Height="+ this.tabTextBox1.Font.Height);
            //       MessageBox.Show("wrote to json: y scroll, index 0: " + tabTextBox1.GetPositionFromCharIndex(0).Y.ToString());
            //     MessageBox.Show("Y scroll, selectionstart: " + tabTextBox1.GetPositionFromCharIndex(tabTextBox1.SelectionStart).Y.ToString());

            //      tabTextBox1.SelectionStart = newYscroll
        }
        private void historyListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (historyListBox.SelectedItem == null) return;

            if (searchInProgress) return;
            if (historyListBoxClicked)
            {
                historyListBox.SelectionMode = SelectionMode.One;
                historyListBoxClicked = false;
                //     MessageBox.Show("clicked item: " + historyListBox.SelectedValue);
                //  return;
            }
            //     updateLeftPanel(historyListBox.SelectedItem.ToString());
            // find selection in history List
            //     try
            //    {
            displayItem(historyListBox.SelectedItem.ToString());

        }

        private void displayItem(string itemName)
        {
            int currentHistoryEntryIndex = history.FindIndex(h => h.tabName.Equals(itemName, StringComparison.Ordinal));

            //     MessageBox.Show("currentHistoryEntryIndex: " + currentHistoryEntryIndex);
            //???     if (currentFilename.Contains(itemName)) return;
            currentFilename = history.ElementAt(currentHistoryEntryIndex).fileName;

            //       else currentHistoryEntryIndex = newHistoryEntryIndex;
            //          currentHistoryEntryIndex = history.FindIndex(h => h.tabName.Equals(historyListBox.SelectedItem.ToString(), StringComparison.Ordinal));
            //    tabTextBox1.SelectionStart = newYscroll

            //     var h = history.Find(file => Path.GetFileNameWithoutExtension(file.fileName) == historyListBox.SelectedItem.ToString());
            //     string text = System.IO.File.ReadAllText(h.fileName, Encoding.Default);
            //     currentFilename = h.fileName;
            string text = System.IO.File.ReadAllText(history.ElementAt(currentHistoryEntryIndex).fileName, Encoding.Default);
            fontSize = history.ElementAt(currentHistoryEntryIndex).fontSize;
            fontName = history.ElementAt(currentHistoryEntryIndex).fontName;
            updateFontInTabbox();
          //  this.fontButton.Text = fontName;
            titleFontButton.Text = fontName.PadRight(16);


            currentView = history.ElementAt(currentHistoryEntryIndex).view;
           // this.viewButton.Text = currentView;
            titleViewButton.Text = currentView;

            SendMessage(this.tabTextBox1.Handle, EM_GETRECT, IntPtr.Zero, ref tabTextBox1Rect);
            //        linesPerScreen = (tabTextBox1Rect.Bottom - tabTextBox1Rect.Top) * 1.0 / this.tabTextBox1.Font.Height;// works?
            linesPerScreen = ((double)tabTextBox1.Height / (double)(tabTextBox1.Font.Height + 1));

            setupVisibleTabPanels(text, history.ElementAt(currentHistoryEntryIndex).yScroll);
            ///// TODO :: Fix this ..  doesn't scroll correctly
            //      tabTextBox1.SelectionStart = history.ElementAt(currentHistoryEntryIndex).yScroll;
            //      tabTextBox1.ScrollToCaret();
            //     scrollToCharIndex(history.ElementAt(currentHistoryEntryIndex).yScroll);
            scrollToLine(history.ElementAt(currentHistoryEntryIndex).yScroll);

            //            MessageBox.Show("scroll to selectionstart: " + history.ElementAt(currentHistoryEntryIndex).yScroll.ToString());
            //      tabTextBox1. tabTextBox1.GetPositionFromCharIndex(0).Y

            //   MessageBox.Show(history.ElementAt(historyListBox.SelectedIndex).tabName);
            //      }
            //      catch (Exception ex)
            //      {
            //           tabTextBox1.Text = "\n\n\n\nhistoryListBox_SelectedIndexChanged() Could not load File: " + openFileDialog1.FileName + " \n " + ex.ToString();
            //      }
            currentViewSM();

            //      MessageBox.Show("lines per screen =" + linesPerScreen + ", textbox height=" + tabTextBox1.Height + ", caretposition=" + tabTextBox1.SelectionStart.ToString());
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //            Properties.Settings.Default["HistoryPanelWidth"] = this.splitContainer1.SplitterDistance;
            Properties.Settings.Default.HistoryPanelWidth = this.historySplitContainer.SplitterDistance;
            Properties.Settings.Default.Save();
            //    MessageBox.Show("width saved");
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
            //viewButton.Text = currentView;
            titleViewButton.Text = currentView;
            int linesToScroll = Convert.ToInt32((-1.0 * tabTextBox1.GetPositionFromCharIndex(0).Y / (double)(tabTextBox1.Font.Height + 1)));// * linesPerScreen);// / tabTextBox1.Height;

            setupVisibleTabPanels(tabTextBox1.Text, linesToScroll);

            currentViewSM();

            updateViewJson(currentFilename, currentView);
        }

        private void currentViewSM()
        {
            switch (currentView)
            {
                case "Auto":
                    // TODO :: auto logic here .... for now pick 2 panels
                    // check number of lines in file
                    //int linesPerScreen = Convert.ToInt32(((double)tabTextBox1.Height / (double)(tabTextBox1.Font.Height + 1)));
                    //linesPerScreen = (tabTextBox1Rect.Bottom - tabTextBox1Rect.Top) * 1.0 / this.tabTextBox1.Font.Height;

                    string[] currentTab = tabTextBox1.Text.Split('\n');
                    int linesInCurrentTab = currentTab.Length;

                    //   using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
                    using (Graphics gt1 = tabTextBox1.CreateGraphics())
                    {
                        SizeF lineWidth;
                        List<float> lineWidths = new List<float>();
                        IList<int> lineWidths2 = new List<int>();

                        //      using (StringReader reader = new StringReader(tabTextBox1.Text))
                        // should we maybe read only the visible screen to decide on the splitterdistance ???
                        //         foreach (string line in currentTab.Skip(Convert.ToInt16(linesPerScreen)).Take(Convert.ToInt16(linesPerScreen)))

                        foreach (string line in currentTab.Take(Convert.ToInt16(linesPerScreen)))
                        {
                            //  string line;
                            //   while ((line = reader.ReadLine()) != null)
                            //   {
                            lineWidth = gt1.MeasureString(line.Trim(), new System.Drawing.Font(fontName,
                                 fontSize, System.Drawing.FontStyle.Regular,
                                 System.Drawing.GraphicsUnit.Point, ((byte)(0))));
                            //    MessageBox.Show(lineWidth.Width.ToString());
                            if (lineWidth.Width > 10) lineWidths.Add(lineWidth.Width);
                            int lineWidth2 = TextRenderer.MeasureText(line, tabTextBox1.Font).Width;
                            if (lineWidth2 > 10) lineWidths2.Add(lineWidth2);

                            //    }
                        }
                        // playing with kmeans
                        double[][] kmeans = new double[lineWidths.Count][];
                        List<double> doubles = new List<double>(lineWidths.Count);
                        int counter = 0;
                        //       List<float> lstOrderedA = doubles.OrderBy(item => item).ToList();

                        //     List<float> sortedLineWidths =  (from m in lineWidths order by m.propertyName).ToList();
                        //      var list = (from t in lineWidths
                        //                  orderby t.doubleVal).ToList();
                        lineWidths.Sort();
                        foreach (float f in lineWidths)
                        {
                            //   doubles.Add(Convert.ToDouble(f));
                            kmeans[counter] = new double[] { Convert.ToDouble(f), Convert.ToDouble(f) };
                            counter++;
                        }
                        if (lineWidths.Count > 0) KMeans.Main_method(kmeans);

                        float screenWidth = gt1.VisibleClipBounds.Width;


                        if (lineWidths.Count > 1)
                        {
                            string printString = "lineWidths list \n------------\n";
                            foreach (object o in lineWidths)
                            {
                                // Add the fields you want to show here
                                printString += o.ToString() + "    |    ";
                            }
                            //     MessageBox.Show(printString);

                            // check median (widest or average??) line in tab
                            float median = MedianCalc.Median(lineWidths);
                            float upperquartile = MedianCalc.NthOrderStatistic(lineWidths, lineWidths.Count * 3 / 4);
                            float max = MedianCalc.NthOrderStatistic(lineWidths, lineWidths.Count - 1);
                            float maxminus1 = MedianCalc.NthOrderStatistic(lineWidths, lineWidths.Count - 2);

                            double stdDev = MedianCalc.StandardDeviation(lineWidths);
                            double average = lineWidths.Average();


                            float median2 = MedianCalc.Median(lineWidths2);
                            float upperquartile2 = MedianCalc.NthOrderStatistic(lineWidths2, lineWidths2.Count * 3 / 4);
                            float max2 = MedianCalc.NthOrderStatistic(lineWidths2, lineWidths2.Count - 1);
                            float max2minus1 = MedianCalc.NthOrderStatistic(lineWidths2, lineWidths2.Count - 2);

                            double stdDev2 = MedianCalc.StandardDeviation(lineWidths2);
                            double average2 = lineWidths2.Average();
                            // let's find a std deviation , check for upper outlier and ignore


                            //     float max = 
                            //       MessageBox.Show("line median: " + median +
                            //           "\n tabTextBox1 width: " + tabTextBox1.Width +
                            //           "\n lineWidths.Count: " + lineWidths.Count
                            //           );
                            if ((linesInCurrentTab > linesPerScreen))
                            {
                                // adjust slider position of 
                                ShowTwoTabPanels();

                                // choose width of tabtextbox1 .. don't use max but instead look for some value within stddev from max
                                float newTabTabTextBoxWidth = max2; // Convert.ToInt16(max2);
                                int count = 0;
                                int compare = Convert.ToInt32(stdDev2) + MedianCalc.NthOrderStatistic(lineWidths2, lineWidths2.Count - 2);
                                //  while (newTabTabTextBoxWidth <= (stdDev2 + MedianCalc.NthOrderStatistic(lineWidths2, lineWidths2.Count - 2 - count)))
                                while (newTabTabTextBoxWidth > (compare))
                                {
                                    newTabTabTextBoxWidth = MedianCalc.NthOrderStatistic(lineWidths2, lineWidths2.Count - 2 - count); // max minus 1
                                    count++;
                                    compare = Convert.ToInt32(stdDev2 / 2) + MedianCalc.NthOrderStatistic(lineWidths2, lineWidths2.Count - 2 - count);
                                }

                                // set splitter distance for 2 panel view
                                //  tabSplitContainer.SplitterDistance = Convert.ToInt32(newTabTabTextBoxWidth);// 600; // Convert.ToInt16(median);
                                tabSplitContainer.SplitterDistance = (tabSplitContainer.Width - 2 * System.Windows.Forms.SystemInformation.VerticalScrollBarWidth) / 2;

                                if (1 == 0) MessageBox.Show("MedianCalc.Median(lineWidths): " + MedianCalc.Median(lineWidths) + "   2) " + median2 +
                                      "\n  MedianCalc.NthOrderStatistic(lineWidths,0) max-1: " + maxminus1 + "   2) " + max2minus1 +
                                      "\n  MedianCalc.NthOrderStatistic(lineWidths,0) min: " + MedianCalc.NthOrderStatistic(lineWidths, 0) +// "   2) " + min2 +
                                      "\n  MedianCalc.NthOrderStatistic(lineWidths,0) max: " + MedianCalc.NthOrderStatistic(lineWidths, lineWidths.Count - 1) + "   2) " + max2 +
                                      "\n  stdDev: 1) " + stdDev + "   2) " + stdDev2 +
                                      "\n  average: 1) " + average + "   2) " + average2 +
                                      "\n tabTextBox1 width: " + tabTextBox1.Width +
                                      "\n screenWidth: " + screenWidth +
                                      "\n tabSplitContainer.SplitterDistance: " + tabSplitContainer.SplitterDistance +
                                      "\n tabSplitContainer.Width width: " + tabSplitContainer.Width +
                                      "\n tabSplitContainer2.Width width: " + tabSplitContainer2.Width +

                                      "\n lineWidths.Count: " + lineWidths.Count
                                      );
                                return;
                            }

                        }

                    }


                    if (linesInCurrentTab < linesPerScreen)
                    {
                        ShowOneTabPanel();
                    }
                    else if (linesInCurrentTab < (linesPerScreen * 2))
                    {
                        ShowTwoTabPanels();

                    }
                    else
                    {
                        ShowThreeTabPanels();
                        tabSplitContainer.SplitterDistance = (tabSplitContainer.Width) / 3;// + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
                        tabSplitContainer2.SplitterDistance = (tabSplitContainer2.Width) / 2 + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
                    }
                    // int cix = tabTextBox1.GetFirstCharIndexFromLine(lineNum);
                    // int cix2 = tabTextBox1.GetFirstCharIndexFromLine(lineNum + linesPerScreen);
                    break;
                case "1 split":
                    ShowOneTabPanel();
                    break;
                case "2 splits":
                    ShowTwoTabPanels();
                    break;
                case "3 splits":
                    ShowThreeTabPanels();
                    //          tabSplitContainer.SplitterDistance = (tabSplitContainer.Width - 3 * System.Windows.Forms.SystemInformation.VerticalScrollBarWidth) / 3;
                    tabSplitContainer.SplitterDistance = (tabSplitContainer.Width) / 3;// + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
                    tabSplitContainer2.SplitterDistance = (tabSplitContainer2.Width) / 2 + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;

                    break;
            }
        }

        private void updateFontInTabbox()
        {
            this.tabTextBox1.Font = new System.Drawing.Font(fontName,
                        fontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabTextBox2.Font = new System.Drawing.Font(fontName,
                        fontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabTextBox3.Font = new System.Drawing.Font(fontName,
                        fontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void fontLargerButton_Click(object sender, EventArgs e)
        {
            fontSize += 0.75F;
            updateFontInTabbox();
            if (currentFilename != "") updateFontsizeJson(currentFilename, fontSize);
            //     Properties.Settings.Default["FontSize"] = fontSize;
            //     Properties.Settings.Default.Save();
        }

        private void fontSmallerButton_Click(object sender, EventArgs e)
        {
            fontSize -= 0.75F;
            updateFontInTabbox();
            if (currentFilename != "") updateFontsizeJson(currentFilename, fontSize);
            //      Properties.Settings.Default["FontSize"] = fontSize;
            //      Properties.Settings.Default.Save();
        }

        private void moveToTopButton_Click(object sender, EventArgs e)
        {
            if (historyListBox.SelectedItem == null) return;
            //MessageBox.Show("Item: " + historyListBox.SelectedItem.ToString());
            updateAccessedDateJson(historyListBox.SelectedItem.ToString(), DateTime.Now);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {

        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Copy window location to app settings
            Properties.Settings.Default.WindowLocation = this.Location;

            // Copy window size to app settings
            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.WindowSize = this.Size;
            }
            else
            {
                Properties.Settings.Default.WindowSize = this.RestoreBounds.Size;
            }

            // Save settings
            Properties.Settings.Default.Save();
        }



        private void tabTextBox1_MouseDown(object sender, MouseEventArgs e)
        {

            return;             // debug below here

            double pixelsPerLine = tabTextBox1.Height * 1.0 / linesPerScreen;
            int lineToScroll1 = Convert.ToInt32((-1.0 * tabTextBox1.GetPositionFromCharIndex(0).Y / pixelsPerLine));// * linesPerScreen);// / tabTextBox1.Height;

            double dlinesToScroll = (-1.0 * tabTextBox1.GetPositionFromCharIndex(0).Y / (double)tabTextBox1.Font.Height);// * linesPerScreen);// / tabTextBox1.Height;
            int lineToScroll2 = Convert.ToInt32((-1.0 * tabTextBox1.GetPositionFromCharIndex(0).Y / (double)tabTextBox1.Font.Height));// * linesPerScreen);// / tabTextBox1.Height;
            int lineToScroll = Convert.ToInt32((-1.0 * tabTextBox1.GetPositionFromCharIndex(0).Y / (double)(tabTextBox1.Font.Height - 1)));// * linesPerScreen);// / tabTextBox1.Height;

            //      int lineToScroll = Convert.ToInt32(Math.Floor((-1.0 *( tabTextBox1.GetPositionFromCharIndex(0).Y+tabTextBox1.Font.Height) / (double)tabTextBox1.Font.Height)));// * linesPerScreen);// / tabTextBox1.Height;

            int scrollCharIndex = tabTextBox1.GetFirstCharIndexFromLine(lineToScroll);

            tabTextBox2.Text = "\n\n\n\n\n" + "selectionstart: " + tabTextBox1.SelectionStart.ToString() + "\n"
                   + ",\n tabTextBox1 height=" + tabTextBox1.Height + "\n"
                   //     + ",\n font height =" + tabTextBox1.Font. + "\n"

                   + ",\n GetPositionFromCharIndex(0).Y: " + tabTextBox1.GetPositionFromCharIndex(0).Y.ToString() + "\n"
                   + ",\n font height =" + tabTextBox1.Font.Height.ToString() + "\n"
                   + ",\n yposfromindex/pixelsperline =" + lineToScroll1 + "\n"
                   + ",\n yposfromindex/fontheight =" + lineToScroll + "\n"
                   + ",\n lines per screen =" + linesPerScreen + "\n"
                   + ",\n NumberOfVisibleLines = " + NumberOfVisibleLines + "\n"
                   + ",\n dlinesToScroll = " + dlinesToScroll + "\n"
                  + ",\n scrollCharIndex =" + scrollCharIndex.ToString()
                 ;

        }
        private void tabTextBox1_DoubleClick(object sender, EventArgs e)
        {
            //   tabTextBox1.ScrollToCaret();
            scrollToLine(tabTextBox1.GetLineFromCharIndex(tabTextBox1.SelectionStart));
        }

        private void calculateSize()
        {
            Size size = TextRenderer.MeasureText(tabTextBox1.Text, tabTextBox1.Font);
            //    textBox1.Width = size.Width;
            //    textBox1.Height = size.Height;
        }

        private void scrollToLine(int lineNum)
        {
            //      MessageBox.Show("lineNum=" + lineNum);
            //  int linesPerScreen = Convert.ToInt32(((double)tabTextBox1.Height / (double)(tabTextBox1.Font.Height + 1)));

            int cix = tabTextBox1.GetFirstCharIndexFromLine(lineNum);
            int cix2 = tabTextBox1.GetFirstCharIndexFromLine(lineNum - 3 + Convert.ToInt16(linesPerScreen));
            if (cix2 == -1)
            {
                cix2 = 0;
                //  use one view instead of 2...
                ShowOneTabPanel();
                //       MessageBox.Show("ok");
            }
            int cix3 = tabTextBox1.GetFirstCharIndexFromLine((lineNum - 3 + Convert.ToInt16(linesPerScreen)) * 2);
            if (cix3 == -1)
            {
                cix3 = 0;
                //  use two panels instead of 3...
                ShowTwoTabPanels();
            }
            //      MessageBox.Show("cix=" + cix);


            if (VISIBLE_TAB_PANELS == 1)
            {
                tabTextBox1.SelectionStart = 0;
                tabTextBox1.ScrollToCaret();
                tabTextBox1.SelectionStart = cix;
                tabTextBox1.ScrollToCaret();
            }
            else if (VISIBLE_TAB_PANELS == 2)
            {
                tabTextBox1.SelectionStart = 0;
                tabTextBox1.ScrollToCaret();
                tabTextBox1.SelectionStart = cix;
                tabTextBox1.ScrollToCaret();

                tabTextBox2.SelectionStart = 0;
                tabTextBox2.ScrollToCaret();
                tabTextBox2.SelectionStart = cix2;
                tabTextBox2.ScrollToCaret();
            }
            else if (VISIBLE_TAB_PANELS == 3)
            {
                tabTextBox1.SelectionStart = 0;
                tabTextBox1.ScrollToCaret();
                tabTextBox1.SelectionStart = cix;
                tabTextBox1.ScrollToCaret();

                tabTextBox2.SelectionStart = 0;
                tabTextBox2.ScrollToCaret();
                tabTextBox2.SelectionStart = cix2;
                tabTextBox2.ScrollToCaret();

                tabTextBox3.SelectionStart = 0;
                tabTextBox3.ScrollToCaret();
                tabTextBox3.SelectionStart = cix3;
                tabTextBox3.ScrollToCaret();
            }

        }

        public int NumberOfVisibleLines
        {
            get
            {
                int topIndex = tabTextBox1.GetCharIndexFromPosition(new System.Drawing.Point(1, 1));
                int bottomIndex = tabTextBox1.GetCharIndexFromPosition(new System.Drawing.Point(1, tabTextBox1.Height - 1));
                int topLine = tabTextBox1.GetLineFromCharIndex(topIndex);
                int bottomLine = tabTextBox1.GetLineFromCharIndex(bottomIndex);
                int n = bottomLine - topLine + 1;
                return n;
            }

            /*
             *  int startLine = tabTextBox1.GetLineFromCharIndex(ix);
                int numVisibleLines = NumberOfVisibleLines;

                // only scroll if the line to scroll-to, is larger than the 
                // the number of lines that can be displayed at once.
                if (startLine > numVisibleLines)
                {
                    int cix = tabTextBox1.GetFirstCharIndexFromLine(startLine - numVisibleLines/3 +1);
                    tabTextBox1.Select(cix, cix+1);
                    tabTextBox1.ScrollToCaret();
                }
                */
        }

        private void ShowOneTabPanel()
        {
            // show 1 panel for tab view
            this.tabSplitContainer.Panel2Collapsed = true;
            this.tabSplitContainer.Panel2.Hide();
            this.tabSplitContainer2.Panel2Collapsed = true;
            this.tabSplitContainer2.Panel2.Hide();
            VISIBLE_TAB_PANELS = 1;
        }

        private void ShowTwoTabPanels()
        {
            // show 2 panels for tab view
            this.tabSplitContainer.Panel2Collapsed = false;
            this.tabSplitContainer.Panel2.Show();
            this.tabSplitContainer2.Panel2Collapsed = true;
            this.tabSplitContainer2.Panel2.Hide();
            VISIBLE_TAB_PANELS = 2;
            //  this.tabSplitContainer.SplitterDistance = 550;
            //   this.tabTextBox2.Text = tabTextBox1.Text;

        }

        private void ShowThreeTabPanels()
        {
            // show 3 panels for tab view
            this.tabSplitContainer.Panel2Collapsed = false;
            this.tabSplitContainer.Panel2.Show();
            this.tabSplitContainer2.Panel2Collapsed = false;
            this.tabSplitContainer2.Panel2.Show();
            //       this.tabTextBox3.Text = tabTextBox1.Text;
            VISIBLE_TAB_PANELS = 3;

        }


        const int EM_GETRECT = 0xB2;

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref RECT lParam);

        private void youtubeButton_Click(object sender, EventArgs e)
        {
            string searchquery = historyListBox.SelectedItem.ToString().Replace(' ', '+').Replace('-', '+');  // use + instead of spaces
            // open youtube browser and search for tabname
            System.Diagnostics.Process.Start("https://www.youtube.com/results?search_query=" + searchquery);

        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            //    foreach (string file in files) {
            foreach (string s in files)
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
                    this.historyListBox.SelectedIndex = 1;
                    //   MessageBox.Show("Processing " + s);
                    //    processNewFile(s);
                    openTab(s);



                }

            }
            //     this.historyListBox.SelectedIndex = 0;

            //    updateLeftPanel();
            // select most recent tab to start
            //  if (historyListBox.Items.Count > 0) this.historyListBox.SelectedIndex = 0;
            ///   }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            searchBoxAction();


        }

        private void searchBox_MouseClick(object sender, MouseEventArgs e)
        {
            searchBoxAction();
        }

        private void searchBoxAction()
        {
            if (searchBox.Text.Length < 3) return;
            historyListBox.SelectionMode = SelectionMode.MultiSimple;
            searchHistoryItems.Clear();

            searchInProgress = true;
            for (int i = 0; i < historyListBox.Items.Count; i++)
            {
                if (historyListBox.Items[i].ToString().Contains(searchBox.Text))
                {
                    historyListBox.SetSelected(i, true);
                    searchHistoryItems.Add(historyListBox.Items[i].ToString());
                }
                else
                {
                    historyListBox.SetSelected(i, false);
                }
            }

        }
    }

}
