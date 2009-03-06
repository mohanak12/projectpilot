using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Painter
{
    public partial class FormPainter : Form
    {
        public FormPainter()
        {
            InitializeComponent();
        }

        private void buttonPaint_Click(object sender, EventArgs e)
        {
            using (Graphics g = this.pictureBoxPainting.CreateGraphics())
            {
                using (Pen pen = new Pen(Color.Blue, 10))
                {
                    g.DrawRectangle(pen, 100, 100, 400, 200);
                }
            }
        }
    }
}
