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
using System.Drawing;
using System.Threading;

namespace Texac_Poker
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
          

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(@"E:/Новая папка/22.png", UriKind.RelativeOrAbsolute);
            bi.EndInit();

            image1.Source = bi;
            // System.Diagnostics.Process.Start("https://radio.yandex.ru/genre/rock");
            // System.Diagnostics.Process.Start("https://retrowave.ru/");
            // System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=hHW1oY26kxQ");
        }
       
        DealCards gamemod = new DealCards(5);
        private async void Button_Click(object sender, RoutedEventArgs e)// START GAME
        {
            fold = false;
            rise = false;
            chek_call = false;
            
            //    Thread t = new Thread(delegate () {
            //        gamemod.River(this); });
            //    t.Start();


            await Task.Run(() => gamemod.River(this));


            //диллер не ставит блайнд
            // слева от диллераа -меньший блайнд
            // второй слева - больший 
            // третий слева -получает ход

            // после пре флопа ходит слева от диллер
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }//close

        public bool fold;
        public bool rise;
        public bool chek_call;

        private void Button_Click_Fold(object sender, RoutedEventArgs e)
        {
            fold = true;
        }
        private void Button_Click_Chek(object sender, RoutedEventArgs e)
        {
            chek_call = true;
        }
        private void Button_Click_Rise(object sender, RoutedEventArgs e)
        {
            rise = true;
        }
    }
}
