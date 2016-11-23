using System;
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
        public float fontSize { get; set; }
        public int yScroll { get; set; }
        public String view { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime AccessedDate { get; set; }
        override public String ToString()
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }
    }
}
