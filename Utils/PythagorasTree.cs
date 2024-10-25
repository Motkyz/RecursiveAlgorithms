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
        private readonly Func<int> _getDelay;

        public PythagorasTree(Canvas canvas, Func<int> getDelay)
        {
            _canvas = canvas;
            _getDelay = getDelay;
        }

        public async Task DrawTree(Point pointStart, double angle, int depth, double length, CancellationToken cancellationToken)
        {
            if (depth == 0 || cancellationToken.IsCancellationRequested) return;

            Point pointEnd = new Point(pointStart.X + length * Math.Cos(angle), pointStart.Y + length * Math.Sin(angle));

            DrawBranch(pointStart, pointEnd);

            await Task.Delay(_getDelay(), cancellationToken);

            await DrawTree(pointEnd, angle - Math.PI / 4, depth - 1, length * 0.75, cancellationToken);
            await DrawTree(pointEnd, angle + Math.PI / 4, depth - 1, length * 0.75, cancellationToken);

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
