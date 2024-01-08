using Dungeon_XXX;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.Windows;
using System.Threading;


public class Enemy
{
   
    private TimeSpan shootingCooldown = TimeSpan.FromSeconds(1.0);
    private DateTime lastShotTime = DateTime.MinValue;
    private double shootingDistance = 450;
    private Canvas LobbyCan;
    private double enemySpeedX = 2;
    private double enemySpeedY = 2;
    public Rectangle EnemyRect { get; private set; }
    public List<Bullet> Bullets { get; set; } = new List<Bullet>();
    public int Health { get; set; } = 7;
    public int damage = 1;

    public Enemy(double startX, double startY, Canvas lobbyCanvas)
    {
        LobbyCan = lobbyCanvas;
        EnemyRect = new Rectangle
        {
            Width = 50,
            Height = 50,
            Fill = Brushes.Red,
            Tag = "Enemy"
            
        };

        Canvas.SetLeft(EnemyRect, startX);
        Canvas.SetTop(EnemyRect, startY);

        Bullets = new List<Bullet>();
       
    }


   
    public void CheckPlayerBulletCollisions(List<Rectangle> playerBullets)
    {
        Rect enemyRectBounds = new Rect(Canvas.GetLeft(EnemyRect), Canvas.GetTop(EnemyRect), EnemyRect.Width, EnemyRect.Height);

        foreach (var bullet in playerBullets.ToList())
        {
            if ((string)bullet.Tag == "MyBullet")
            {
                Rect bulletRect = new Rect(Canvas.GetLeft(bullet), Canvas.GetTop(bullet), bullet.Width, bullet.Height);

                if (bulletRect.IntersectsWith(enemyRectBounds))
                {
                    Health -= damage;
                    if (Health <= 0)
                    {
                        Die();
                    }

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        LobbyCan.Children.Remove(bullet);
                        playerBullets.Remove(bullet);
                    });
                }
            }
        }
    }




    private void Die()
    {
        if (LobbyCan.Children.Contains(EnemyRect))
        {
            LobbyCan.Children.Remove(EnemyRect);
        }
    }


    public void MoveTowardsPlayer(Rectangle Player, List<Rectangle> playerBullets)
    {


        CheckPlayerBulletCollisions(playerBullets);
        if (Health > 0)
        {
            double playerX = Canvas.GetLeft(Player) + Player.Width / 2;
            double playerY = Canvas.GetTop(Player) + Player.Height / 2;

            double enemyX = Canvas.GetLeft(EnemyRect) + EnemyRect.Width / 2;
            double enemyY = Canvas.GetTop(EnemyRect) + EnemyRect.Height / 2;

            double distanceToPlayer = Math.Sqrt(Math.Pow(playerX - enemyX, 2) + Math.Pow(playerY - enemyY, 2));

            if (distanceToPlayer > shootingDistance)
            {
                double angleToPlayer = Math.Atan2(playerY - enemyY, playerX - enemyX);

                double deltaX = enemySpeedX * Math.Cos(angleToPlayer);
                double deltaY = enemySpeedY * Math.Sin(angleToPlayer);

                double newLeft = Canvas.GetLeft(EnemyRect) + deltaX;
                double newTop = Canvas.GetTop(EnemyRect) + deltaY;

                if (newLeft >= 0 && newLeft + EnemyRect.Width <= LobbyCan.ActualWidth &&
                    newTop >= 0 && newTop + EnemyRect.Height <= LobbyCan.ActualHeight)
                {
                    Rect futureRect = new Rect(newLeft, newTop, EnemyRect.Width, EnemyRect.Height);

                    foreach (var obstacle in LobbyCan.Children.OfType<Rectangle>().Where(obj => obj.Tag != null && obj.Tag.ToString() == "Collide"))
                    {
                        Rect obstacleRect = new Rect(Canvas.GetLeft(obstacle), Canvas.GetTop(obstacle), obstacle.Width, obstacle.Height);

                        if (futureRect.IntersectsWith(obstacleRect))
                        {
                            return;
                        }
                    }

                    Canvas.SetLeft(EnemyRect, newLeft);
                    Canvas.SetTop(EnemyRect, newTop);

                }

            }

            else
            {
                // Проверяем, прошло ли достаточно времени с момента последнего выстрела
                if ((DateTime.Now - lastShotTime) >= shootingCooldown)
                {
                    // Если прошло достаточно времени, выполняем выстрел и обновляем время последнего выстрела
                    ShootAtPlayer(Player, LobbyCan);
                    lastShotTime = DateTime.Now;
                }
            }
        }
    }






    public void UpdateBullets(Canvas LobbyCan, Rectangle Player, int PlayerHealth)
    {
        List<Bullet> bulletsCopy = new List<Bullet>(Bullets);

        foreach (var bullet in bulletsCopy)
        {
            
            bullet.UpdatePosition();
            

            // Проверяем столкновение с элементами, имеющими тег "Collide"
            var obstacles = LobbyCan.Children.OfType<Rectangle>().Where(obj => obj.Tag != null && obj.Tag.ToString() == "Collide").ToList();

            foreach (var obstacle in obstacles)
            {
                Rect bulletRect = new Rect(Canvas.GetLeft(bullet.BulletRect), Canvas.GetTop(bullet.BulletRect), bullet.BulletRect.Width, bullet.BulletRect.Height);
                Rect obstacleRect = new Rect(Canvas.GetLeft(obstacle), Canvas.GetTop(obstacle), obstacle.Width, obstacle.Height);

                if (bulletRect.IntersectsWith(obstacleRect))
                {
                    // Удаляем пулю, если она столкнулась с элементом, имеющим тег "Collide"
                    LobbyCan.Children.Remove(bullet.BulletRect);
                    Bullets.Remove(bullet);
                    break;
                }
            }
            // Проверяем столкновение с игроком
            Rect bulletRect1 = new Rect(Canvas.GetLeft(bullet.BulletRect), Canvas.GetTop(bullet.BulletRect), bullet.BulletRect.Width, bullet.BulletRect.Height);
            Rect playerRect = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);

            if (bulletRect1.IntersectsWith(playerRect))
            {
                PlayerHealth -= 1;


                LobbyCan.Children.Remove(bullet.BulletRect);
                if (PlayerHealth <= 0)
                {

                    LobbyCan.Children.Remove(Player);

                }


                // После этого вы можете проверить, умер ли игрок и выполнить соответствующие действия.
                // Например, вызвать метод Die() для игрока, который удалит его с холста.
                
            }



        }
    }

  

    public void ShootAtPlayer(Rectangle Player, Canvas LobbyCan)
    {
        
        double enemyCenterX = Canvas.GetLeft(EnemyRect) + EnemyRect.Width / 2;
        double enemyCenterY = Canvas.GetTop(EnemyRect) + EnemyRect.Height / 2;

        double playerCenterX = Canvas.GetLeft(Player) + Player.Width / 2;
        double playerCenterY = Canvas.GetTop(Player) + Player.Height / 2;

        // Рассчитываем направление от врага к игроку
        double directionX = playerCenterX - enemyCenterX;
        double directionY = playerCenterY - enemyCenterY;

        // Нормализуем вектор направления
        double length = Math.Sqrt(directionX * directionX + directionY * directionY);
        directionX /= length;
        directionY /= length;

        // Создаем новую пулю и добавляем ее в список Bullets
        Bullet bullet = new Bullet(enemyCenterX, enemyCenterY, directionX, directionY);
        Bullets.Add(bullet);
        
        // Отображаем пулю
        LobbyCan.Children.Add(bullet.BulletRect);
    }
    

}