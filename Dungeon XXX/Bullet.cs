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
    public class Bullet
    {
        public Rectangle BulletRect { get; set; }
        public double DirectionX { get; set; }
        public double DirectionY { get; set; }
        public double Speed { get; set; }

        public Bullet(double startX, double startY, double directionX, double directionY)
        {
            BulletRect = new Rectangle
            {
                
                Width = 10,
                Height = 20,
                Fill = Brushes.Red,
              //  Visibility = Visibility.Hidden
            };

            Canvas.SetLeft(BulletRect, startX);
            Canvas.SetTop(BulletRect, startY);

            DirectionX = directionX;
            DirectionY = directionY;
            Speed = 5;
        }



        public void UpdatePosition()
        {

            Canvas.SetLeft(BulletRect, Canvas.GetLeft(BulletRect) + DirectionX * Speed);
            Canvas.SetTop(BulletRect, Canvas.GetTop(BulletRect) + DirectionY * Speed);
        }

        public bool IsOutOfBounds(double canvasWidth, double canvasHeight)
        {
            double left = Canvas.GetLeft(BulletRect);
            double top = Canvas.GetTop(BulletRect);

            return left < 0 || top < 0 || left > canvasWidth || top > canvasHeight;
        }
        

    }
}