using Lift.Core;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lift.UI
{
    public sealed partial class MainWindow : Window
    {
        readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        readonly Core.Lift lift = new(1, 6, null);

        public MainWindow()
        {
            InitializeComponent();

            FloorStatus.Text = lift.CurrentFloor.ToString();

            lift.FloorChanged += (floor) =>
            {
                dispatcherQueue.TryEnqueue(() =>
                {
                    FloorStatus.Text = floor.ToString();
                });
            };

            lift.DoorChanged += (door) =>
            {
                dispatcherQueue.TryEnqueue(() =>
                {
                    DoorStatus.Text = door.ToString();

                    if (door == Door.Open)
                    {
                        ResetButtonBackground(lift.CurrentFloor);
                    }
                });
            };

            lift.StatusChanged += (status) =>
            {
                dispatcherQueue.TryEnqueue(() =>
                {
                    LiftStatus.Text = GetLiftStatus(status);
                });
            };

            lift.Activate();
        }

        private void ButtonCall_Click(object sender, RoutedEventArgs e)
        {
            HandleFloorButtonClick(sender, floor =>
            {
                lift.Call(floor);
                return Task.CompletedTask;
            });
        }

        private void ButtonSelectFloor_Click(object sender, RoutedEventArgs e)
        {
            HandleFloorButtonClick(sender, async floor =>
            {
                await lift.SelectFloor(floor);
            });
        }

        private async void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            await lift.OpenDoor();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            lift.CloseDoor();
        }

        private void ButtonAlarm_Click(object sender, RoutedEventArgs e)
        {
            lift.SoundAlarm();
        }

        private void HandleFloorButtonClick(object sender, Func<int, Task> action)
        {
            if (sender is Button button)
            {
                int targetFloor = GetTargetFloor(button);

                if (ShouldCallLift(targetFloor))
                {
                    HighlightButtonBackground(targetFloor);
                    _ = action(targetFloor); // Fire and forget or await depending on context
                }
            }
        }


        private int GetTargetFloor(Button button)
        {
            return int.Parse(button.Content.ToString() ?? lift.CurrentFloor.ToString());
        }

        private bool ShouldCallLift(int targetFloor)
        {
            return lift.CurrentFloor != targetFloor || (lift.CurrentFloor == targetFloor && lift.Door == Door.Closed);
        }

        private static string GetLiftStatus(Status status)
        {
            switch (status)
            {
                case Status.MovingUp:
                    return "Moving Up";  
                case Status.MovingDown:
                    return "Moving Down";
                default:
                    return "Stopped";
            }
        }

        private void HighlightButtonBackground(int floor)
        {
            foreach (var child in GetAllButtons(ButtonGrid))
            {
                if (child is Button button)
                {
                    if (int.TryParse(button.Content.ToString(), out int buttonFloor) && buttonFloor == floor)
                    {
                        button.Background = new SolidColorBrush(Colors.CadetBlue);
                    }
                }
            }
        }

        private void ResetButtonBackground(int floor)
        {
            foreach (var child in GetAllButtons(ButtonGrid))
            {
                if (child is Button button)
                {
                    if (int.TryParse(button.Content.ToString(), out int buttonFloor) && buttonFloor == floor)
                    {
                        button.Background = new SolidColorBrush(ColorHelper.FromArgb(0xB3, 0xFF, 0xFF, 0xFF));
                    }
                }
            }
        }

        private IEnumerable<Button> GetAllButtons(DependencyObject parent)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Button button)
                {
                    yield return button;
                }
                else
                {
                    foreach (var nestedButton in GetAllButtons(child))
                    {
                        yield return nestedButton;
                    }
                }
            }
        }
    }
}
