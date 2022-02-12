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

namespace ForcePlot
{
    partial class frmGraph
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGraph));
            this.txtEquation = new System.Windows.Forms.TextBox();
            this.worker = new System.ComponentModel.BackgroundWorker();
            this.pgBar = new System.Windows.Forms.ProgressBar();
            this.workerWatch = new System.Windows.Forms.Timer(this.components);
            this.btnPlot = new System.Windows.Forms.Button();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.btnReset = new System.Windows.Forms.Button();
            this.colorDlg = new System.Windows.Forms.ColorDialog();
            this.panelControls = new System.Windows.Forms.Panel();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.saveDlg = new System.Windows.Forms.SaveFileDialog();
            this.lblTrace = new System.Windows.Forms.Label();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtEquation
            // 
            this.txtEquation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEquation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEquation.Location = new System.Drawing.Point(3, 7);
            this.txtEquation.Name = "txtEquation";
            this.txtEquation.Size = new System.Drawing.Size(592, 26);
            this.txtEquation.TabIndex = 0;
            this.txtEquation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEquation_KeyDown);
            // 
            // worker
            // 
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
            this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.worker_ProgressChanged);
            this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
            // 
            // pgBar
            // 
            this.pgBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pgBar.Location = new System.Drawing.Point(601, 7);
            this.pgBar.Name = "pgBar";
            this.pgBar.Size = new System.Drawing.Size(107, 26);
            this.pgBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pgBar.TabIndex = 20;
            this.pgBar.Visible = false;
            // 
            // workerWatch
            // 
            this.workerWatch.Enabled = true;
            this.workerWatch.Interval = 200;
            this.workerWatch.Tick += new System.EventHandler(this.workerWatch_Tick);
            // 
            // btnPlot
            // 
            this.btnPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlot.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPlot.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPlot.ImageKey = "RecordHS.png";
            this.btnPlot.ImageList = this.imageListIcons;
            this.btnPlot.Location = new System.Drawing.Point(601, 7);
            this.btnPlot.Name = "btnPlot";
            this.btnPlot.Size = new System.Drawing.Size(107, 26);
            this.btnPlot.TabIndex = 1;
            this.btnPlot.Text = "Plot";
            this.btnPlot.UseVisualStyleBackColor = true;
            this.btnPlot.Click += new System.EventHandler(this.btnPlot_Click);
            // 
            // imageListIcons
            // 
            this.imageListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIcons.ImageStream")));
            this.imageListIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListIcons.Images.SetKeyName(0, "saveHS.png");
            this.imageListIcons.Images.SetKeyName(1, "CanvasScaleHS.png");
            this.imageListIcons.Images.SetKeyName(2, "RecordHS.png");
            this.imageListIcons.Images.SetKeyName(3, "Help.png");
            // 
            // btnReset
            // 
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReset.ImageKey = "CanvasScaleHS.png";
            this.btnReset.ImageList = this.imageListIcons;
            this.btnReset.Location = new System.Drawing.Point(3, 39);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(59, 23);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // colorDlg
            // 
            this.colorDlg.Color = System.Drawing.Color.Red;
            // 
            // panelControls
            // 
            this.panelControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControls.BackColor = System.Drawing.SystemColors.Control;
            this.panelControls.Controls.Add(this.btnHelp);
            this.panelControls.Controls.Add(this.btnSave);
            this.panelControls.Controls.Add(this.txtEquation);
            this.panelControls.Controls.Add(this.btnReset);
            this.panelControls.Controls.Add(this.pgBar);
            this.panelControls.Controls.Add(this.btnPlot);
            this.panelControls.Location = new System.Drawing.Point(11, 416);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(711, 84);
            this.panelControls.TabIndex = 8;
            this.panelControls.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelControls_MouseDown);
            this.panelControls.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelControls_MouseMove);
            this.panelControls.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelControls_MouseUp);
            // 
            // btnHelp
            // 
            this.btnHelp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHelp.ImageKey = "Help.png";
            this.btnHelp.ImageList = this.imageListIcons;
            this.btnHelp.Location = new System.Drawing.Point(131, 39);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(53, 23);
            this.btnHelp.TabIndex = 21;
            this.btnHelp.Text = "Help";
            this.btnHelp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnSave
            // 
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.ImageKey = "saveHS.png";
            this.btnSave.ImageList = this.imageListIcons;
            this.btnSave.Location = new System.Drawing.Point(68, 39);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(57, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // saveDlg
            // 
            this.saveDlg.DefaultExt = "graph";
            this.saveDlg.Filter = "Graph file (*.graph)|*.graph|PNG image (*.PNG)|*.PNG";
            this.saveDlg.Title = "Save graph to file";
            // 
            // lblTrace
            // 
            this.lblTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTrace.BackColor = System.Drawing.Color.Transparent;
            this.lblTrace.ForeColor = System.Drawing.Color.DimGray;
            this.lblTrace.Location = new System.Drawing.Point(622, 396);
            this.lblTrace.Name = "lblTrace";
            this.lblTrace.Size = new System.Drawing.Size(100, 17);
            this.lblTrace.TabIndex = 9;
            this.lblTrace.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmGraph
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(734, 512);
            this.Controls.Add(this.lblTrace);
            this.Controls.Add(this.panelControls);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 360);
            this.Name = "frmGraph";
            this.Text = "ForcePlot";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(239)))), ((int)(((byte)(16)))));
            this.ResizeEnd += new System.EventHandler(this.frmGraph_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.frmGraph_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmGraph_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmGraph_DragEnter);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmGraph_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseDown);
            this.MouseLeave += new System.EventHandler(this.frmGraph_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseWheel);
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtEquation;
        private System.ComponentModel.BackgroundWorker worker;
        private System.Windows.Forms.ProgressBar pgBar;
        private System.Windows.Forms.Timer workerWatch;
        private System.Windows.Forms.Button btnPlot;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ColorDialog colorDlg;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog saveDlg;
        private System.Windows.Forms.Label lblTrace;
        private System.Windows.Forms.ImageList imageListIcons;
        private System.Windows.Forms.Button btnHelp;
    }
}