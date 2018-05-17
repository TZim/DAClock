using System;
using System.Net;
using System.Drawing;
using System.Windows.Forms;

namespace DAClock
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.label2 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(129, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 31);
            this.label1.TabIndex = 1;
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(190, -1);
            this.monthCalendar1.MaxSelectionCount = 1;
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 2;
            this.monthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(64, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 31);
            this.label2.TabIndex = 3;
            this.label2.Text = "--° C";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // timer2
            // 
            this.timer2.Interval = 600000;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(2, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(176, 133);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 170);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Analog Clock";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            //label1.Text = now.ToLongTimeString();
            draw_time(now);
        }
         
        private void timer2_tick(object sender, EventArgs e)
        {
            get_data();
        }

        private void get_data()
        {
            string OWMID = "&appid=bf84ccfefb4efc97324fdb60fdd42038";
            string wxurl = "http://api.openweathermap.org/data/2.5/weather?lat=" + 42.21 + "&lon=" + -71.06 + OWMID;
            //"http://api.openweathermap.org/data/2.5/weather?zip=02139,us" + OWMID;
            string data = null;

            try
            {
                data = (new WebClient()).DownloadString(wxurl);
            }
            catch (WebException ignore) { }

            int index = -1;
            if (data != null)
                index = data.IndexOf("\"temp\"", 0, data.Length, StringComparison.Ordinal);
            string newtext = "--.- °C";
            if (index >= 0)
            {
                string str1 = data.Substring(index + 7, 6);
                str1 = getNextNumberStr(str1);
                bool parsed = false;
                float temp = 0;
                if (str1 != null)
                {
                    try
                    {
                        temp = float.Parse(str1);
                        temp -= (float)273.15; // Kelvin to Celsius
                        parsed = true;
                    }
                    catch (FormatException) { }
                    catch (OverflowException) { }    
                }
                if (parsed)    
                    newtext = String.Format("{0:0.0} °C", temp);
            }
            label2.Text = newtext;
        }

        private string getNextNumberStr(string str)
        {
            string numeric = "";
            foreach (char ch in str)
            {
                if ((ch >= '0' && ch <= '9') || ch == '.')
                    numeric = numeric + ch;
            }
            return numeric;
        }

        private DateTime last_time = new DateTime(0);
        private const int ticks_per_sec = 10000000;

        private const int draw_scale = 4;
        private const int border_padx = 3 * draw_scale;
        private const int border_pady = 3 * draw_scale;
        private const int blockinc = 2 * draw_scale;
        private const int blockx = 3 * blockinc;
        private const int blockdivs = 5;
        private const int blocky = blockdivs * blockinc;
        private const int hrblockdivs = 3;
        private const int hrblocky = hrblockdivs * blockinc;
        private const int dotoffx = 2 * draw_scale;
        //private const int dotoffy = 4 * draw_scale;
        private const int dotx = 2 * draw_scale;
        private const int doty = dotx;
        private const int block_gap = 1 * draw_scale;
        private const int block3_gap = 4 * draw_scale;
        private const int hr_min_gap = 3 * draw_scale;
        private const int min_sec_gap = 2 * draw_scale;


        private void draw_time(DateTime now)
        {
            long delta = (now.Ticks - last_time.Ticks) / ticks_per_sec;
            //if (delta < 1)
            //    return;
            last_time = now;
            graph = pictureBox1.CreateGraphics();
            //if (delta == 1)
            //    draw_time_inc(now, last_time);
            //else
                draw_time_full(now);
            graph.Dispose();

            delta = (now.Ticks - time_of_last_date_change) / ticks_per_sec;
            if (delta > 60 * 15) // Revert calendar highlighted date after 15 minutes
                monthCalendar1.SelectionStart = monthCalendar1.SelectionEnd = now;
        }

        //private void draw_time_inc(DateTime now, DateTime last)
        //{

        //}
        
        private Brush blackBrush = new SolidBrush(Color.Black);
        private Brush grayBrush = new SolidBrush(Color.DarkGray);
        private Brush redBrush = new SolidBrush(Color.Red);
        private Brush blueBrush = new SolidBrush(Color.Blue);
        private Brush whiteBrush = new SolidBrush(Color.White);

        private Pen hourPen = new Pen(Color.Black, 5);
        private Pen minPen = new Pen(Color.Black, 3);
        private Pen secPen = new Pen(Color.Red, 3);
        private Pen tickPen = new Pen(Color.Black, 1);

        private void draw_time_full(DateTime now)
        {
            pictureBox1.BackColor = Color.White;

            int sec_tick = now.Second;
            int min_tick = now.Minute;
            int hr_tick = now.Hour;
            int full_hr_tick = hr_tick;

            if (hr_tick >= 12)
                hr_tick -= 12;

            int center_x = 90;
            int center_y = 60;

            hourPen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
            minPen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
            secPen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
            tickPen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;

            graph.Clear(Color.White);

            double angle = (Math.PI * (hr_tick - 3 + min_tick / 60.0 + sec_tick / 360.0) / 6.0);
            graph.DrawLine(hourPen, center_x, center_y, center_x + (float)(35.0 * Math.Cos(angle)), center_y + (float)(35.0 * Math.Sin(angle)));

            angle = (Math.PI * (min_tick - 15 + sec_tick / 60.0) / 30.0);
            graph.DrawLine(minPen, center_x, center_y, center_x + (float)(50.0 * Math.Cos(angle)), center_y + (float)(50.0 * Math.Sin(angle)));

            angle = (Math.PI * (sec_tick - 15) / 30.0);
            graph.DrawLine(secPen,
                center_x + (float)(35.0 * Math.Cos(angle)), center_y + (float)(35.0 * Math.Sin(angle)),
                center_x + (float)(50.0 * Math.Cos(angle)), center_y + (float)(50.0 * Math.Sin(angle)));

            for (int i = 0; i < 12; i++)
            {
                angle = (Math.PI * (i - 3) / 6.0);
                graph.DrawLine(tickPen,
                    center_x + (float)(55.0 * Math.Cos(angle)), center_y + (float)(55.0 * Math.Sin(angle)),
                    center_x + (float)(60.0 * Math.Cos(angle)), center_y + (float)(60.0 * Math.Sin(angle)));
            }

            return;


            hr_tick = hr_tick * hrblockdivs + min_tick / (60 / hrblockdivs);
            int auxtick = min_tick % (60 / hrblockdivs);
            auxtick = auxtick * blockinc / (60 / hrblockdivs);

            draw_time_row(border_pady, hrblockdivs, hr_tick, auxtick, full_hr_tick >= 12 ? blackBrush : grayBrush, whiteBrush);
            draw_time_row(border_pady + hrblocky + hr_min_gap, blockdivs, min_tick, 0, blackBrush, whiteBrush);
            draw_time_row(border_pady + hrblocky + blocky + hr_min_gap + min_sec_gap, blockdivs, sec_tick, 0, redBrush, whiteBrush);
        }

        private void draw_time_row(int offsety, int divs, int tick, int auxtick, Brush dark, Brush light)
        {
            int offsetx = border_padx;
            for (int blockn = 0; blockn < 6; blockn++, offsetx += blockx + block_gap)
            {
                draw_time_block(offsetx, offsety, divs, blockn, tick, auxtick, dark, light);
                if (blockn == 2)
                    offsetx += block3_gap - block_gap;
            }
        }

        Graphics graph;

        private void draw_time_block(int offsetx, int offsety, int divs, int blockn, int tick, int auxtick, Brush dark, Brush light)
        {
            bool ascending = tick < 6 * divs;

            if (!ascending)
                tick -= 6 * divs;

            int targn = tick / divs;
            int subtick = tick % divs;
 
            bool blockdark = ascending == (blockn <= targn);
            bool dotdark = !blockdark;

            int height = divs * blockinc;
            int ht = height;
            int dotoffy = (divs - 1) * draw_scale;

            if (targn == blockn)
            {
                ht = blockinc * subtick + auxtick;
                if (subtick < (divs+1) / 2)
                    dotdark = !dotdark;
            }

            graph.FillRectangle(blockdark ? dark : light, offsetx, offsety + height - ht, blockx, ht);
            graph.FillRectangle(blockdark ? light : dark, offsetx, offsety, blockx, height - ht);
            int dht = 0;
            if (targn == blockn && ht >= dotoffy && ht < dotoffy + doty)
                dht += auxtick;
            graph.FillRectangle(dotdark ? light : dark, offsetx + dotoffx, offsety + dotoffy + doty - dht, dotx, dht);
            graph.FillRectangle(dotdark ? dark : light, offsetx + dotoffx, offsety + dotoffy, dotx, doty - dht);
        }
        

        long time_of_last_date_change = 0;

        private void note_time_of_date_change()
        {
            long now = DateTime.Now.Ticks;
            time_of_last_date_change = now;
        }

        private System.Windows.Forms.Timer timer1;
        private Label label1;
        private MonthCalendar monthCalendar1;
        private Label label2;
        private Timer timer2;
        private PictureBox pictureBox1;
    }
}


//{   "coord":{"lon":-71.11,"lat":42.37},
//    "weather":[{"id":501,"main":"Rain","description":"moderate rain","icon":"10d"},
//               {"id":211,"main":"Thunderstorm","description":"proximity thunderstorm","icon":"11d"}],
//    "base":"cmc stations",
//    "main":{"temp":298.52,"pressure":1019,"humidity":77,"temp_min":293.15,"temp_max":302.15},
//    "wind":{"speed":6.7,"deg":310},"rain":{"1h":1.27},"clouds":{"all":75},"dt":1439678446,
//    "sys":{"type":1,"id":1282,"message":0.0051,"country":"US","sunrise":1439632338,"sunset":1439682268},
//    "id":6254926,"name":"Massachusetts","cod":200}
