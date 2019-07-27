using CpPrint.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CpPrint
{
    public delegate void WriteLogDelegate(string msg);
    public delegate void RefreshMoneyDelegate(string msg);
    public delegate void SendVoiceDelegate(string msg);
    public delegate void SaveRecordDelegate(BetRecord record);
}


