using Lift.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace Lift.UI
{
    public sealed partial class MainWindow : Window
    {
        readonly Core.Lift lift = new(1, 6, null);

        public MainWindow()
        {
            InitializeComponent();

            this.FloorStatus.Text = lift.CurrentFloor.ToString();

            lift.FloorChanged += (floor) =>
            {
                this.FloorStatus.Text = floor.ToString();
            };

            lift.DoorChanged += (door) =>
            {
                this.DoorStatus.Text = door.ToString();
            };

            lift.StatusChanged += (status) =>
            {
                this.LiftStatus.Text = GetLiftStatus(status);
            };
        }

        private async void ButtonCall_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var targetFloor = int.Parse(button?.Content.ToString() ?? lift.CurrentFloor.ToString());
            await lift.Call(targetFloor);
        }

        private async void ButtonSelectFloor_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var targetFloor = int.Parse(button?.Content.ToString() ?? lift.CurrentFloor.ToString());
            await lift.SelectFloor(targetFloor);
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
