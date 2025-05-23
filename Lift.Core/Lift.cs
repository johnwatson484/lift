namespace Lift.Core
{
    public class Lift
    {
        private int minFloor;
        private int maxFloor;
        private Door door;
        private Status status;

        public int CurrentFloor { get; private set; }

        public delegate void UpdateFloor(int floor);
        public event UpdateFloor? FloorChanged;

        public delegate void UpdateDoor(Door door);
        public event UpdateDoor? DoorChanged;

        public delegate void UpdateStatus(Status status);
        public event UpdateStatus? StatusChanged;

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

        public void OpenDoor()
        {
            if (door != Door.Open && status == Status.Stopped)
            {
                door = Door.Open;
                DoorChanged?.Invoke(door);
            }
        }

        public void CloseDoor()
        {
            if (door != Door.Closed && status == Status.Stopped)
            {
                door = Door.Closed;
                DoorChanged?.Invoke(door);
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
            status = Status.Stopped;
            StatusChanged?.Invoke(status);
        }

        private async Task MoveUp()
        {
            status = Status.MovingUp;
            StatusChanged?.Invoke(status);
            await Task.Delay(3000);
            CurrentFloor++;
            FloorChanged?.Invoke(CurrentFloor);
        }

        private async Task MoveDown()
        {
            status = Status.MovingDown;
            StatusChanged?.Invoke(status);
            await Task.Delay(3000);
            CurrentFloor--;
            FloorChanged?.Invoke(CurrentFloor);
        }

        public void SoundAlarm()
        {
            Console.WriteLine("Alarm sounded!");
        }
    }

    public enum Door
    {
        Closed,
        Open
    }

    public enum Status
    {
        Stopped,
        MovingUp,
        MovingDown
    }
}
