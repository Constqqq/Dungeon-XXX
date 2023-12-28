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
using System.Windows.Shapes;
using System.Windows.Threading;



namespace Dungeon_XXX
{
    /// <summary>
    /// Логика взаимодействия для Lobby.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        private DispatcherTimer GameTimer = new DispatcherTimer();
        private bool UpkeyPressed, RightKeyPressed, DownKeyPressed, LeftKeyPressed;
        float SpeedX, SpeedY, Friction = 0.88f, Speed = 2;
        private void KeyboardDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                UpkeyPressed = true;
            }
            if (e.Key == Key.S)
            {
                DownKeyPressed = true;
            }
            if (e.Key == Key.A)
            {
                LeftKeyPressed = true;
            }
            if (e.Key == Key.D)
            {
                RightKeyPressed = true;
            }
        }

        private void KeyBoardUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                UpkeyPressed = false;
            }
            if (e.Key == Key.S)
            {
                DownKeyPressed = false;
            }
            if (e.Key == Key.A)
            {
                LeftKeyPressed = false;
            }
            if (e.Key == Key.D)
            {
                RightKeyPressed = false;
            }
        }

        public Lobby()
        {
            InitializeComponent();
            LobbyCan.Focus();

            GameTimer.Interval = TimeSpan.FromMilliseconds(16);
            GameTimer.Tick += Gametick;
            GameTimer.Start();
        }
        private void Gametick (object sender, EventArgs e)
            {
            if (UpkeyPressed)
            {
                SpeedY += Speed;
            }
            if (DownKeyPressed)
            {
                SpeedY -= Speed;
            }
            if (RightKeyPressed)
            {
                SpeedX += Speed;
            }
            if (LeftKeyPressed)
            {
                SpeedX -= Speed;
            }
            SpeedX = SpeedX * Friction;
            SpeedY = SpeedY * Friction;
            Canvas.SetLeft(Player, Canvas.GetLeft(Player) + SpeedX);
            Canvas.SetTop(Player, Canvas.GetTop(Player) - SpeedY);
        }
        
       
    }
}
