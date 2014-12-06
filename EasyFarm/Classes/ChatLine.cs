
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ZeroLimits.FarmingTool
{
    public class ChatLine
    {
        public ChatLine(FFACE.ChatTools.ChatLine line)
        {
            if (line == null)
            {
                IsEmptyOrNull = true;
                return;
            }
            this.Line = line;

            var argb = System.Windows.Media.Color.FromArgb(line.Color.A, line.Color.R, line.Color.G, line.Color.B);

            this.Color = new SolidColorBrush(argb);

            this.Text = line.Text;
        }

        public FFACE.ChatTools.ChatLine Line { get; set; }

        public Brush Color { get; set; }

        public bool IsEmptyOrNull { get; set; }

        public String Text { get; set; }
    }
}
