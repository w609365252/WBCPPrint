using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpPrint.ViewModel
{
    public class OpenNext
    {
        public string CurrExpectNo { get; set; }

        public DateTime NextOpenTime { get; set; }

        public DateTime NextCloseTime { get; set; }

        public string NextExpectNo { get; set; }

        public DateTime CurrTime { get; set; }
    }
}
