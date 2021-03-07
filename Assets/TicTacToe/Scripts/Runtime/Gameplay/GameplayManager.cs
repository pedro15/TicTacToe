using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TicTacToe.Core;
using TicTacToe.Gameplay.GUI;
using TicTacToe.Gameplay.Players;

namespace TicTacToe.Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField]
        private Sprite Icon_X = default;
        [SerializeField]
        private Sprite Icon_O = default;
        [SerializeField]
        private Sprite Icon_None = default;
        [SerializeField]
        private GameButton[] buttons = default;

        public Game game { get; private set; }

        private void Start()
        {
            game = new Game(new HumanPlayer() , new AIPlayer());
            int[,] cells = game.grid.cells;

            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    int index = (y * cells.GetLength(1)) + x;
                    buttons[index].Init(new Vector2Int(x, y));
                }
            }

            game.OnMovementMade += Game_OnMovementMade;
        
        }

        private void Game_OnMovementMade(GameMove move)
        {
            int index = (move.y * game.grid.cells.GetLength(1)) + move.x;
            switch (move.side)
            {
                case PlayerSide.None:
                    buttons[index].Graphic.sprite = Icon_None;
                    break;
                case PlayerSide.Player_O:
                    buttons[index].Graphic.sprite = Icon_O;
                    buttons[index].Graphic.color = Color.green;
                    break;
                case PlayerSide.Player_X:
                    buttons[index].Graphic.sprite = Icon_X;
                    buttons[index].Graphic.color = Color.red;
                    break;
            }
        }
    }
}