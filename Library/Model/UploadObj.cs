using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class UploadObj
    {
        public string batchId { get; set; }
        public List<ScanImg> items { get; set; }

    }
}
