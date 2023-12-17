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

namespace Dungeon_XXX
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        
        Window main = new Window();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void exbut_MouseEnter(object sender, MouseEventArgs e)
        {
            exbut.Source = BitmapFrame.Create(new Uri(@"C:\Users\Shepe\source\repos\Dungeon XXX\Dungeon XXX\Menupng\ExitAction.png"));
        }

        private void exbut_MouseLeave(object sender, MouseEventArgs e)
        {
            exbut.Source = BitmapFrame.Create(new Uri(@"C:\Users\Shepe\source\repos\Dungeon XXX\Dungeon XXX\Menupng\ExitMenu1.png"));
        }

        private void stbut_MouseEnter(object sender, MouseEventArgs e)
        {
            stbut.Source = BitmapFrame.Create(new Uri(@"C:\Users\Shepe\source\repos\Dungeon XXX\Dungeon XXX\Menupng\StartActiom.png"));
        }

        private void stbut_MouseLeave(object sender, MouseEventArgs e)
        {
            stbut.Source = BitmapFrame.Create(new Uri(@"C:\Users\Shepe\source\repos\Dungeon XXX\Dungeon XXX\Menupng\StartMenu2.png"));
        }

        private void setbut_MouseEnter(object sender, MouseEventArgs e)
        {
            setbut.Source = BitmapFrame.Create(new Uri(@"C:\Users\Shepe\source\repos\Dungeon XXX\Dungeon XXX\Menupng\SettingsAction.png"));
        }

        private void setbut_MouseLeave(object sender, MouseEventArgs e)
        {
            setbut.Source = BitmapFrame.Create(new Uri(@"C:\Users\Shepe\source\repos\Dungeon XXX\Dungeon XXX\Menupng\SettingsMenu3.png"));
        }

        private void exbut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void stbut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
            var game = new Lobby();
            game.Owner = this;
            game.ShowDialog();
            Rectangle pl = new Rectangle();
            pl.HorizontalAlignment = HorizontalAlignment.Left;
            pl.VerticalAlignment = VerticalAlignment.Top;
            pl.Height = 55;
            pl.Width = 55;
            pl.Margin = new Thickness(400, 225, 0, 0);
            pl.Fill = Brushes.Black;
            pl.Stroke = Brushes.Black;
           



        }

      
    }
}
