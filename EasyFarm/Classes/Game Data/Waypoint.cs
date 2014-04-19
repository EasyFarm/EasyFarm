using FFACETools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyFarm.Classes
{
    public class Waypoint
    {
        public Waypoint() 
        {
            _position = new FFACE.Position();
        }

        private FFACE.Position _position;

        public Waypoint(FFACE.Position position)
        {
            _position = position;
        }

        public float X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public float Y
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public float Z
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public float H
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public FFACE.Position Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public override string ToString()
        {
            return "X: " + Position.X + "Z: " + Position.Z;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Position == (obj as Waypoint).Position;
        }
    }
}
