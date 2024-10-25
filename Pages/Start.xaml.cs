using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using RecursiveAlgorithms.Utils;
using RecursiveAlgorithms.Pages;

namespace RecursiveAlgorithms
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Page
    {
        public Start()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)StandardRadio.IsChecked)
            {
                if (!int.TryParse(DisksCountTextBox.Text, out int height) || height < 1)
                {
                    _ = MessageBox.Show(Window.GetWindow(this), "Height of Tower is invalid!", "",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Generating.TowerHeight = height;
                Generating.IsHanoi = (bool)StandardRadio.IsChecked;
            }
            else
            {
                if (!int.TryParse(FractalDepthTextBox.Text, out int depth) || depth < 1)
                {
                    _ = MessageBox.Show(Window.GetWindow(this), "Depth of Fractal is invalid!", "",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Generating.FractalDepth = depth;
            }

            _ = NavigationService.Navigate(new Uri("/Pages/Generating.xaml", UriKind.Relative));
        }

        private void DisksCountTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                GenerateButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, GenerateButton));
            }
        }
    }
}