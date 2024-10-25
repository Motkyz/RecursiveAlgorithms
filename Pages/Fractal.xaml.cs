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
        private CancellationTokenSource _cancellationTokenSource;
        public bool IsGenerationEnabled => _cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested;

        public Fractal()
        {
            InitializeComponent();
        }

        private void BackToStartPageButton_Click(object sender, RoutedEventArgs e)
        {
            _ = NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
            Content = null;
            GC.Collect();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            FractalCanvas.Children.Clear();

            _cancellationTokenSource = new CancellationTokenSource();

            NotifyPropertyChanged(nameof(IsGenerationEnabled));

            try
            {
                if ((bool)Binary.IsChecked)
                {
                    await DrawBinary(Generating.FractalDepth, _cancellationTokenSource.Token);
                }
                else
                {
                    await DrawPythagoras(Generating.FractalDepth, _cancellationTokenSource.Token);
                }
            }

            catch (OperationCanceledException) { }

            finally
            {
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
                NotifyPropertyChanged(nameof(IsGenerationEnabled));
            }
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }

        public async Task DrawBinary(int depth, CancellationToken cancellationToken)
        {
            double height = FractalCanvas.ActualHeight;
            double width = FractalCanvas.ActualWidth;

            Point pointStart = new Point(width / 2, 0);

            var fractal = new BinaryTree(FractalCanvas, () => (int)DelaySlider.Value);

            cancellationToken.ThrowIfCancellationRequested();

            await fractal.DrawTree(pointStart, depth, width / 2, height, cancellationToken);
        }

        public async Task DrawPythagoras(int depth, CancellationToken cancellationToken)
        {
            double height = FractalCanvas.ActualHeight;
            double width = FractalCanvas.ActualWidth;

            Point pointStart = new Point(width / 2, height);

            var fractal = new PythagorasTree(FractalCanvas, () => (int)DelaySlider.Value);

            cancellationToken.ThrowIfCancellationRequested();

            await fractal.DrawTree(pointStart, -Math.PI / 2, depth, 100, cancellationToken);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(IsGenerationEnabled))
            {
                StartButton.IsEnabled = IsGenerationEnabled;
            }
        }
    }
}
