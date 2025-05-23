namespace Lift.Core
{
    public class Lift
    {
        private int minFloor;
        private int maxFloor;
        private Door door;

        public int CurrentFloor { get; set; }

        public delegate void UpdateFloor(int floor);
        public event UpdateFloor FloorChanged;

        public Lift(int minFloor, int maxFloor, int? currentFloor)
        {
            this.minFloor = minFloor;
            this.maxFloor = maxFloor;
            CurrentFloor = currentFloor ?? minFloor;
            door = Door.Closed;
        }

        public async Task Call(int floor)
        {
            await SetTargetFloor(floor);
        }

        public async Task SelectFloor(int floor)
        {
            await SetTargetFloor(floor);
        }

        private async Task SetTargetFloor(int floor)
        {
            if (floor != CurrentFloor)
            {
                CloseDoor();
                await MoveToFloor(floor);
            }

            OpenDoor();
        }

        private void OpenDoor()
        {
            if (door != Door.Open)
            {
                door = Door.Open;
            }
        }

        private void CloseDoor()
        {
            if (door != Door.Closed)
            {
                door = Door.Closed;
            }
        }

        private async Task MoveToFloor(int targetFloor)
        {
            while (CurrentFloor != targetFloor)
            {
                if(CurrentFloor < targetFloor)
                {
                    await MoveUp();
                }

                if(CurrentFloor > targetFloor)
                {
                    await MoveDown();
                }
            }
        }

        private async Task MoveUp()
        {
            await Task.Delay(5000);
            CurrentFloor++;
            FloorChanged?.Invoke(CurrentFloor);
        }

        private async Task MoveDown()
        {
            await Task.Delay(5000);
            CurrentFloor--;
            FloorChanged?.Invoke(CurrentFloor);
        }
    }

    public enum Door
    {
        Closed,
        Open
    }
}
