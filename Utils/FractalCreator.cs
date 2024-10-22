using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace RecursiveAlgorithms.Utils
{
    public class FractalCreator
    {
        private List<Line> _lines = new List<Line>();
        public static int _depth;
        private const double maxThick = 3;
        private const double minThick = 0.5;

        public FractalCreator(Point pointStart, int depth, double width, double height)
        {
            //_depth = depth;
            CreateLines(pointStart, depth, width, height);
        }

        public List<Line> GetLines()
        {
            return _lines;
        }

        void CreateLines(Point pointStart, int depth, double width, double height)
        {
            double xOffset = width / 2;
            double yOffset = height / _depth;

            double thickness = depth * 0.75 + 0.5 > maxThick ? maxThick : depth * 0.75 + 0.5;

            Point pointEnd1 = new Point(pointStart.X - xOffset, pointStart.Y + yOffset);
            Point pointEnd2 = new Point(pointStart.X + xOffset, pointStart.Y + yOffset);

            _lines.Add(new Line()
            {
                X1 = pointStart.X,
                Y1 = pointStart.Y,
                X2 = pointEnd1.X,
                Y2 = pointEnd1.Y,
                StrokeThickness = thickness,
                Stroke = Brushes.Red
            });

            _lines.Add(new Line()
            {
                X1 = pointStart.X,
                Y1 = pointStart.Y,
                X2 = pointEnd2.X,
                Y2 = pointEnd2.Y,
                StrokeThickness = thickness,
                Stroke = Brushes.Red
            });

            if (depth != 1)
            {
                CreateLines(pointEnd1, depth - 1, width / 2, height);
                CreateLines(pointEnd2, depth - 1, width / 2, height);
            }
        }
    }
}
