using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RecursiveAlgorithms.Utils.HanoiLogic;
using System.Windows.Controls;
using System.Windows;
using RecursiveAlgorithms.Utils;
using System.Threading;

namespace RecursiveAlgorithms.Pages
{
    public partial class Fractal : Page
    {
        public Fractal()
        {
            InitializeComponent();
        }

        private void BackToStartPageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _ = NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
            Content = null;
            GC.Collect();
        }

        private void PlayPauseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FractalCanvas.Children.Clear();
            Draw(FractalCreator._depth);
        }

        public void Draw(int depth)
        {
            double height = FractalCanvas.ActualHeight;
            double width = FractalCanvas.ActualWidth;

            Point pointStart = new Point(width / 2, 0);

            var fractal = new FractalCreator(pointStart, depth, width / 2, height);
            var lines = fractal.GetLines();

            foreach (var line in lines)
            {
                FractalCanvas.Children.Add(line);
            }
        }
    }
}
