using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RecursiveAlgorithms.Utils
{
    public class Fractal
    {
        public static int Depth { get; set; }
        protected Canvas Canvas { get; set; }
        protected Func<int> GetDelay;

        public Fractal(Canvas canvas, Func<int> getDelay)
        {
            Canvas = canvas;
            GetDelay = getDelay;
        }

        protected void DrawLine(Point pointStart, Point pointEnd, double thickness, Brush color)
        {
            Line line = new Line()
            {
                X1 = pointStart.X,
                Y1 = pointStart.Y,
                X2 = pointEnd.X,
                Y2 = pointEnd.Y,
                Stroke = color,
                StrokeThickness = thickness
            };

            Canvas.Children.Add(line);
        }
    }
}
