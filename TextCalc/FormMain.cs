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

        void button1_Click(object sender, EventArgs e)
        {
            TopMost = !TopMost;
        }

        void button2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox3.Clear();
        }

        void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        void textBoxs_KeyDown(object sender, KeyEventArgs e)
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

        void textBoxs_KeyPress(object sender, KeyPressEventArgs e)
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

        void textBoxs_TextChanged(object sender, EventArgs e)
        {
            calcFunc();
        }

        void calcFunc()
        {
            textBox4.Clear();
            formatTextBox(textBox2);
            formatTextBox(textBox3);
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
                    if (!String.IsNullOrEmpty(tb1[i]) && !String.IsNullOrEmpty(tb2[i]))
                    {
                        if (tb1[i].Contains(".") || tb2[i].Contains(".") || operation == 3)
                        {
                            double value1 = -1;
                            Double.TryParse(tb1[i], NumberStyles.Number, CultureInfo.InvariantCulture, out value1);
                            double value2 = -1;
                            Double.TryParse(tb2[i], NumberStyles.Number, CultureInfo.InvariantCulture, out value2);
                            string line = (operation == 0 ? (value1 + value2) : (operation == 1 ? (value1 - value2) : (operation == 2 ? (value1 * value2) : (value1 / value2)))).ToString().Replace(",", ".");
                            if (line.Contains(".") && line.Length > line.IndexOf(".") + 7)
                            {
                                line = line.Remove(line.IndexOf(".") + 7);
                            }
                            textBox4.Text = textBox4.Text + line + Environment.NewLine;
                        }
                        else
                        {
                            long value1 = -1;
                            Int64.TryParse(tb1[i], out value1);
                            long value2 = -1;
                            Int64.TryParse(tb2[i], out value2);
                            textBox4.Text = textBox4.Text + (operation == 0 ? (value1 + value2) : (operation == 1 ? (value1 - value2) : (value1 * value2))) + Environment.NewLine;
                        }
                    }
                }
            }
        }

        void formatTextBox(TextBox textbox)
        {
            if (textbox.Text.Length > 0)
            {
                textbox.TextChanged -= textBoxs_TextChanged;
                if (past && textbox.Focused)
                {
                    past = false;
                    textbox.Text = textbox.Text.Replace(",", ".").Replace("..", ".") + (textbox.Lines[textbox.Lines.Length - 1].Length > 0 ? Environment.NewLine : "");
                }
                else
                {
                    textbox.Text = textbox.Text.Replace(",", ".").Replace("..", ".");
                }
                textbox.TextChanged += textBoxs_TextChanged;
                textbox.SelectionStart = textbox.Text.Length;
                textbox.ScrollToCaret();
            }
        }
    }
}
