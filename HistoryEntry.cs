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
        public int num { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime AccessedDate { get; set; }
        override public String ToString()
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }
    }
}
