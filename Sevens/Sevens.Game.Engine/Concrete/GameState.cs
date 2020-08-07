using Sevens.Game.Engine.Interfaces;
using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sevens.Game.Engine.Concrete
{
    public class GameState : IGameState
    {
        public IList<IPlayer> Players { get;  }

        private IDictionary<int, MoveHistory> _history;
        private int turnNumber = 1;
        public GameState(IList<IPlayer> players)
        {
            _history = new Dictionary<int, MoveHistory>();
            Players = players;
        }

        public async Task<GameOverModel> IsGameOver()
        {
            GameOverModel result = new GameOverModel
            {
                IsGameOver = false
            };

            if (Players.Any(x => x.HasPlayerWon()))
            {
                result.IsGameOver = true;
                result.Player = Players.Where(x => x.HasPlayerWon()).First();
            }

            return await Task.FromResult(result);
        }

        public void AddMove(MoveHistory move)
        {
            if (move == null) throw new ArgumentNullException("move");

            _history.Add(turnNumber++, move);
        }

        public async Task<IDictionary<int, MoveHistory>> GetHistory()
        {
            return await Task.FromResult(_history);
        }

        public async Task<MoveHistory> GetHistory(int index)
        {
            if (index < 0 || index > (_history.Count - 1)) throw new ArgumentOutOfRangeException("index");

            return await Task.FromResult(_history[index]);
        }
    }
}
