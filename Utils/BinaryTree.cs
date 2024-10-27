using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;

namespace RecursiveAlgorithms.Utils
{
    public class BinaryTree : Fractal
    {
        private readonly int _lvls;
        private const double maxThick = 2;

        public BinaryTree(Canvas canvas, Func<int> getDelay) : base(canvas, getDelay)
        {
            _lvls = Depth;
        }

        public async Task DrawTree(Point pointStart, int depth, double width, double height, CancellationToken cancellationToken)
        {
            if (depth == 0 || cancellationToken.IsCancellationRequested) return;

            double xOffset = width / 2;
            double yOffset = height / _lvls;

            double thickness = depth * 0.5 + 0.1 > maxThick ? maxThick : depth * 0.5 + 0.1;

            Point pointEnd1 = new Point(pointStart.X - xOffset, pointStart.Y + yOffset);
            Point pointEnd2 = new Point(pointStart.X + xOffset, pointStart.Y + yOffset);

            DrawLine(pointStart, pointEnd1, thickness, Brushes.Red);
            DrawLine(pointStart, pointEnd2, thickness, Brushes.Red);

            await Task.Delay(GetDelay(), cancellationToken);

            await DrawTree(pointEnd1, depth - 1, width / 2, height, cancellationToken);
            await DrawTree(pointEnd2, depth - 1, width / 2, height, cancellationToken);
        }
    }
}
