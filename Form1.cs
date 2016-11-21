using Newtonsoft.Json;
using System;
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

public partial class Form1 : Form
    {
        String jsonFile = "";
        String jsonText = "";
        JsonTextReader jsonReader;
        JsonSerializer jsonSerializer = new JsonSerializer();
        List<HistoryEntry> history = new List<HistoryEntry>();
        Single fontSize = 8.25F;
        String fontName = "";

        public Form1()
        {
            InitializeComponent();
            // tableLayoutPanel1.ColumnCount = 1; // default to 1 panel
            splitContainer1.Panel1Collapsed = true;
            splitContainer1.Panel1.Hide();
            historyListBox.Visible = false;
            fontName = Properties.Settings.Default.Font.Name.ToString();
            fontButton.Text = fontName;
            this.tabTextBox1.Font = new System.Drawing.Font(fontName,
                        fontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            jsonFile = Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.ApplicationData), "tabscreenfit.json");
            // read file into a string and deserialize JSON to a type
            try
            {
                jsonText = File.ReadAllText(@jsonFile);
                jsonReader = new JsonTextReader(new StringReader(jsonText));
                jsonReader.SupportMultipleContent = true;
                while (true)
                {
                     if (!jsonReader.Read())
                     {
                        break;
                    }
                    //JsonSerializer jsonSerializer = new JsonSerializer();
                    HistoryEntry history1 = jsonSerializer.Deserialize<HistoryEntry>(jsonReader);
                    history.Add(history1);
                }
                //MessageBox.Show("Using history json file: " + jsonFile);
                foreach (HistoryEntry h in history)
                {
                    //MessageBox.Show("file: " + h.fileName);
                    //       historyListBox.Text += Path.GetFileNameWithoutExtension(h.toString()) + "\n";
                    historyListBox.Items.Add(h.ToString());

                }
            }
            catch (FileNotFoundException)
            {
                // create file on first run
                try
                {
                    File.WriteAllText(@jsonFile, "");
                    MessageBox.Show("Created history json file: " + jsonFile);
                    //string path = Server.MapPath("~/App_Data/");
                    // Write that JSON to txt file,  
                    //System.IO.File.WriteAllText(path + "output.json", jsondata);

                }
                catch(Exception ex)
                {
                    MessageBox.Show("History not enabled." +  jsonFile + " File errror:  " + ex.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("History not enabled." + jsonFile + " File errror: " + ex.ToString());
            }
                // deserialize JSON directly from a file
            //    using(StreamReader file = File.OpenText(@"c:\movie.json"))
            //   {
            //        JsonSerializer serializer = new JsonSerializer();
            //        HistoryEntry history2 = (Movie)serializer.Deserialize(file, typeof(HistoryEntry));
            //    }


        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            // No code needed here for this sample.
        }

        private void showButton_Click(object sender, EventArgs e)
        {
            // Show the Open File dialog. If the user chooses OK, load the 
            // tab that the user chose.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string text = System.IO.File.ReadAllText(@openFileDialog1.FileName);
                    tabTextBox1.Text = text;
                    // Create object
                    HistoryEntry entry = new HistoryEntry
                    {
                        fileName = openFileDialog1.FileName,
                        CreatedDate = DateTime.Now,
                        AccessedDate = DateTime.Now,
                        num = 0
                    };
                    // serialize JSON to a string and then write string to a file
                    File.AppendAllText(@jsonFile, JsonConvert.SerializeObject(entry));
                }
                catch (Exception ex)
                {
                    tabTextBox1.Text = "Could not load File: " + openFileDialog1.FileName + " \n " + ex.ToString();
                }
            }

            // METHOD #2
            // serialize JSON directly to a file
            //      using (StreamWriter str = new StreamWriter(@jsonFile))
            //      {
            //         JsonSerializer serializer = new JsonSerializer();
            //          serializer.Serialize("abcd");

            //   serializer.Serialize(file, movie);
            //str.Write(jsonFile);
            //      }

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

        private void historyListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (historyListBox.SelectedItem == null) return;
            // find selection in history List
            try
            {
                var h = history.Find(file => Path.GetFileNameWithoutExtension(file.fileName) == historyListBox.SelectedItem.ToString());
                string text = System.IO.File.ReadAllText(h.fileName);
                tabTextBox1.Text = text;
            }
            catch (Exception ex)
            {
                tabTextBox1.Text = "Could not load File: " + openFileDialog1.FileName + " \n " + ex.ToString();
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Properties.Settings.Default["Panel1Width"] = this.splitContainer1.SplitterDistance;
            Properties.Settings.Default.Save();
        }
    }

}
