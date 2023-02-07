using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calcolatrice
{
    public partial class Form1 : Form
    {
        int sign;
        Boolean sw = false;
        int num1;
        int num2;
        public Form1()
        {
            InitializeComponent();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 0;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 1;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 2;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 3;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 4;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 5;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 6;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 7;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 8;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += 9;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            sign = 0; //+
            num1 = int.Parse(richTextBox1.Text);
            sw = true;
            richTextBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sign = 1; //-
            num1 = int.Parse(richTextBox1.Text);
            sw = true;
            richTextBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sign = 2; //x
            num1 = int.Parse(richTextBox1.Text);
            sw = true;
            richTextBox1.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sign = 3; //:
            num1 = int.Parse(richTextBox1.Text);
            sw = true;
            richTextBox1.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // =
            if(sw == false)
            {
                num1 += num1;
                richTextBox1.Text = num1.ToString();
            }else if(sw == true)
            {
                num2 = int.Parse(richTextBox1.Text);
                if (sign == 0)
                {
                    num1 += num2;
                }
                if (sign == 1)
                {
                    num1 -= num2;
                }
                if (sign == 2)
                {
                    num1 *= num2;
                }
                if (sign == 3)
                {
                    try
                    {
                        num1 /= num2;
                    }
                    catch (DivideByZeroException)
                    {
                        richTextBox1.Text = "";
                        MessageBox.Show("Divisione per zero", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    
                }
                sw = false;
                richTextBox1.Text = num1.ToString();
            }
            
        }
    }
}
