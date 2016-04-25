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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using WMPLib;

//Amusing Writer
namespace AmuWriter
{
    public partial class MainWindow : Window
    {
        private int playerCount = 30;
        private WindowsMediaPlayer[] players;
        private KeyboardListener _listener;
        private ContextMenu menu;

        public MainWindow()
        {
            menu = new System.Windows.Controls.ContextMenu();
            MenuItem itemExit = new MenuItem();
            itemExit.Header = "종료";
            itemExit.Click += itemExit_Click;
            menu.Items.Add(itemExit);

            InitializeComponent();
            this.StateChanged += MainWindow_StateChanged;
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        void itemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
                this.WindowState = System.Windows.WindowState.Normal;
        }

        void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
            */
            if (e.RightButton == MouseButtonState.Pressed)
            {
                menu.IsOpen = true;
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            players = new WindowsMediaPlayer[playerCount];
            for (int i = 0; i < playerCount; i++)
            {
                players[i] = new WindowsMediaPlayer();
            }

            _listener = new KeyboardListener();
            _listener.OnKeyPressed += _listener_OnKeyPressed;
            _listener.HookKeyboard();
        }

        void _listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            string soundFile = "Key_";
            soundFile += new Random().Next(3).ToString();

            switch (e.KeyPressed)
            {
                case Key.Back:
                    soundFile = "Backspace";
                    break;
                case Key.Return:
                    soundFile = "Return";
                    break;
                case Key.Space:
                    soundFile = "Spacebar";
                    break;
            }

            if (!string.IsNullOrEmpty(soundFile))
                PlaySound(AppDomain.CurrentDomain.BaseDirectory + @"KeySound\" + soundFile + ".wav");
        }

        void PlaySound(string fileURL)
        {
            for (int i = 0; i < playerCount; i++)
            {
                if (players[i].playState == WMPPlayState.wmppsPlaying ||
                    players[i].playState == WMPPlayState.wmppsBuffering)
                    continue;
                else
                {
                    players[i].URL = fileURL;
                    players[i].controls.play();
                    return;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _listener.UnHookKeyboard();
        }
    }
}
