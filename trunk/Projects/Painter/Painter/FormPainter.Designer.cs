namespace Painter
{
    partial class FormPainter
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
            this.panelPainting = new System.Windows.Forms.Panel();
            this.panelLower = new System.Windows.Forms.Panel();
            this.buttonPaint = new System.Windows.Forms.Button();
            this.pictureBoxPainting = new System.Windows.Forms.PictureBox();
            this.panelPainting.SuspendLayout();
            this.panelLower.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPainting)).BeginInit();
            this.SuspendLayout();
            // 
            // panelPainting
            // 
            this.panelPainting.Controls.Add(this.pictureBoxPainting);
            this.panelPainting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPainting.Location = new System.Drawing.Point(0, 0);
            this.panelPainting.Name = "panelPainting";
            this.panelPainting.Size = new System.Drawing.Size(617, 279);
            this.panelPainting.TabIndex = 0;
            // 
            // panelLower
            // 
            this.panelLower.Controls.Add(this.buttonPaint);
            this.panelLower.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLower.Location = new System.Drawing.Point(0, 279);
            this.panelLower.Name = "panelLower";
            this.panelLower.Size = new System.Drawing.Size(617, 48);
            this.panelLower.TabIndex = 1;
            // 
            // buttonPaint
            // 
            this.buttonPaint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonPaint.Location = new System.Drawing.Point(240, 8);
            this.buttonPaint.Name = "buttonPaint";
            this.buttonPaint.Size = new System.Drawing.Size(137, 31);
            this.buttonPaint.TabIndex = 0;
            this.buttonPaint.Text = "Paint!";
            this.buttonPaint.UseVisualStyleBackColor = true;
            this.buttonPaint.Click += new System.EventHandler(this.buttonPaint_Click);
            // 
            // pictureBoxPainting
            // 
            this.pictureBoxPainting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxPainting.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxPainting.Name = "pictureBoxPainting";
            this.pictureBoxPainting.Size = new System.Drawing.Size(617, 279);
            this.pictureBoxPainting.TabIndex = 0;
            this.pictureBoxPainting.TabStop = false;
            // 
            // FormPainter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 327);
            this.Controls.Add(this.panelPainting);
            this.Controls.Add(this.panelLower);
            this.Name = "FormPainter";
            this.Text = "Painter";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelPainting.ResumeLayout(false);
            this.panelLower.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPainting)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelPainting;
        private System.Windows.Forms.Panel panelLower;
        private System.Windows.Forms.PictureBox pictureBoxPainting;
        private System.Windows.Forms.Button buttonPaint;
    }
}

