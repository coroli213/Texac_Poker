using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;

namespace Texac_Poker{
    struct Player{

        public int bet;
        public int gold;
        public string name;
        public bool playNow;
        public byte seatPlace;

        public HandValue power;
        public Card[] playerHand;
        //-----------
        public void Fold() {
          //  if  (playNow)
                 playNow = false;
          //  else playNow = true;
        }
        public void Add_win(int bank) { gold += bank; }
        public void Pay_to_bank (DealCards bank, int bet)   
        {
            this.gold -= bet;
            bank.bank += bet;
        }
        public static bool operator ==(Player c1, Player c2)
        {
            if (c1.playerHand == c2.playerHand)
                return true;
            return false;
        }
        public static bool operator !=(Player c1, Player c2)
        {
            if (c1.playerHand != c2.playerHand)
                return true;
            return false;
        }
        public Player(byte place, int capital = 0, string nameC = "Player")
        {
            bet = 0;
            gold = capital;
            playNow = true;
            seatPlace = place;
            power = new HandValue();
            playerHand = new Card[2];
            this.name = nameC + " ";
            if(name!="Player")
            this.name+=  place.ToString();

        }
    }
    //------------------------------
    class DealCards : DeckOfCards
    {
        public List<Player> players;
        public Card[] dealerHand;
        public int bank;

        public DealCards(int player_count){

            bank = 0;
            dealerHand = new Card[5];
            players = new List<Player>();

            for (byte c = 1; c < player_count+1 ; c++)
                players.Add(new Player(c));
            setUpDeck();
            GetHand();
        }
        public void GetHand()
        {
            //2 cards from deck for players
            byte count = 0;
            for (int c = 0; c < players.Count(); c++)
                for (int i = 0; i < 2; i++)
                {
                    players[c].playerHand[i] = Deck[count]; count++;
                }
            //5 cards from deck for the computer
            for (int i = 0; i < 5; ++i, count++)
                dealerHand[i] = Deck[count];
        }
        public void River(MainWindow main){
            // ЗАМЕНИТЬ НА DECK
            var queue1 = new Queue<Player>();
            foreach (var b in players)
                queue1.Enqueue(b);

            queue1.Dequeue();
            
            var gamers = new List<Player>(players); // current game players
            var folder = new List<Player>(gamers);
            folder.Clear();
            var queue  = players;                   // full game stack


            int cureent_max_bet = 0;
            int count = 0;                          // pos player in  queue
            byte cont = 0;

            for (byte round = 0; round < 4; round++){//4 раунда 

                if (round == 0)
                {
                    DrawCards.DrawDealerHand(dealerHand, main, 0);
                    EvaluateHands(main,0); }
                else{
                    DrawCards.DrawDealerHand(dealerHand, main, round + 2);
                    EvaluateHands(main, round + 2); }
                // оцениваем комбинушки персонажей / Рисуем руку диллера

                for (bool check = true; count < queue.Count; count++, check=true){
                    // count - номер плэйера в очереди;
                    if (!queue[count].playNow) continue;
                    DrawCards.DrawPlayerTrunShape(queue[count].seatPlace - 1, main);                
                    DrawCards.DrawPlayersHand(players, main, queue[count].seatPlace);
                    // рисуем руку текущего игрока

                    if(round==0) DrawCards.DrawWinHandShape(main, queue[count], dealerHand, 0 ,        queue[count].seatPlace - 1);
                    else         DrawCards.DrawWinHandShape(main, queue[count], dealerHand, round + 2, queue[count].seatPlace - 1);
                    // отрисовать победную комбинацию
                    
                    while (check){
                        //отслеживаем какую кнопку нажал

                        if (main.fold){
                            // ФОЛД - УДАЛИТЬ ИГРОКА // закрыть его карты или тип того
                            var temp = gamers[queue[count].seatPlace - 1];
                            temp.playNow = false;
                            gamers[queue[count].seatPlace - 1] = temp;

                            folder.Add(temp);
                            // удаляем из пула игроков
                            // и идем к следующему
                            // MessageBox.Show("player " + queue[count].seatPlace + " been deleted", "Ошибка при вводе имени", MessageBoxButton.OK, MessageBoxImage.Error);
                            main.fold = false;
                            check = false;
                        }
                        if (main.rise)
                        {
                            // ЕСЛИ КТО-ТО РЭЙЗИТ 
                            // ВСЕХ КТО БЫЛ ДО НЕГО ОТПРАВЛЯЮТСЯ 
                            // В КОНЕЦ ОЧЕРЕДИ
                            for (byte ni = 0; ni < queue[count].seatPlace; ni++)
                                queue.Add(gamers[ni]);
                            // ЗАПЛАТИТЬ В банк значение Бокса с цифрами ставку
                            // НУЖНО ВНЕСТИ В БАНК БОЛЬШЕ ТЕКУЩЕЙ СТАВКИ
                            queue[count].Pay_to_bank(this, cureent_max_bet);

                            MessageBox.Show("rise", "Ошибка при вводе имени", MessageBoxButton.OK, MessageBoxImage.Error);
                            main.rise = false;
                            check = false;
                        }
                        if (main.chek_call){
                            // ЧЕК/КОЛЛ - ПЕРЕЙТИ КСЛЕДУЮЩЕМУ
                            // удаляем у игрока монетки
                            // закидываем в банк
                            // идем дальше
                            queue[count].Pay_to_bank(this, cureent_max_bet);

                          //  MessageBox.Show("player " + queue[count].seatPlace + " skiped", "Ошибка при вводе имени", MessageBoxButton.OK, MessageBoxImage.Error);
                            main.chek_call = false;
                            check = false;
                        }
                    }

                    DrawCards.DrawPlayersHideHand(players, main);

                    switch (queue[count].seatPlace - 1)
                    {
                        case 0:
                            main.Dispatcher.Invoke(DispatcherPriority.Background, new
                            Action(() => { main.tx1.Text = ""; }));
                            break;

                        case 1:
                            main.Dispatcher.Invoke(DispatcherPriority.Background, new
                            Action(() => { main.tx2.Text = ""; }));
                            break;

                        case 2:
                            main.Dispatcher.Invoke(DispatcherPriority.Background, new
                            Action(() => { main.tx3.Text = ""; }));
                            break;

                        case 3:
                            main.Dispatcher.Invoke(DispatcherPriority.Background, new
                            Action(() => { main.tx4.Text = ""; }));
                            break;

                        case 4:
                            main.Dispatcher.Invoke(DispatcherPriority.Background, new
                            Action(() => { main.tx5.Text = ""; }));
                            break;
                    }
                }

                cureent_max_bet = 0;

                cont = 0;
                foreach (var player in gamers){
                        queue.Add(player); }
            }
            
            players.Reverse(); players = players.Take(5).ToList(); players.Reverse();

            EvaluateHands(main, 5);

            TakeWinner(main);
            //победку оформляем
        }
        public void EvaluateHands(MainWindow btns, int dealerCnt)
        {
            for (byte i = 0; i < players.Count; i++)
            {
                HandValue j = new HandEvaluator(players[i].playerHand, dealerHand, dealerCnt).handValue;
                Player d = players[i];
                d.power = j;
                players[i] = d;
            }
        }
        public void TakeWinner(MainWindow main){        

            List<byte>   winners_places = new List<byte>();
            List<Player> winners        = new List<Player>();

                 if (Is_Royal    ().Count     != 0){

                winners = Is_Royal();
                foreach (var b in winners)
                    winners_places.Add(Whitch_player(b));
            }
            else if (Is_StraightFlush().Count != 0)
            {
                winners = Is_StraightFlush();
                foreach (var b in winners)
                    winners_places.Add(Whitch_player(b));

            }
            else if (Is_FourKind ().Count     != 0)
            {
                winners = Is_FourKind();
                foreach (var b in winners)
                    winners_places.Add(Whitch_player(b));

            }
            else if (Is_FullHouse().Count     != 0)
            {
                winners = Is_FullHouse();
                foreach (var b in winners)
                    winners_places.Add(Whitch_player(b));

            }
            else if (Is_Flush    ().Count     != 0)
            {
                winners = Is_Flush();
                foreach (var b in winners)
                    winners_places.Add(Whitch_player(b));

            }
            else if (Is_Straight ().Count     != 0)
            {
                winners = Is_Straight();
                foreach (var b in winners)
                    winners_places.Add(Whitch_player(b));
            }
            else if (Is_ThreeKind().Count     != 0)
            {
                winners = Is_ThreeKind();
                foreach (var b in winners)
                    winners_places.Add(Whitch_player(b));
            }
            else if (Is_TwoPairs ().Count     != 0)
            {
                winners = Is_TwoPairs();
                foreach (var b in winners)
                    winners_places.Add(Whitch_player(b));
            }
            else if (Is_OnePair  ().Count     != 0)
            {
                winners = Is_OnePair();
                foreach (var b in winners)
                    winners_places.Add(Whitch_player(b));
            }
            else if (Is_Kiker    ().Count     != 0)
            {
                winners = Is_Kiker();
                foreach (var b in winners)
                    winners_places.Add(Whitch_player(b));
            }

            bank /= winners.Count;
            for (byte i = 0; i < players.Count; i++)
                DrawCards.DrawWinHandShape(main, players[i], dealerHand, 9, i);

            DrawCards.DrawPlayersHand(players, main, 8);
            foreach (var b in winners)
            {
                b.Add_win(bank);
                MessageBox.Show("player " + b.seatPlace + " win with " + b.power.Total + "CLAP! CLAP! CALP!", " ", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
        public byte Whitch_player(Player player)
        {
            for (byte i = 0; i < players.Count; i++)
                if (players[i] == player)
                    return i;
            return 99;//ОШИБКА
        }
        // возвращаем список победителей
        private List<Player> Is_Royal()        
        {
            List<Player> winners = new List<Player>();

            for (byte i = 0; i < players.Count; i++)
                if (players[i].power.Total == (HAND)9)
                    winners.Add(players[i]);
            return  winners; }
        private List<Player> Is_StraightFlush()
        {
            List<Player> winners = new List<Player>();

            for (byte i = 0; i < players.Count; i++)
                if (players[i].power.Total == (HAND)8)
                    winners.Add(players[i]);

            if (winners.Count == 1) return winners;
            // если 1 то победитель
            // иначе мерюются старшими картами
            var o = winners.OrderBy(c => c.power.HighCombCard).Reverse();
            o.Where(с => с.power.HighCombCard == o.ElementAt(0).power.HighCombCard);
            //если больше 2 штук то на борде и возвращаем всех 
            return o.ToList();
        }
        private List<Player> Is_FourKind()     
        {
            List<Player> winners = new List<Player>();

            for (byte i = 0; i < players.Count; i++)
                if (players[i].power.Total == (HAND)7)
                    winners.Add(players[i]);

            if (winners.Count == 1) return winners;
            // сначала сравним карту каре
            var o = winners.OrderBy(c => c.power.HighCombCard).Reverse();
            o.Where(с => с.power.HighCombCard == o.ElementAt(0).power.HighCombCard);

            if (o.Count() == 1) return o.ToList();
            // если 2-3 на борде то то 1 победитель

            // если все на борде мереемся  рукой
            o.OrderBy(c => c.power.HighKiker).Reverse();
            o.Where  (с => с.power.HighKiker == o.ElementAt(0).power.HighKiker);
            //если больше 2 штук то на борде и возвращаем всех 
            return o.ToList();
        }
        private List<Player> Is_FullHouse()    
        {
            List<Player> winners = new List<Player>();

            for (byte i = 0; i < players.Count; i++)
                if (players[i].power.Total == (HAND)6)
                    winners.Add(players[i]);

            if (winners.Count == 1) return winners;
            // сначала сравним карту каре 1- тройка 2- двойка
            var o = winners.OrderBy(c => c.power.HighCombCard).Reverse();
            o.Where(с => с.power.HighCombCard == o.ElementAt(0).power.HighCombCard);
            if (o.Count() == 1) return o.ToList();

            o.OrderBy(c => c.power.HighKiker).Reverse();
            o.Where(с => с.power.HighKiker == o.ElementAt(0).power.HighKiker);
            //если больше 2 штук то на борде и возвращаем всех 
            return o.ToList();
        }
        private List<Player> Is_Flush()        
        {
            List<Player> winners = new List<Player>();

            for (byte i = 0; i < players.Count; i++)
                if (players[i].power.Total == (HAND)5)
                    winners.Add(players[i]);

            if (winners.Count == 1) return winners;
            //если весь  на борде - делим выигрыш
            //иначе всегда есть карта старше!!!
            var o = winners.OrderBy(c => c.power.HighCombCard).Reverse();
            o.Where(с => с.power.HighCombCard == o.ElementAt(0).power.HighCombCard);
            if (o.Count() == 1) return o.ToList();
            //если больше 2 штук то на борде и возвращаем всех 
            return o.ToList();
        }
        private List<Player> Is_Straight()     
        {
            List<Player> winners = new List<Player>();

            for (byte i = 0; i < players.Count; i++)
                if (players[i].power.Total == (HAND)4)
                    winners.Add(players[i]);

            if (winners.Count == 1) return winners;
            //eсли весь на борде - мерюются старшей картой. иначе 
            //cтрит определяется по старшей карте комбинации
            var o = winners.OrderBy(c => c.power.HighCombCard).Reverse();
            o.Where(с => с.power.HighCombCard == o.ElementAt(0).power.HighCombCard);
            if (o.Count() == 1) return o.ToList();
            //если больше 2 штук то на борде и возвращаем всех 
            return o.ToList();
        }
        private List<Player> Is_ThreeKind()    
        {
            List<Player> winners = new List<Player>();

            for (byte i = 0; i < players.Count; i++)
                if (players[i].power.Total == (HAND)3)
                    winners.Add(players[i]);

            if (winners.Count == 1) return winners;
            // сначала сравним карту тройки
            var o = winners.OrderBy(c => c.power.HighCombCard).Reverse();
            o.Where(с => с.power.HighCombCard == o.ElementAt(0).power.HighCombCard);

            if (o.Count() == 1) return o.ToList();

            o.OrderBy(c => c.power.HighKiker).Reverse();
            o.Where(с => с.power.HighKiker == o.ElementAt(0).power.HighKiker);
            //если больше 2 штук то на борде и возвращаем всех 
            return o.ToList();
        }
        private List<Player> Is_TwoPairs()     
        {
            List<Player> winners = new List<Player>();

            for (byte i = 0; i < players.Count; i++)
                if (players[i].power.Total == (HAND)2)
                    winners.Add(players[i]);

            if (winners.Count == 1) return winners;
            // сначала сравним карту
            var o = winners.OrderBy(c => c.power.HighCombCard).Reverse();
            o.Where(с => с.power.HighCombCard == o.ElementAt(0).power.HighCombCard);

            if (o.Count() == 1) return o.ToList();

            o.OrderBy(c => c.power.HighKiker).Reverse();
            o.Where(с => с.power.HighKiker == o.ElementAt(0).power.HighKiker);
            //если больше 2 штук то на борде и возвращаем всех 
            return o.ToList();
        }
        private List<Player> Is_OnePair()      
        {
            List<Player> winners = new List<Player>();

            for (byte i = 0; i < players.Count; i++)
                if (players[i].power.Total == (HAND)1)
                    winners.Add(players[i]);

            if (winners.Count == 1) return winners;
            // сначала сравним карту двойки
            var o = winners.OrderBy(c => c.power.HighCombCard).Reverse();
            o.TakeWhile(с => с.power.HighCombCard == o.Max(s=>s.power.HighCombCard));

            if (o.Count() == 1) return o.ToList();

            o.OrderBy(c => c.power.HighKiker).Reverse();
            o.Where  (с => с.power.HighKiker == o.ElementAt(0).power.HighKiker);
            //если больше 2 штук то на борде и возвращаем всех 
            return o.ToList();
        }
        private List<Player> Is_Kiker()        
        {
            List<Player> winners = new List<Player>();

            for (byte i = 0; i < players.Count; i++)
                if (players[i].power.Total == (HAND)0)
                    winners.Add(players[i]);
            
            // сначала сравним карту двойки
            var o = winners.OrderBy(c => c.power.HighCombCard).Reverse();
            o.Where(с => с.power.HighCombCard == o.ElementAt(0).power.HighCombCard);

            return o.ToList();
            
            //если больше 2 штук то на борде и возвращаем всех 
          
        }
    }
}
