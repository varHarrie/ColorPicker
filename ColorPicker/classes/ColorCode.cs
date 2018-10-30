using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorPicker.classes
{
    public class ColorCode
    {
        private Color color;

        public ColorCode() { this.color=Color.FromRgb(0x00,0x00,0x00);}
        public ColorCode(Color c)
        {
            color = c;
        }

        public Color GetColor()
        {
            return color;
        }

        public void SetColor(Color c)
        {
            this.color = c;
        }

        public String GetCodeInHex()
        {;
            return "#" + color.R.ToString("X2")+color.G.ToString("X2")+color.B.ToString("X2");
        }

        public String GetCodeInDec()
        {
            return "rgb(" + Convert.ToInt32(color.R) + "," + Convert.ToInt32(color.G) + "," + Convert.ToInt32(color.B) + ")";
        }
    }
}
