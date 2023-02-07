using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Somma
{
    public partial class Form1 : Form
    {
        int n1, n2, n3;

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            n1 = Int32.Parse(textBox1.Text);
            n2 = Int32.Parse(textBox2.Text);
            n3 = n1 + n2;
            textBox3.Text = n3.ToString();
        }
    }
}
