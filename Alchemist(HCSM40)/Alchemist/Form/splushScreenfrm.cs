using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class splushScreenfrm : Form
    {
        public splushScreenfrm()
        {
            InitializeComponent();

            timer1.Tick += timer1_Tick;
        }

        private Timer timer1 = new Timer();

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void splushScreenfrm_Shown(object sender, EventArgs e)
        {
            timer1.Interval = 3000;
            timer1.Enabled = true;
        }
    }
}
