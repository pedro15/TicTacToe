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

        public delegate void d_OnTurnChanged(PlayerSide side);
        public event d_OnTurnChanged OnTurnChanged;

        public PlayerSide currentTurn { get; private set; } = PlayerSide.Player_O;

        public GameGrid grid { get; private set; }

        private HumanPlayer Player_O;
        private HumanPlayer Player_X;

        private GamePlayer selectedPlayer;
        
        public Game()
        {
            grid = new GameGrid();
            Player_O = new HumanPlayer(PlayerSide.Player_O);
            Player_X = new HumanPlayer(PlayerSide.Player_X);

            selectedPlayer = Player_O;

            Timing.RunCoroutine(I_WaitForPlayer(), K_WaitPlayer);
        }

        private IEnumerator<float> I_WaitForPlayer()
        {
            while(true)
            {
                if(selectedPlayer.Move(out GameMove movement))
                {
                    grid.PlacePlayerOnSquare(movement.x , movement.y , movement.side);

                    if (OnMovementMade != null) OnMovementMade.Invoke(movement);

                    yield return Timing.WaitForSeconds(0.2f);

                    // Check for win

                    if (grid.IsPlayerWinner(currentTurn))
                    {
                        Debug.Log("Game Over. Winner: " + currentTurn);
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

                    yield return Timing.WaitForSeconds(1f);

                    if (OnTurnChanged != null) OnTurnChanged.Invoke(currentTurn);
                }
                yield return Timing.WaitForOneFrame;
            }
            Timing.KillCoroutines(K_WaitPlayer);
            yield break;
        }

    }
}