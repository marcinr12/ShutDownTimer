using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShutdownApp
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer = null;
        ulong counter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                shutDownAt();
            if (checkBox2.Checked == true)
                shutDownAfter();
            if (checkBox1.Checked == false && checkBox2.Checked == false)
                label2.Text = "Please select shut down option!";

        }




        private void button2_Click(object sender, EventArgs e)
        {
            string strCmdText = "shutdown -a";
            System.Diagnostics.Process.Start("CMD.exe", "/C" + strCmdText);
            if(timer == null || !timer.Enabled)
            {
                label2.Text = "Operation is not executing!";
                return;
            }
            timer.Stop();
            label1.Text = "";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            --counter;
            if (counter < 0)
                timer.Stop();
            else if (counter > 0)
                label1.Text = timeSpanConvertion(counter);
            
        }

        private string timeSpanConvertion(ulong secs)
        {
            TimeSpan t = TimeSpan.FromSeconds(secs);
            string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
            return answer;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
                label2.Text = "";
            }
                
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
                label2.Text = "";
            }
                
        }

        public void shutDownAt()
        {
            string strCmdText = "shutdown -s -t ";

            DateTime sdTime = dateTimePicker1.Value;
            DateTime currentTime = DateTime.Now;

            int compare = DateTime.Compare(sdTime, currentTime);
            if (compare < 0)
            {
                //MessageBox.Show("Incorrect time");
                label2.Text = "Incorrect time!";
                return;
            }


            ulong dSeconds = (ulong)(sdTime - currentTime).TotalSeconds;
            label1.Text = counter.ToString();
            counter = (ulong)dSeconds;
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(timer1_Tick);
            timer.Interval = 1000;      // 1 second
            timer.Start();

            // /K -> CMD opened
            // /C -> CMD closed
            System.Diagnostics.Process.Start("CMD.exe", "/C" + strCmdText + dSeconds.ToString());
        }

        public void shutDownAfter()
        {
            uint dHours = (uint)numericUpDown1.Value;
            uint dMinutes = (uint)numericUpDown2.Value;
            ulong dSeconds = (ulong)numericUpDown3.Value;

            dSeconds = dMinutes * 60 + dHours * 60 * 60;

            if (dSeconds == 0)
                return;

            counter = (ulong)dSeconds;
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(timer1_Tick);
            timer.Interval = 1000;      // 1 second
            timer.Start();

            string strCmdText = "shutdown -s -t ";

            System.Diagnostics.Process.Start("CMD.exe", "/C" + strCmdText + dSeconds);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            checkBox2.Checked = false;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = true;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = true;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            checkBox2.Checked = true;
        }
    }
}
