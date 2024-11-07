using RecursiveAlgorithms.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RecursiveAlgorithms.Pages
{
    public partial class HanoiGenerating : Page
    {
        public static int TowerHeight { get; set; }

        private static List<Move> moves = new List<Move>();

        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        public HanoiGenerating()
        {
            InitializeComponent();
            Loaded += Generating_Loaded;
        }

        private async void Generating_Loaded(object sender, RoutedEventArgs e)
        {
            moves = new List<Move>();

            try
            {
                await Task.Run(async () =>
                    await HanoiLogic.SolveHanoi(HanoiLogic.Peg.P1, HanoiLogic.Peg.P2, HanoiLogic.Peg.P3, TowerHeight, moves, cts.Token)
                    );
                HanoiPage.Moves = new ReadOnlyCollection<Move>(moves);

                _ = NavigationService.Navigate(new Uri("/Pages/Towers.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                string msg = ex is StackOverflowException
                    ? "Can't generate solution for this tower!\nTry a lower height."
                    : "Something went wrong!";

                if (!(ex is OperationCanceledException))
                {
                    _ = MessageBox.Show(Window.GetWindow(this), msg, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }

                NavigationService.GoBack();
            }

            moves = new List<Move>();
            Content = null;
            GC.Collect();
        }

        private void CancelGeneratingButton_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }
    }
}