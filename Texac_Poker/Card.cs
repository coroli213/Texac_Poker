using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Texac_Poker
{
    public class Card //одна карта представляет собой
    {      
        public enum SUIT
        {
            CLUBS,  //♣
            HEARTS, //♥
            SPADES, //♠
            DIAMONDS//♦
        }
        public enum VALUE:byte
        {
            TWO = 2, THREE, FOUR, FIVE, SIX,   SEVEN,
            EIGHT,   NINE,  TEN,  JACK, QUEEN, KING, ACE
        }
        public SUIT  MySuit  { get; set; }
        public VALUE MyValue { get; set; }
    }
}
