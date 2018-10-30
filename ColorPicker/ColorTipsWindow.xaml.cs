using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ColorPicker.classes;

namespace ColorPicker
{
    /// <summary>
    /// ColorTipsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ColorTipsWindow : Window
    {
        private Boolean isFix = false;
        private Boolean isPause = false;
        public ColorTipsWindow()
        {
            InitializeComponent();
            this.PreviewMouseRightButtonUp += delegate { this.Hide(); };
            this.PreviewMouseDoubleClick += ColorTipsWindow_PreviewMouseDoubleClick;
            this.PreviewMouseLeftButtonDown+=delegate{this.DragMove();};
            this.MouseEnter += delegate { isPause = true; };
            this.MouseLeave += delegate { isPause = false; };
        }

        void ColorTipsWindow_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            isFix = !isFix;
            if (isFix)
                bd.BorderBrush=new SolidColorBrush(Color.FromRgb(0xff,0xff,0xff));
            else
            bd.BorderBrush=new SolidColorBrush(Color.FromRgb(0x00,0x00,0x00));
        }

        public void ShowTips(ColorCode code, Point point)
        {
            if(isPause)return;
            if (!isFix)
            {
                this.Top = point.Y - 20;
                this.Left = point.X;
            }
            TbHex.Text = code.GetCodeInHex();
            rect.Fill = new SolidColorBrush(code.GetColor());
            this.Show();
            Storyboard sb=new Storyboard();
            DoubleAnimation da = new DoubleAnimation(0,1, TimeSpan.FromSeconds(0.3));
            Storyboard.SetTarget(da,this);
            Storyboard.SetTargetProperty(da, new PropertyPath(Window.OpacityProperty));
            sb.Children.Add(da);
            sb.Begin();
        }
    }
}
