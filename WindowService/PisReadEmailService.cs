#region License
// /****************************** Module Header ******************************\
// Module Name:  PisReadEmailService.cs
// Project:    Pis-WindowService
// Copyright (c) Microsoft Corporation.
// 
// <Description of the file>
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// \***************************************************************************/
#endregion
using System;
using System.ServiceProcess;
using System.Timers;

namespace WindowService
{
    public partial class PisReadEmailService : ServiceBase
    {
        private int numticks = 0;
        private bool flag;
        private Timer _timer;
        public PisReadEmailService()
        {
            InitializeComponent();
            
        }

        protected override void OnStart(string[] args)
        {

            _timer = new Timer();
            _timer.Interval = 60 * 60 * 1000; //minute*second*milisecond
            _timer.Elapsed += timer1_Tick;
            _timer.Enabled = true;
            flag = true;

            Library.Library.CreateLog("OnStart");
        }

        protected override void OnStop()
        {
            flag = false;
            Library.Library.CreateLog("Read Email Service Done");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Library.Library.CreateLog("Timer_Tick, Flag " + flag);

            if (flag)
            {
                Library.Library.CreateLog("Timer Stop");
                _timer.Stop();
                try
                {
                    Library.Library.ReadEmailExchange();

                }
                catch (Exception ex)
                {
                   Library.Library.CreateLog(ex.Message);
                    flag = true;
                }
                _timer.Start();
                Library.Library.CreateLog("Timer Start");

            }
        }
    }
}