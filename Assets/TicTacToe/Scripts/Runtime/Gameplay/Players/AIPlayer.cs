using System.Collections;
using System.Collections.Generic;
using TicTacToe.Core;
using Debug = UnityEngine.Debug;
using System.Threading.Tasks;
using System;

namespace TicTacToe.Gameplay.Players
{
    public class AIPlayer : GamePlayer
    {
        private bool ai_running = false;
        private bool ai_done = false;

        Tuple<int, GameMove> result = null;

        public override bool Move(out GameMove selectedMove, PlayerSide side , GameGrid grid)
        {
            if (!ai_running && !ai_done)
            {
                ExecuteAI(side, grid);
                ai_running = true;
                selectedMove = GameMove.Invalid;
                return false;
            }else if (ai_done)
            {
                Debug.Log("Done!");
                selectedMove = result.Item2;
                ai_done = false;
                ai_running = false;
                result = null;
                return true;
            }

            selectedMove = GameMove.Invalid;
            return false;
        }

        private async void ExecuteAI(PlayerSide side, GameGrid grid)
        {
            Debug.Log("Execute AI");
            Tuple<int,GameMove> bestMove = await Task.Run(() => Negamax(GetColorFromSide(side) ,1, new GameGrid(grid.cells)));
            Debug.Log("Result: " + bestMove.Item1);
            ai_done = true;
            result = bestMove;
        }

        private int GetColorFromSide(PlayerSide side)
        {
            if (side == PlayerSide.Player_O)
                return 1;
            else if (side == PlayerSide.Player_X)
                return -1;

            return 0;
        }

        private PlayerSide GetSideFromColor(int color)
        {
            if (color == 1)
                return PlayerSide.Player_O;
            else if (color == -1)
                return PlayerSide.Player_X;

            return PlayerSide.None;
        }

        private async Task<Tuple<int , GameMove>> Negamax(int color , int depth, GameGrid grid)
        {
            if (grid.IsTerminal(out PlayerSide winner))
            {
                int wincolor = GetColorFromSide(winner);

                if (wincolor == 0) return new Tuple<int, GameMove>(0,GameMove.Invalid);

                if (wincolor == color)
                    return new Tuple<int, GameMove>(color * 100, GameMove.Invalid);
                else
                    return new Tuple<int, GameMove>(color * -100, GameMove.Invalid);
            }

            List<GameMove> moves = grid.GetPossibleMoves(GetSideFromColor(color));

            int bestScore = int.MinValue;
            GameMove selected = GameMove.Invalid;

            for (int i = 0; i < moves.Count; i++)
            {
                GameMove current = moves[i];

                if (grid.PlacePlayerOnSquare(current))
                {
                    grid.UnMakeMove(current.x, current.y);
                    Tuple<int, GameMove> result = await Negamax(-color , depth -1, grid);
                    int score = -result.Item1;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        selected = current;
                    }
                }
            }
            return new Tuple<int, GameMove>(bestScore, selected);
        }


    }
}