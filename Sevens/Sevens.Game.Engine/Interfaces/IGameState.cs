using Sevens.Game.Engine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sevens.Game.Engine.Interfaces
{
    public interface IGameState
    {
        Task<GameOverModel> IsGameOver();
        void AddMove(MoveHistory move);
        Task<IDictionary<int, MoveHistory>> GetHistory();
        Task<MoveHistory> GetHistory(int index);
    }
}
