using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ColorPicker.classes;

namespace ColorPicker
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {


        private Cursor pickerCursor;
        private Boolean autoPick;
        private DispatcherTimer timer;
        private ColorTipsWindow tipsWin;
        private int counter;
        //private MouseHook hook;
        private Point mousePoint;

        public MainWindow()
        {
            InitializeComponent();
            //初始化变量
            pickerCursor = new Cursor(new MemoryStream(Properties.Resources.picker));
            autoPick = false;
            tipsWin=new ColorTipsWindow();
            counter = 0;
            mousePoint = Picker.GetMousePoint();
           // MessageBox.Show(timer.Interval.ToString());
            //初始化事件
            this.Loaded += MainWindow_Loaded;
            this.MouseLeftButtonDown += delegate{ this.DragMove();};

            gdTutorials.PreviewMouseLeftButtonDown += delegate { gdTutorials.Visibility = Visibility.Collapsed; };

            colorBtn1.PreviewMouseLeftButtonUp += ColorBtn_OnMouseLeftButtonUp;
            colorBtn2.PreviewMouseLeftButtonUp += ColorBtn_OnMouseLeftButtonUp;
            colorBtn3.PreviewMouseLeftButtonUp += ColorBtn_OnMouseLeftButtonUp;
            colorBtn4.PreviewMouseLeftButtonUp += ColorBtn_OnMouseLeftButtonUp;
            BtnCpyCode1.Click += delegate { Clipboard.SetDataObject(TbColorCode1.Text); };
            BtnCpyCode2.Click += delegate { Clipboard.SetDataObject(TbColorCode2.Text); };

            BtnClose.Click += BtnClose_Click;
            BtnMin.Click += delegate { this.WindowState = WindowState.Minimized; };
            BtnFix.Click += BtnFix_Click;
            BtnAuto.Click += BtnAuto_Click;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += timer_Tick;
            //timer.Start();

            //hook = new MouseHook();
            //hook.MouseMoveEvent += hook_MouseMoveEvent;
            //hook.SetHook();
        }

        void hook_MouseMoveEvent(object sender, MouseEventArgs e)
        {
            this.counter = 0;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (counter==50)
            {
                Point p = Picker.GetMousePoint();
                ColorCode code = new ColorCode(Picker.GetPixelColor((int)p.X, (int)p.Y));
                tipsWin.ShowTips(code, p);
                counter = -1;
                //System.Console.WriteLine("------------"+counter);
                return;
            }
            //System.Console.WriteLine(counter);
            Point mouseCurPoint = Picker.GetMousePoint();
            if (Math.Abs(mousePoint.X - mouseCurPoint.X) < 5 && Math.Abs(mousePoint.Y - mouseCurPoint.Y) < 5)
            {
                if (counter>-1)
                    counter++;
            }
            else
            {
                counter = 0;
                mousePoint = mouseCurPoint;
            }
        }

        void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            /*if (hook!=null)
            {
                hook.UnHook();
            }*/
            timer.Stop();
            tipsWin.Close();
            Storyboard sb=new Storyboard();
            DoubleAnimation db=new DoubleAnimation(0,new Duration(TimeSpan.FromMilliseconds(300)));
            Storyboard.SetTarget(db,this);
            Storyboard.SetTargetProperty(db,new PropertyPath(Window.OpacityProperty));
            sb.Children.Add(db);
            sb.Completed += delegate { this.Close(); };
            sb.Begin();
        }

        void BtnAuto_Click(object sender, RoutedEventArgs e)
        {
            autoPick = !autoPick;
            if (autoPick)
            {
                timer.Start();
                //hook.SetHook();
                (sender as Button).ToolTip = "关闭自动获取（鼠标静止自动获取屏幕颜色）";
                ImgAuto.Source = new BitmapImage(new Uri(@"Resources/btn_auto1.png", UriKind.Relative));
            }
            else
            {
                timer.Stop();
                //hook.UnHook();
                tipsWin.Hide();
                (sender as Button).ToolTip = "启用自动获取（鼠标静止自动获取屏幕颜色）";
                ImgAuto.Source = new BitmapImage(new Uri(@"Resources/btn_auto2.png", UriKind.Relative));
            }
                
        }

        void BtnFix_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = !this.Topmost;
            if (this.Topmost)
            {
                (sender as Button).ToolTip = "取消窗口置顶";
                ImgFix.Source = new BitmapImage(new Uri(@"Resources/btn_fix1.png", UriKind.Relative));
            }
            else
            {
                (sender as Button).ToolTip = "启用窗口置顶";
                ImgFix.Source = new BitmapImage(new Uri(@"Resources/btn_fix2.png", UriKind.Relative));
            }

        }

        private void ColorBtn_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ColorRBtn rb = sender as ColorRBtn;
            ShowColorCode(rb.ColorCodeValue);
        }

        public void ShowColorCode(ColorCode color)
        {
            TbColorCode1.Text = color.GetCodeInHex();
            TbColorCode2.Text = color.GetCodeInDec();
        }
    }
}
