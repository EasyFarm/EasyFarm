using System;
using EasyFarm.Engine;
using System.Reactive.Linq;
using FFACETools;

namespace EasyFarm.Engine
{

    public class MainStream
    {
        public IObservable<GameState> stateStream { get; private set; }
        public PlayerStream playerStream { get; private set; }

        public MainStream(GameState GameState)
        {
            stateStream = Observable.Timer(TimeSpan.FromSeconds(1)).Select(playersState => GameState);
        }

        public class PlayerStream : MainStream
        {
            public PlayerStream(GameState GameState) : base(GameState) { }

            #region PlayerStream
            public IObservable<short> AttackPower
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.AttackPower);
                }
            }
            public IObservable<float> CastCountDown
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.CastCountDown);
                }
            }
            public IObservable<float> CastMax
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.CastMax);
                }
            }
            public IObservable<float> CastPercent
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.CastPercent);
                }
            }
            public IObservable<short> CastPercentEx
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.CastPercentEx);
                }
            }
            public IObservable<short> Defense
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.Defense);
                }
            }
            public IObservable<FFACE.PlayerElements> Elements
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.Elements);
                }
            }
            public IObservable<ushort> EXPForLevel
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.EXPForLevel);
                }
            }
            public IObservable<ushort> EXPIntoLevel
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.EXPIntoLevel);
                }
            }
            public IObservable<LoginStatus> GetLoginStatus
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.GetLoginStatus);
                }
            }
            public IObservable<FFACE.TRADEINFO> GetTradeWindowInformation
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.GetTradeWindowInformation);
                }
            }
            public IObservable<int> HomePoint_ID
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.HomePoint_ID);
                }
            }
            public IObservable<int> HPCurrent
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.HPCurrent);
                }
            }
            public IObservable<int> HPMax
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.HPMax);
                }
            }
            public IObservable<int> HPPCurrent
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.HPPCurrent);
                }
            }
            public IObservable<int> ID
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.ID);
                }
            }
            public IObservable<bool> IsExpMode
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.IsExpMode);
                }
            }
            public IObservable<bool> IsMeritMode
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.IsMeritMode);
                }
            }
            public IObservable<byte> LimitMode
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.LimitMode);
                }
            }
            public IObservable<ushort> LimitPoints
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.LimitPoints);
                }
            }
            public IObservable<Job> MainJob
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.MainJob);
                }
            }
            public IObservable<short> MainJobLevel
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.MainJobLevel);
                }
            }
            public IObservable<short> MeritPoints
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.MeritPoints);
                }
            }
            public IObservable<int> MPCurrent
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.MPCurrent);
                }
            }
            public IObservable<int> MPMax
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.MPMax);
                }
            }
            public IObservable<int> MPPCurrent
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.MPPCurrent);
                }
            }
            public IObservable<string> Name
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.Name);
                }
            }
            public IObservable<Nation> Nation
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.Nation);
                }
            }
            public IObservable<byte> PlayerServerID
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.PlayerServerID);
                }
            }
            public IObservable<float> PosH
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.PosH);
                }
            }
            public IObservable<FFACE.Position> Position
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.Position);
                }
            }
            public IObservable<float> PosX
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.PosX);
                }
            }
            public IObservable<float> PosY
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.PosY);
                }
            }
            public IObservable<float> PosZ
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.PosZ);
                }
            }
            public IObservable<short> Rank
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.Rank);
                }
            }
            public IObservable<short> RankPoints
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.RankPoints);
                }
            }
            public IObservable<byte> Residence_ID
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.Residence_ID);
                }
            }
            public IObservable<FFACE.PlayerStats> StatModifiers
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.StatModifiers);
                }
            }
            public IObservable<FFACE.PlayerStats> Stats
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.Stats);
                }
            }
            public IObservable<Status> Status
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.Status);
                }
            }
            public IObservable<StatusEffect[]> StatusEffects
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.StatusEffects);
                }
            }
            public IObservable<Job> SubJob
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.SubJob);
                }
            }
            public IObservable<short> SubJobLevel
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.SubJobLevel);
                }
            }
            public IObservable<bool> Synthing
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.Synthing);
                }
            }
            public IObservable<short> Title_ID
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.Title_ID);
                }
            }
            public IObservable<int> TPCurrent
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.TPCurrent);
                }
            }
            public IObservable<ViewMode> ViewMode
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.ViewMode);
                }
            }
            public IObservable<Weather> Weather
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.Weather);
                }
            }
            public IObservable<Zone> Zone
            {
                get
                {
                    return stateStream.Select(x => x.FFInstance.Instance.Player.Zone);
                }
            }
            #endregion
        }
    }
}