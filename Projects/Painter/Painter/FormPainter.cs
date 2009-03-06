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
                    Rectangle rectangle = new Rectangle(new Point(30, 30), 100, 100);
                    Circle circle = new Circle(new Point(70, 70), 150);
                    Line line = new Line(new Point(15, 15), new Point(100, 150));


                    Rectangle rectangle1 = new Rectangle(new Point(90, 90), 120, 100);

                    PainterGDI painter = new PainterGDI(pen);
                    painter.AddElement(rectangle);

                    pen.Color = Color.Red;
                    painter.AddElement(rectangle1);


                    pen.Color = Color.Yellow;
                    painter.AddElement(circle);
                    painter.AddElement(line);
                    painter.PaintAll(g);
                    pen.Color = Color.Red;
                    painter.Paint(g, painter.Elements[1]);
                }

            }

        }
    }
}
