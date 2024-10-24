﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using RecursiveAlgorithms.Pages;

namespace RecursiveAlgorithms.Utils
{
    public class BinaryTree
    {
        private readonly Canvas _canvas;
        private readonly int _lvls;
        private readonly Func<int> _getDelay;

        private const double maxThick = 2;

        public BinaryTree(Canvas canvas, Func<int> getDelay)
        {
            _canvas = canvas;
            _lvls = Generating.FractalDepth;
            _getDelay = getDelay;
        }

        public async Task DrawTree(Point pointStart, int depth, double width, double height, CancellationToken cancellationToken)
        {
            if (depth == 0 || cancellationToken.IsCancellationRequested) return;

            double xOffset = width / 2;
            double yOffset = height / _lvls;

            double thickness = depth * 0.5 + 0.1 > maxThick ? maxThick : depth * 0.5 + 0.1;

            Point pointEnd1 = new Point(pointStart.X - xOffset, pointStart.Y + yOffset);
            Point pointEnd2 = new Point(pointStart.X + xOffset, pointStart.Y + yOffset);

            DrawBranch(pointStart, pointEnd1, thickness);
            DrawBranch(pointStart, pointEnd2, thickness);

            await Task.Delay(_getDelay(), cancellationToken);

            await DrawTree(pointEnd1, depth - 1, width / 2, height, cancellationToken);
            await DrawTree(pointEnd2, depth - 1, width / 2, height, cancellationToken);

        }
        void DrawBranch(Point pointStart, Point pointEnd, double thickness)
        {
            Line line = new Line()
            {
                X1 = pointStart.X,
                Y1 = pointStart.Y,
                X2 = pointEnd.X,
                Y2 = pointEnd.Y,
                StrokeThickness = thickness,
                Stroke = Brushes.Red
            };

            _canvas.Children.Add(line);
        }
    }
}
