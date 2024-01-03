using Dungeon_XXX;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.Windows;

public class Enemy
{

    
    private double enemySpeed = 2;
    public Rectangle EnemyRect { get; set; }
    public List<Bullet> Bullets { get; set; } = new List<Bullet>();

    public Enemy(double startX, double startY)
    {
        EnemyRect = new Rectangle
        {
            Width = 50,
            Height = 50,
            Fill = Brushes.Red
        };

        Canvas.SetLeft(EnemyRect, startX);
        Canvas.SetTop(EnemyRect, startY);

        Bullets = new List<Bullet>();
    }


    public void MoveTowardsPlayer(Rectangle Player)
    {
        double playerX = Canvas.GetLeft(Player) + Player.Width / 2;
        double playerY = Canvas.GetTop(Player) +Player.Height / 2;

        double enemyX = Canvas.GetLeft(EnemyRect) + EnemyRect.Width / 2;
        double enemyY = Canvas.GetTop(EnemyRect) + EnemyRect.Height / 2;

        double angleRad = Math.Atan2(playerY - enemyY, playerX - enemyX);

        double deltaX = enemySpeed * Math.Cos(angleRad);
        double deltaY = enemySpeed * Math.Sin(angleRad);

        double newLeft = Canvas.GetLeft(EnemyRect) + deltaX;
        double newTop = Canvas.GetTop(EnemyRect) + deltaY;

        Canvas.SetLeft(EnemyRect, newLeft);
        Canvas.SetTop(EnemyRect, newTop);
    }


    public void UpdateBullets(Canvas LobbyCan)
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

            // Удаляем пулю, если она выходит за пределы холста
            
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