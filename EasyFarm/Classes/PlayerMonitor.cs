using System.Threading;
using System.Threading.Tasks;
using EasyFarm.States;
using MemoryAPI;

namespace EasyFarm.Classes
{
    public class PlayerMonitor
    {
        private CancellationTokenSource _tokenSource;
        private readonly PlayerMovementTracker _movementTracker;

        public PlayerMonitor(IMemoryAPI memory)
        {
            _movementTracker = new PlayerMovementTracker(memory);
        }

        public void Start()
        {
            _tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(Monitor, _tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void Stop()
        {
            _tokenSource.Cancel();
        }

        private void Monitor()
        {
            while (true)
            {
                if (_tokenSource.IsCancellationRequested)
                {
                    _tokenSource.Token.ThrowIfCancellationRequested();
                }

                _movementTracker.RunComponent();

                TimeWaiter.Pause(100);
            }
        }
    }
}
