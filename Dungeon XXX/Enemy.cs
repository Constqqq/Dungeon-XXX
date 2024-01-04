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
    private Canvas LobbyCan;
    private double enemySpeedX = 2;
    private double enemySpeedY = 2;
    public Rectangle EnemyRect { get; set; }
    public List<Bullet> Bullets { get; set; } = new List<Bullet>();

    public Enemy(double startX, double startY, Canvas lobbyCanvas)
    {
        LobbyCan = lobbyCanvas;
        EnemyRect = new Rectangle
        {
            Width = 50,
            Height = 50,
            Fill = Brushes.Red,
            Tag = "User"
        };

        Canvas.SetLeft(EnemyRect, startX);
        Canvas.SetTop(EnemyRect, startY);

        Bullets = new List<Bullet>();
       
    }





    public void MoveTowardsPlayer(Rectangle Player)
    {
        double playerX = Canvas.GetLeft(Player) + Player.Width / 2;
        double playerY = Canvas.GetTop(Player) + Player.Height / 2;

        double enemyX = Canvas.GetLeft(EnemyRect) + EnemyRect.Width / 2;
        double enemyY = Canvas.GetTop(EnemyRect) + EnemyRect.Height / 2;

        double angleToPlayer = Math.Atan2(playerY - enemyY, playerX - enemyX);

        // Передвигаемся к игроку сначала по направлению угла
        double deltaX = enemySpeedX * Math.Cos(angleToPlayer);
        double deltaY = enemySpeedY * Math.Sin(angleToPlayer);

        double newLeft = Canvas.GetLeft(EnemyRect) + deltaX;
        double newTop = Canvas.GetTop(EnemyRect) + deltaY;

        // Проверяем, есть ли препятствие на пути
        if (ObstacleOnPath(newLeft, newTop))
        {
            // Ищем направление для обхода препятствия
            double avoidanceAngle = FindAvoidanceAngle(enemyX, enemyY, playerX, playerY);
            deltaX = enemySpeedX * Math.Cos(avoidanceAngle);
            deltaY = enemySpeedY * Math.Sin(avoidanceAngle);

            newLeft = Canvas.GetLeft(EnemyRect) + deltaX;
            newTop = Canvas.GetTop(EnemyRect) + deltaY;
        }

        Canvas.SetLeft(EnemyRect, newLeft);
        Canvas.SetTop(EnemyRect, newTop);
    }

    private bool ObstacleOnPath(double newLeft, double newTop)
    {
        Rect futureRect = new Rect(newLeft, newTop, EnemyRect.Width, EnemyRect.Height);

        foreach (var obstacle in LobbyCan.Children.OfType<Rectangle>().Where(obj => obj.Tag != null && obj.Tag.ToString() == "Collide"))
        {
            Rect obstacleRect = new Rect(Canvas.GetLeft(obstacle), Canvas.GetTop(obstacle), obstacle.Width, obstacle.Height);

            if (futureRect.IntersectsWith(obstacleRect))
            {
                return true; // Препятствие на пути
            }
        }

        return false; // Нет препятствия
    }

    private double FindAvoidanceAngle(double enemyX, double enemyY, double playerX, double playerY)
    {
        double avoidanceAngle = 0;
        double angleIncrement = Math.PI / 180; // Угол увеличения (в радианах)

        double maxViewAngle = Math.PI / 4; // Максимальный угол обзора

        // Ищем свободное направление для обхода препятствия
        for (double angle = -maxViewAngle; angle <= maxViewAngle; angle += angleIncrement)
        {
            double testX = enemyX + enemySpeedX * Math.Cos(angle);
            double testY = enemyY + enemySpeedY * Math.Sin(angle);

            if (!ObstacleOnPath(testX, testY))
            {
                avoidanceAngle = angle;
                break;
            }
        }

        return avoidanceAngle;
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