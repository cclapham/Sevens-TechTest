using Sevens.Game.Engine.Concrete;
using Sevens.Game.Engine.Enums;
using Sevens.Game.Engine.Interfaces;
using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sevens.Game.Data
{
    public class GameService
    {
        private IList<IPlayer> _players;

        private IGameBoard _board;
        private IDeck _deck;
        private IGameState _state;
        private IHumanPlayer _humanPlayer;

        public IPlayer CurrentPlayer;
        

        public GameService(IGameBoard board, IDeck deck)
        {
            _board = board;
            _deck = deck;
        }

        public async Task<Game> SetupGameAsync()
        {
            var boardState = _board.ResetGameBoardState();

            //Create a new game with 3 players, 1 himan 1 unskilled AI and 1 skilled AI.
            _players = new List<IPlayer>
            {
                new HumanPlayer(_board),
                new SkilledPlayer(_board),
                new UnSkilledPlayer(_board)
            };

            _state = new GameState(_players);

            _deck.Shuffle();
            _deck.Deal(ref _players);

            //Find out which player goes first and set as current player
            CurrentPlayer = _players.Where(x => x.HasSevenDiamonds()).First();
            _humanPlayer = _players.First(x => x.IsHuman()) as IHumanPlayer;

            var response = new Game
            {
                BoardState = boardState,
                History = await _state.GetHistory(),
                GameOver = await _state.IsGameOver(),
                PlayerHand = _humanPlayer.ShowHand(),
                NextPlayer = CurrentPlayer
            };


            return response;
        }

        public async Task<Game> StartGame()
        {
            var boardState = _board.GetGameBoardState();

            CurrentPlayer.FirstTurn(_state);

            var response = new Game
            {
                BoardState = boardState,
                History = await _state.GetHistory(),
                GameOver = await _state.IsGameOver(),
                PlayerHand = _humanPlayer.ShowHand(),
                NextPlayer = await GetNextPlayerAsync()
            };

            CurrentPlayer = response.NextPlayer;

            return response;
        }

        private async Task<IPlayer> GetNextPlayerAsync()
        {
            try
            {
                int nextPlayerIndex = (_players.IndexOf(CurrentPlayer) + 1) % _players.Count();

                return await Task.FromResult(_players.ElementAt(nextPlayerIndex));
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Game> PlayGame()
        {
            try
            {
                var player = (IAIPlayer)CurrentPlayer;

                player.TakeTurn(_state);

                var response = new Game
                {
                    BoardState = _board.GetGameBoardState(),
                    History = await _state.GetHistory(),
                    GameOver = await _state.IsGameOver(),
                    PlayerHand = _humanPlayer.ShowHand(),
                    NextPlayer = await GetNextPlayerAsync()
                };

                CurrentPlayer = response.NextPlayer;

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Game> PlayGame(Card? card)
        {
            try
            {
                var player = (IHumanPlayer)CurrentPlayer;

                player.TakeTurn(card, _state);

                var response = new Game
                {
                    BoardState = _board.GetGameBoardState(),
                    History = await _state.GetHistory(),
                    GameOver = await _state.IsGameOver(),
                    PlayerHand = _humanPlayer.ShowHand(),
                    NextPlayer = await GetNextPlayerAsync()
                };

                CurrentPlayer = response.NextPlayer;

                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
