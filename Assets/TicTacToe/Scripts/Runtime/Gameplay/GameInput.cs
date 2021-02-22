using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicTacToe.Core;

namespace TicTacToe.Gameplay
{
    public static class GameInput
    {
        public static Vector2Int MoveCoord { get; private set; }

        private static bool IsMoved = false;

        public static void TriggerMoveSquare(Vector2Int coord)
        {
            MoveCoord = coord;
            IsMoved = true;
        }

        public static bool Moved() => IsMoved; 

        public static void ResetInput()
        {
            MoveCoord = Vector2Int.zero;
            IsMoved = false;
        }
    }
}