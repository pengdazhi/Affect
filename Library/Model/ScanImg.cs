using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class ScanImg
    {
        public string billNo { get; set; }
        public string storePath { get; set; }
        public string serialNum { get; set; }
        public string isBarcode { get; set; }
        public int orderNum { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int scanNum { get; set; }
    }
}
