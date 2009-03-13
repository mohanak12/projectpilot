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
            IList<PaintElement> elements = new List<PaintElement>();
            Rectangle rectangle = new Rectangle(new Point(30, 30), 100, 100);
            Circle circle = new Circle(new Point(70, 70), 50);
            Line line = new Line(new Point(15, 15), new Point(100, 150));
            Rectangle rectangle1 = new Rectangle(new Point(90, 90), 120, 100);
            Triangle triangle = new Triangle(new Point(290, 90), new Point(190, 290), new Point(375, 165));
            PieSlice piceSlice = new PieSlice(new Point(325, 225), new Point(385, 250), 60);

            elements.Add(rectangle);
            elements.Add(circle);
            elements.Add(line);
            elements.Add(rectangle1);
            elements.Add(triangle);
            //elements.Add(piceSlice);
            
            using (Graphics g = this.pictureBoxPainting.CreateGraphics())
            {
                IDrawingEngine drawingEngine = new GDIdrawingEngine(g);

                foreach (PaintElement element in elements)
                {
                    element.Draw(drawingEngine);
                }
            }

        }
    }
}
