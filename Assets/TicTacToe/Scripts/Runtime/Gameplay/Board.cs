using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace TicTactoe.Gameplay
{
    public enum PlayerSide : int
    {
        None = 0,
        Player_X = 1,
        Player_O = 2
    }

    public sealed class Board
    {
        private const int board_size = 3;
        private int[,] squares;

        public int FreePositionsCount { get; private set; }

        public Board()
        {
            squares = new int[board_size , board_size];
            GetFreePositions();
        }

        public Board(string _notation)
        {
            squares = new int[board_size , board_size];
            ParseFromNotation(_notation);
            GetFreePositions();
        }

        // ================== [ PUBLIC API ] ==================

        public bool MakeMove(Vector2Intx _position , PlayerSide _player)
        {
            if (!IsPositionValid(_position)) 
            {
                Debug.LogError("Invalid move position!");
                return false;
            }
            squares[_position.x , _position.y] = (int)_player;
            return true;
        }

        public bool UnMakeMove(Vector2Intx _position)
        {
            return MakeMove(_position , PlayerSide.None);
        }
        
        public bool IsPositionValid(Vector2Intx _position)
        {
            return _position.x >= 0 && _position.x < board_size && _position.y >= 0 && _position.y < board_size;
        }
        
        public List<Vector2Intx> GetFreePositions()
        {
            FreePositionsCount = 0;
            List<Vector2Intx> result = new List<Vector2Intx>();
            for (int y = 0; y < board_size; y++)
            {
                for (int x = 0; x < board_size; x++)
                {
                    if (squares[x,y] == 0)
                    {
                        FreePositionsCount++;
                        result.Add(new Vector2Intx(x,y));
                    }
                }
            }
            return result;
        }

        public bool IsTerminal(out PlayerSide _winner )
        {
            if (IsWinner(PlayerSide.Player_O))
            {
                _winner = PlayerSide.Player_O;
                return true;
            }else if (IsWinner(PlayerSide.Player_X))
            {
                _winner = PlayerSide.Player_X;
                return true;
            }
            _winner = PlayerSide.None;
            return FreePositionsCount == 0;
        }

        public bool IsWinner(PlayerSide _side)
        {
            return WinnerHorizontal(_side) || WinnerVertical(_side) || WinnerDiagonal(_side);
        }

        // ================== [ PRIVATE API ] ==================

        private bool CheckWin_Diagonal(bool _left, PlayerSide _side)
        {
            int start_index = _left ? 0 : board_size - 1;
            int end_index = _left ? board_size - 1 : 0;
            int increment = _left ? 1 : -1;
            int y = 0;
            bool result = true;
            for (int x = start_index; x < end_index; x+= increment)
            {
                Vector2Intx current_position = new Vector2Intx(x, y);
                if (!IsPositionValid(current_position)) break;
                result = result && squares[x, y] == (int)_side;
                y++;
            }
            return result;
        }

        private bool CheckWin_Vertical(int x, PlayerSide _side)
        {
            bool result = true;
            for (int y = 0; y < board_size; y++)
            {
                result = result && squares[x,y] == (int)_side;
            }
            return result;
        }

        private bool CheckWin_Horizontal(int y , PlayerSide _side)
        {
            bool result = true;
            for (int x = 0; x < board_size; x++)
            {
                result = result && squares[x,y] == (int)_side;
            }
            return result;
        }

        private bool WinnerDiagonal(PlayerSide _side)
        {
            return CheckWin_Diagonal(false, _side) || CheckWin_Diagonal(true , _side);
        }

        private bool WinnerVertical(PlayerSide _side)
        {
            for (int x = 0; x < board_size; x++)
            {
                if (CheckWin_Vertical(x , _side)) return true;
            }
            return false;
        }

        private bool WinnerHorizontal(PlayerSide _side)
        {
            for (int y = 0; y < board_size; y++)
            {
                if (CheckWin_Horizontal(y , _side)) return true;
            }
            return false;
        }

        // ================== [ INTERNAL METHODS ] ==================

        private void ParseFromNotation(string _notation)
        {
            int x = 0, y = 0;
            for (int i = 0; i < _notation.Length; i++)
            {
                char current = _notation[i];
                if (char.ToLower(current) == 'x')
                {
                    squares[x,y] = (int)PlayerSide.Player_X;
                    x++;
                }else if (char.ToLower(current) == 'o')
                {
                    squares[x,y] = (int)PlayerSide.Player_O;
                    x++;
                }
                
                if (x >= board_size)
                {
                    y++;
                    x = 0;
                }
                if (y >= board_size)
                {
                    break;
                }
            }
        }

    }
}