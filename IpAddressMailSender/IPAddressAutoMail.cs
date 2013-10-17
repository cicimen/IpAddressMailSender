using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Timers;
using System.Configuration;

namespace IpAddressMailSender
{
    public partial class IPAddressAutoMail : ServiceBase
    {
        Timer timer = new Timer();
        string IPAddress = "";

        public IPAddressAutoMail()
        {
            InitializeComponent();
            this.ServiceName = "IPAddressAutoMail";
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }


        protected override void OnStart(string[] args)
        {
            this.timer = new Timer(int.Parse(ConfigurationManager.AppSettings["timerMiliSeconds"]));
            this.timer.AutoReset = true;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            this.timer.Start();
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string currentIP = IP.GetIP();
            if(currentIP == null)
            {
                SMTPMail.SendMailThroughGmail("whatismyip.com çalışmıyor.");
            }
            if(currentIP != this.IPAddress)
            {
                this.IPAddress=currentIP;
                SMTPMail.SendMailThroughGmail(this.IPAddress);
            }
        }

        protected override void OnStop()
        {
        }
    }
}
