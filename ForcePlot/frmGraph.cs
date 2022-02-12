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
using System.Windows.Forms;

namespace ForcePlot
{
    public partial class frmGraph : Form
    {
#if DEBUG
        DateTime startTime = DateTime.Now;
        frmDebug debugFrm;
#endif
        public string firstArg;

        const int controlsHeight = 100;
        Color graphCol = Color.Red;
        Color TgraphCol = Color.Red;
        Pen axesCol = new Pen(Brushes.Black, 3);

        bool RunWorker;

        double minY, maxY, stepY, minX, maxX, stepX;
        double TminY, TmaxY, TstepY, TminX, TmaxX, TstepX;
        double scale = 16;
        double Tscale = 16;
        Point offset;
        int mouseLastX, mouseLastY;
        bool mouseDown;

        Bitmap Tbit = new Bitmap(1, 1);
        Bitmap bit = new Bitmap(1, 1);
        Point bitOffset = new Point(0, 0);
        double bitScale = 1;

        string currEquation = "y = x ^ 2";
        term LHS, RHS;
        term tempLHS, tempRHS;

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
#if DEBUG
            startTime = DateTime.Now; 
#endif

            tempLHS = LHS; tempRHS = RHS;
            TminY = minY; TmaxY = maxY; TstepY = stepY; TminX = minX; TmaxX = maxX; TstepX = stepX;
            Tscale = scale;
            TgraphCol = graphCol;
            Tbit = new Bitmap((int)((TmaxX - TminX) / TstepX) + 2, (int)((TmaxY - TminY) / TstepY) + 2);

            int Xo = (int)Math.Round(-minX * scale);
            int Yo = (int)Math.Round(maxY * scale);

            //horizontal sweep
            int numpoints = (int)((TmaxX - TminX) / TstepX) + 2;

            {
                double x = TminX;
                for (int i = 0; x < TmaxX; x += TstepX, i++)
                {
                    foreach (double y in solveY(x))
                    {
                        Tbit.SetPixel(Xo + (int)Math.Round(x * Tscale), Yo + (int)Math.Round(y * -Tscale), TgraphCol);
                    }

                    if (worker.CancellationPending) { e.Cancel = true; break; }
                    worker.ReportProgress((i * 100) / numpoints / 2);
                }
            }

            //vertical sweep
            numpoints = (int)((TmaxY - TminY) / TstepY) + 2;

            {
                double y = TminY;
                for (int i = 0; y < TmaxY; y += TstepY, i++)
                {
                    foreach (double x in solveX(y))
                    {
                        Tbit.SetPixel(Xo + (int)Math.Round(x * Tscale), Yo + (int)Math.Round(y * -Tscale), TgraphCol);
                    }

                    if (worker.CancellationPending) { e.Cancel = true; break; }
                    worker.ReportProgress(50 + (i * 100) / numpoints / 2);
                }
            }
        }

        System.Collections.Generic.List<double> solveY(double x)
        {
            System.Collections.Generic.List<double> answers = new List<double>();
            int noOfAnswersL = 1;
            int noOfAnswersR = 1;

            int[] prevSign = new int[noOfAnswersL * noOfAnswersR];

            for (double y = TmaxY; y > TminY; y -= TstepY)
            {
                double LHSvalue = tempLHS.getValue(x, y);
                double RHSvalue = tempRHS.getValue(x, y);

                for (int Lans = 0; Lans < noOfAnswersL; Lans++)
                {
                    for (int Rans = 0; Rans < noOfAnswersR; Rans++)
                    {
                        double d = LHSvalue - RHSvalue;
                        if (double.IsNaN(d))
                        {
                            prevSign[Lans * noOfAnswersR + Rans] = 0;
                        }
                        else
                        {
                            if (d == 0)
                            {
                                answers.Add(y);
                                prevSign[Lans * noOfAnswersR + Rans] = 0;
                            }
                            else
                            {
                                int sign = Math.Sign(d);
                                if (!(sign == prevSign[Lans * noOfAnswersR + Rans] || prevSign[Lans * noOfAnswersR + Rans] == 0))
                                {
                                    answers.Add(y);
                                }
                                prevSign[Lans * noOfAnswersR + Rans] = sign;
                            }
                        }
                    }
                }
            }
            return answers;
        }

        System.Collections.Generic.List<double> solveX(double y)
        {
            System.Collections.Generic.List<double> answers = new List<double>();
            int noOfAnswersL = 1;
            int noOfAnswersR = 1;

            int[] prevSign = new int[noOfAnswersL * noOfAnswersR];

            for (double x = TmaxX; x > TminX; x -= TstepX)
            {
                double LHSvalue = tempLHS.getValue(x, y);
                double RHSvalue = tempRHS.getValue(x, y);

                for (int Lans = 0; Lans < noOfAnswersL; Lans++)
                {
                    for (int Rans = 0; Rans < noOfAnswersR; Rans++)
                    {
                        double d = LHSvalue - RHSvalue;
                        if (double.IsNaN(d))
                        {
                            prevSign[Lans * noOfAnswersR + Rans] = 0;
                        }
                        else
                        {
                            if (d == 0)
                            {
                                answers.Add(x);
                                prevSign[Lans * noOfAnswersR + Rans] = 0;
                            }
                            else
                            {
                                int sign = Math.Sign(d);
                                if (!(sign == prevSign[Lans * noOfAnswersR + Rans] || prevSign[Lans * noOfAnswersR + Rans] == 0))
                                {
                                    answers.Add(x);
                                }
                                prevSign[Lans * noOfAnswersR + Rans] = sign;
                            }
                        }
                    }
                }
            }
            return answers;
        }

        public frmGraph()
        {
            InitializeComponent();
        }

        void enterEquation()
        {
            if (!txtEquation.Text.Contains(('=')))
            { txtEquation.Text = "y = " + txtEquation.Text; }

            try
            {
                currEquation = MathConverter.FormatEquation(txtEquation.Text);
                txtEquation.Text = currEquation;
                LHS = MathConverter.FromString(txtEquation.Text.Split(new char[] { '=' }, 2)[0]);
                RHS = MathConverter.FromString(txtEquation.Text.Split(new char[] { '=' }, 2)[1]);
            }

            catch
            {
                LHS = MathConverter.FromString("1");
                RHS = MathConverter.FromString("0");

                System.Windows.Forms.MessageBox.Show(this, "Unable to interpret the equation. Please refer the online documentation for help.", "Equation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SetGraphRange();
            RestartWorker();

            txtEquation.SelectAll();
        }

        void SetGraphRange()
        {
            minX = -offset.X / scale;
            maxX = (this.Width - offset.X) / scale ;
            minY = (controlsHeight + offset.Y - this.Height - 3) / scale;
            maxY = (offset.Y + 3) / scale;

            stepX = 1 / scale; stepY = 1 / scale;
        }

        void RestartWorker()
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
                RunWorker = true;
            }
            else
            {
                pgBar.Visible = true;
                btnPlot.Visible = false;

                worker.RunWorkerAsync();
            }
        }

        private void frmGraph_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            offset = new Point((int)(this.Width / 2.1), (this.Height - controlsHeight) / 2);
            scale = 16; scale = 16;

            panelControls.Location = new Point(0, this.Height - controlsHeight);
            panelControls.Height = controlsHeight;

            if (firstArg == null)
            {
#if DEBUG
                txtEquation.Text = "y=x^(1/3)";
#else
                txtEquation.Text = "y=x^2";
#endif
                enterEquation();
            }
            else
            {
                OpenFile(firstArg);
            }

#if DEBUG
            debugFrm = new frmDebug();
            debugFrm.Show();
#endif
        }
        
        private void txtEquation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                e.SuppressKeyPress = true;

                enterEquation();

            }
        }

        private void frmGraph_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLastX = e.X - offset.X;
            mouseLastY = e.Y - offset.Y;
            mouseDown = true;
        }

        private void frmGraph_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;

            SetGraphRange();
            RestartWorker();
        }

        private void frmGraph_MouseWheel(object sender, MouseEventArgs e)
        {
            scale *= Math.Pow(2, (e.Delta / 120));
            if (scale > 8192) { scale = 8192; return; }
            if (scale < 0.0078125) { scale = 0.0078125; return; }
            this.Invalidate(false);
            SetGraphRange();
            RestartWorker();

        }

        private void frmGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                offset.X = e.X - mouseLastX;
                offset.Y = e.Y - mouseLastY;
                this.Invalidate(false);
            }
        }

        private void frmGraph_Resize(object sender, EventArgs e)
        {
            if (this.Focused)
            {
                SetGraphRange();
                RestartWorker();
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgBar.Value = e.ProgressPercentage;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
#if DEBUG
                debugFrm.lblTime.Text = "last time = " + (int)(DateTime.Now - startTime).TotalMilliseconds + "ms"; 
#endif
                bit = Tbit;
                bitScale = Tscale;
                bitOffset = new Point((int)Math.Round(-TminX * Tscale), (int)Math.Round(TmaxY * Tscale));
                this.Invalidate(false);
            }
            pgBar.Value = 0;
            pgBar.Visible = false;
            btnPlot.Visible = true;
        }

        private void workerWatch_Tick(object sender, EventArgs e)
        {
            if (RunWorker)
            {
                if (!worker.IsBusy)
                {
                    pgBar.Visible = true;
                    btnPlot.Visible = false;

                    RunWorker = false;
                    worker.RunWorkerAsync();
                }
            }
        }

        private void btnPlot_Click(object sender, EventArgs e)
        {
            enterEquation();        
        }

        private void frmGraph_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(axesCol, offset.X, 0, offset.X, this.Height - controlsHeight);
            e.Graphics.DrawLine(axesCol, 0, offset.Y, this.Width, offset.Y);

            e.Graphics.DrawImage(
                bit,
                new Rectangle(
                    (int)(offset.X - (bitOffset.X * (scale / bitScale))),
                    (int)(offset.Y - (bitOffset.Y * (scale / bitScale))),
                    (int)(bit.Width * (scale / bitScale)),
                    (int)(bit.Height * (scale / bitScale))),
                new Rectangle(0, 0, bit.Width, bit.Height),
                GraphicsUnit.Pixel);
          
            //Y axis
            for (int y = offset.Y + 64; y < this.Height - controlsHeight; y += 64)
            {
                e.Graphics.DrawLine(Pens.Black, offset.X - 10, y, offset.X + 10, y);
                e.Graphics.DrawString(((offset.Y - y) / scale).ToString(), DefaultFont, Brushes.Black, offset.X + 15, y - 5);
            }
            for (int y = offset.Y -64; y > 0; y -= 64)
            {
                e.Graphics.DrawLine(Pens.Black, offset.X - 10, y, offset.X + 10, y);
                e.Graphics.DrawString(((offset.Y - y) / scale).ToString(), DefaultFont, Brushes.Black, offset.X + 15, y - 5);
            }

            //X axis
            for (int x = offset.X + 64; x < this.Width; x += 64)
            {
                e.Graphics.DrawLine(Pens.Black, x, offset.Y - 10, x, offset.Y + 10);
                e.Graphics.DrawString((( x - offset.X) / scale).ToString(), DefaultFont, Brushes.Black, x - 10, offset.Y + 15);
            }
            for (int x = offset.X - 64; x > 0; x -= 64)
            {
                e.Graphics.DrawLine(Pens.Black, x, offset.Y - 10, x, offset.Y + 10);
                e.Graphics.DrawString(((x - offset.X) / scale).ToString(), DefaultFont, Brushes.Black, x - 10, offset.Y + 15);
            }

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            offset = new Point((int)(this.Width / 2.1), (this.Height - controlsHeight) / 2);
            scale = 16; scale = 16;

            SetGraphRange();
            this.Invalidate(false);
            RestartWorker();
        }

        private void btnCol_Click(object sender, EventArgs e)
        {
            if (colorDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                graphCol = colorDlg.Color;
                RestartWorker();
            }
        }

        private void link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            link.LinkVisited = true;
            System.Diagnostics.Process.Start("http://forceplot.codeplex.com");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (saveDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    System.IO.StreamWriter fl = new System.IO.StreamWriter(saveDlg.FileName, false, System.Text.Encoding.Unicode);
                    fl.WriteLine("equation:" + currEquation);
                    fl.WriteLine("scale:" + scale.ToString());
                    fl.Close();
                }
                catch
                {
                    MessageBox.Show(this, "An error occured while saving to '" + saveDlg.FileName + "'.", "Error while saving", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void OpenFile(string filepath)
        {
            try
            {
                System.IO.StreamReader fl = new System.IO.StreamReader(filepath, System.Text.Encoding.Unicode);
                while (!fl.EndOfStream)
                {
                    string line = fl.ReadLine();
                    if (line.StartsWith("equation:"))
                    {
                        txtEquation.Text = line.Substring("equation:".Length);
                    }
                    else if (line.StartsWith("scale:"))
                    {
                        scale = double.Parse(line.Substring("scale:".Length));
                    }
                }
                fl.Close();
                enterEquation();
            }
            catch
            {
                MessageBox.Show(this, "An error occured while opening '" + filepath + "'.", "Error while opening", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
