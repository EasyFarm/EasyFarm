
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

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