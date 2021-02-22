using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicTacToe.Core;

namespace TicTacToe.Gameplay.Players
{
    public abstract class GamePlayer
    {
        public readonly PlayerSide side;
        public abstract bool Move(out GameMove playedMove);
        
        public GamePlayer(PlayerSide _side)
        {
            side = _side;
        }
    }
}