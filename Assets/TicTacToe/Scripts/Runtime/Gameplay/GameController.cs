using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using TicTactoe.Gameplay.Players;
using Debug = UnityEngine.Debug;

namespace TicTactoe.Gameplay
{
    public enum GameState : int
    {
        None = 0,
        Playing = 1,
        GameOver = 2
    }

    public class GameController 
    {
        private const string K_ProcessGame = "GAME_PROCESS";
        public delegate void d_OnGameFinish(PlayerSide winner);
        public event d_OnGameFinish OnGameFinish;
        public delegate void d_OnMovementMade(PlayerSide player , Vector2Intx _position);
        public event d_OnMovementMade OnMovementMade;
        public delegate void d_OnTurnChanged(PlayerSide turn);
        public event d_OnTurnChanged OnTurnChanged;
        public GameState CurrentState { get; private set; } = GameState.None;
        public Board board { get; private set; }
        public PlayerSide CurrentTurn { get; private set; } = PlayerSide.None;        
        private GamePlayer Player_O = null;
        private GamePlayer Player_X = null;
        private readonly PlayerSide initialTurn;

        private GamePlayer currentPlayer
        {
            get 
            {
                if(CurrentTurn == PlayerSide.Player_O)
                    return Player_O;
                else if (CurrentTurn == PlayerSide.Player_X)
                    return Player_X;

                return null;
            }
        }

        public GameController(GamePlayer _player_O , GamePlayer _player_X , PlayerSide _firstTurn)
        {
            board = new Board();
            initialTurn = _firstTurn;
            Player_O = _player_O;
            Player_O.Init(PlayerSide.Player_O);
            Player_X = _player_X;
            Player_X.Init(PlayerSide.Player_X);
            CurrentTurn = _firstTurn;
            initialTurn = _firstTurn;
        }

        public GameController(GamePlayer _player_O , GamePlayer _player_X, PlayerSide _firstTurn, string _BoardNotation)
        {
            board = new Board(_BoardNotation);
            initialTurn = _firstTurn;
            Player_O = _player_O;
            Player_O.Init(PlayerSide.Player_O);
            Player_X = _player_X;
            Player_X.Init(PlayerSide.Player_X);
            CurrentTurn = _firstTurn;
            initialTurn = _firstTurn;
        }

        // ==================== Public API

        public void StartGame()
        {
            CurrentState = GameState.Playing;
            Timing.RunCoroutine(I_ProcessGame() , K_ProcessGame);
        }

        public void ResetGame()
        {
            if (CurrentState != GameState.Playing)
            {
                Timing.RunCoroutine(I_ProcessGame(), K_ProcessGame);
                CurrentState = GameState.Playing;
            }
            CurrentTurn = initialTurn;
            board.Reset();
        }

        public void NewButtonTap(Vector2Intx _position)
        {
            currentPlayer.OnBoardButtonTap(_position);
        }

        // ==================== Private API

        private void SwitchTurn()
        {
            CurrentTurn = (CurrentTurn == PlayerSide.Player_O) ? PlayerSide.Player_X : PlayerSide.Player_O;
            currentPlayer.OnEnterTurn();
            if (OnTurnChanged != null ) OnTurnChanged.Invoke(CurrentTurn);
        }

        private IEnumerator<float> I_ProcessGame()
        {
            while(true)
            {
                if(currentPlayer == null ) throw new System.ArgumentException("Null Player!");
                if (currentPlayer.GetMove(out Vector2Intx _movePosition , board))
                {
                    if(board.MakeMove(_movePosition , CurrentTurn))
                    {
                        if(OnMovementMade != null) OnMovementMade.Invoke(CurrentTurn , _movePosition);
                        if (board.IsTerminal(out PlayerSide _winner))
                        {
                            Debug.LogWarning("Terminal board. Game Over, winner: " + _winner);
                            if (OnGameFinish != null) OnGameFinish.Invoke(_winner);
                            CurrentState = GameState.GameOver;
                            break;
                        }
                        SwitchTurn();
                    }else 
                    {
                        Debug.LogWarning("Invalid move!");
                        currentPlayer.OnEnterTurn();
                    }
                }
                yield return Timing.WaitForOneFrame;
            }

            Timing.KillCoroutines(K_ProcessGame);
            yield break;
        }

    }
}