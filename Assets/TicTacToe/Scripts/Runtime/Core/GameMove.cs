using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe.Core
{
    public struct GameMove
    {
        public readonly PlayerSide side;
        public readonly int x;
        public readonly int y;

        public GameMove(PlayerSide _side, int _x , int _y)
        {
            side = _side;
            x = _x;
            y = _y;
        }

        public bool IsValid()
        {
            return x > 0 && y > 0 && side != PlayerSide.None;
        }

        public static GameMove Invalid => new GameMove(PlayerSide.None, -1, -1);
    }
}