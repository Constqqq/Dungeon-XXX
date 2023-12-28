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
            exbut.Source = BitmapFrame.Create(new Uri(@"Menupng\ExitAction.png", UriKind.Relative));
        }

        private void exbut_MouseLeave(object sender, MouseEventArgs e)
        {
            exbut.Source = BitmapFrame.Create(new Uri(@"Menupng\ExitMenu1.png", UriKind.Relative));
        }

        private void stbut_MouseEnter(object sender, MouseEventArgs e)
        {
            stbut.Source = BitmapFrame.Create(new Uri(@"Menupng\StartActiom.png", UriKind.Relative));
        }

        private void stbut_MouseLeave(object sender, MouseEventArgs e)
        {
            stbut.Source = BitmapFrame.Create(new Uri(@"Menupng\StartMenu2.png", UriKind.Relative));
        }

        private void setbut_MouseEnter(object sender, MouseEventArgs e)
        {
            setbut.Source = BitmapFrame.Create(new Uri(@"Menupng\SettingsAction.png", UriKind.Relative));
        }

        private void setbut_MouseLeave(object sender, MouseEventArgs e)
        {
            setbut.Source = BitmapFrame.Create(new Uri(@"Menupng\SettingsMenu3.png", UriKind.Relative));
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
            
           


            
        }
        
      
    }

    
}
