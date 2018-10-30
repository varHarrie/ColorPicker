using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ColorPicker.classes
{
    public class ColorRBtn:RadioButton
    {
        public static Cursor pickerCursor = new Cursor(new MemoryStream(Properties.Resources.picker));
        private ColorCode colorCode;

        public Color ColorValue
        {
            get { return colorCode.GetColor(); }
            set
            {
                this.colorCode.SetColor(value);
                this.Background=new SolidColorBrush(colorCode.GetColor());
            }
        }

        public ColorCode ColorCodeValue { get { return colorCode; } }

        public ColorRBtn()
        {
            this.ToolTip = " ";
            this.colorCode=new ColorCode();
            this.Cursor = pickerCursor;
            this.PreviewMouseLeftButtonUp += ColorRBtn_PreviewMouseLeftButtonUp;
            this.PreviewMouseRightButtonUp += ColorRBtn_PreviewMouseRightButtonUp;
            this.ToolTipOpening += ColorRBtn_ToolTipOpening;
        }

        void ColorRBtn_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            this.ToolTip = this.ColorCodeValue.GetCodeInHex() + "\n" + this.ColorCodeValue.GetCodeInDec();
        }

        void ColorRBtn_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetDataObject(this.ColorCodeValue.GetCodeInHex());
        }

        void ColorRBtn_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement src = e.Source as FrameworkElement;
            Point p = Picker.GetMousePoint(src);
            this.ColorValue = Picker.GetPixelColor((int)p.X, (int)p.Y);
            this.IsChecked = true;
        }
    }
}
