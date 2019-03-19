using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Texac_Poker
{   //-----------------------------------
    public enum HAND
    {
        Nothing=0,    //кикер
        OnePair,      //пара
        TwoPairs,     //две пары
        ThreeKind,    //тройка
        Straight,     //стрит
        Flush,        //флеш
        FullHouse,    //фулл-хаус
        FourKind,     //каре
        StraightFlush,//cтрит флеш
        RoyalFlush    //флеш рояль
    }
    //-----------------------------------
    public struct HandValue
    {
        public HAND Total;                     //Комбинация
        public uint HighCombCard { get; set; } //Наивысшая карта комбинации
        public uint HighKiker    { get; set; } //Наибольший кикер

        public List<Card> WinHand;             //Победная комбинация
        //---------------
        public HandValue(uint i = 0)
        {
            Total = (HAND) i;
            HighKiker    = i;
            HighCombCard = i;
            WinHand = new List<Card>(); }
        public void FillValue(HAND combo, uint max_combo_card, uint max_kiker, List<Card> win_stack)
        {
            Total = combo;
            HighCombCard = max_combo_card;
            HighKiker = max_combo_card;
            WinHand = win_stack; }
    }
    //-----------------------------------
    class HandEvaluator: Card
    {
        private int clubSum;    //♣
        private int heartsSum;  //♥
        private int spadesSum;  //♠
        private int diamondSum; //♦

        public Card[]    Cards;
        public HandValue handValue;
        public HandEvaluator(Card[] playerHand,Card[] dealerHand, int dealerCnt)
        {
               clubSum = 0;
             spadesSum = 0;
             heartsSum = 0;
            diamondSum = 0;
            
            Cards = new Card[playerHand.Count() + dealerCnt];
       
            for (byte i = 0; i < 2; i++)
                Cards[i] = playerHand[i];

            for (byte i = 2, j=0; i < dealerCnt + 2; i++,j++)
                Cards[i] = dealerHand[j];
           
            handValue = new HandValue();

            EvaluateHand();
        }
        //-----------------------------------
        public  void EvaluateHand()   
        {
            getNumberOfSuit();

            if (Cards.Count() == 2)
            {
                if (OnePair()) 
                    return;
                else{
                    Kiker();                               
                    return; } }
            if (Cards.Count() >= 5)
            {
                     if (RoyalFlush())    // 5 карт
                    return;
                else if (StraightFlush()) // 5 карт
                    return;
                else if (FourOfKind())    // 4 карты
                    return;
                else if (FullHouse())     // 5 карт
                    return;
                else if (Flush())         // 5 карт
                    return;
                else if (Straight())      // 5 карт
                    return;
                else if (ThreeOfKind())   // 3 карты
                    return;
                else if (TwoPairs())      // 4 карты
                    return;
                else if (OnePair())       // 2 карты
                    return;               
                else {                    // 1 картa
                    Kiker();
                    return; }
            }

        }//choosebest combination
        private void getNumberOfSuit()
        {
            foreach (var element in Cards)
            {
                     if (element.MySuit == Card.SUIT.HEARTS)
                    heartsSum++;
                else if (element.MySuit == Card.SUIT.DIAMONDS)
                    diamondSum++;
                else if (element.MySuit == Card.SUIT.CLUBS)
                    clubSum++;
                else if (element.MySuit == Card.SUIT.SPADES)
                    spadesSum++; } }
        //-----------------------------------
        // ПРОВЕРКА КОМБИНАЦИЙ ИГРОКА
        private bool RoyalFlush()    {
            // может лежать весь на борде тогда мерюются кикерами
            // если хоть одна карта на руке то победа
                 if (diamondSum >= 5) {
                 //--------------------
                     var watch = Cards.Where  (s => (int)s.MySuit == 3)
                                      .OrderBy(s =>  s.MyValue);
                     byte expected = 10;
                     foreach (var check in watch)
                     if ((int)check.MyValue == expected)
                         expected++;

                if (expected == 14) {
                    handValue.FillValue((HAND)9, 0, 0, watch.Reverse().Take(5).ToList());
                    return true; }  
                else return false;
                //--------------------
            }
            else if (heartsSum  >= 5){
                //--------------------
                var watch = Cards.Where(s => (int)s.MySuit == 1)
                                 .OrderBy(s => s.MyValue);
                byte expected = 10;
                foreach (var check in watch)
                    if ((int)check.MyValue == expected)
                        expected++;

                if (expected == 14){
                    handValue.FillValue((HAND)9, 0, 0, watch.Reverse().Take(5).ToList());
                    return true; }
                else return false;
                //--------------------
            }
            else if (spadesSum  >= 5){
                //--------------------
                var watch = Cards.Where(s => (int)s.MySuit == 2)
                                 .OrderBy(s => s.MyValue);
                byte expected = 10;
                foreach (var check in watch)
                    if ((int)check.MyValue == expected)
                        expected++;

                if (expected == 14){
                    handValue.FillValue((HAND)9, 0, 0, watch.Reverse().Take(5).ToList());
                    return true; }
                else return false;
                //--------------------
            }
            else if (clubSum    >= 5){
                //--------------------
                var watch = Cards.Where(s => (int)s.MySuit == 0)
                                 .OrderBy(s => s.MyValue);
                byte expected = 10;
                foreach (var check in watch)
                    if ((int)check.MyValue == expected)
                        expected++;

                if (expected == 14) { handValue.FillValue((HAND)9, 0, 0, watch.Reverse().Take(5).ToList());
                    return true; }
                else return false;
                //--------------------
            }
            else return false;

        }// non tested
        private bool StraightFlush() {
            //стрит будут различаться старшей картой!!!!

                 if (diamondSum >= 5){
                //--------------------
                var watch = Cards.Where  (s => (int)s.MySuit == 3)
                                 .OrderBy(s => s.MyValue).Reverse();

                for (byte i = 0; i < watch.Count() - 4; i++)
                    if (watch.ElementAt(i    ).MyValue == watch.ElementAt(i + 1).MyValue + 1 &&
                        watch.ElementAt(i + 1).MyValue == watch.ElementAt(i + 2).MyValue + 1 &&
                        watch.ElementAt(i + 2).MyValue == watch.ElementAt(i + 3).MyValue + 1 &&
                        watch.ElementAt(i + 3).MyValue == watch.ElementAt(i + 4).MyValue + 1 ){

                        handValue.FillValue((HAND)8, (uint)watch.ElementAt(i).MyValue, 0, watch.Skip(i).Take(5).ToList());
                        return true; }
                return false;
                //--------------------
            }
            else if (heartsSum  >= 5){
                //--------------------
                var watch = Cards.Where  (s => (int)s.MySuit == 1)
                                 .OrderBy(s => s.MyValue).Reverse();

                for (byte i = 0; i < watch.Count() - 4; i++)
                    if (watch.ElementAt(i).MyValue + 1 == watch.ElementAt(i + 1).MyValue &&
                        watch.ElementAt(i + 1).MyValue + 1 == watch.ElementAt(i + 2).MyValue &&
                        watch.ElementAt(i + 2).MyValue + 1 == watch.ElementAt(i + 3).MyValue &&
                        watch.ElementAt(i + 3).MyValue + 1 == watch.ElementAt(i + 4).MyValue){

                        handValue.FillValue((HAND)8, (uint)watch.ElementAt(i).MyValue, 0, watch.Skip(i).Take(5).ToList());
                        return true; }
                return false;
                //--------------------
            }
            else if (spadesSum  >= 5){
                //--------------------
                var watch = Cards.Where  (s => (int)s.MySuit == 2)
                                 .OrderBy(s => s.MyValue).Reverse();

                for (byte i = 0; i < watch.Count() - 4; i++)
                    if (watch.ElementAt(i).MyValue + 1 == watch.ElementAt(i + 1).MyValue &&
                        watch.ElementAt(i + 1).MyValue + 1 == watch.ElementAt(i + 2).MyValue &&
                        watch.ElementAt(i + 2).MyValue + 1 == watch.ElementAt(i + 3).MyValue &&
                        watch.ElementAt(i + 3).MyValue + 1 == watch.ElementAt(i + 4).MyValue){

                        handValue.FillValue((HAND)8, (uint)watch.ElementAt(i).MyValue, 0, watch.Skip(i).Take(5).ToList());
                        return true; }
                return false;
                //--------------------
            }
            else if (clubSum    >= 5){
                //--------------------
                var watch = Cards.Where  (s => (int)s.MySuit == 0)
                                 .OrderBy(s => s.MyValue).Reverse();

                for (byte i = 0; i < watch.Count() - 4; i++)
                    if (watch.ElementAt(i).MyValue + 1 == watch.ElementAt(i + 1).MyValue &&
                        watch.ElementAt(i + 1).MyValue + 1 == watch.ElementAt(i + 2).MyValue &&
                        watch.ElementAt(i + 2).MyValue + 1 == watch.ElementAt(i + 3).MyValue &&
                        watch.ElementAt(i + 3).MyValue + 1 == watch.ElementAt(i + 4).MyValue){

                        handValue.FillValue((HAND)8, (uint)watch.ElementAt(i).MyValue, 0, watch.Skip(i).Take(5).ToList());
                        return true; }
                return false;
                //--------------------
            }
            else return false;

        }// non tested
        private bool FourOfKind()    
        {   //если все на борде мереемся  рукой
            //если 2-3 на борде то то 1 победитель
            var sort = Cards.OrderBy(s => s.MyValue);

            for (byte i = 0; i < Cards.Count() - 4; i++)
                if ((sort.ElementAt(i  ).MyValue == sort.ElementAt(i + 1).MyValue) &&
                    (sort.ElementAt(i+1).MyValue == sort.ElementAt(i + 2).MyValue) &&
                    (sort.ElementAt(i+2).MyValue == sort.ElementAt(i + 3).MyValue)){
                    
                    handValue.FillValue((HAND)7, (uint)sort.ElementAt(i).MyValue, 0, sort.Skip(i).Take(4).ToList());
                    return true; }
            return false;
        }// non tested
        private bool FullHouse()     
        {   //еесли весь на борде делим выигрыш
            //иначе старшей картой Двойки или Тройки
            var sort = Cards.OrderBy(s => s.MyValue).Reverse();
            List<Card> result = new List<Card>();

            byte i = 0;
            for (; i < Cards.Count()-2; i++)
                if ((sort.ElementAt(i).MyValue == sort.ElementAt(i + 1).MyValue) &&
                    (sort.ElementAt(i).MyValue == sort.ElementAt(i + 2).MyValue)){

                    result.Add(sort.ElementAt(i));
                    result.Add(sort.ElementAt(i+1));
                    result.Add(sort.ElementAt(i+2));
                    i += 3;
                    break; }
            //---------------
            for (; i < Cards.Count() - 1; i++)
                if ((sort.ElementAt(i).MyValue == sort.ElementAt(i + 1).MyValue)){

                    result.Add(sort.ElementAt(i));
                    result.Add(sort.ElementAt(i + 1));
                    break;
                }
            //---------------
            if  (result.Count != 5)   return false;
            else handValue.FillValue((HAND)6,(uint)result[0].MyValue, (uint)result[4].MyValue, result);
                 return true;
        }// non tested
        private bool Flush()         
        {   //если весь  на борде - делим выигрыш
            //иначе всегда есть карта старше!!!
                 if (clubSum >= 5) {

                var sort = Cards.Where(s => (int)s.MySuit == 0).OrderBy(s=>s.MyValue).Reverse();
                handValue.FillValue((HAND)5, (uint)sort.First(s => s.MyValue==s.MyValue).MyValue, 0, sort.Take(5).ToList());
                return true; }
            else if (heartsSum  >= 5){

                var sort = Cards.Where(s => (int)s.MySuit == 1).OrderBy(s => s.MyValue).Reverse();
                handValue.FillValue((HAND)5, (uint)sort.First(s => s.MyValue == s.MyValue).MyValue, (uint)Cards.Take(2).Max(s => s.MyValue), sort.Take(5).ToList());
                return true;
            }
            else if (spadesSum  >= 5){

                var sort = Cards.Where(s => (int)s.MySuit == 2).OrderBy(s => s.MyValue).Reverse();
                handValue.FillValue((HAND)5, (uint)sort.First(s => s.MyValue == s.MyValue).MyValue, (uint)Cards.Take(2).Max(s => s.MyValue), sort.Take(5).ToList());
                return true;
            }
            else if (diamondSum >= 5){

                var sort = Cards.Where(s => (int)s.MySuit == 3).OrderBy(s => s.MyValue).Reverse();
                handValue.FillValue((HAND)5, (uint)sort.First(s => s.MyValue == s.MyValue).MyValue, (uint)Cards.Take(2).Max(s => s.MyValue), sort.Take(5).ToList());
                return true;
            }
            return false;
        }// non tested
        private bool Straight()      
        {   //eсли весь на борде - мерюются старшей картой. иначе 
            //cтрит определяется по старшей карте комбинации
            var sort = Cards.OrderBy(s => s.MyValue).Reverse();

            for (byte i = 0; i < sort.Count() - 4; i++)
                if (sort.ElementAt(i    ).MyValue == sort.ElementAt(i + 1).MyValue + 1 &&
                    sort.ElementAt(i + 1).MyValue == sort.ElementAt(i + 2).MyValue + 1 &&
                    sort.ElementAt(i + 2).MyValue == sort.ElementAt(i + 3).MyValue + 1 &&
                    sort.ElementAt(i + 3).MyValue == sort.ElementAt(i + 4).MyValue + 1){
                    
                    handValue.FillValue((HAND)4, (uint)sort.ElementAt(i).MyValue, 0, sort.Skip(i).Take(5).ToList());
                    return  true; }
            return false;
        }// non tested
        private bool ThreeOfKind()   
        {//если 3 на борде то мерюются кикерами в руке
         //если 2 на борде то 2 может иметь комбо  и тогда мерюются кикерами != 7
         //если 1 на борде то 1 может иметь комбо
            var sort = Cards.OrderBy(s => s.MyValue).Reverse();

            for (byte i = 0; i < Cards.Count() -2 ; i++)
                if ((sort.ElementAt(i).MyValue == sort.ElementAt(i + 1).MyValue) &&
                    (sort.ElementAt(i).MyValue == sort.ElementAt(i + 2).MyValue)){
                    //если выбрали 2 - 3 на борде
                    //             1 - 2 на борде
                    //             0 - 3 на борде
                    handValue.FillValue((HAND)3, (uint)sort.ElementAt(i).MyValue, (uint)sort.First(s => s.MyValue != sort.ElementAt(i).MyValue).MyValue, sort.Skip(i).Take(3).ToList());
                    return true; }
            return false;
        }// non tested
        private bool TwoPairs()      
        {
            var sort = Cards.OrderBy(s => s.MyValue).Reverse();
            List<Card> twoPair = new List<Card>();

            for (byte i = 0; i < sort.Count() - 1; i++)
                if (sort.ElementAt(i).MyValue == sort.ElementAt(i+1).MyValue && (twoPair.Count != 4)){

                    twoPair.Add(sort.ElementAt(i));
                    twoPair.Add(sort.ElementAt(i + 1));}
          
            if (twoPair.Count == 4){                //Большее                   и   Меньшее значчение
                handValue.FillValue((HAND)2, (uint)twoPair.ElementAt(0).MyValue, (uint)twoPair.ElementAt(3).MyValue, twoPair.ToList());
                 return true; }
            else return false;
        }// non tested
        private bool OnePair()       {
            //если на борде то кикер старший
            //если 1 на борде то могут 3 победителя и 
            //если в руке то 2 победителя, равный исход
            var sort = Cards.OrderBy(s => s.MyValue).Reverse();

            for (byte i = 0; i < sort.Count() - 1; i++)
                if (sort.ElementAt(i).MyValue == sort.ElementAt(i + 1).MyValue){
                    if (sort.Count() != 2)
                        handValue.FillValue((HAND)1, (uint)sort.ElementAt(i).MyValue, (uint)sort.Where(s => s.MyValue != sort.ElementAt(i).MyValue).First(s=>true).MyValue, sort.Skip(i).Take(2).ToList());
                    else
                        handValue.FillValue((HAND)1, (uint)sort.ElementAt(i).MyValue, 0, sort.Skip(i).Take(2).ToList());

                    return true; }
            return false;
        }// non tested
        private bool Kiker()         {

            handValue.FillValue((HAND)0, 0, (uint)Cards.Take(7).Max(s=>s.MyValue),
                Cards.Take(2).OrderBy(s => s.MyValue).Reverse().Take(1).ToList());
            return true;
           
        }// non tested
        //-----------------------------------
    }
}        
