using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabScreenFit
{
    class SyncTextBox : RichTextBox
    {
        public SyncTextBox()
        {
            this.Multiline = true;
            this.ScrollBars = RichTextBoxScrollBars.Both; //ScrollBars.Vertical;
        }

        public Control[] Buddies { get; set; }

        private static bool scrolling;   // In case buddy tries to scroll us
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            // Trap WM_VSCROLL message and pass to buddy
            if (Buddies != null)
            {
                foreach (Control ctr in Buddies)
                {
                    if (ctr != this)
                    {
                        //  if scrolling with mousewheel
                        if ((m.Msg == 0x20a) && !scrolling && ctr.IsHandleCreated)
                        {
                            scrolling = true;
                            SendMessage(ctr.Handle, m.Msg, m.WParam, m.LParam);
                            scrolling = false;
                        }
                        // click on scroll bar and scroll
                        else if  ((m.Msg == 0x115) && !scrolling && ctr.IsHandleCreated)
                        {
                            scrolling = true;
                  //          MessageBox.Show("m.WParam: " + m.WParam.ToString() + "\n m.LParam: " + m.LParam.ToString());
                  //          SendMessage(ctr.Handle, m.Msg, m.WParam, m.LParam);
                            scrolling = false;
                        }

                    }
                }
            }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
}
