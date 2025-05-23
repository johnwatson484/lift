using Lift.Core;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

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
            var button = sender as Button;
            var targetFloor = int.Parse(button?.Content.ToString() ?? lift.CurrentFloor.ToString());
            lift.Call(targetFloor);
        }

        private void ButtonSelectFloor_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var targetFloor = int.Parse(button?.Content.ToString() ?? lift.CurrentFloor.ToString());
            lift.SelectFloor(targetFloor);
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            lift.OpenDoor();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            lift.CloseDoor();
        }

        private void ButtonAlarm_Click(object sender, RoutedEventArgs e)
        {
            lift.SoundAlarm();
        }

        private string GetLiftStatus(Status status)
        {
            switch(status)
            {
                case Status.MovingUp:
                    return "Moving Up";  
                case Status.MovingDown:
                    return "Moving Down";
                default:
                    return "Stopped";
            }
        }
    }
}
