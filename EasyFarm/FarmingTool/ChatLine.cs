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
