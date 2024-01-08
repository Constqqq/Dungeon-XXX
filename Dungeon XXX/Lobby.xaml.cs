﻿using System;
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
using Dungeon_XXX;






namespace Dungeon_XXX
{
    /// <summary>
    /// Логика взаимодействия для Lobby.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        public int x = 5;
        private List<Enemy> enemies = new List<Enemy>();
        private WaveManager waveManager;
        public List<Wave> waves = new List<Wave>();
        private Enemy enemy;
        public int PlayerHealth = 100;
        private int magazineSize = 10;
        private int totalAmmo = 999999;
        private int bulletsInMagazine;
        private bool isReloading;
        private double currentBulletAngle = 0;
        private Rectangle Bullet;
        private DispatcherTimer GameTimer = new DispatcherTimer();
        private bool UpkeyPressed, RightKeyPressed, DownKeyPressed, LeftKeyPressed;
        float SpeedX, SpeedY, Friction = 0.88f, Speed = 2;

        public class Wave
        {
            public int EnemyCount { get; set; }
            public TimeSpan DelayBeforeNextWave { get; set; }

            public Wave(int enemyCount, TimeSpan delay)
            {
                EnemyCount = enemyCount;
                DelayBeforeNextWave = delay;
            }
        }

        public class WaveManager
        {
            private List<Wave> waves;
            private int currentWaveIndex = 0;
            private DateTime lastWaveEndTime = DateTime.MinValue;

            public WaveManager(List<Wave> waves)
            {
                this.waves = waves;
            }

            public void UpdateWave(Canvas lobbyCanvas, Rectangle Player, List<Rectangle> playerBullets, Canvas LobbyCan, int PlayerHealth, List<Enemy> enemies, int x)
            {
                if (currentWaveIndex < waves.Count)
                {
                    Wave currentWave = waves[currentWaveIndex];
                    DateTime waveStartTime = lastWaveEndTime + currentWave.DelayBeforeNextWave;

                    if (DateTime.Now >= waveStartTime)
                    {
                        SpawnEnemies(currentWave.EnemyCount, lobbyCanvas, Player, playerBullets, LobbyCan, PlayerHealth, enemies, x);
                        lastWaveEndTime = DateTime.Now;
                        currentWaveIndex++;
                        
                    }
                }
            }

            private void SpawnEnemies(int count, Canvas lobbyCanvas, Rectangle Player, List<Rectangle> playerBullets, Canvas LobbyCan, int PlayerHealth, List<Enemy> enemies, int x)
            {
                x++;
                Random random = new Random();

                for (int i = 0; i < count; i++)
                {
                    double startX = random.Next((int)lobbyCanvas.ActualWidth - 50);
                    double startY = random.Next((int)lobbyCanvas.ActualHeight - 50);

                    // Создаем врага и добавляем на холст
                    Enemy enemy = new Enemy(startX, startY, lobbyCanvas);
                    enemies.Add(enemy);
                    lobbyCanvas.Children.Add(enemy.EnemyRect);
                    enemy.UpdateBullets(LobbyCan, Player, PlayerHealth);
                    enemy.MoveTowardsPlayer(Player, playerBullets);
                }
            }
        }

        public Lobby()
        

        {

            InitializeComponent();
            
            LobbyCan.Focus();
            LobbyCan.IsHitTestVisible = true;
            
            GameTimer.Interval = TimeSpan.FromMilliseconds(16);
            GameTimer.Tick += Gametick;
            GameTimer.Start();
            InitializeBullet();
            InitializeAmmo();
            this.MouseDown += LobbyCan_MouseDown;
            //InitializeEnemy(LobbyCan);
            List<Wave> waves = new List<Wave>
            {
                new Wave (1, TimeSpan.FromSeconds(0)),
                new Wave (5, TimeSpan.FromSeconds(20)),
                 new Wave (7, TimeSpan.FromSeconds(20)),
                  new Wave (8, TimeSpan.FromSeconds(20)),
                   new Wave (9, TimeSpan.FromSeconds(20)),
                    new Wave (10, TimeSpan.FromSeconds(20)),
                     new Wave (11, TimeSpan.FromSeconds(20)),
                      new Wave (12, TimeSpan.FromSeconds(20)),
                       new Wave (13, TimeSpan.FromSeconds(20)),
                        new Wave (14, TimeSpan.FromSeconds(20)),
                         new Wave (15, TimeSpan.FromSeconds(20)),
                          new Wave (x, TimeSpan.FromSeconds(20)),
                           new Wave (x, TimeSpan.FromSeconds(20)),
                            new Wave (x, TimeSpan.FromSeconds(20)),
                             new Wave (x, TimeSpan.FromSeconds(20)),
                              new Wave (x, TimeSpan.FromSeconds(20)),
                               new Wave (x, TimeSpan.FromSeconds(20)),
                                new Wave (x, TimeSpan.FromSeconds(20)),
                                 new Wave (x, TimeSpan.FromSeconds(20)),
                                  new Wave (x, TimeSpan.FromSeconds(20)),
                                   new Wave (x, TimeSpan.FromSeconds(20)),
                                    new Wave (x, TimeSpan.FromSeconds(20)),
                                     new Wave (x, TimeSpan.FromSeconds(20)),
                                      new Wave (x, TimeSpan.FromSeconds(20)),
                                       new Wave (x, TimeSpan.FromSeconds(20)),
                                        new Wave (x, TimeSpan.FromSeconds(20)),
                                         new Wave (x, TimeSpan.FromSeconds(20)),
                                          new Wave (x, TimeSpan.FromSeconds(20)),
                                           new Wave (x, TimeSpan.FromSeconds(20)),
                                            new Wave (x, TimeSpan.FromSeconds(20)),
                                             new Wave (x, TimeSpan.FromSeconds(20)),
                                              new Wave (x, TimeSpan.FromSeconds(20)),
                                               new Wave (x, TimeSpan.FromSeconds(20)),
                                                new Wave (x, TimeSpan.FromSeconds(20)),
                                                 new Wave (x, TimeSpan.FromSeconds(20)),
                                                  new Wave (x, TimeSpan.FromSeconds(20)),
                                                   new Wave (x, TimeSpan.FromSeconds(20)),
                                                    new Wave (x, TimeSpan.FromSeconds(20)),
                                                     new Wave (x, TimeSpan.FromSeconds(20)),
                                                      new Wave (x, TimeSpan.FromSeconds(20)),
                                                       new Wave (x, TimeSpan.FromSeconds(20)),
                                                        new Wave (x, TimeSpan.FromSeconds(20)),
                                                         new Wave (x, TimeSpan.FromSeconds(20)),
                                                          new Wave (x, TimeSpan.FromSeconds(20)),
                                                           new Wave (x, TimeSpan.FromSeconds(20)),
                                                            new Wave (x, TimeSpan.FromSeconds(20)),
                                                             new Wave (x, TimeSpan.FromSeconds(20)),
                                                              new Wave (x, TimeSpan.FromSeconds(20)),
                                                               new Wave (x, TimeSpan.FromSeconds(20)),
                                                                new Wave (x, TimeSpan.FromSeconds(20)),
                                                                 new Wave (x, TimeSpan.FromSeconds(20)),
                                                                  new Wave (x, TimeSpan.FromSeconds(20)),
                                                                   new Wave (x, TimeSpan.FromSeconds(20)),
                                                                    new Wave (x, TimeSpan.FromSeconds(20)),
                                                                     new Wave (x, TimeSpan.FromSeconds(20)),
                                                                      new Wave (x, TimeSpan.FromSeconds(20)),
                                                                       new Wave (x, TimeSpan.FromSeconds(20)),
                                                                        new Wave (x, TimeSpan.FromSeconds(20)),
                                                                         new Wave (x, TimeSpan.FromSeconds(20)),
                                                                          new Wave (x, TimeSpan.FromSeconds(20)),
                                                                           new Wave (x, TimeSpan.FromSeconds(20)),
                                                                            new Wave (x, TimeSpan.FromSeconds(20)),
                                                                             new Wave (x, TimeSpan.FromSeconds(20)),
                                                                              new Wave (x, TimeSpan.FromSeconds(20)),
                                                                               new Wave (x, TimeSpan.FromSeconds(20)),
                                                                                new Wave (x, TimeSpan.FromSeconds(20)),
                                                                                 new Wave (x, TimeSpan.FromSeconds(20)),
                                                                                  new Wave (x, TimeSpan.FromSeconds(20)),
                                                                                   new Wave (x, TimeSpan.FromSeconds(20)),
                                                                                    new Wave (x, TimeSpan.FromSeconds(20)),
                                                                                     new Wave (x, TimeSpan.FromSeconds(20)),
                                                                                      new Wave (x, TimeSpan.FromSeconds(20)),
                                                                                       new Wave (x, TimeSpan.FromSeconds(20)),
                                                                                        new Wave (x, TimeSpan.FromSeconds(20)),
                                                                                         new Wave (x, TimeSpan.FromSeconds(20)),

            };

            waveManager = new WaveManager(waves);
           
        }
        public List<Rectangle> playerBullets = new List<Rectangle>();

        private void InitializeEnemy(Canvas LobbyCan)
        {
            enemy = new Enemy(50, 50, LobbyCan);
            LobbyCan.Children.Add(enemy.EnemyRect);
        }

       public Image playerImage = new Image()
        {
            Source = new BitmapImage(new Uri (@"dead\MestoPravo.png",UriKind.Relative)),
           Width = 50,
           Height = 50,

        };


     



        private void InitializeAmmo()
        {
            bulletsInMagazine = magazineSize;
            totalAmmo = 999999;
            isReloading = false;

            UpdateAmmoInfo();  // Добавлен вызов для обновления информации о патронах
        }

        private void UpdateAmmoInfo()
        {
            AmmoInfo.Text = $"Ammo: {totalAmmo}";
            MagazineInfo.Text = $"Magazine: {bulletsInMagazine}/{magazineSize}";
        }


        private void LobbyCan_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(LobbyCan);

            currentBulletAngle = Math.Atan2(mousePosition.Y - (Canvas.GetTop(Player) + Player.Height / 2),
                                             mousePosition.X - (Canvas.GetLeft(Player) + Player.Width / 2));
        }


        private void LobbyCan_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !isReloading)
            {
                if (bulletsInMagazine > 0)
                {
                    Point mousePosition = e.GetPosition(LobbyCan);
                    double angleRad = Math.Atan2(mousePosition.Y - (Canvas.GetTop(Player) + Player.Height / 2),
                                                 mousePosition.X - (Canvas.GetLeft(Player) + Player.Width / 2));

                    CreateBullet(angleRad);
                    bulletsInMagazine--;

                    if (bulletsInMagazine == 0)
                    {
                        StartReload();
                    }

                    UpdateAmmoInfo();  // Добавлен вызов для обновления информации о патронах после выстрела
                }
                else if (totalAmmo > 0)
                {
                    StartReload();
                    UpdateAmmoInfo();  // Добавлен вызов для обновления информации о патронах после начала перезарядки
                }
            }
        }


        private void StartReload()
        {
            if (!isReloading && totalAmmo > 0 && bulletsInMagazine < magazineSize)
            {
                isReloading = true;

                var reloadTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
                reloadTimer.Tick += (sender, args) =>
                {
                    int bulletsToReload = Math.Min(magazineSize - bulletsInMagazine, totalAmmo);
                    bulletsInMagazine += bulletsToReload;
                    totalAmmo -= bulletsToReload;
                    isReloading = false;

                    reloadTimer.Stop();
                    UpdateAmmoInfo();  // Добавлен вызов для обновления информации о патронах после перезарядки
                };

                reloadTimer.Start();
            }
        }
        private void CreateBullet(double angleRad)
        {
            Rectangle bullet = new Rectangle
            {
                Tag ="MyBullet",
                Width = 10,
                Height = 20,
                Fill = Brushes.Red,
                Visibility = Visibility.Visible
            };


            double startX = Canvas.GetLeft(Player) + Player.Width / 2 - bullet.Width / 2;
            double startY = Canvas.GetTop(Player) + Player.Height / 2 - bullet.Height / 2;

            Canvas.SetLeft(bullet, startX);
            Canvas.SetTop(bullet, startY);

            LobbyCan.Children.Add(bullet);
            playerBullets.Add(bullet);

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(20) };
            timer.Tick += (sender, args) =>
            {
                MoveBullet(bullet, angleRad);
            };

            timer.Start();
        }

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
            if (e.Key == Key.R)
            {
                StartReload();
            }

        }



        private void MoveBullet(Rectangle bullet, double angleRad)
        {

            double bulletSpeed = 7;

            double deltaX = bulletSpeed * Math.Cos(angleRad);
            double deltaY = bulletSpeed * Math.Sin(angleRad);

            double newLeft = Canvas.GetLeft(bullet) + deltaX;
            double newTop = Canvas.GetTop(bullet) + deltaY;

            // Проверка столкновения с объектами, имеющими тег "Collide"
            foreach (var x in LobbyCan.Children.OfType<Rectangle>().Where(obj => (string)obj.Tag == "Collide"))
            {
                
                Rect bulletRect = new Rect(newLeft, newTop, bullet.Width, bullet.Height);
                Rect collideRect = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                if (bulletRect.IntersectsWith(collideRect))
                {
                    // Удалить пулю
                    LobbyCan.Children.Remove(bullet);
                    playerBullets.Remove(bullet);
                    return;
                }
               
            }

            Canvas.SetLeft(bullet, newLeft);
            Canvas.SetTop(bullet, newTop);
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

        

        private void InitializeBullet()
        {
            Bullet = new Rectangle
            {
                Width = 10,
                Height = 20,
                Fill = Brushes.Red,
                Visibility = Visibility.Hidden,
                Tag = "Bullet"
            };

            LobbyCan.Children.Add(Bullet);
        }


        


        private void Gametick(object sender, EventArgs e)
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
            Collide("x");
            Canvas.SetTop(Player, Canvas.GetTop(Player) - SpeedY);
            Collide("y");
            waveManager.UpdateWave(LobbyCan, Player,  playerBullets, LobbyCan, PlayerHealth, enemies, x);

            foreach (var enemy in enemies)
            {
                enemy.MoveTowardsPlayer(Player, playerBullets);
                enemy.UpdateBullets(LobbyCan, Player, PlayerHealth);
            }

            


        }





        private void Collide(string Dir)
        {
        foreach (var x in LobbyCan.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "Collide")
                {
                    Rect PlayerHB = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);
                    Rect ToCollide = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                         if (PlayerHB.IntersectsWith(ToCollide))
                    {
                        if (Dir == "x")
                        {
                            Canvas.SetLeft(Player, Canvas.GetLeft(Player) - SpeedX);
                            SpeedX = 0;
                        }
                        else
                        {
                            Canvas.SetTop(Player, Canvas.GetTop(Player) + SpeedY);
                            SpeedY = 0;
                        }


                    }
                }

                
            }
        }

       


    }
}
