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

        private async void PlayPauseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FractalCanvas.Children.Clear();

            if ((bool)Binary.IsChecked)
            {
                await DrawBinary(Generating.FractalDepth);
            }
            else
            {
                await DrawPythagoras(Generating.FractalDepth);
            }
        }

        public async Task DrawBinary(int depth)
        {
            double height = FractalCanvas.ActualHeight;
            double width = FractalCanvas.ActualWidth;

            Point pointStart = new Point(width / 2, 0);

            var fractal = new BinaryTree(FractalCanvas);
            await fractal.DrawTree(pointStart, depth, width/2, height);
        }

        public async Task DrawPythagoras(int depth)
        {
            double height = FractalCanvas.ActualHeight;
            double width = FractalCanvas.ActualWidth;

            Point pointStart = new Point(width / 2, height);

            var fractal = new PythagorasTree(FractalCanvas);
            await fractal.DrawTree(pointStart, -Math.PI / 2, depth, 100);
        }
    }
}
