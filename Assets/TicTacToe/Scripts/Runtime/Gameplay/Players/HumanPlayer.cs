using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicTacToe.Core;

namespace TicTacToe.Gameplay.Players
{
    public class HumanPlayer : GamePlayer
    {
        public override bool Move(out GameMove playedMove , PlayerSide side , GameGrid grid)
        {
            if (GameInput.Moved())
            {
                playedMove = new GameMove(side, GameInput.MoveCoord.x, GameInput.MoveCoord.y);
                GameInput.ResetInput();
                return true;
            }
            playedMove = new GameMove();
            return false;
        }
    }
}