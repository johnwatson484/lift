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
        public void Call(int floor)
        {
            SetTargetFloor(floor);
        }

        public void SelectFloor(int floor)
        {
            SetTargetFloor(floor);
        }

        private void SetTargetFloor(int floor)
        {
            if (floor != CurrentFloor)
            {
                MoveToFloor(floor);
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

        private void MoveToFloor(int targetFloor)
        {
            while (CurrentFloor != targetFloor)
            {
                if(CurrentFloor < targetFloor)
                {
                    MoveUp();
                }

                if(CurrentFloor > targetFloor)
                {
                    MoveDown();
                }
            }
        }

        private void MoveUp()
        {
            CurrentFloor++;
            FloorChanged?.Invoke(CurrentFloor);
        }

        private void MoveDown()
        {
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
