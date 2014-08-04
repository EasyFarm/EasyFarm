using FFACETools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZeroLimits.FarmingTool
{
    public class ChatLog
    {
        private FFACE _fface;
        private Timer _timer = new Timer();
        public ObservableCollection<ChatLine> ChatLines;
        public bool IsWorking = false;

        private FarmingTools _ftools 
        {
            get { return FarmingTools.GetInstance(_fface); }
        }

        public ChatLog(FFACE fface)
        {
            this._fface = fface;
            this._timer.Interval = 100;
            this._timer.Tick += ChatLog_Tick;
            this.ChatLines = new ObservableCollection<ChatLine>();
            this._timer.Enabled = true;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void ChatLog_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < _fface.Chat.GetLineCount; i++)
			{
                var line = new ChatLine(_fface.Chat.GetNextLine(LineSettings.CleanAll));
                if (line.IsEmptyOrNull) continue;
			    this.ChatLines.Add(line);
			}
        }
    }
}