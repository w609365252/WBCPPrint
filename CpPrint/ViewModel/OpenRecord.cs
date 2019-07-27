using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpPrint.ViewModel
{
    public class OpenRecord
    {
        public string ExpectNo { get; set; }

        public string OpenNo { get; set; }

        public string OpenTime { get; set; }

        public bool IsBeted { get; set; }
    }

    public class WaitBetModel {



        public string ExpectNo { get; set; }

        /// <summary>
        /// 第1名 大
        /// </summary>
        public string BetTitle { get; set; }

        public decimal Amount { get; set; }

        /// <summary>
        /// 1/大
        /// </summary>
        public string BetContent { get; set; }

        public bool IsBeted { get; set; }
    
        public string PlanName { get; set; }
    }

}
