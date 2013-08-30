using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using FFACETools;

namespace EasyFarm.PathingTools
{
    public class RecoveryPath
    {        
        FFACETools.FFACE Session;
        public List<InteractionPoint> Path = new List<InteractionPoint>();

        public RecoveryPath()
        {

        }

        public RecoveryPath(FFACETools.FFACE session)
        {
            Session = session;
        }

        public void Play()
        {
            foreach ( var Point in Path )
            {
                Session.Navigator.Goto( Point.Position, false );

                if ( !string.IsNullOrWhiteSpace( Point.Target.ToString() ) )
                {
                    while ( Session.Target.Name.Equals( Point.Target.ToString() ) )
                    {
                        Session.Navigator.FaceHeading( Point.Position );
                        System.Threading.Thread.Sleep( 50 );
                        Session.Windower.SendKeyPress( FFACETools.KeyCode.TabKey );
                        System.Threading.Thread.Sleep( 50 );
                    }
                }

                if ( !Point.Keys.Count.Equals( 0 ) )
                {
                    foreach ( var key in Point.Keys )
                    {
                        Session.Windower.SendKeyPress( key );
                        System.Threading.Thread.Sleep( 1000 );
                    }
                }
            }
        }
    }

    public class InteractionPoint
    {
        public InteractionPoint()
        {

        }

        public FFACE.Position Position = new FFACE.Position();
        public List<KeyCode> Keys = new List<KeyCode>();
        public StringBuilder Target = new StringBuilder();

        public InteractionPoint(FFACETools.FFACE.Position position, List<FFACETools.KeyCode> keys, string target)
        {
            Position = position;
            Keys = keys;
            Target.Clear().Append(target);
        }
    }
}
