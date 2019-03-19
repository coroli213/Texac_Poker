using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Texac_Poker
{
    class DrawCards
    {
        public static void DrawDealerHand     (Card[] dealerHand,MainWindow imgs, int count=3)
        {
            BitmapImage[] cards_fr = new BitmapImage[5]; 
            for (byte i = 0; i < 5; i++) { cards_fr[i] = new BitmapImage(); }
            string card;

            for (byte i = 0; i < count; i++)
            {
                cards_fr[i].BeginInit();
                card = (dealerHand[i].MyValue + "_" + dealerHand[i].MySuit).ToLower();
                cards_fr[i].UriSource = new Uri(@"C:/Users/maxim/Downloads/minmax/" + card + ".png", UriKind.RelativeOrAbsolute);

                cards_fr[i].EndInit();
                cards_fr[i].Freeze();

                switch (i){

                    case 0:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => { imgs.board1.Source = cards_fr[i]; }));           
                        break;
                    case 1:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => { imgs.board2.Source = cards_fr[i]; }));
                        break;
                    case 2:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => { imgs.board3.Source = cards_fr[i]; }));
                        break;
                    case 3:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => { imgs.board4.Source = cards_fr[i]; }));
                        break;
                    case 4:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => { imgs.board5.Source = cards_fr[i]; }));
                        break; } }
        }
        public static void DrawPlayersHand    (List<Player> players, MainWindow imgs, int player = 8)
        {
            if (player != 8)
            {
                BitmapImage[] map = new BitmapImage[2]; string image;
                for (byte j = 0; j < 2; j++)
                     map [j] = new BitmapImage();

                map[0].BeginInit();
                map[1].BeginInit();

                image = (players[player-1].playerHand[0].MyValue + "_" + players[player - 1].playerHand[0].MySuit).ToLower();
                map[0].UriSource = new Uri(@"C:/Users/maxim/Downloads/minmax/" + image + ".png", UriKind.RelativeOrAbsolute);

                image = (players[player - 1].playerHand[1].MyValue + "_" + players[player - 1].playerHand[1].MySuit).ToLower();
                map[1].UriSource = new Uri(@"C:/Users/maxim/Downloads/minmax/" + image + ".png", UriKind.RelativeOrAbsolute);

                map[0].EndInit(); map[0].Freeze();
                map[1].EndInit(); map[1].Freeze();

                switch (player - 1)
                {
                    case 0:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => {
                            imgs.player1l.Source = map[0];
                            imgs.player1r.Source = map[1];  }));
                        break;
                    case 1:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => {
                            imgs.player2l.Source = map[0];
                            imgs.player2r.Source = map[1]; }));
                        break;
                    case 2:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => {
                            imgs.player3l.Source = map[0];
                            imgs.player3r.Source = map[1]; }));
                        break;
                    case 3:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => {
                            imgs.player4l.Source = map[0];
                            imgs.player4r.Source = map[1]; }));
                        break;
                    case 4:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => {
                            imgs.player5l.Source = map[0];
                            imgs.player5r.Source = map[1]; }));
                        break;
                }                
                return;
            }

            BitmapImage[,] fr = new BitmapImage[5, 2]; string d;
            for (byte i = 0; i < 5; i++)
            for (byte j = 0; j < 2; j++)
                 fr[i, j] = new BitmapImage(); 

            for (byte i = 0; i < players.Count; i++)
            {
                fr[i, 0].BeginInit();
                fr[i, 1].BeginInit();

                d = (players[i].playerHand[0].MyValue + "_" + players[i].playerHand[0].MySuit).ToLower();
                fr[i, 0].UriSource = new Uri(@"C:/Users/maxim/Downloads/minmax/" + d + ".png", UriKind.RelativeOrAbsolute);

                d = (players[i].playerHand[1].MyValue + "_" + players[i].playerHand[1].MySuit).ToLower();
                fr[i, 1].UriSource = new Uri(@"C:/Users/maxim/Downloads/minmax/" + d + ".png", UriKind.RelativeOrAbsolute);

                fr[i, 0].EndInit();
                fr[i, 1].EndInit();

                fr[i, 0].Freeze();
                fr[i, 1].Freeze();

                switch (i)
                {
                    case 0:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => {
                            imgs.player1l.Source = fr[0, 0];
                            imgs.player1r.Source = fr[0, 1]; }));
                        break;
                    case 1:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => {
                            imgs.player2l.Source = fr[1, 0];
                            imgs.player2r.Source = fr[1, 1]; }));
                        break;
                    case 2:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => {
                            imgs.player3l.Source = fr[2, 0];
                            imgs.player3r.Source = fr[2, 1]; }));
                        break;
                    case 3:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => {
                            imgs.player4l.Source = fr[3, 0];
                            imgs.player4r.Source = fr[3, 1]; }));
                        break;
                    case 4:
                        imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                        Action(() => {
                            imgs.player5l.Source = fr[4, 0];
                            imgs.player5r.Source = fr[4, 1]; }));
                        break;
                }
            }
        }
        public static void DrawPlayersHideHand(List<Player> players, MainWindow imgs)
        {
            BitmapImage map = new BitmapImage();
            map.BeginInit();

            map.UriSource = new Uri(@"C:\Users\maxim\Downloads\minmax\image_part_053.png", UriKind.RelativeOrAbsolute);
            map.EndInit(); map.Freeze();
            for (byte i = 0; i < 5; i++)
                
                    switch (i)
                    {
                        case 0:
                            imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                            Action(() =>
                            {
                                imgs.player1l.Source = map;
                                imgs.player1r.Source = map;
                            }));
                            break;
                        case 1:
                            imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                            Action(() =>
                            {
                                imgs.player2l.Source = map;
                                imgs.player2r.Source = map;
                            }));
                            break;
                        case 2:
                            imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                            Action(() =>
                            {
                                imgs.player3l.Source = map;
                                imgs.player3r.Source = map;
                            }));
                            break;
                        case 3:
                            imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                            Action(() =>
                            {
                                imgs.player4l.Source = map;
                                imgs.player4r.Source = map;
                            }));
                            break;
                        case 4:
                            imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                            Action(() =>
                            {
                                imgs.player5l.Source = map;
                                imgs.player5r.Source = map;
                            }));
                            break;
                    }
                
              
        }
        public static void DrawWinHandShape   (MainWindow imgs, Player player, Card[] dealerHand, int dealerCount, int playerNumber)
        {
            imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
            Action(() => { imgs.canvas.Children.Clear(); }));

            switch (playerNumber)
            {
                case 0:
                    imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                    Action(() => { imgs.tx1.Text = player.power.Total.ToString(); }));
                    break;

                case 1:
                    imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                    Action(() => { imgs.tx2.Text = player.power.Total.ToString(); }));
                    break;

                case 2:
                    imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                    Action(() => { imgs.tx3.Text = player.power.Total.ToString(); }));
                    break;

                case 3:
                    imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                    Action(() => { imgs.tx4.Text = player.power.Total.ToString(); }));
                    break;

                case 4:
                    imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                    Action(() => { imgs.tx5.Text = player.power.Total.ToString(); }));
                    break;
            }
            if (dealerCount == 9) return;
            for(byte i = 0; i < player.power.WinHand.Count; i++){

                for(byte j = 0; j < dealerCount; j++){

                    if (player.power.WinHand[i].MySuit  == dealerHand[j].MySuit &&
                        player.power.WinHand[i].MyValue == dealerHand[j].MyValue){ 

                        DrawDealerCardShape(j, imgs); } }

                for (byte j = 0; j < player.playerHand.Count(); j++) { 

                    if (player.power.WinHand[i].MySuit  == player.playerHand[j].MySuit &&
                        player.power.WinHand[i].MyValue == player.playerHand[j].MyValue){

                        byte whitch = j == 0 ? whitch = 1 : whitch = 2;

                        DrawPlayerCardShape(playerNumber, whitch, imgs); } } }
        }
        public static void DrawDealerCardShape(int cardNumber, MainWindow imgs)
        {
            imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
            Action(() => {

                int x = 110 * cardNumber;
                Polyline horArr = new Polyline();
                horArr.Points = new PointCollection();

                horArr.Points.Add(new Point(420 + x, 300));
                horArr.Points.Add(new Point(520 + x, 300));
                horArr.Points.Add(new Point(520 + x, 438));
                horArr.Points.Add(new Point(420 + x, 438));
                horArr.Points.Add(new Point(420 + x, 300));

                horArr.Stroke = Brushes.Gold;
                imgs.canvas.Children.Add(horArr); }));
        }
        public static void DrawPlayerCardShape(int playerNum,  int whitchCard, MainWindow imgs)
        {
            imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
            Action(() => {

                switch (playerNum) {

                    case 0:

                        Polyline cardl0 = new Polyline();
                        Polyline cardr0 = new Polyline();
                        cardl0.Points = new PointCollection();
                        cardr0.Points = new PointCollection();

                        if (whitchCard == 1)
                        {
                            cardl0.Points.Add(new Point(585, 590));
                            cardl0.Points.Add(new Point(685, 590));
                            cardl0.Points.Add(new Point(685, 725));
                            cardl0.Points.Add(new Point(585, 725));
                            cardl0.Points.Add(new Point(585, 590)); }
                        else
                        {
                            cardr0.Points.Add(new Point(585 + 110, 590));
                            cardr0.Points.Add(new Point(685 + 110, 590));
                            cardr0.Points.Add(new Point(685 + 110, 725));
                            cardr0.Points.Add(new Point(585 + 110, 725));
                            cardr0.Points.Add(new Point(585 + 110, 590)); }

                        cardl0.Stroke = Brushes.Gold;
                        cardr0.Stroke = Brushes.Gold;

                        imgs.canvas.Children.Add(cardl0);
                        imgs.canvas.Children.Add(cardr0);
                        break;

                    case 1:

                        Polyline cardl1 = new Polyline();
                        Polyline cardr1 = new Polyline();
                        cardl1.Points = new PointCollection();
                        cardr1.Points = new PointCollection();

                        if (whitchCard == 1)
                        {
                            cardl1.Points.Add(new Point(70, 464));
                            cardl1.Points.Add(new Point(170, 464));
                            cardl1.Points.Add(new Point(170, 600));
                            cardl1.Points.Add(new Point(70, 600));
                            cardl1.Points.Add(new Point(70, 464)); }
                        else
                        {
                            cardr1.Points.Add(new Point(70 + 110, 464));
                            cardr1.Points.Add(new Point(170 + 110, 464));
                            cardr1.Points.Add(new Point(170 + 110, 600));
                            cardr1.Points.Add(new Point(70 + 110, 600));
                            cardr1.Points.Add(new Point(70 + 110, 464)); }

                        cardl1.Stroke = Brushes.Gold;
                        cardr1.Stroke = Brushes.Gold;

                        imgs.canvas.Children.Add(cardl1);
                        imgs.canvas.Children.Add(cardr1);
                        break;

                    case 2:

                        Polyline cardl2 = new Polyline();
                        Polyline cardr2 = new Polyline();
                        cardl2.Points = new PointCollection();
                        cardr2.Points = new PointCollection();

                        if (whitchCard == 1)
                        {
                            cardl2.Points.Add(new Point(230, 65));
                            cardl2.Points.Add(new Point(330, 65));
                            cardl2.Points.Add(new Point(330, 202));
                            cardl2.Points.Add(new Point(230, 202));
                            cardl2.Points.Add(new Point(230, 65)); }
                        else
                        {
                            cardr2.Points.Add(new Point(230 + 110, 65));
                            cardr2.Points.Add(new Point(330 + 110, 65));
                            cardr2.Points.Add(new Point(330 + 110, 202));
                            cardr2.Points.Add(new Point(230 + 110, 202));
                            cardr2.Points.Add(new Point(230 + 110, 65)); }

                        cardl2.Stroke = Brushes.Gold;
                        cardr2.Stroke = Brushes.Gold;

                        imgs.canvas.Children.Add(cardl2);
                        imgs.canvas.Children.Add(cardr2);
                        break;

                    case 3:

                        Polyline cardl3 = new Polyline();
                        Polyline cardr3 = new Polyline();
                        cardl3.Points = new PointCollection();
                        cardr3.Points = new PointCollection();

                        if (whitchCard == 1)
                        {
                            cardl3.Points.Add(new Point(930, 65));
                            cardl3.Points.Add(new Point(1030, 65));
                            cardl3.Points.Add(new Point(1030, 202));
                            cardl3.Points.Add(new Point(930, 202));
                            cardl3.Points.Add(new Point(930, 65)); }
                        else
                        {
                            cardr3.Points.Add(new Point(930 + 110, 65));
                            cardr3.Points.Add(new Point(1030 + 110, 65));
                            cardr3.Points.Add(new Point(1030 + 110, 202));
                            cardr3.Points.Add(new Point(930 + 110, 202));
                            cardr3.Points.Add(new Point(930 + 110, 65)); }

                        cardl3.Stroke = Brushes.Gold;
                        cardr3.Stroke = Brushes.Gold;

                        imgs.canvas.Children.Add(cardl3);
                        imgs.canvas.Children.Add(cardr3);
                        break;

                    case 4:

                        Polyline cardl4 = new Polyline();
                        Polyline cardr4 = new Polyline();
                        cardl4.Points = new PointCollection();
                        cardr4.Points = new PointCollection();

                        if (whitchCard == 1)
                        {
                            cardl4.Points.Add(new Point(1090, 464));
                            cardl4.Points.Add(new Point(1190, 464));
                            cardl4.Points.Add(new Point(1190, 600));
                            cardl4.Points.Add(new Point(1090, 600));
                            cardl4.Points.Add(new Point(1090, 464)); } 
                        else
                        {
                            cardr4.Points.Add(new Point(1090 + 110, 464));
                            cardr4.Points.Add(new Point(1190 + 110, 464));
                            cardr4.Points.Add(new Point(1190 + 110, 600));
                            cardr4.Points.Add(new Point(1090 + 110, 600));
                            cardr4.Points.Add(new Point(1090 + 110, 464)); }

                        cardl4.Stroke = Brushes.Gold;
                        cardr4.Stroke = Brushes.Gold;

                        imgs.canvas.Children.Add(cardl4);
                        imgs.canvas.Children.Add(cardr4);
                        break;
                }  }));
        }
        public static void DrawPlayerTrunShape(int  playerNum, MainWindow imgs)
        {
            switch (playerNum)
            {
                case 0:

                    imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                    Action(() => {

                        Polyline cardl0 = new Polyline();
                        cardl0.Points = new PointCollection();

                        cardl0.Points.Add(new Point(585, 590));
                        cardl0.Points.Add(new Point(800, 590));
                        cardl0.Points.Add(new Point(800, 725));
                        cardl0.Points.Add(new Point(585, 725));
                        cardl0.Points.Add(new Point(585, 590));

                        cardl0.Stroke = Brushes.Gold;

                        imgs.canvas.Children.Add(cardl0); }));
                    break;

                case 1:

                    imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                    Action(() => {

                        Polyline cardl1 = new Polyline();
                        cardl1.Points = new PointCollection();

                        cardl1.Points.Add(new Point(70, 464));
                        cardl1.Points.Add(new Point(280, 464));
                        cardl1.Points.Add(new Point(280, 600));
                        cardl1.Points.Add(new Point(70, 600));
                        cardl1.Points.Add(new Point(70, 464));

                        cardl1.Stroke = Brushes.Gold;

                        imgs.canvas.Children.Add(cardl1); }));                   
                    break;

                case 2:

                    imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                    Action(() => {

                        Polyline cardl2 = new Polyline();
                        cardl2.Points = new PointCollection();

                        cardl2.Points.Add(new Point(230, 65));
                        cardl2.Points.Add(new Point(440, 65));
                        cardl2.Points.Add(new Point(440, 202));
                        cardl2.Points.Add(new Point(230, 202));
                        cardl2.Points.Add(new Point(230, 65));

                        cardl2.Stroke = Brushes.Gold;

                        imgs.canvas.Children.Add(cardl2); }));
                    break;

                case 3:

                    imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                    Action(() => {
                        Polyline cardl3 = new Polyline();
                        cardl3.Points   = new PointCollection();
                   
                        cardl3.Points.Add(new Point(930,  65));
                        cardl3.Points.Add(new Point(1140, 65));
                        cardl3.Points.Add(new Point(1140, 202));
                        cardl3.Points.Add(new Point(930,  202));
                        cardl3.Points.Add(new Point(930,  65));
                   
                        cardl3.Stroke = Brushes.WhiteSmoke;
                        imgs.canvas.Children.Add(cardl3);  }));
                    break;

                case 4:

                    imgs.Dispatcher.Invoke(DispatcherPriority.Background, new
                    Action(() => {

                        Polyline cardl4 = new Polyline();
                        cardl4.Points = new PointCollection();

                        cardl4.Points.Add(new Point(1090, 464));
                        cardl4.Points.Add(new Point(1300, 464));
                        cardl4.Points.Add(new Point(1300, 600));
                        cardl4.Points.Add(new Point(1090, 600));
                        cardl4.Points.Add(new Point(1090, 464));

                        cardl4.Stroke = Brushes.Gold;

                        imgs.canvas.Children.Add(cardl4); }));
                    break;
            }
        }
         //-------------------------------
    }   }