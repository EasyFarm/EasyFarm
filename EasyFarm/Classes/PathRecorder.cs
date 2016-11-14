using MemoryAPI;
using System;
using System.Timers;
using MemoryAPI.Navigation;

namespace EasyFarm.Classes
{
    /// <summary>
    /// Class for dealing with a character's path. 
    /// </summary>
    public class PathRecorder
    {
        /// <summary>
        /// Callback for handling position added. 
        /// </summary>
        /// <param name="position"></param>
        public delegate void PositionAdded(Position position);

        /// <summary>
        /// Fired when a position is recorded. 
        /// </summary>
        public event PositionAdded OnPositionAdded;

        /// <summary>
        ///     Recorder to record new waypoints into our path.
        /// </summary>
        private readonly Timer _recorder;

        /// <summary>
        ///     Used by the recorder to avoid duplicate, successive waypoints.
        ///     (Identicle waypoints are allowed, just not in succession.)
        /// </summary>
        private Position _lastPosition;

        /// <summary>
        /// The memory source to retrieve the character's position from. 
        /// </summary>
        private readonly IMemoryAPI _memory;

        /// <summary>
        /// Create a new <see cref="PathRecorder"/> with saving and 
        /// loading features. 
        /// </summary>
        /// <param name="memory"></param>
        public PathRecorder(IMemoryAPI memory)
        {
            _memory = memory;
            _recorder = new Timer(1000);
            _recorder.Elapsed += RouteRecorder_Tick;
        }

        /// <summary>
        /// Whether the path recorder is recording the character's path. 
        /// </summary>
        public bool IsRecording { get; set; }

        /// <summary>
        /// The recording interval. 
        /// </summary>
        public double Interval
        {
            get { return _recorder.Interval; }
            set { _recorder.Interval = value; }
        }

        /// <summary>
        /// Starts recording of character's path. 
        /// </summary>
        public void Start()
        {
            _recorder.Start();
            IsRecording = true;
        }

        /// <summary>
        /// Stops recording of character's path. 
        /// </summary>
        public void Stop()
        {
            _recorder.Stop();
            IsRecording = false;
        }        

        /// <summary>
        ///     Records a new path for the player to travel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouteRecorder_Tick(object sender, EventArgs e)
        {
            // Add a new waypoint only when we are not standing at 
            // our last position. 
            var pos = _memory.Player.Position;

            Position position = new Position()
            {
                H = pos.H,
                X = pos.X,
                Y = pos.Y,
                Z = pos.Z
            };

            AddNewPosition(position);
        }

        public void AddNewPosition(Position position)
        {
            // Update the path if we've changed out position. Rotating our heading does not
            // count as the player moving. 
            if (_lastPosition == null ||
                position.X != _lastPosition.X ||
                position.Z != _lastPosition.Z)
            {
                // Fire positon added event. 
                OnPositionAdded?.Invoke(position);
                _lastPosition = position;
            }
        }
    }
}