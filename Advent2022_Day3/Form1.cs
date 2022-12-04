using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Advent2022_Day3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            string[] lines = txtInput.Text.Replace("\r", "").Split('\n');

            int totalPriority = 0;

            foreach(string l in lines)
            {
                int middle = l.Length / 2;
                string part1 = l.Substring(0, middle);
                string part2 = l.Substring(middle, middle);

                foreach(char c in part1)
                {
                    if(part2.Contains(c))
                    {
                        totalPriority += GetPriority(c);
                        break;
                    }
                }
            }

            MessageBox.Show(""+totalPriority);
        }

        private int GetPriority(char c)
        {
            if(c >= 'a' && c <= 'z')
                return c - 'a' + 1;
            else
                return c - 'A' + 27;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] lines = txtInput.Text.Replace("\r", "").Split('\n');

            int totalPriority = 0;

            List<string> groupLines = new List<string>();

            int memberNum = 0;
            int totalCount = 0;
            int groupPriority = 0;

            foreach (string l in lines)
            {
                if(totalCount % 3 == 0)
                {
                    groupPriority = ProcessGroupPriority(groupLines);
                    totalPriority += groupPriority;

                    // new group
                    groupLines = new List<string>();
                    memberNum = 0;
                }

                groupLines.Add(l);

                totalCount++;
                memberNum++;
            }

            //last group
            groupPriority = ProcessGroupPriority(groupLines);
            totalPriority += groupPriority;

            MessageBox.Show("" + totalPriority);
        }

        private int ProcessGroupPriority(List<string> groupLines)
        {
            if (groupLines.Count != 3)
                return 0;

            string line1 = groupLines[0];
            string line2 = groupLines[1];
            string line3 = groupLines[2];

            foreach (char c in line1)
            {
                if(line2.Contains(c))
                {
                    if(line3.Contains(c))
                    {
                        return GetPriority(c);
                    }
                }
            }

            return 0;
        }
    }
}
