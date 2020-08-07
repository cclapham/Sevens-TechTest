using Sevens.Game.Engine.Enums;
using Sevens.Game.Engine.Interfaces;
using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sevens.Game.Engine.Concrete
{
    public class SkilledPlayer : PlayerBase, IAIPlayer
    {
        public SkilledPlayer(IGameBoard board) : base(board)
        {
        }

        public void TakeTurn(IGameState state)
        {
            //For simplicity the skill we assume is that this player will rank it's cards and play the card with the highest score

            /******************************
             * ** card scoring rules     **
             * ****************************
             * 
             * Suit with most cards is highest value suit
             * scores 4-1
             * 
             * add count below 7 vs count above 7
             * 
             * */

            IDictionary<Card, int> scores = CalculateScore();

            //Now we have our dictionary of best cards we need to find the highest scoring eligible card and play it
            var bestCards = scores.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();

            var move = new MoveHistory
            {
                Player = this
            };
            foreach(var card in bestCards)
            {
                if(_board.IsEligible(card))
                {
                    //Play the card
                    _board.SetGameBoardState(card);
                    UpdateHand(card, true);
                    move.move = card;

                    state.AddMove(move);
                    return;
                }
            }

            //if no cards are eligible, no cards are played.
            state.AddMove(move);
        }


        private IDictionary<Card, int> CalculateScore()
        {
            //Add hand to dictionary with a score of 0
            var result = _hand.Select(x => new { Key = x, Value = 0 }).ToDictionary(x => x.Key, x => x.Value);

            var diamonds = _hand.Where(x => x.Suit == Suits.Diamonds).ToList();
            var spades = _hand.Where(x => x.Suit == Suits.Spades).ToList();
            var hearts = _hand.Where(x => x.Suit == Suits.Hearts).ToList();
            var clubs = _hand.Where(x => x.Suit == Suits.Clubs).ToList();

            int dScore = 0;
            int sScore = 0;
            int hScore = 0;
            int cScore = 0;

            //Each suit has a score of 1 then gets +1 for every suit it beats
            CompareCounts(diamonds.Count(), spades.Count(), ref dScore, ref sScore);
            CompareCounts(diamonds.Count(), hearts.Count(), ref dScore, ref hScore);
            CompareCounts(diamonds.Count(), clubs.Count(), ref dScore, ref cScore);

            CompareCounts(spades.Count(), hearts.Count(), ref sScore, ref hScore);
            CompareCounts(spades.Count(), clubs.Count(), ref sScore, ref cScore);

            CompareCounts(hearts.Count(), clubs.Count(), ref hScore, ref cScore);

            //Update results
            foreach(var card in diamonds)
            {
                result[card] += dScore;
            }
            foreach (var card in spades)
            {
                result[card] += sScore;
            }
            foreach (var card in hearts)
            {
                result[card] += hScore;
            }
            foreach (var card in clubs)
            {
                result[card] += cScore;
            }

            //Now we have a suit based score go through each suit and give it a count above/below 7 score
            //Diamonds
            var db7 = diamonds.Where(x => (int)x.Value <= 7).ToList();
            var da7 = diamonds.Where(x => (int)x.Value >=7 ).ToList();
            
            //Spades
            var sb7 = spades.Where(x => (int)x.Value <= 7).ToList();
            var sa7 = spades.Where(x => (int)x.Value >= 7).ToList();

            //Hearts
            var hb7 = hearts.Where(x => (int)x.Value <= 7).ToList();
            var ha7 = hearts.Where(x => (int)x.Value >= 7).ToList();

            //Clubs
            var cb7 = clubs.Where(x => (int)x.Value <= 7).ToList();
            var ca7 = clubs.Where(x => (int)x.Value >= 7).ToList();

            //Update results
            foreach(var card in db7)
            {
                result[card] += db7.Count();
            }
            foreach (var card in da7)
            {
                result[card] += da7.Count();
            }

            foreach (var card in sb7)
            {
                result[card] += sb7.Count();
            }
            foreach (var card in sa7)
            {
                result[card] += sa7.Count();
            }

            foreach (var card in hb7)
            {
                result[card] += hb7.Count();
            }
            foreach (var card in ha7)
            {
                result[card] += ha7.Count();
            }

            foreach (var card in cb7)
            {
                result[card] += cb7.Count();
            }
            foreach (var card in ca7)
            {
                result[card] += ca7.Count();
            }

            return result;
        }

        private void CompareCounts(int suit1, int suit2, ref int suit1Score, ref int suit2Score)
        {
            if (suit1 == suit2)
                return;

            if (suit1 > suit2)
                suit1Score++;
            else
                suit2Score++;
        }

        public override string Name()
        {
            return "Skilled AI";
        }
    }
}
