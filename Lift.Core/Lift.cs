using System.Drawing;
using System.Threading.Tasks;

namespace Lift.Core
{
    public class Lift
    {
        readonly int minFloor;
        readonly int maxFloor;

        readonly Queue<int> callQueue = new ();
        readonly int traversalIntervalInSeconds = 1;
        bool isTraversing = false;
        CancellationTokenSource? traversalCts;

        public int CurrentFloor { get; private set; }
        public Door Door { get; private set; }
        public Status status { get; private set; }

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
            Door = Door.Closed;
        }


        public void Activate()
        {
            traversalCts = new CancellationTokenSource();
            Task.Run(() => ActivateQueue(traversalCts.Token));
        }

        public void Shutdown()
        {
            traversalCts?.Cancel();
        }

        private async Task ActivateQueue(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (!isTraversing && callQueue.Count > 0)
                {
                    isTraversing = true;
                    await TraverseToNextFloor();
                    isTraversing = false;
                }

                await Task.Delay(traversalIntervalInSeconds * 1000, token);
            }
        }

        private async Task TraverseToNextFloor()
        {
            var floor = callQueue.Dequeue();
            await SetTargetFloor(floor);
        }

        public void Call(int floor)
        {
            AddFloorToCallQueue(floor);
        }

        public async Task SelectFloor(int floor)
        {
            AddFloorToCallQueue(floor);
            await Task.Delay(1000);
        }

        private void AddFloorToCallQueue(int floor)
        {
            if (IsValidFloor(floor) && !callQueue.Contains(floor))
            {
                callQueue.Enqueue(floor);
            }
        }

        

        private async Task SetTargetFloor(int floor)
        {
            if (floor != CurrentFloor)
            {
                CloseDoor();
                await MoveToFloor(floor);
            }

            await Task.Delay(1000);
            OpenDoor();
        }

        public async Task OpenDoor()
        {
            if (Door != Door.Open && status == Status.Stopped)
            {
                Door = Door.Open;
                DoorChanged?.Invoke(Door);
                await Task.Delay(3000);
            }
        }

        public void CloseDoor()
        {
            if (Door != Door.Closed && status == Status.Stopped)
            {
                Door = Door.Closed;
                DoorChanged?.Invoke(Door);
            }
        }

        private async Task MoveToFloor(int targetFloor)
        {
            while (CurrentFloor != targetFloor)
            {
                if(CurrentFloor < targetFloor && CurrentFloor < maxFloor)
                {
                    await MoveUp();
                }

                if(CurrentFloor > targetFloor && CurrentFloor > minFloor)
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

        private bool IsValidFloor(int floor)
        {
            return floor >= minFloor && floor <= maxFloor;
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
