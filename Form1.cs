using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EtwHidlogger;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
/*USB Hid Logger
 Author: MSDN.WhiteKnight (https://github.com/MSDN-WhiteKnight)
 Based on Ruxcon2016ETW KeyloggerPOC (https://github.com/CyberPoint/Ruxcon2016ETW/tree/master/KeyloggerPOC)*/

namespace HidLogger
{
    public partial class Form1 : Form, IPrint
    {
        StringBuilder sb;

        public void Print(string text)
        {
            if (sb == null) sb = new StringBuilder(50000);
            if (text == null) return;
            this.Invoke((MethodInvoker)(() =>
            {
                sb.AppendLine(text);
            }));
        }

        public Form1()
        {
            InitializeComponent();
            UsbEventSource.Instance = this;
        }

        private void bStartCapture_Click(object sender, EventArgs e)
        {
            string session = tbSessionName.Text;            

            try
            {
                UsbEventSource.StartCapture(session, tbFile.Text);
                MessageBox.Show("Capture sesion is started");   
            }
            catch (Exception ex)
            {
                tbEvents.Text = ex.ToString();
            }            
        }

        private void bStopCapture_Click(object sender, EventArgs e)
        {
            try
            {
                UsbEventSource.StopCapture();
                MessageBox.Show("Capture sesion is stopped");
            }
            catch (Exception ex)
            {
                tbEvents.Text = ex.ToString();
            }  
                
        }

        private void bViewEvents_Click(object sender, EventArgs e)
        {
            tbEvents.Text = "";
            sb = new StringBuilder(50000);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ETWTraceEventSource src = new ETWTraceEventSource(tbFile.Text);
                src.Dynamic.All += UsbEventSource.EventCallback;
                src.Process();

                string s;
                if 
                    (sb.Length <= 100000) s = sb.ToString();
                else 
                    s = sb.ToString().Substring(0, 99999) +
                        Environment.NewLine + "(some text trimmed)";

                tbEvents.Text = s;
                bSave.Enabled = true;
            }
            catch (Exception ex)
            {
                tbEvents.Text = ex.ToString();
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void bBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            
            using (sf)
            {
                sf.RestoreDirectory = true;
                sf.DefaultExt = "etl";
                sf.Filter = "Event Tracing log (*.etl)|*.etl";
                sf.CheckFileExists = false;
                sf.CheckPathExists = false;
                sf.OverwritePrompt = false;
                if (sf.ShowDialog(this) != DialogResult.Cancel)
                {
                    tbFile.Text = sf.FileName;
                }
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();

            using (sf)
            {
                sf.RestoreDirectory = true;
                sf.DefaultExt = "txt";
                sf.Filter = "Plain text (*.txt)|*.txt";
                if (sf.ShowDialog(this) != DialogResult.Cancel)
                {
                    System.IO.File.WriteAllText(sf.FileName, sb.ToString());
                }
            }
        }
    }
}
