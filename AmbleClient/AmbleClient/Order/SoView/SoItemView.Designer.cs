﻿namespace AmbleClient.SO
{
    partial class SoItemView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoItemView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbOp = new System.Windows.Forms.ToolStripButton();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.soItemsControl1 = new AmbleClient.SO.SoItemsControl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOp,
            this.tsbClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(694, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbOp
            // 
            this.tsbOp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbOp.Image = ((System.Drawing.Image)(resources.GetObject("tsbOp.Image")));
            this.tsbOp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOp.Name = "tsbOp";
            this.tsbOp.Size = new System.Drawing.Size(71, 22);
            this.tsbOp.Text = "Op&&Close";
            this.tsbOp.Click += new System.EventHandler(this.tsbOp_Click);
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(43, 22);
            this.tsbClose.Text = "Close";
            // 
            // soItemsControl1
            // 
            this.soItemsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.soItemsControl1.Location = new System.Drawing.Point(0, 25);
            this.soItemsControl1.Name = "soItemsControl1";
            this.soItemsControl1.Size = new System.Drawing.Size(694, 477);
            this.soItemsControl1.TabIndex = 1;
            // 
            // SoItemView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 502);
            this.Controls.Add(this.soItemsControl1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SoItemView";
            this.Text = "SoItemView";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbOp;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private SoItemsControl soItemsControl1;

    }
}