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
            this.btnReset = new System.Windows.Forms.Button();
            this.colorDlg = new System.Windows.Forms.ColorDialog();
            this.panelControls = new System.Windows.Forms.Panel();
            this.link = new System.Windows.Forms.LinkLabel();
            this.btnCol = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.saveDlg = new System.Windows.Forms.SaveFileDialog();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtEquation
            // 
            this.txtEquation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEquation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEquation.Location = new System.Drawing.Point(3, 3);
            this.txtEquation.Name = "txtEquation";
            this.txtEquation.Size = new System.Drawing.Size(726, 26);
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
            this.pgBar.Location = new System.Drawing.Point(729, 3);
            this.pgBar.Name = "pgBar";
            this.pgBar.Size = new System.Drawing.Size(102, 26);
            this.pgBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pgBar.TabIndex = 2;
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
            this.btnPlot.Location = new System.Drawing.Point(729, 3);
            this.btnPlot.Name = "btnPlot";
            this.btnPlot.Size = new System.Drawing.Size(102, 26);
            this.btnPlot.TabIndex = 4;
            this.btnPlot.Text = "Plot";
            this.btnPlot.UseVisualStyleBackColor = true;
            this.btnPlot.Click += new System.EventHandler(this.btnPlot_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(3, 35);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "Reset view";
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
            this.panelControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.panelControls.Controls.Add(this.btnSave);
            this.panelControls.Controls.Add(this.link);
            this.panelControls.Controls.Add(this.btnCol);
            this.panelControls.Controls.Add(this.txtEquation);
            this.panelControls.Controls.Add(this.btnReset);
            this.panelControls.Controls.Add(this.pgBar);
            this.panelControls.Controls.Add(this.btnPlot);
            this.panelControls.Location = new System.Drawing.Point(0, 382);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(834, 84);
            this.panelControls.TabIndex = 8;
            // 
            // link
            // 
            this.link.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.link.AutoSize = true;
            this.link.Location = new System.Drawing.Point(714, 40);
            this.link.Name = "link";
            this.link.Size = new System.Drawing.Size(117, 13);
            this.link.TabIndex = 9;
            this.link.TabStop = true;
            this.link.Text = "forceplot.codeplex.com";
            this.link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.link_LinkClicked);
            // 
            // btnCol
            // 
            this.btnCol.Location = new System.Drawing.Point(84, 35);
            this.btnCol.Name = "btnCol";
            this.btnCol.Size = new System.Drawing.Size(79, 23);
            this.btnCol.TabIndex = 8;
            this.btnCol.Text = "Change color";
            this.btnCol.UseVisualStyleBackColor = true;
            this.btnCol.Click += new System.EventHandler(this.btnCol_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(169, 35);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save graph";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // saveDlg
            // 
            this.saveDlg.DefaultExt = "graph";
            this.saveDlg.Filter = "Graph files (*.graph)|*.graph|All files (*.*)|*.*";
            this.saveDlg.Title = "Save graph to file";
            // 
            // frmGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(834, 478);
            this.Controls.Add(this.panelControls);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 360);
            this.Name = "frmGraph";
            this.Text = "ForcePlot";
            this.Load += new System.EventHandler(this.frmGraph_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmGraph_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.frmGraph_MouseWheel);
            this.Resize += new System.EventHandler(this.frmGraph_Resize);
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
        private System.Windows.Forms.Button btnCol;
        private System.Windows.Forms.LinkLabel link;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog saveDlg;
    }
}