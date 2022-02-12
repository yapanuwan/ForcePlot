//------------------------------------------------------------------------------------------------
//Copyright © 2012 Nuwan Yapa
//
//This file is part of ForcePlot.
//
//ForcePlot is free software: you can redistribute it and/or modify it under the terms of the
//GNU General Public License as published by the Free Software Foundation, either version 2 of
//the License, or (at your option) any later version.
//
//ForcePlot is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
//even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//General Public License for more details.
//
//You should have received a copy of the GNU General Public License along with ForcePlot. If not,
//see http://www.gnu.org/licenses/.
//------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ForcePlot
{
    
    public partial class frmDebug : Form
    {

        public frmDebug()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = MathConverter.FormatEquation(textBox1.Text);
            label2.Text = MathConverter.FromString(textBox1.Text).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label3.Text = MathConverter.FormatEquation(textBox2.Text);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                textBox1.Text = MathConverter.FormatEquation(textBox1.Text);
                label2.Text = MathConverter.FromString(textBox1.Text).ToString();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                label3.Text = MathConverter.FormatEquation(textBox2.Text);
            }
        }

    }
}
