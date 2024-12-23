using RecursiveAlgorithms.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RecursiveAlgorithms.Pages
{
    public partial class HanoiPage
    {
        /// <summary>
        /// Maximum height of disks.
        /// </summary>
        private const double MaxDiskHeight = 50;

        /// <summary>
        /// Current height of disks.
        /// </summary>
        private double DisksHeight;

        /// <summary>
        /// Width of Pegs.
        /// </summary>
        private const double PegsWidth = 20;

        /// <summary>
        /// Height of Pegs.
        /// </summary>
        private const double PegsHeight = 260;

        /// <summary>
        /// Height of Floor.
        /// </summary>
        private const double FloorHeight = 25;

        /// <summary>
        /// Ratio of Floor width to Canvas width.
        /// </summary>
        private const double FloorToCanvasWidthRatio = 0.9;

        /// <summary>
        /// Thikness of border of disks.
        /// </summary>
        private static readonly Thickness FloorBorderThickness = new Thickness(0, 1, 0, 0);

        /// <summary>
        /// Brush of border of floor.
        /// </summary>
        private static readonly SolidColorBrush FloorBorderBrush = Brushes.Brown;

        /// <summary>
        /// Background of disks.
        /// </summary>
        private static readonly SolidColorBrush DisksBackgroundBrush =
            new SolidColorBrush(Color.FromRgb(51, 79, 150));

        private static readonly SolidColorBrush DisksBorderBrush =
            new SolidColorBrush(Color.FromRgb(51, 79, 150));

        /// <summary>
        /// Brush of pegs and floor.
        /// </summary>
        private static readonly SolidColorBrush RodsBrush =
            new SolidColorBrush(Color.FromRgb(194, 148, 78));

        /// <summary>
        /// Thikness of border of disks.
        /// </summary>
        private static readonly Thickness DisksBorderThickness = new Thickness(1, 1, 1, 0.5);

        /// <summary>
        /// <c>Canvas.Left</c> value for disks on each peg.
        /// </summary>
        private readonly double[] DisksCanvasLeft = new double[3];

        /// <summary>
        /// Minimum <c>Canvas.Bottom</c> value for disks.
        /// </summary>
        private double DisksMinBottom;

        /// <summary>
        /// <c>Canvas.Bottom</c> value for disks while moving between pegs.
        /// </summary>
        private double DisksMoveBottom;

        /// <summary>
        /// A list which contains disk moves.
        /// </summary>
        public static ReadOnlyCollection<Move> Moves { get; set; }

        /// <summary>
        /// Total moves count.
        /// </summary>
        private readonly int TotalMoves;

        private int _movesCursor;

        /// <summary>
        /// A cursor which points to current move.
        /// </summary>
        private int MovesCursor
        {
            get => _movesCursor;
            set
            {
                _movesCursor = value;

                if (value == 0)
                {
                    PrevMoveButton.IsEnabled = false;
                    PlayPauseButton.IsEnabled = true;
                    NextMoveButton.IsEnabled = true;
                }
                else if (value == TotalMoves)
                {
                    NextMoveButton.IsEnabled = false;
                    PlayPauseButton.IsEnabled = false;
                    PrevMoveButton.IsEnabled = true;
                }
                else
                {
                    PrevMoveButton.IsEnabled = true;
                    PlayPauseButton.IsEnabled = true;
                    NextMoveButton.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Represents the remaining moves.
        /// </summary>
        public string RemainingMoves
        {
            get
            {
                int rem = TotalMoves - MovesCursor;
                StringBuilder str = new StringBuilder(rem > 0 ? rem.ToString("#,0") : "No");
                _ = str.Append(" Move");
                if (rem != 1) { _ = str.Append('s'); }
                return str.ToString();
            }
            set => OnPropertyChanged();
        }

        /// <summary>
        /// Disks on peg 1.
        /// </summary>
        private readonly List<Border> P1Disks = new List<Border>();

        /// <summary>
        /// Disks on peg 2.
        /// </summary>
        private readonly List<Border> P2Disks = new List<Border>();

        /// <summary>
        /// Disks on peg 3.
        /// </summary>
        private readonly List<Border> P3Disks = new List<Border>();

        /// <summary>
        /// Array of pegs.
        /// </summary>
        private readonly List<Border>[] Pegs;

        /// <summary>
        /// Cancellation Token Source for play/pause.
        /// </summary>
        private CancellationTokenSource PlayCTS = new CancellationTokenSource(1);

        /// <summary>
        /// Maximum duration for an animation in milliseconds.
        /// </summary>
        private const double MaxAnimationDuration = 1000;

        /// <summary>
        /// Minimum duration for an animation in milliseconds.
        /// </summary>
        private const double MinAnimationDuration = 75;

        /// <summary>
        /// Current duration of animations.
        /// </summary>
        private double AnimationTime;

        private int _animationSpeed;

        /// <summary>
        /// Animation speed (percent).
        /// </summary>
        public double AnimationSpeed
        {
            get => _animationSpeed;
            set
            {
                _animationSpeed = (int)value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}