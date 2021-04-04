using System.Collections;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;
using System.Linq;

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
        public const int board_size = 3;
        private int[,] squares;

        public int FreePositionsCount { get; private set; }

        public Board()
        {
            squares = new int[board_size , board_size];
            GetFreePositions();
        }
        
        public Board(string _notation)
        {
            FreePositionsCount = board_size * board_size;
            squares = new int[board_size , board_size];
            ParseFromNotation(_notation);
        }

        // ================== [ PUBLIC API ] ==================

        public static PlayerSide GetAdversary(PlayerSide _player)
        {
            if(_player == PlayerSide.Player_O)
                return PlayerSide.Player_X;
            else if (_player == PlayerSide.Player_X)
                return PlayerSide.Player_O;

            return PlayerSide.None;
        }

        public int[,] GetBoardData() => squares;
        
        public bool MakeMove(Vector2Intx _position , PlayerSide _player)
        {
            if (!IsPositionValid(_position) || squares[_position.x,_position.y] != 0) return false;
            squares[_position.x , _position.y] = (int)_player;
            FreePositionsCount += (_player == PlayerSide.None) ? 1 : -1;
            return true;
        }
        
        public bool UnMakeMove(Vector2Intx _position)
        {
            if (!IsPositionValid(_position)) return false;
            squares[_position.x , _position.y] = 0;
            return true;
        }
        
        public bool IsPositionValid(Vector2Intx _position)
        {
            return _position.x >= 0 && _position.x < board_size && _position.y >= 0 && _position.y < board_size;
        }

        public void Reset()
        {
            FreePositionsCount = board_size * board_size;
            for(int x = 0; x < board_size; x++)
                for (int y = 0; y < board_size; y++)
                    squares[x,y] = 0;
        }
        
        public List<Vector2Intx> GetFreePositions()
        {
            List<Vector2Intx> result = new List<Vector2Intx>();
            for (int y = 0; y < board_size; y++)
            {
                for (int x = 0; x < board_size; x++)
                {
                    if (squares[x,y] == 0)
                    {
                        result.Add(new Vector2Intx(x,y));
                    }
                }
            }
            return result;
        }

        public bool IsTerminal(out PlayerSide _winner , PlayerSide currentPlayer = PlayerSide.Player_O)
        {
            if (IsWinner(currentPlayer))
            {
                _winner = currentPlayer;
                return true;
            }else if (IsWinner(GetAdversary(currentPlayer)))
            {
                _winner = GetAdversary(currentPlayer);
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
            int value = (int)_side;
            int y = 0;

            int increment = _left ? 1 : -1;
            int start = _left ? 0 : board_size - 1;
            int end = _left ? board_size -1 : 0;
            int score = 0;

            for (int x = start; _left ? (x <= end) : (x >= end) ; x+= increment)
            {
                if (!IsPositionValid(new Vector2Intx(x,y))) break;
                if(squares[x,y] == value) score++;
                y++;
            }
            return score == 3;
        }

        private bool CheckWin_Vertical(int x, PlayerSide _side)
        {
            int score = 0;
            for (int y = 0; y < board_size; y++)
            {
                if (squares[x,y] == (int)_side)
                    score++;
                    
            }
            return score == 3;
        }

        private bool CheckWin_Horizontal(int y , PlayerSide _side)
        {
            int score = 0;
            for (int x = 0; x < board_size; x++)
            {
                if (squares[x,y] == (int)_side)
                    score++;
            }
            return score == 3;
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
            if (string.IsNullOrEmpty(_notation)) return;
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

        public string Serialize(bool prettyPrint = true)
        {
            string result = "";
            for(int y = 0; y < board_size; y++)
            {
                for (int x = 0; x < board_size; x++)
                {
                    PlayerSide current_side = (PlayerSide)squares[x,y];
                    char sideval = '?';
                    if (current_side == PlayerSide.Player_O)
                        sideval = 'O';
                    else if (current_side == PlayerSide.Player_X)
                        sideval = 'X';

                    result += sideval + "-";
                }
                if(prettyPrint) result += "\n";
            }
            return result;
        }

        private bool SquaresEquals(int[,] other)
        {
            if (other.Length != squares.Length) return false;
            bool result = true;
            for(int y = 0; y < board_size; y++)
            {
                for (int x = 0; x < board_size; x++)
                {
                    result &= squares[x,y] == other[x,y];
                }
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Board other = (Board)obj;
            return other.FreePositionsCount.Equals(FreePositionsCount) && SquaresEquals(other.squares);
        }

        public override int GetHashCode()
        {
            int hash = 8;
            hash += FreePositionsCount.GetHashCode();
            hash += squares.GetHashCode();
            return hash;
        }

    }
}