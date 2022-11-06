using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace TextCalc
{
    public partial class FormMain : Form
    {
        int operation = 0;
        bool past = false;

        public FormMain()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '+' && e.KeyChar != '-' && e.KeyChar != '*' && e.KeyChar != '/')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '+')
            {
                operation = 0;
            }
            else if (e.KeyChar == '-')
            {
                operation = 1;
            }
            else if (e.KeyChar == '*')
            {
                operation = 2;
            }
            else if (e.KeyChar == '/')
            {
                operation = 3;
            }
            calcFunc();
        }

        private void textBoxs_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender != null && e.Control)
            {
                if (e.KeyCode == Keys.A)
                {
                    ((TextBox)sender).SelectAll();
                }
                else if (e.KeyCode == Keys.V)
                {
                    past = true;
                }
            }
        }

        private void textBoxs_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                if (e.KeyChar == ',')
                {
                    e.KeyChar = '.';
                }
                if (e.KeyChar != '.')
                {
                    e.Handled = true;
                }
            }
        }

        private void textBoxs_TextChanged(object sender, EventArgs e)
        {
            calcFunc();
        }

        private void calcFunc()
        {
            textBox4.Clear();
            if (textBox2.Text.Length > 0)
            {
                textBox2.TextChanged -= textBoxs_TextChanged;
                if (past)
                {
                    past = false;
                    textBox2.Text = textBox2.Text.Replace(",", ".").Replace("..", ".") + (textBox2.Lines[textBox2.Lines.Length - 1].Length > 0 ? Environment.NewLine : "");
                }
                else
                {
                    textBox2.Text = textBox2.Text.Replace(",", ".").Replace("..", ".");
                }
                textBox2.TextChanged += textBoxs_TextChanged;
                textBox2.SelectionStart = textBox2.Text.Length;
                textBox2.ScrollToCaret();
            }
            if (textBox3.Text.Length > 0)
            {
                textBox3.TextChanged -= textBoxs_TextChanged;
                if (past)
                {
                    past = false;
                    textBox3.Text = textBox3.Text.Replace(",", ".").Replace("..", ".") + (textBox2.Lines[textBox2.Lines.Length - 1].Length > 0 ? Environment.NewLine : "");
                }
                else
                {
                    textBox3.Text = textBox3.Text.Replace(",", ".").Replace("..", ".");
                }
                textBox3.TextChanged += textBoxs_TextChanged;
                textBox3.SelectionStart = textBox3.Text.Length;
                textBox3.ScrollToCaret();
            }
            if (textBox2.Text.Length > 0 && textBox3.Text.Length > 0)
            {
                List<string> tb1 = new List<string>();
                tb1.AddRange(textBox2.Lines);
                List<string> tb2 = new List<string>();
                tb2.AddRange(textBox3.Lines);
                int count1 = tb1.Count;
                int count2 = tb2.Count;
                for (int i = 0; i < count1 && i < count2; i++)
                {
                    if (!string.IsNullOrEmpty(tb1[i]) && !string.IsNullOrEmpty(tb2[i]))
                    {
                        if (tb1[i].Contains(".") || tb2[i].Contains(".") || operation == 3)
                        {
                            double value1 = -1;
                            Double.TryParse(tb1[i], NumberStyles.Number, CultureInfo.InvariantCulture, out value1);
                            double value2 = -1;
                            Double.TryParse(tb2[i], NumberStyles.Number, CultureInfo.InvariantCulture, out value2);
                            textBox4.Text = textBox4.Text + (operation == 0 ? (value1 + value2) : (operation == 1 ? (value1 - value2) : (operation == 2 ? (value1 * value2) : (value1 / value2)))).ToString().Replace(",", ".") + Environment.NewLine;
                        }
                        else
                        {
                            int value1 = -1;
                            Int32.TryParse(tb1[i], out value1);
                            int value2 = -1;
                            Int32.TryParse(tb2[i], out value2);
                            textBox4.Text = textBox4.Text + (operation == 0 ? (value1 + value2) : (operation == 1 ? (value1 - value2) : (value1 * value2))).ToString() + Environment.NewLine;
                        }
                    }
                }
            }
        }
    }
}
