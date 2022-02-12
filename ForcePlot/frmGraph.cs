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
using System.Runtime.InteropServices;

namespace ForcePlot
{
    public partial class frmGraph : Form
    {
#if DEBUG
        System.Diagnostics.Stopwatch PerfCounter = new System.Diagnostics.Stopwatch();
        long calTime;
#endif
        Point prevCenter;
        bool mouseDownDragForm;
        Point mouseLastDragForm;
        const int subSamplingRate = 1;
        const int controlsHeight = 105;
        Pen axesCol = new Pen(Brushes.Black, 3);
        bool RunWorker;
        int Ystart, Yend, Xstart, Xend;
        int TYstart, TYend, TXstart, TXend;
        double scale;
        double Tscale;
        Point offset;
        int mouseLastX, mouseLastY;
        bool mouseDown;
        bool mouseMoved;
        FormWindowState prevWinState = FormWindowState.Normal;
        Size prevWinSize;
        Bitmap Tbit = new Bitmap(1, 1);
        Bitmap bit = new Bitmap(1, 1);
        Point bitCenter = new Point(0, 0);
        double bitScale = 1;
        string currEquation;
        term LHS, RHS;
        term TLHS, TRHS;
        Bitmap eqBit = new Bitmap(1, 1);

        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS { public int Left; public int Right; public int Top; public int Bottom; }
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
#if DEBUG
            PerfCounter.Start();
#endif
            TLHS = LHS; TRHS = RHS;
            TYstart = Ystart / subSamplingRate; TYend = Yend / subSamplingRate;
            TXstart = Xstart / subSamplingRate; TXend = Xend / subSamplingRate;
            Tscale = scale / subSamplingRate;
            int noOfAnswersL = 1;
            int noOfAnswersR = 1;
            int VLines = TYend - TYstart;
            double[, ,] differences = new double[TXend - TXstart, TYend - TYstart, noOfAnswersL * noOfAnswersR];
            for (int iy = 0; iy < (TYend - TYstart); iy++)
            {
                double y = (iy + TYstart) / -Tscale;
                for (int ix = 0; ix < (TXend - TXstart); ix++)
                {
                    double x = (ix + TXstart) / Tscale;
                    double LHSvalue = TLHS.getValue(x, y);
                    double RHSvalue = TRHS.getValue(x, y);
                    for (int Lans = 0; Lans < noOfAnswersL; Lans++)
                    {
                        for (int Rans = 0; Rans < noOfAnswersR; Rans++)
                        {
                            differences[ix, iy, Lans * noOfAnswersR + Rans] = (LHSvalue - RHSvalue);
                        }
                    }
                }
                if (worker.CancellationPending) { e.Cancel = true; return; }
                worker.ReportProgress((iy * 100) / VLines);
            }
            worker.ReportProgress(100);
#if DEBUG
            PerfCounter.Stop();
            calTime = PerfCounter.ElapsedMilliseconds;
            PerfCounter.Reset();
            PerfCounter.Start();
#endif
            Tbit = new Bitmap(TXend - TXstart + 1, TYend - TYstart + 1);
            System.Drawing.Imaging.BitmapData bitData = Tbit.LockBits(new Rectangle(0, 0, Tbit.Width, Tbit.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Tbit.PixelFormat);
            int stride = bitData.Stride;
            byte[] bitBytes = new byte[bitData.Stride * bitData.Height];
            int MaxX = differences.GetLength(0) - 1;
            int MaxY = differences.GetLength(1) - 1;
            int MaxI = differences.GetLength(2);
            for (int ix = 0; ix < MaxX; ix++)
            {
                for (int iy = 0; iy < MaxY; iy++)
                {
                    for (int i = 0; i < MaxI; i++)
                    {
                        if (differences[ix, iy, i] == 0f)
                        {
                            addAlpha(bitBytes, bitData.Stride, ix, iy, 1);
                        }
                        else if (!double.IsNaN(differences[ix, iy, i]))
                        {
                            if (Has0(differences[ix, iy, i], differences[ix + 1, iy, i]))
                            {
                                double mid = center(differences[ix, iy, i], differences[ix + 1, iy, i]);
                                addAlpha(bitBytes, stride, ix, iy, 1 - mid);
                                addAlpha(bitBytes, stride, ix + 1, iy, mid);
                            }
                            if (Has0(differences[ix, iy, i], differences[ix, iy + 1, i]))
                            {
                                double mid = center(differences[ix, iy, i], differences[ix, iy + 1, i]);
                                addAlpha(bitBytes, stride, ix, iy, 1 - mid);
                                addAlpha(bitBytes, stride, ix, iy + 1, mid);
                            }
                        }
                    }
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(bitBytes, 0, bitData.Scan0, bitBytes.Length);
            Tbit.UnlockBits(bitData);
#if DEBUG
            PerfCounter.Stop();
#endif
        }


        public frmGraph(string firstArg)
        {
            InitializeComponent();

            try
            {
                if (DwmIsCompositionEnabled())
                {
                    MARGINS margs = new MARGINS();
                    margs = new MARGINS();
                    margs.Bottom = controlsHeight - 38;
                    DwmExtendFrameIntoClientArea(this.Handle, ref margs);
                    panelControls.BackColor = this.TransparencyKey;
                }
            }
            catch { }

            offset.X = this.ClientSize.Width / 2;
            offset.Y = ((this.Height - controlsHeight) / 2) + 25;
            panelControls.Location = new Point(0, this.Height - controlsHeight);
            panelControls.Height = controlsHeight;
            panelControls.Width = this.ClientSize.Width;
            lblTrace.Location = new Point(panelControls.Location.X + panelControls.Width - lblTrace.Width, panelControls.Location.Y - lblTrace.Height);
            prevWinSize = this.Size;
            if (firstArg == "")
            {
                scale = 16;
#if DEBUG
                txtEquation.Text = "y=x^(1/3)";
#else
                txtEquation.Text = "y=x^2";
#endif
                enterEquation();
                txtEquation.SelectAll();
            }
            else
            {
                if (!OpenFile(firstArg))
                {
                    MessageBox.Show(this, "An error occured while opening '" + firstArg + "'.", "Error while opening", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void frmGraph_Paint(object sender, PaintEventArgs e)
        {
            if (eqBit.Width > this.ClientSize.Width)
            {
                e.Graphics.DrawImage(eqBit, 0, 0, this.ClientSize.Width, 50 * this.ClientSize.Width / eqBit.Width);
            }
            else
            {
                e.Graphics.DrawImageUnscaled(eqBit, 5, 5);
            }

            e.Graphics.DrawLine(axesCol, offset.X, 0, offset.X, this.Height - controlsHeight);
            e.Graphics.DrawLine(axesCol, 0, offset.Y, this.ClientSize.Width, offset.Y);

            e.Graphics.DrawImage(
                bit,
                (int)(offset.X - (bitCenter.X * (scale / bitScale))),
                (int)(offset.Y - (bitCenter.Y * (scale / bitScale))),
                (int)(bit.Width * (scale / bitScale)),
                (int)(bit.Height * (scale / bitScale)));

            for (int y = offset.Y + 64; y < this.Height - controlsHeight; y += 64)
            {
                e.Graphics.DrawLine(Pens.Black, offset.X - 10, y, offset.X + 10, y);
                e.Graphics.DrawString(((offset.Y - y) / scale).ToString(), DefaultFont, Brushes.Black, offset.X + 15, y - 5);
            }
            for (int y = offset.Y - 64; y > 0; y -= 64)
            {
                e.Graphics.DrawLine(Pens.Black, offset.X - 10, y, offset.X + 10, y);
                e.Graphics.DrawString(((offset.Y - y) / scale).ToString(), DefaultFont, Brushes.Black, offset.X + 15, y - 5);
            }

            for (int x = offset.X + 64; x < this.ClientSize.Width; x += 64)
            {
                e.Graphics.DrawLine(Pens.Black, x, offset.Y - 10, x, offset.Y + 10);
                e.Graphics.DrawString(((x - offset.X) / scale).ToString(), DefaultFont, Brushes.Black, x - 10, offset.Y + 15);
            }
            for (int x = offset.X - 64; x > 0; x -= 64)
            {
                e.Graphics.DrawLine(Pens.Black, x, offset.Y - 10, x, offset.Y + 10);
                e.Graphics.DrawString(((x - offset.X) / scale).ToString(), DefaultFont, Brushes.Black, x - 10, offset.Y + 15);
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("calculation=" + calTime + "ms");
                System.Diagnostics.Debug.WriteLine("drawing=" + PerfCounter.ElapsedMilliseconds + "ms");
#endif
                bit.Dispose();
                bit = Tbit;
                bitScale = Tscale;
                bitCenter.X = -TXstart;
                bitCenter.Y = -TYstart;
                this.Invalidate(false);
                pgBar.Visible = false;
            }
            pgBar.Value = 0;
        }

        #region HelperMethods

        bool Has0(double aValidDouble1, double double2)
        {
            return !double.IsNaN(double2) && Math.Sign(aValidDouble1) == -Math.Sign(double2);
        }

        float center(double d1, double d2)
        {
            double denominator = d1 - d2;
            if (denominator == 0) { return 0.5f; }
            return (float)(d1 / (d1 - d2));
        }

        void addAlpha(byte[] byteArray, int stride, int x, int y, double alphaOver255)
        {
            byteArray[(y * stride) + (x * 4) + 2] = 255;
            if (byteArray[(y * stride) + (x * 4) + 3] < (alphaOver255 * 255))
            {
                byteArray[(y * stride) + (x * 4) + 3] = (byte)(alphaOver255 * 255);
            }
        }

        void SetGraphRange()
        {
            prevCenter.X = this.ClientSize.Width / 2 - offset.X;
            prevCenter.Y = this.Height / 2 - offset.Y; ;
            Xstart = -offset.X - 1;
            Xend = this.ClientSize.Width - offset.X + 1;
            Ystart = -offset.Y - 1;
            Yend = this.Height - controlsHeight - offset.Y + 1;
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
                worker.RunWorkerAsync();
            }
        }

        bool OpenFile(string filepath)
        {
            System.IO.StreamReader fl = new System.IO.StreamReader(filepath, System.Text.Encoding.Unicode);
            bool equationFound = false;
            scale = 16;
            while (!fl.EndOfStream)
            {
                string line = fl.ReadLine();
                if (line.StartsWith("equation:"))
                {
                    txtEquation.Text = line.Substring("equation:".Length);
                    equationFound = true;
                }
                else if (line.StartsWith("scale:"))
                {
                    scale = double.Parse(line.Substring("scale:".Length));
                }
            }
            fl.Close();
            if (!equationFound) { return false; }
            return enterEquation();
        }

        bool enterEquation()
        {
            bool success = false;
            if (!txtEquation.Text.Contains(('=')))
            { txtEquation.Text = "y = " + txtEquation.Text; }
            try
            {
#if DEBUG
                PerfCounter.Reset();
                PerfCounter.Start();
#endif
                string Formattedeq = MathConverter.FormatEquation(txtEquation.Text);
                txtEquation.Text = Formattedeq.Replace(" ", "");
                if (txtEquation.SelectionStart == 0 && txtEquation.SelectionLength == 0) { txtEquation.Select(txtEquation.Text.Length, 0); }
                string[] splitEq = Formattedeq.Split(new char[] { '=' }, 2);
                term unoptimizedLHS;
                term unoptimizedRHS;
                unoptimizedLHS = MathConverter.FromString(splitEq[0]);
                unoptimizedRHS = MathConverter.FromString(splitEq[1]);
                LHS = unoptimizedLHS.optimize();
                RHS = unoptimizedRHS.optimize();
#if DEBUG
                PerfCounter.Stop();
                System.Diagnostics.Debug.WriteLine(LHS.ToString() + "=" + RHS.ToString());
                System.Diagnostics.Debug.WriteLine(1 + "x" + 1 + " comparisons per pixel");
                System.Diagnostics.Debug.WriteLine("equation processing=" + PerfCounter.ElapsedMilliseconds + "ms");
                PerfCounter.Reset();
                PerfCounter.Start();
#endif
                currEquation = txtEquation.Text;
                Font eqFont = new Font("Arial Narrow", 30, FontStyle.Regular);
                Brush eqBrush = Brushes.LightGray;
                Bitmap Lbit = unoptimizedLHS.ToBitmap(eqFont, eqBrush);
                Bitmap Rbit = unoptimizedRHS.ToBitmap(eqFont, eqBrush);
                eqBit = new Bitmap(Lbit.Width + 45 + Rbit.Width, 50);
                Graphics eqBitGraphic = Graphics.FromImage(eqBit);
                eqBitGraphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                eqBitGraphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                eqBitGraphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                eqBitGraphic.DrawImageUnscaled(Lbit, 0, 0);
                eqBitGraphic.DrawString("=", eqFont, eqBrush, Lbit.Width, 0);
                eqBitGraphic.DrawImageUnscaled(Rbit, Lbit.Width + 40, 0);
#if DEBUG
                PerfCounter.Stop();
                System.Diagnostics.Debug.WriteLine("equation bitmap=" + PerfCounter.ElapsedMilliseconds + "ms");
#endif
                success = true;
            }
            catch
            {
                LHS = MathConverter.FromString("1");
                RHS = MathConverter.FromString("0");
                eqBit = new Bitmap(1, 1);
            }
            SetGraphRange();
            RestartWorker();
            return success;
        }

        #endregion

        #region UIevents

        private void txtEquation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                e.SuppressKeyPress = true;
                if (!enterEquation())
                {
                    System.Windows.Forms.MessageBox.Show(this, "Unable to interpret the equation. Please refer the online documentation for help.", "Equation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmGraph_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLastX = e.X - offset.X;
            mouseLastY = e.Y - offset.Y;
            mouseDown = true;
            mouseMoved = false;
        }

        private void frmGraph_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            if (mouseMoved)
            {
                SetGraphRange();
                RestartWorker();
            }
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
                mouseMoved = true;
                this.Invalidate(false);
            }

            lblTrace.Text = "(" + ((e.X - offset.X) / scale).ToString("0.00") + "," + ((e.Y - offset.Y) / -scale).ToString("0.00") + ")";
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgBar.Value = e.ProgressPercentage;
        }

        private void workerWatch_Tick(object sender, EventArgs e)
        {
            if (RunWorker)
            {
                if (!worker.IsBusy)
                {
                    pgBar.Visible = true;
                    RunWorker = false;
                    worker.RunWorkerAsync();
                }
            }
        }

        private void btnPlot_Click(object sender, EventArgs e)
        {
            if (!enterEquation())
            {
                System.Windows.Forms.MessageBox.Show(this, "Unable to interpret the equation. Please refer the online documentation for help.", "Equation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            offset.X = this.ClientSize.Width / 2;
            offset.Y = ((this.Height - controlsHeight) / 2) + 25;
            scale = 16; scale = 16;
            this.Invalidate(false);
            SetGraphRange();
            RestartWorker();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveDlg.FileName = "";
            if (saveDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if (saveDlg.FilterIndex == 1)
                    {
                        System.IO.StreamWriter fl = new System.IO.StreamWriter(saveDlg.FileName, false, System.Text.Encoding.Unicode);
                        fl.WriteLine("equation:" + currEquation);
                        fl.WriteLine("scale:" + scale.ToString());
                        fl.Close();
                    }
                    else
                    {
                        Bitmap saveBit = new Bitmap(this.ClientSize.Width, this.Height - controlsHeight);
                        Graphics saveGraphics = Graphics.FromImage(saveBit);
                        saveGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        saveGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                        saveGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        saveGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        while (worker.IsBusy)
                        {
                            if (MessageBox.Show(this, "Graph calculation is still in progress. Please wait.", "Cannot save while calculating",
                                MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Cancel)
                            {
                                return;
                            }
                        }
                        saveGraphics.DrawLine(axesCol, offset.X, 0, offset.X, this.Height - controlsHeight);
                        saveGraphics.DrawLine(axesCol, 0, offset.Y, this.ClientSize.Width, offset.Y);

                        saveGraphics.DrawImage(
                            bit,
                            (int)(offset.X - (bitCenter.X * (scale / bitScale))),
                            (int)(offset.Y - (bitCenter.Y * (scale / bitScale))),
                            (int)(bit.Width * (scale / bitScale)),
                            (int)(bit.Height * (scale / bitScale)));

                        //Y axis
                        for (int y = offset.Y + 64; y < this.Height - controlsHeight; y += 64)
                        {
                            saveGraphics.DrawLine(Pens.Black, offset.X - 10, y, offset.X + 10, y);
                            saveGraphics.DrawString(((offset.Y - y) / scale).ToString(), DefaultFont, Brushes.Black, offset.X + 15, y - 5);
                        }
                        for (int y = offset.Y - 64; y > 0; y -= 64)
                        {
                            saveGraphics.DrawLine(Pens.Black, offset.X - 10, y, offset.X + 10, y);
                            saveGraphics.DrawString(((offset.Y - y) / scale).ToString(), DefaultFont, Brushes.Black, offset.X + 15, y - 5);
                        }

                        //X axis
                        for (int x = offset.X + 64; x < this.ClientSize.Width; x += 64)
                        {
                            saveGraphics.DrawLine(Pens.Black, x, offset.Y - 10, x, offset.Y + 10);
                            saveGraphics.DrawString(((x - offset.X) / scale).ToString(), DefaultFont, Brushes.Black, x - 10, offset.Y + 15);
                        }
                        for (int x = offset.X - 64; x > 0; x -= 64)
                        {
                            saveGraphics.DrawLine(Pens.Black, x, offset.Y - 10, x, offset.Y + 10);
                            saveGraphics.DrawString(((x - offset.X) / scale).ToString(), DefaultFont, Brushes.Black, x - 10, offset.Y + 15);
                        }

                        saveBit.Save(saveDlg.FileName);
                    }
                }
                catch
                {
                    MessageBox.Show(this, "An error occured while saving to '" + saveDlg.FileName + "'.", "Error while saving", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmGraph_ResizeEnd(object sender, EventArgs e)
        {
            if (!(this.Size == prevWinSize))
            {
                prevWinSize = this.Size;
                SetGraphRange();
                RestartWorker();
            }
        }

        private void frmGraph_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                offset.X = this.ClientSize.Width / 2 - prevCenter.X;
                offset.Y = this.Height / 2 - prevCenter.Y;
                if (this.WindowState != prevWinState)
                {
                    prevWinState = this.WindowState;
                    SetGraphRange();
                    RestartWorker();
                }
                this.Invalidate(false);
            }
        }

        private void frmGraph_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void frmGraph_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Array dropData = (Array)e.Data.GetData(DataFormats.FileDrop);
                if (dropData != null)
                {
                    if (dropData.Length > 0)
                    {
                        OpenFile(dropData.GetValue(0).ToString());
                        this.Activate();
                    }
                }
            }
            catch { }
        }

        private void panelControls_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownDragForm = true;
            mouseLastDragForm = e.Location;
        }

        private void panelControls_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDownDragForm = false;
        }

        private void panelControls_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownDragForm && this.WindowState == FormWindowState.Normal)
            {
                this.Location = new Point(this.Location.X + e.X - mouseLastDragForm.X, this.Location.Y + e.Y - mouseLastDragForm.Y);
            }
        }

        private void frmGraph_MouseLeave(object sender, EventArgs e)
        {
            lblTrace.Text = "";
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://forceplot.codeplex.com/documentation");
        }

        #endregion

    }
}
