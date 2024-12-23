﻿using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace RecursiveAlgorithms.Utils
{
    public class BarnsliFern : Fractal
    {
        private readonly double _ratio;

        public BarnsliFern(Canvas canvas, Func<int> getDelay) : base(canvas, getDelay)
        {
            _ratio = canvas.ActualHeight / (Depth * 10) / 3;
        }

        public async Task DrawFern(Point pointStart, double length, double angle, CancellationToken cancellationToken)
        {
            if (length > 1 || cancellationToken.IsCancellationRequested)
            {
                double x = Math.Round(pointStart.X + length * _ratio * Math.Cos(angle));
                double y = Math.Round(pointStart.Y - length * _ratio * Math.Sin(angle));
                Point pointEnd = new Point(x, y);

                await CreateLine(pointStart, pointEnd, 3, Brushes.Green);

                await Task.Delay(GetDelay(), cancellationToken);

                pointStart.X = x;
                pointStart.Y = y;

                await DrawFern(pointStart, length * 0.4, angle + 14 * Math.PI / 30, cancellationToken);
                await DrawFern(pointStart, length * 0.4, angle - 14 * Math.PI / 30, cancellationToken);

                await DrawFern(pointStart, length * 0.7, angle + Math.PI / 30, cancellationToken);
            }
        }
    }
}