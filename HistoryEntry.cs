using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabScreenFit
{
    class HistoryEntry
    {
        public string fileName { get; set; }
        public string tabName { get; set; }
        public Single fontSize { get; set; }
        public string fontName { get; set; }
        public int yScroll { get; set; }
        public string view { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime AccessedDate { get; set; }

        override public String ToString()
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }
    }
    public class ComparerDateTime : IComparer
    {
        public int Compare(object x, object y)
        {
            HistoryEntry X = x as HistoryEntry;
            HistoryEntry Y = y as HistoryEntry;

            return X.AccessedDate.CompareTo(Y.AccessedDate);
        }
    }
}
