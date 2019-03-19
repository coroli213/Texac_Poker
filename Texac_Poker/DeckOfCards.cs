using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Texac_Poker
{  
    class DeckOfCards : Card //Колода карт, перемешивание
    {
        const int NUM_OF_CARDS = 52;
        public Card[] Deck { get; }

        public DeckOfCards(){ Deck = new Card[NUM_OF_CARDS]; }
        //create deck 52 cards: 13 Values each, with 4 suits and shuffle
        public void setUpDeck()   
        {
            int i = 0;
            foreach (SUIT  s in Enum.GetValues(typeof(SUIT)))
            foreach (VALUE v in Enum.GetValues(typeof(VALUE))){
                    //--------------------------------------------
                    Deck[i] = new Card { MySuit = s, MyValue = v };

                    i++; }
            //--------------------------------------------
            ShuffleCards();
        }
        public void ShuffleCards()
        {
            Random rand = new Random();
            Card temp;

            //shuffle 1000 times
            for (int shuffle_count = 0; shuffle_count < 1000; shuffle_count++)
            {
                for (int i = 0; i < NUM_OF_CARDS; i++)
                {   //swap the card
                    int secondCardIndex = rand.Next(13);
                    temp = Deck[i];
                    Deck[i] = Deck[secondCardIndex];
                    Deck[secondCardIndex] = temp; } }   
            //---------------------------------------
        }
        //-------------------------------------------
}   }
