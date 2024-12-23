﻿using RecursiveAlgorithms.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace RecursiveAlgorithms.Pages
{
    public partial class HanoiPage : Page, INotifyPropertyChanged
    {
        public HanoiPage()
        {
            Loaded += Tower_Loaded;
            InitializeComponent();
            Pegs = new List<Border>[]
            {
                P1Disks, P2Disks, P3Disks
            };
            TotalMoves = Moves.Count;
        }

        private void Tower_Loaded(object sender, RoutedEventArgs e)
        {
            // Remove Generating page from navigation history
            _ = NavigationService.RemoveBackEntry();

            double canvasWidth = TowerCanvas.ActualWidth;
            double floorWidth = canvasWidth * FloorToCanvasWidthRatio;
            double floorCanvasLeft = (canvasWidth - floorWidth) / 2;

            for (int i = 0; i < 3; i++)
            {
                Border Peg = new Border()
                {
                    Width = PegsWidth,
                    Height = PegsHeight - 5,
                    Background = RodsBrush,
                };

                _ = TowerCanvas.Children.Add(Peg);
                Canvas.SetBottom(Peg, 5);
                Canvas.SetLeft(Peg, floorCanvasLeft + (((2 * i) + 1) * floorWidth / 6) - (PegsWidth / 2));
            }

            Border Floor = new Border()
            {
                Width = floorWidth,
                Height = FloorHeight,
                Background = RodsBrush,
                BorderBrush = FloorBorderBrush,
                BorderThickness = FloorBorderThickness,
            };
            _ = TowerCanvas.Children.Add(Floor);
            Canvas.SetLeft(Floor, floorCanvasLeft);
            Canvas.SetBottom(Floor, 0);

            // Calculate needed fields
            double maxDiskWidth = floorWidth / 3;
            DisksCanvasLeft[0] = floorCanvasLeft;
            DisksCanvasLeft[1] = DisksCanvasLeft[0] + maxDiskWidth;
            DisksCanvasLeft[2] = DisksCanvasLeft[1] + maxDiskWidth;
            DisksMinBottom = FloorHeight;
            DisksMoveBottom = PegsHeight + 15;

            MovesCursor = 0;

            double MinDiskWidth = PegsWidth * 3;
            double MaxTotalDisksHeight = PegsHeight - FloorHeight - 15;

            int height = HanoiGenerating.TowerHeight;

            int disksCount = height;

            DisksHeight = Math.Min(MaxDiskHeight, MaxTotalDisksHeight / height);
            double disksWidthStep = (maxDiskWidth - MinDiskWidth) / disksCount;
            CornerRadius cornerRadius = new CornerRadius(DisksHeight / 16);

            // Add all disks to first peg if tower is standard.
            int ex = 0;
            for (int i = 0; i < disksCount; i++)
            {
                double padding = (i * disksWidthStep / 2) + 5;
                Border disk = new Border()
                {
                    Width = maxDiskWidth,
                    Height = DisksHeight,
                    Padding = new Thickness(padding, 0, padding, 0),
                    SnapsToDevicePixels = true,
                    Child = new Border()
                    {
                        CornerRadius = cornerRadius,
                        Background = DisksBackgroundBrush,
                        BorderBrush = DisksBorderBrush,
                        BorderThickness = DisksBorderThickness,
                        SnapsToDevicePixels = true
                    }
                };
                Pegs[i % 3 * ex].Add(disk);
                _ = TowerCanvas.Children.Add(disk);
            }

            RedrawTower();

            RemainingMoves = "";
        }

        private void BackToStartPageButton_Click(object sender, RoutedEventArgs e)
        {
            PlayCTS.Cancel();
            Moves = null;
            NavigationService.GoBack();
            Content = null;
            GC.Collect();
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AnimationSpeed = SpeedSlider.Value;
            AnimationTime = MaxAnimationDuration -
                    (Math.Log10(AnimationSpeed) * (MaxAnimationDuration - MinAnimationDuration) / 2);
        }

        private async void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPauseButton();

            // If CancellationToken is true, it means play isn't running
            if (PlayCTS.IsCancellationRequested)
            {
                //FastReverseButton.IsEnabled = false;
                PrevMoveButton.IsEnabled = false;
                NextMoveButton.IsEnabled = false;
                //FastForwardButton.IsEnabled = false;

                PlayCTS = new CancellationTokenSource();
                Task result = PlayAsync(PlayCTS.Token);
                try
                {
                    await result;
                }
                catch (OperationCanceledException) { }

                PlayCTS.Cancel();                       // To indicate play isn't running
                ShowPlayButton();
                MovesCursor = MovesCursor;              // Update buttons
                ControlsGrid.IsEnabled = true;
            }
            else
            {
                PlayCTS.Cancel();
                ControlsGrid.IsEnabled = false;
            }
        }

        private async void NextMoveButton_Click(object sender, RoutedEventArgs e)
        {
            ControlsGrid.IsEnabled = false;
            await PerformMoveAsync(Moves[MovesCursor++]);
            ControlsGrid.IsEnabled = true;
            RemainingMoves = "";         // To update properties binded to it
        }

        private async void PrevMoveButton_Click(object sender, RoutedEventArgs e)
        {
            ControlsGrid.IsEnabled = false;
            await PerformMoveAsync(Move.Reverse(Moves[--MovesCursor]));
            ControlsGrid.IsEnabled = true;
            RemainingMoves = "";         // To update properties binded to it
        }

        /// <summary>
        /// Changes PlayPauseButton to Play mode.
        /// </summary>
        private void ShowPlayButton()
        {
            PlayPauseButton.Content = "\u25B6";
            PlayPauseButton.ToolTip = "Play";
        }

        /// <summary>
        /// Changes PlayPauseButton to Pause mode.
        /// </summary>
        private void ShowPauseButton()
        {
            PlayPauseButton.Content = "\u23F8";
            PlayPauseButton.ToolTip = "Pause";
        }

        /// <summary>
        /// Performs moves in <c>Moves</c> list until it is stopped or reached the end.
        /// </summary>
        /// <param name="cancellationToken">a <c>CancellationToken</c> to stop the task when needed.</param>
        /// <returns>A <c>Task</c> object.</returns>
        private async Task PlayAsync(CancellationToken cancellationToken)
        {
            while (_movesCursor < TotalMoves)
            {
                await PerformMoveAsync(Moves[_movesCursor++]);
                RemainingMoves = "";         // To update properties binded to it
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Performs move of a <c>Move</c> object.
        /// </summary>
        /// <param name="move">A <c>Move</c> object.</param>
        /// <param name="visual">Determines should the move be visual.</param>
        /// <returns>A <c>Task</c> object.</returns>
        private async Task PerformMoveAsync(Move move, bool visual = true)
        {
            int srcNum = (int)move.Source;
            List<Border> src = Pegs[srcNum];
            int dstNum = (int)move.Destination;
            List<Border> dst = Pegs[dstNum];

            // Move logically
            int srcLastIndex = src.Count - 1;
            Border disk = src[srcLastIndex];
            dst.Add(src[srcLastIndex]);
            src.RemoveAt(srcLastIndex);

            if (visual)
            {
                // Move visually
                TimeSpan at = TimeSpan.FromMilliseconds(AnimationTime);

                await AnimateAsync(DisksMoveBottom, disk, Canvas.BottomProperty, at);
                await AnimateAsync(DisksCanvasLeft[dstNum], disk, Canvas.LeftProperty, at);
                await AnimateAsync(DisksMinBottom + ((dst.Count - 1) * DisksHeight), disk, Canvas.BottomProperty, at);
            }

            RemainingMoves = "";         // To update properties binded to it
        }

        /// <summary>
        /// Changes a double-typed property of an element animatedly.
        /// </summary>
        /// <param name="end">The final value of property.</param>
        /// <param name="element">The element which should be animated.</param>
        /// <param name="property">The property which should be animated. Must use double value.</param>
        /// <param name="time">The time animation should take in milliseconds.</param>
        /// <returns>A <c>Task</c> object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When <c>time</c> is less than or equal to 0.</exception>
        /// <exception cref="ArgumentException">When <c>property.PropertyType</c> isn't double.</exception>
        private async Task AnimateAsync(double end, UIElement element, DependencyProperty property, TimeSpan time)
        {
            if (time.Ticks <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (property.PropertyType.FullName != "System.Double")
            {
                throw new ArgumentException();
            }

            TaskCompletionSource<bool> acs = new TaskCompletionSource<bool>();

            Storyboard storyboard = new Storyboard()
            {
                FillBehavior = FillBehavior.Stop
            };

            DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
            _ = animation.KeyFrames.Add(new EasingDoubleKeyFrame(end, KeyTime.FromTimeSpan(time), new SineEase()));
            storyboard.Children.Add(animation);

            Storyboard.SetTarget(storyboard, element);
            Storyboard.SetTargetProperty(storyboard, new PropertyPath(property));
            storyboard.Completed += (sender, e) =>
            {
                acs.SetResult(true);
            };

            storyboard.Begin();

            _ = await acs.Task;

            element.SetValue(property, end);
        }

        /// <summary>
        /// Sets place of all disks based on <c>pegs</c> array.
        /// </summary>
        private void RedrawTower()
        {
            for (int i = 0; i < Pegs.Length; i++)
            {
                int pDisksCount = Pegs[i].Count;
                for (int j = 0; j < pDisksCount; j++)
                {
                    Border disk = Pegs[i][j];
                    Canvas.SetBottom(disk, DisksMinBottom + (j * DisksHeight));
                    Canvas.SetLeft(disk, DisksCanvasLeft[i]);
                }
            }
        }
    }
}