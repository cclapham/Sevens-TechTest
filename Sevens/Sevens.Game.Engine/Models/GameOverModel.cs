using Sevens.Game.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sevens.Game.Engine.Models
{
    public class GameOverModel
    {
        public IPlayer Player { get; set; }
        public bool IsGameOver { get; set; }
    }
}
