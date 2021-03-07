using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicTacToe.Core;

namespace TicTacToe.Gameplay.Players
{
    public abstract class GamePlayer
    {
        public abstract bool Move(out GameMove playedMove , PlayerSide side , GameGrid grid); 
    }
}