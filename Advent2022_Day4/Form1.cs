using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Advent_Day4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string[] lines = textBox1.Text.Replace("\r", "").Split('\n');

            int nb = 0;

            foreach(string line in lines)
            {
                string s1 = line.Split(',').First();
                string s2 = line.Split(',').Last();

                var l1 = Explose(s1);
                var l2 = Explose(s2);

                if(l1.All(o => l2.Contains(o)) || l2.All(o => l1.Contains(o)))
                {
                    nb++;
                }
            }

            MessageBox.Show("" + nb);
        }

        private List<int> Explose(string s)
        {
            var l = new List<int>();

            string first = s.Split('-').First();
            string last = s.Split('-').Last();

            int start = int.Parse(first);
            int end = int.Parse(last);

            for(int i = start; i <= end; i++)
            {
                l.Add(i);
            }

            return l;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] lines = textBox1.Text.Replace("\r", "").Split('\n');

            int nb = 0;

            foreach (string line in lines)
            {
                string s1 = line.Split(',').First();
                string s2 = line.Split(',').Last();

                var l1 = Explose(s1);
                var l2 = Explose(s2);

                if (l1.Any(o => l2.Contains(o)) || l2.Any(o => l1.Contains(o)))
                {
                    nb++;
                }
            }

            MessageBox.Show("" + nb);
        }
    }
}
