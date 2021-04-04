using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;
using Math = System.Math;

namespace TicTactoe.Gameplay.Players
{
    public class AIPlayer : GamePlayer
    {
        private bool ai_running;
        private bool completed;
        private Vector2Intx choosenMove;

        public override bool GetMove(out Vector2Intx _movePosition, Board _board)
        {
            if (!ai_running && !completed)
            {
                RunAI(_board);
            }
            _movePosition = choosenMove;
            return !ai_running && completed;
        }

        public override void OnEnterTurn()
        {
            completed = false;
            ai_running = false;
            choosenMove = Vector2Intx.one * -1;
        }

        private int GetColor(PlayerSide side)
        {
            if (side == PlayerSide.Player_O)
                return 1;
            else if (side == PlayerSide.Player_X)
                return -1;

            return 0;
        }

        private PlayerSide GetSide(int color)
        {
            if (color == -1)
                return PlayerSide.Player_X;
            else if (color == 1)
                return PlayerSide.Player_O;

            return PlayerSide.None;
        }
        
        private int Evaluate(int color , int winner)
        {
            return (100 * winner) * color;
        }

        private async void RunAI(Board board)
        {
            Debug.Log("Run AI");
            ai_running = true;
            choosenMove = await Task.Run(() => ChooseBestMove(currentSide, board));
            completed = true;
            ai_running = false;
            Debug.Log("AI choosen completed " + choosenMove);
        }

        private async Task<Vector2Intx> ChooseBestMove(PlayerSide side , Board board)
        {
            List<Vector2Intx> moves = board.GetFreePositions();
            int best = int.MinValue;
            Vector2Intx best_move = Vector2Intx.one * -1;
            for (int i = 0; i < moves.Count; i++)
            {
                Vector2Intx currentMove = moves[i];
                if (board.MakeMove(currentMove , side))
                {
                    int score = await Negamax(board , GetColor(side) , 10 , int.MinValue , int.MaxValue);
                    if (score > best)
                    {
                        best = score;
                        best_move = currentMove;
                    }
                    board.UnMakeMove(currentMove);
                }
            }
            return best_move;
        }

        private async Task<int> Negamax(Board board , int color , int depth, int alpha, int beta)
        {
            PlayerSide currentSide = GetSide(color);
            if (board.IsTerminal(out PlayerSide winner ,currentSide) || depth == 0)
            {
                Debug.Log("Is terminal :: " + GetSide(color) + " Winner " + winner + " Board " + board.Serialize());
                return Evaluate(color , GetColor(winner));
            }

            List<Vector2Intx> moves = board.GetFreePositions();

            int best = int.MinValue;
            for (int i = 0; i < moves.Count; i++)
            {
                Vector2Intx current = moves[i];
                if(board.MakeMove(current, currentSide))
                {
                    int current_score = await Negamax(board , -color , depth - 1 , -beta , -alpha) * -1 ;
                    Debug.Log("Current score" + current_score);
                    best = Math.Max(best , current_score);
                    alpha = Math.Max(current_score , alpha);
                    board.UnMakeMove(current);
                    if(alpha >= beta ) break; 
                }
            }

            return best;
        }
    }
}