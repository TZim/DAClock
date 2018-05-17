using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAClock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();

            timer2.Tick += new EventHandler(timer2_tick);
            timer2.Start();

            get_data();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            get_data();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            note_time_of_date_change();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.MyLoc = this.Location;
            }
            else
            {
                Properties.Settings.Default.MyLoc = this.RestoreBounds.Location;
            }
            Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = Properties.Settings.Default.MyLoc;
        }
    }
}
