using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpPrint.ViewModel
{
    public class BetRecord
    {
        //public string CateName { get; set; }

        public string BetTime { get; set; }

        public string BetExpectNo { get; set; }

        public string BetContent { get; set; }

        //public string BetMoney { get; set; }
    }

    public class WaitBetList {

        public string BetExpectNo { get; set; }

        public string BetContent { get; set; }

        public string BetMoney { get; set; }

        public bool IsBeted { get; set; }
    }
}
