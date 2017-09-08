using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssessmentHeartBeat.Events
{
    public class AbnormalHeartRateEventArgs:EventArgs
    {
        public int HeartRate { get; set; }
      
        public DateTime TimeStamp { get; set; }
    }
}