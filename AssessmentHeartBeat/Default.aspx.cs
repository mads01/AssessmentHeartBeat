using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AssessmentHeartBeat.Events;


namespace AssessmentHeartBeat
{
    public partial class Default : System.Web.UI.Page
    {
        private HeartBeatEntities context;
        public delegate void AbnormalHeartRate(object s, AbnormalHeartRateEventArgs e);
        public event AbnormalHeartRate abnormalHeartRate;

        public delegate void GenerateHeartRate(object s, EventArgs e);
        public event GenerateHeartRate generateHeartRates;
        bool normalFlag;



        protected void Page_Load(object sender, EventArgs e)
        {
            normalFlag = true;
             context = new HeartBeatEntities();
            abnormalHeartRate += new AbnormalHeartRate(AbnormalRateFound);
            generateHeartRates += new GenerateHeartRate(GenerateRandomRates);

        }

        protected void btnStartHeartBeat_Click(object sender, EventArgs e)
        {

            this.generateHeartRates(this, e);


        }


        void GenerateRandomRates(object s, EventArgs e)
        {
            Random heartRateRandom = new Random();
            while (normalFlag)
            {
               

                int heartRate = heartRateRandom.Next(20, 300);
               
                if (heartRate > 140 || heartRate < 80)
                {
                    normalFlag = false;
                    AbnormalHeartRateEventArgs args = new AbnormalHeartRateEventArgs
                    {
                        HeartRate = heartRate,
                        TimeStamp = DateTime.Now

                    };
                    this.abnormalHeartRate(this, args);

                }
            }
            

        }
        void AbnormalRateFound(object s, AbnormalHeartRateEventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Abnormal heart beat is recorded : '+ " + e.HeartRate + "  )", true);
            LogToDataBase(e.TimeStamp, e.HeartRate);

        }

        private void LogToDataBase(DateTime dateLogged,int heartRate)
        {
            var LogEntry = new HeartBeatLog
            {
                HeartBeat = heartRate,
                TimeStamp=dateLogged

            };
           
            context.HeartBeatLogs.Add(LogEntry);
            context.SaveChanges();
        }
    }
}