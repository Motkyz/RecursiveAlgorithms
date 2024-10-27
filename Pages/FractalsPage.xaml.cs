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
using System.Windows.Media;

namespace RecursiveAlgorithms.Pages
{
    public partial class FractalsPage : Page
    {
        private CancellationTokenSource _cancellationTokenSource;
        public bool IsGenerationEnabled => _cancellationTokenSource == null || _cancellationTokenSource.IsCancellationRequested;

        public FractalsPage()
        {
            InitializeComponent();
            DepthInput.Text = Fractal.Depth.ToString();
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
            Canvas.Children.Clear();

            if (!int.TryParse(DepthInput.Text, out int depth) || depth < 1)
            {
                _ = MessageBox.Show(Window.GetWindow(this), "Depth of FractalsPage is invalid!", "",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Fractal.Depth = depth;

            _cancellationTokenSource = new CancellationTokenSource();

            NotifyPropertyChanged(nameof(IsGenerationEnabled));

            try
            {
                if ((bool)Binary.IsChecked)
                {
                    await DrawBinaryTree(_cancellationTokenSource.Token);
                }
                else if ((bool)Pythagoras.IsChecked)
                {
                    await DrawPythagorasTree(_cancellationTokenSource.Token);
                }
                else
                {
                    await DrawBarnsliFern(_cancellationTokenSource.Token);
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

        public async Task DrawBinaryTree(CancellationToken cancellationToken)
        {
            var fractal = new BinaryTree(Canvas, () => (int)DelaySlider.Value);

            cancellationToken.ThrowIfCancellationRequested();

            Point pointStart = new Point(Canvas.ActualWidth / 2, 0);
            await fractal.DrawTree(pointStart, Fractal.Depth, Canvas.ActualWidth / 2, Canvas.ActualHeight, cancellationToken);
        }

        public async Task DrawPythagorasTree(CancellationToken cancellationToken)
        {
            var fractal = new PythagorasTree(Canvas, () => (int)DelaySlider.Value);

            cancellationToken.ThrowIfCancellationRequested();

            Point pointStart = new Point(Canvas.ActualWidth / 2, Canvas.ActualHeight);
            await fractal.DrawTree(pointStart, 100, -Math.PI / 2, Fractal.Depth, cancellationToken);
        }

        public async Task DrawBarnsliFern(CancellationToken cancellationToken)
        {
            var fractal = new BarnsliFern(Canvas, () => (int)DelaySlider.Value);

            cancellationToken.ThrowIfCancellationRequested();

            Point pointStart = new Point(Canvas.ActualWidth / 2, Canvas.ActualHeight);
            await fractal.DrawFern(pointStart, Fractal.Depth * 10, Math.PI / 2, cancellationToken);
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
