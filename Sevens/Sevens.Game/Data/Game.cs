using Sevens.Game.Engine.Enums;
using Sevens.Game.Engine.Interfaces;
using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sevens.Game.Data
{
    public class Game
    {
        public IDictionary<Suits, InGameSuit> BoardState { get; set; }
        public GameOverModel GameOver { get; set; }
        public IList<Card> PlayerHand { get; set; }
        public IDictionary<int, MoveHistory> History { get; set; }
        public IPlayer NextPlayer { get; set; }
    }
}
