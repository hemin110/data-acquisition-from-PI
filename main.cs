using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PiUtinity
{
    public partial class main : Form
    {
        opeart opea;
        public main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            opea = new opeart();
            opea.Show();
            this.Hide();
        }
    }
}
