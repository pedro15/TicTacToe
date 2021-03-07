using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using TicTacToe.Gameplay;
using TicTacToe.Gameplay.Players;

namespace TicTacToe.Core
{
    public class Game
    {
        private const string K_WaitPlayer = "WAIT_FOR_PLAYER_INPUT";

        public delegate void d_OnMovementMade(GameMove move);
        public event d_OnMovementMade OnMovementMade;

        public delegate void d_OnGameOver(PlayerSide winner);
        public event d_OnGameOver OnGameOver;

        public PlayerSide currentTurn { get; private set; } = PlayerSide.Player_O;

        public GameGrid grid { get; private set; }

        private GamePlayer Player_O;
        private GamePlayer Player_X;

        private GamePlayer selectedPlayer;

        public Game(GamePlayer _Player_O , GamePlayer _Player_X)
        {
            grid = new GameGrid();
            Player_O = _Player_O;
            Player_X = _Player_X;

            selectedPlayer = Player_O;

            Timing.RunCoroutine(I_WaitForPlayer(), K_WaitPlayer);
        }

        private IEnumerator<float> I_WaitForPlayer()
        {
            while(true)
            {
                if (selectedPlayer.Move(out GameMove movement , currentTurn , grid))
                {
                    if (grid.cells[movement.x, movement.y] != 0)
                    {
                        Debug.LogWarning($"Invalid move! : {movement.x},{movement.y} == {grid.cells[movement.x,movement.y]}");
                        continue;
                    }

                    grid.PlacePlayerOnSquare(movement.x, movement.y, movement.side);
                    if (OnMovementMade != null) OnMovementMade.Invoke(movement);


                    // Check for win

                    if (grid.IsPlayerWinner(currentTurn))
                    {
                        Debug.Log("Game Over. Winner: " + currentTurn);
                        if (OnGameOver != null) OnGameOver.Invoke(currentTurn);
                        break;
                    }

                    // Check for draw

                    if (grid.EmptySpacesCount() == 0)
                    {
                        Debug.Log("Is a draw");
                        if (OnGameOver != null) OnGameOver.Invoke(PlayerSide.None);
                        break;
                    }

                    // Switch turn

                    if (currentTurn == PlayerSide.Player_O)
                    {
                        selectedPlayer = Player_X;
                        currentTurn = PlayerSide.Player_X;
                    }
                    else if (currentTurn == PlayerSide.Player_X)
                    {
                        selectedPlayer = Player_O;
                        currentTurn = PlayerSide.Player_O;
                    }
                }
                yield return Timing.WaitForOneFrame;
            }
            Timing.KillCoroutines(K_WaitPlayer);
            yield break;
        }

    }
}