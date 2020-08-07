using Microsoft.Extensions.Caching.Memory;
using Sevens.Game.Engine.Enums;
using Sevens.Game.Engine.Extensions;
using Sevens.Game.Engine.Interfaces;
using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sevens.Game.Engine.Concrete
{
    public class GameBoard : IGameBoard
    {
        private IMemoryCache _memoryCache;
        private readonly string _gameStateCacheKey = "GameStateKey";

        public GameBoard(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IDictionary<Suits, InGameSuit> GetGameBoardState()
        {
            if(!_memoryCache.TryGetValue(_gameStateCacheKey, out IDictionary<Suits, InGameSuit> state))
            {
                //Create a new GameState and add it to the cache
                state = ResetGameBoardState();

                _memoryCache.Set(_gameStateCacheKey, state);
            }

            return state;
        }

        public IDictionary<Suits, InGameSuit> ResetGameBoardState()
        {
            // Each suit has a state containing highest and lowest card value currently in play or bool for unopened suit, i.e. the seven isn't in play.
            var diamonds = new InGameSuit(Suits.Diamonds);
            var spades = new InGameSuit(Suits.Spades);
            var hearts = new InGameSuit(Suits.Hearts);
            var clubs = new InGameSuit(Suits.Clubs);

            var state = new Dictionary<Suits, InGameSuit>
            {
                {Suits.Diamonds, diamonds },
                {Suits.Spades, spades },
                {Suits.Hearts, hearts },
                {Suits.Clubs, clubs }
            };

            _memoryCache.Set(_gameStateCacheKey, state);

            return state;
        }

        public void SetGameBoardState(Card card)
        {
            if (card == null) throw new ArgumentNullException();

            var state = GetGameBoardState();

            var suitState = state[card.Suit];


            //Check if the card is eligible to be played
            if (!suitState.IsOpen)
            {
                if(card.Value == CardValue.Seven)
                { 
                    //We are good to play
                    suitState.IsOpen = true;
                    suitState.LowValue = card.Value;
                    suitState.HighValue = card.Value;


                    _memoryCache.Set(_gameStateCacheKey, state);

                    return;
                }
                else
                {
                    cardError(card);
                }
            }
            var cardValue = (int)card.Value;
            var nextHighCard = (int)suitState.HighValue + 1;
            var nextLowCard = (int)suitState.LowValue - 1;
            
            if(cardValue == nextHighCard)
            {
                //We are good to play
                suitState.HighValue = card.Value;
            }
            else if(cardValue == nextLowCard)
            {
                //We are good to play
                suitState.LowValue = card.Value;
            }
            else
            {
                cardError(card);
            }

            _memoryCache.Set(_gameStateCacheKey, state);
        }

        public bool IsEligible(Card card)
        {
            if (card == null) throw new ArgumentNullException();

            var state = GetGameBoardState();

            var suitState = state[card.Suit];


            //Check if the card is eligible to be played
            if (!suitState.IsOpen)
            {
                if(card.Value == CardValue.Seven) return true;

                return false;
            }

            var cardValue = (int)card.Value;
            var nextHighCard = (int)suitState.HighValue + 1;
            var nextLowCard = (int)suitState.LowValue - 1;

            if (cardValue == nextHighCard)
            {
                return true;
            }

            if (cardValue == nextLowCard)
            {
                return true;
            }

            return false;
        }

        public bool IsFirstTurn()
        {
            var state = GetGameBoardState();

            if (state[Suits.Diamonds].IsOpen)
                return false;
            return true;
        }
        private void cardError(Card card)
        {
            throw new UnableToPlayCardException($"The card \"{card.GetFriendlyName()}\" cannot be played at this time");
        }


    }
}
