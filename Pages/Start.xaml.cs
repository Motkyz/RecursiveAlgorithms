using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using RecursiveAlgorithms.Utils;
using RecursiveAlgorithms.Pages;

namespace RecursiveAlgorithms
{
    public partial class Start : Page
    {
        public Start()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Hanoi.IsChecked == true)
            {
                if (!int.TryParse(DisksCountTextBox.Text, out int height) || height < 1)
                {
                    _ = MessageBox.Show(Window.GetWindow(this), "Height of tower is invalid!", "",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                HanoiGenerating.TowerHeight = height;

                _ = NavigationService.Navigate(new Uri("/Pages/HanoiGenerating.xaml", UriKind.Relative));
            }
            else
            {
                if (!int.TryParse(FractalDepthTextBox.Text, out int depth) || depth < 1)
                {
                    _ = MessageBox.Show(Window.GetWindow(this), "Depth of fractal is invalid!", "",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Fractal.Depth = depth;

                _ = NavigationService.Navigate(new Uri("/Pages/FractalsPage.xaml", UriKind.Relative));
            } 
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