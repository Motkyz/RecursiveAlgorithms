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
    public class PythagorasTree
    {
        private readonly Canvas _canvas;

        public PythagorasTree(Canvas canvas)
        {
            _canvas = canvas;
        }

        public async Task DrawTree(Point pointStart, double angle, int depth, double length)
        {
            Point pointEnd = new Point(pointStart.X + length * Math.Cos(angle), pointStart.Y + length * Math.Sin(angle));

            DrawBranch(pointStart, pointEnd);

            await Task.Delay(1);

            if (depth > 0)
            {
                await DrawTree(pointEnd, angle - Math.PI / 4, depth - 1, length * 0.75);
                await DrawTree(pointEnd, angle + Math.PI / 4, depth - 1, length * 0.75);
            }
        }

        private void DrawBranch(Point pointStart, Point pointEnd)
        {
            Line line = new Line
            {
                X1 = pointStart.X,
                Y1 = pointStart.Y,
                X2 = pointEnd.X,
                Y2 = pointEnd.Y,
                Stroke = Brushes.Green,
                StrokeThickness = 2
            };

            _canvas.Children.Add(line);
        }
    }
}
