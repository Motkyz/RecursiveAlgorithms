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
    public class PythagorasTree : Fractal
    {
        public PythagorasTree(Canvas canvas, Func<int> getDelay) : base(canvas, getDelay) { }

        public async Task DrawTree(Point pointStart, double length, double angle, int depth, CancellationToken cancellationToken)
        {
            if (depth == 0 || cancellationToken.IsCancellationRequested) return;

            Point pointEnd = new Point(pointStart.X + length * Math.Cos(angle), pointStart.Y + length * Math.Sin(angle));

            DrawLine(pointStart, pointEnd, 2, Brushes.Blue);

            await Task.Delay(GetDelay(), cancellationToken);

            await DrawTree(pointEnd, length * 0.75, angle - Math.PI / 4, depth - 1, cancellationToken);
            await DrawTree(pointEnd, length * 0.75, angle + Math.PI / 4, depth - 1, cancellationToken);
        }
    }
}
