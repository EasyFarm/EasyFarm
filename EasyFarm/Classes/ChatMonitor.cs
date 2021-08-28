using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyFarm.UserSettings;
using MemoryAPI;
using EasyFarm.ViewModels;


namespace EasyFarm.Classes
{
    public class ChatMonitor 
    {
        private CancellationTokenSource _tokenSource;
        private static readonly object LockObject = new object();

        private readonly IMemoryAPI _fface;

        private int _nmKillCount = 0;
        private int _allKills = 0;
        public int KillCount { get { return _nmKillCount; } }
        public int AllKills { get { return _allKills; } }


        public ChatMonitor(IMemoryAPI api)
        {
            _fface = api;
        }

        public void Watch()
        {
            _tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                using (_tokenSource.Token.Register(Kill(Thread.CurrentThread)))
                {
                    while(true)
                    {
                        try
                        {

                            foreach (var entry in _fface.Chat.ChatEntries)
                            {
                                if (entry.Text.Contains(string.Format("{0} defeats", _fface.Player.Name)))
                                    _allKills++;
                                    
                                if (entry.Text.Contains(string.Format("{0} defeats {1}", _fface.Player.Name, Config.Instance.NotoriousMonsterName)))
                                {
                                    _nmKillCount++;
                                    LogViewModel.Write(string.Format("{0} killed for a total of {1}", Config.Instance.NotoriousMonsterName, _nmKillCount));
                                }
                                    
                            }
                        }
                        catch { }
                        Thread.Sleep(100);
                    }
                    
                }
            }, _tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        }

        public void Stop()
        {
            if (_tokenSource != null)
            {
                _tokenSource.Cancel();
            }
        }

        public Action Kill(Thread thread)
        {
            return () =>
            {
                if (!thread.Join(500)) thread.Interrupt();
            };
        }
    }
}
