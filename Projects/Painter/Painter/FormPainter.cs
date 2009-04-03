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
            //rectangle.DrawingProperties = new GDIDrawingProperties();
            
            Circle circle = new Circle(new Point(70, 70), 50);
            //circle.DrawingProperties = new GDIDrawingProperties();
            
            Line line = new Line(new Point(15, 15), new Point(100, 150));
            line.DrawingProperties = new GDIDrawingProperties(new Colour(255,0,0),10);
            
            Rectangle rectangle1 = new Rectangle(new Point(90, 90), 120, 100);
            rectangle1.DrawingProperties = new GDIDrawingProperties();
            
            Triangle triangle = new Triangle(new Point(290, 90), new Point(190, 290), new Point(375, 165));
            triangle.DrawingProperties = new GDIDrawingProperties();
            
            House house = new House(new Point(350, 350), 250, 400);
            Window window = new Window(new Point(125, 250), 50, 100);

            House house1 = new House(new Point(850, 350), 150, 200);
            House house2 = new House(new Point(950, 650), 250, 100);


            elements.Add(rectangle);
            elements.Add(circle);
            elements.Add(line);
            elements.Add(rectangle1);
            elements.Add(triangle);
            elements.Add(house);
            elements.Add(window);
            elements.Add(house1);
            //elements.Add(house2);
            
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
