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
using Dungeon_XXX;






namespace Dungeon_XXX
{
    /// <summary>
    /// Логика взаимодействия для Lobby.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        private Bonus bonus;
        private bool bonusChoiseInProgress = false;
        private bool waveInProgress = false;
        private Bonus currentBonus;
        public int x = 5;
        private List<Enemy> enemies = new List<Enemy>();
        private WaveManager waveManager;
        public List<Wave> waves = new List<Wave>();
        private Enemy enemy;
        public int PlayerHealth = 6;
        public int magazineSize = 10;
        private int totalAmmo = 999999;
        private int bulletsInMagazine;
        private bool isReloading;
        private double currentBulletAngle = 0;
        private Image Bullet;
        private DispatcherTimer bonusTimer = new DispatcherTimer();
        private DispatcherTimer GameTimer = new DispatcherTimer();
        private bool UpkeyPressed, RightKeyPressed, DownKeyPressed, LeftKeyPressed;
        float SpeedX, SpeedY, Friction = 0.88f, Speed = 1;

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
          //  private bool waveInProgress = false;

            public WaveManager(List<Wave> waves)
            {
                this.waves = waves;
               
                
            }

            
            public void UpdateWave(Canvas lobbyCanvas, Image Player, List<Image> playerBullets, Canvas LobbyCan, int PlayerHealth, List<Enemy> enemies, int x)
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

            private void SpawnEnemies(int count, Canvas lobbyCanvas, Image Player, List<Image> playerBullets, Canvas LobbyCan, int PlayerHealth, List<Enemy> enemies, int x)
            {
                x++;
                Random random = new Random();

                for (int i = 0; i < count; i++)
                {
                    double startX;
                    double startY;

                    // Генерируем случайные координаты за пределами холста
                    if (random.Next(2) == 0)
                    {
                        startX = random.Next((int)lobbyCanvas.ActualWidth, (int)lobbyCanvas.ActualWidth + 50);
                        startY = random.NextDouble() * lobbyCanvas.ActualHeight;
                    }
                    else
                    {
                        startX = random.NextDouble() * lobbyCanvas.ActualWidth;
                        startY = random.Next((int)lobbyCanvas.ActualHeight, (int)lobbyCanvas.ActualHeight + 50);
                    }

                    // Создаем врага и добавляем на холст
                    Enemy enemy = new Enemy(startX, startY, lobbyCanvas);
                    enemies.Add(enemy);
                    lobbyCanvas.Children.Add(enemy.EnemyRect);
                    enemy.UpdateBullets(LobbyCan, Player, ref PlayerHealth);
                    enemy.MoveTowardsPlayer(Player, playerBullets);
                }
            }
        }
            public Lobby()
        

        {

            InitializeComponent();

            bonusTimer = new DispatcherTimer();
            bonusTimer.Interval = TimeSpan.FromSeconds(45); // Интервал в 45 секунд
            bonusTimer.Tick += BonusTimer_Tick;
            bonusTimer.Start();
            LobbyCan.Focus();
            LobbyCan.IsHitTestVisible = true;
            InitializeBonus();
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
                new Wave (5, TimeSpan.FromSeconds(10)),
                 new Wave (7, TimeSpan.FromSeconds(20)),
                  new Wave (8, TimeSpan.FromSeconds(20)),
                   new Wave (9, TimeSpan.FromSeconds(20)),
                    new Wave (10, TimeSpan.FromSeconds(20)),
                     new Wave (11, TimeSpan.FromSeconds(20)),
                      new Wave (12, TimeSpan.FromSeconds(20)),
                       new Wave (13, TimeSpan.FromSeconds(20)),
                        new Wave (14, TimeSpan.FromSeconds(20)),
                         new Wave (15, TimeSpan.FromSeconds(20)),
                          new Wave (15, TimeSpan.FromSeconds(20)),
                           new Wave (15, TimeSpan.FromSeconds(20)),
                            new Wave (15, TimeSpan.FromSeconds(20)),
                             new Wave (15, TimeSpan.FromSeconds(20)),
                              new Wave (15, TimeSpan.FromSeconds(20)),
                               new Wave (15, TimeSpan.FromSeconds(20)),
                                new Wave (15, TimeSpan.FromSeconds(20)),
                                 new Wave (15, TimeSpan.FromSeconds(20)),
                                  new Wave (15, TimeSpan.FromSeconds(20)),
                                   new Wave (15, TimeSpan.FromSeconds(20)),
                                    new Wave (15, TimeSpan.FromSeconds(20)),
                                     new Wave (15, TimeSpan.FromSeconds(20)),
                                      new Wave (15, TimeSpan.FromSeconds(20)),
                                       new Wave (15, TimeSpan.FromSeconds(20)),
                                        new Wave (15, TimeSpan.FromSeconds(20)),
                                         new Wave (15, TimeSpan.FromSeconds(20)),
                                          new Wave (15, TimeSpan.FromSeconds(20)),
                                           new Wave (15, TimeSpan.FromSeconds(20)),
                                            new Wave (15, TimeSpan.FromSeconds(20)),
                                             new Wave (15, TimeSpan.FromSeconds(20)),
                                              new Wave (15, TimeSpan.FromSeconds(20)),
                                               new Wave (15, TimeSpan.FromSeconds(20)),
                                                new Wave (15, TimeSpan.FromSeconds(20)),
                                                 new Wave (15, TimeSpan.FromSeconds(20)),
                                                  new Wave (15, TimeSpan.FromSeconds(20)),
                                                   new Wave (15, TimeSpan.FromSeconds(20)),
                                                    new Wave (15, TimeSpan.FromSeconds(20)),
                                                     new Wave (15, TimeSpan.FromSeconds(20)),
                                                      new Wave (15, TimeSpan.FromSeconds(20)),
                                                       new Wave (15, TimeSpan.FromSeconds(20)),
                                                        new Wave (15, TimeSpan.FromSeconds(20)),
                                                         new Wave (15, TimeSpan.FromSeconds(20)),
                                                          new Wave (15, TimeSpan.FromSeconds(20)),
                                                           new Wave (15, TimeSpan.FromSeconds(20)),
                                                            new Wave (15, TimeSpan.FromSeconds(20)),
                                                             new Wave (15, TimeSpan.FromSeconds(20)),
                                                              new Wave (15, TimeSpan.FromSeconds(20)),
                                                               new Wave (15, TimeSpan.FromSeconds(20)),
                                                                new Wave (15, TimeSpan.FromSeconds(20)),
                                                                 new Wave (15, TimeSpan.FromSeconds(20)),
                                                                  new Wave (15, TimeSpan.FromSeconds(20)),
                                                                   new Wave (15, TimeSpan.FromSeconds(20)),
                                                                    new Wave (15, TimeSpan.FromSeconds(20)),
                                                                     new Wave (15, TimeSpan.FromSeconds(20)),
                                                                      new Wave (15, TimeSpan.FromSeconds(20)),
                                                                       new Wave (15, TimeSpan.FromSeconds(20)),
                                                                        new Wave (15, TimeSpan.FromSeconds(20)),
                                                                         new Wave (15, TimeSpan.FromSeconds(20)),
                                                                          new Wave (15, TimeSpan.FromSeconds(20)),
                                                                           new Wave (15, TimeSpan.FromSeconds(20)),
                                                                            new Wave (15, TimeSpan.FromSeconds(20)),
                                                                             new Wave (15, TimeSpan.FromSeconds(20)),
                                                                              new Wave (15, TimeSpan.FromSeconds(20)),
                                                                               new Wave (15, TimeSpan.FromSeconds(20)),
                                                                                new Wave (15, TimeSpan.FromSeconds(20)),
                                                                                 new Wave (15, TimeSpan.FromSeconds(20)),
                                                                                  new Wave (15, TimeSpan.FromSeconds(20)),
                                                                                   new Wave (15, TimeSpan.FromSeconds(20)),
                                                                                    new Wave (15, TimeSpan.FromSeconds(20)),
                                                                                     new Wave (15, TimeSpan.FromSeconds(20)),
                                                                                      new Wave (15, TimeSpan.FromSeconds(20)),
                                                                                       new Wave (15, TimeSpan.FromSeconds(20)),
                                                                                        new Wave (15, TimeSpan.FromSeconds(20)),
                                                                                         new Wave (15, TimeSpan.FromSeconds(20)),

            };

            waveManager = new WaveManager(waves);
           
        }
        public List<Image> playerBullets = new List<Image>();

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

  /*    public void HpBar(int PlayerHealth)
        {
            if (PlayerHealth == 6)
            {
                Hbar.Source = new BitmapImage(new Uri(@"dead\3.png", UriKind.Relative));
            }
           else  if (PlayerHealth == 5)
            {
                Hbar.Source = new BitmapImage(new Uri(@"dead\25.png", UriKind.Relative));
            }
            else if (PlayerHealth == 4)
            {
                Hbar.Source = new BitmapImage(new Uri(@"dead\2.png", UriKind.Relative));
            }
            else if (PlayerHealth == 3)
            {
                Hbar.Source = new BitmapImage(new Uri(@"dead\15.png", UriKind.Relative));
            }
            else if (PlayerHealth == 2)
            {
                Hbar.Source = new BitmapImage(new Uri(@"dead\1.png", UriKind.Relative));
            }
            else if (PlayerHealth == 1)
            {
                Hbar.Source = new BitmapImage(new Uri(@"dead\05.png", UriKind.Relative));
            }
            else if (PlayerHealth == 0)
            {
                Hbar.Source = new BitmapImage(new Uri(@"dead\0.png", UriKind.Relative));
            }
        }
     */



        private void InitializeAmmo()
        {
            bulletsInMagazine = magazineSize;
            totalAmmo = 999999;
            isReloading = false;

            UpdateAmmoInfo();  // Добавлен вызов для обновления информации о патронах
        }

        private void UpdateAmmoInfo()
        {
            
            MagazineInfo.Text = $" {bulletsInMagazine}/{magazineSize}";
        }
        private void UpdateHpinfo()
        {
            
            Hepe.Text = $" {PlayerHealth}";
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
                 //   Player.Source = BitmapFrame.Create(new Uri(@"dead\Udar.png", UriKind.Relative));
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
            Image bullet = new Image
            {
                Tag ="MyBullet",
                Width = 10,
                Height = 20,
                Source = BitmapFrame.Create(new Uri(@"dead\MyBullet.png", UriKind.Relative)),
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
                Player.Source = BitmapFrame.Create(new Uri(@"dead\MestLevo.png", UriKind.Relative));
            }
            if (e.Key == Key.D)
            {
                RightKeyPressed = true;
                Player.Source = BitmapFrame.Create(new Uri(@"dead\MestPravo.png", UriKind.Relative));
            }
            if (e.Key == Key.R)
            {
                StartReload();
            }

        }



        private void MoveBullet(Image bullet, double angleRad)
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

        private void InitializeBonus()
        {
            bonus = new Bonus("Bonus", (PlayerHealth, magazineSize, Speed) => { });
        }


        private void InitializeBullet()
        {
            Bullet = new Image
            {
                Width = 10,
                Height = 20,
                Source = BitmapFrame.Create(new Uri(@"dead\MyBullet.png", UriKind.Relative)),
                Visibility = Visibility.Hidden,
                Tag = "Bullet"
            };

            LobbyCan.Children.Add(Bullet);
        }


        public void BonusTimer_Tick(object sender, EventArgs e)
        {
            // Вызываем выбор случайного бонуса каждые 45 секунд
            bonus.ChooseRandomBonus( LobbyCan, ref PlayerHealth,  ref magazineSize,  ref Speed);
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
                enemy.UpdateBullets(LobbyCan, Player, ref PlayerHealth);
               // enemy.UpdateEnemyImage(direction);
            }
            UpdateHpinfo();




        }

        public class Bonus
        {
            public string Name { get; set; }
            public Action<int, int, float> ApplyBonus { get; set; }

            public Bonus(string name, Action<int, int, float> applyBonus)
            {
                Name = name;
                ApplyBonus = applyBonus;
            }




            public void ChooseRandomBonus(Canvas lobbyCanvas, ref int PlayerHealth, ref int magazineSize, ref float Speed)
            {
                int a, b = 0;
                Random bns = new Random();

                a = bns.Next(1,3);
                
                
                if (a == 1)
                {
                    MessageBoxResult result = MessageBox.Show($"Choose a bonus:\n1. {"+Патроны"}\n2. {"+Скорость"}", "Bonus Selection", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        magazineSize += 10;
                    }
                    else
                    {
                       Speed += 1;
                    }
                    
                }
               else if (a == 2)
                {
                    MessageBoxResult result = MessageBox.Show($"Choose a bonus:\n1. {"+Здоровье"}\n2. {"+Скорость"}", "Bonus Selection", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        PlayerHealth += 2;
                    }
                    else
                    {
                        Speed += 1;
                    }

                }
                else if (a == 3)
                {
                    MessageBoxResult result = MessageBox.Show($"Choose a bonus:\n1. {"+Здоровье"}\n2. {"+Патроны"}", "Bonus Selection", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        PlayerHealth += 2;
                    }
                    else
                    {
                        Speed += 1;
                    }

                }




                /*      List<Bonus> availableBonuses = new List<Bonus>
    {
        new Bonus("Ammo Bonus", (int ph, int ms, float s) => { magazineSize += 10; }),
        new Bonus("SpeedBonus", (int ph, int ms, float s) => { Speed += 2; }),
        // другие бонусы
    };

                Random random = new Random();
                int randomIndex1 = random.Next(availableBonuses.Count);
                int randomIndex2;

                do
                {
                    randomIndex2 = random.Next(availableBonuses.Count);
                } while (randomIndex2 == randomIndex1);

                Bonus bonus1 = availableBonuses[randomIndex1];
                Bonus bonus2 = availableBonuses[randomIndex2];

                MessageBoxResult result = MessageBox.Show($"Choose a bonus:\n1. {bonus1.Name}\n2. {bonus2.Name}", "Bonus Selection", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    bonus1.ApplyBonus(PlayerHealth, magazineSize, Speed);
                }
                else
                {
                    bonus2.ApplyBonus(PlayerHealth, magazineSize, Speed);
                }*/
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
