using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using PS3Lib;

namespace KingdomHearts
{
    // If you want set instantly the CCAPI so make this :
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public static PS3API PS3 = new PS3API(SelectAPI.ControlConsole);
        private Thread TargetInfo;
        private bool threadIsRunning = false;
/*        private bool currentGame = false;
        private uint[] procs; */

        public Form1()
        {
            TargetInfo = new Thread(new ThreadStart(InfoWorker));
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Ignore this, too lazy to remove it.
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            threadIsRunning = false;
            TargetInfo.Abort();
            PS3.DisconnectTarget();
        }

        private void InfoWorker()
        {
            while (threadIsRunning)
            {
                Thread.Sleep(500);
            }
            TargetInfo.Abort();
        }


        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e) // Sora
        {

        }

        private void metroButton5_Click(object sender, EventArgs e) // connnect and attach
        {
            {
                if (PS3.ConnectTarget())
                {
                    if (!TargetInfo.IsAlive)
                    {
                        threadIsRunning = true;
                        TargetInfo.Start();
                    }
                    if (PS3.AttachProcess())
                    {
                        PS3.CCAPI.RingBuzzer(CCAPI.BuzzerMode.Double);
                        PS3.CCAPI.Notify(CCAPI.NotifyIcon.INFO, "Kingdom Hearts 2.5 RTM Tool Connected!");
                        MetroFramework.MetroMessageBox.Show(this, "Connected and Attached!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Could not attach to process", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Could not connect to PS3", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            } 
            /*            string IPAddrStr = metroTextBox1.Text; -----------Ye Old Connect------------
                        if(PS3.ConnectTarget(IPAddrStr))
                        {
                        //Connected, so we'll attach the process
                          if(PS3.AttachProcess())
                             {
                                MetroFramework.MetroMessageBox.Show(this, "Success!", "Connected and Attached!",MessageBoxButtons.OK, MessageBoxIcon.Information);
                             }
                                else 
                                 {
                                    MetroFramework.MetroMessageBox.Show(this, "Error!", "Could not attach to process",MessageBoxButtons.OK, MessageBoxIcon.Information);
                                 }
                              }
                                   else
                                    {
                                       MetroFramework.MetroMessageBox.Show(this, "Error!", "Could not connect to PS3",MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                        } */
        }

        private void metroCheckBox1_CheckedChanged_1(object sender, EventArgs e) // Call Timer for No MP Charges
        {

            if (metroCheckBox1.Checked)
            {
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }

        }

        private void metroCheckBox2_CheckedChanged(object sender, EventArgs e) // Call Timer for Infinite Munny
        {

            if (metroCheckBox2.Checked)
            {
                timer2.Start();
            }
            else
            {
                timer2.Stop();
            }

        }

        private void timer1_Tick(object sender, EventArgs e) // No MP Charges Loop Timer
        {
            byte[] buffer = new byte[] { 0x00, 0x00, 0x00, 0x00 }; // Value(Bytes)
            PS3.SetMemory(0x0280511C, buffer); // Address
        }

        private void timer2_Tick(object sender, EventArgs e) // Infinite Munny Loop Timer
        {
            byte[] buffer = new byte[] { 0x00, 0xf1, 0x86, 0x49 }; // Value(Bytes)
            PS3.SetMemory(0x00774410, buffer); // Address
        }


    }
}
