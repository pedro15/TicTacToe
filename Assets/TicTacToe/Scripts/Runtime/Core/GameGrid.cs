using Debug = UnityEngine.Debug;
using System.Linq;
using System.Collections.Generic;

namespace TicTacToe.Core
{
    public enum PlayerSide
    {
        None = 0,
        Player_X = 1,
        Player_O = 2
    }

    public class GameGrid
    {
        public int[,] cells { get; private set; }
        public GameGrid()
        {
            cells = new int[3, 3];
        }

        public GameGrid(int [,] data)
        {
            if (data.Length != 9) throw new System.ArgumentException("Invalid data");
            cells = data;
        }

        public GameGrid(string gridstring)
        {
            ParseGridString(gridstring);
        }

        // ==================== [ PRIVATE API ] ==================== 

        private void ParseGridString(string str)
        {
            cells = new int[3, 3];
            int x = 0, y = 0;
            for(int i = 0; i < str.Length; i++)
            {
                char current = str[i];
                if (char.IsNumber(current))
                {
                    cells[x, y] = (int)char.GetNumericValue(current);
                    x++;
                }else 
                {
                    bool valid = false;
                    if (current == 'O') 
                    {
                        cells[x,y] = (int)PlayerSide.Player_O;
                        valid = true;
                    }else if (current == 'X')
                    {
                        cells[x,y] = (int)PlayerSide.Player_X;
                        valid = true;
                    }else if (current == '?')
                    {
                        cells[x,y] = 0;
                        valid = true;
                    }
                    if (valid) x++;
                }

                if (x >= cells.GetLength(0))
                {
                    y++;
                    x = 0;
                }

                if (y >= cells.GetLength(1)) break;
            }
        }

        //       Winning API    


        // Vertical wins
        private int CountVertcialPoints(int x , PlayerSide side)
        {
            int points = 0;

            for(int y = 0; y < cells.GetLength(1); y++)
            {
                 if (cells[x,y] == (int)side)
                 {
                     points++;
                 }else if (cells[x,y] != 0) break;
            }
            return points;
        }

        public bool IsWinnerVertical(PlayerSide side)
        {
            bool result = CountVertcialPoints(0 , side) == 3;
            for(int x = 1 ; x < cells.GetLength(0); x++)
                result = result || (CountVertcialPoints(x , side) == 3);
            return result; 
        }

        // Horizontal wins
        private int CountHorizontalPoints(int y, PlayerSide side)
        {
            int points = 0;

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                if (cells[x,y] == (int)side)
                {
                    points++;
                }else if (cells[x,y] != 0) break;
            }

            return points;
        }

        public bool IsWinnerHorizontal(PlayerSide side)
        {
            bool result = CountHorizontalPoints(0 , side) == 3;
            for (int y = 1; y < cells.GetLength(1); y++)
                result = result || (CountHorizontalPoints(y , side) == 3);

            return result;
        }

        // Diagonal wins
        private int CountDiagonalPoints(bool left, PlayerSide side)
        {
            int count = 0;
            int x = left ? 0 : cells.GetLength(0) -1;
            int y = 0;

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                if (cells[x,y] == (int)side)
                {
                    count++;
                }else if (cells[x,y] != 0) break;

                x += left ? 1 : -1;
                y ++;
            }

            return count;
        }

        public bool IsWinnerDiagonal(PlayerSide side)
        {
           return CountDiagonalPoints(true , side) == 3 || CountDiagonalPoints(false , side) == 3;
        }

        // ==================== [ PUBLIC API ] ==================== 

        public bool IsTerminal(out PlayerSide _winner)
        {
            if (IsPlayerWinner(PlayerSide.Player_X))
            {
                _winner = PlayerSide.Player_X;
                return true;
            }else if (IsPlayerWinner(PlayerSide.Player_O))
            {
                _winner = PlayerSide.Player_O;
                return true;
            }
            
            _winner = PlayerSide.None;
            return EmptySpacesCount() == 0;
        }

        public bool IsPlayerWinner(PlayerSide side)
        {
             return IsWinnerDiagonal(side) || IsWinnerHorizontal(side) || IsWinnerVertical(side);
        }

        public List<GameMove> GetPossibleMoves(PlayerSide playingSide)
        {
            List<GameMove> result = new List<GameMove>();

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y].Equals(0))
                    {
                        result.Add(new GameMove(playingSide, x, y));
                    }
                }
            }

            return result;
        }

        public int EmptySpacesCount()
        {
            int empty_count = 0;
            for (int x = 0; x < cells.GetLength(0); x++)
                for (int y = 0; y < cells.GetLength(1); y++)
                    if (cells[x, y] == 0) empty_count++;

            return empty_count;
        }

        public bool IsOnCorner(int x , int y )
        {
            return (x == cells.GetLength(0) - 1 || x == 0) && (y == cells.GetLength(1) - 1 || y == 0);
        }

        public bool IsValidCoord(int x , int y)
        {
            return (x >= 0 && x < cells.GetLength(0)) && (y >= 0 && y < cells.GetLength(1));
        }

        public void UnMakeMove(int x , int y)
        {
            cells[x, y] = 0;
        }

        public bool PlacePlayerOnSquare(int x , int y , PlayerSide side)
        {
            if (IsValidCoord(x,y))
            {
                cells[x, y] = (int)side;
                return true;
            }
            return false;
        }

        public bool PlacePlayerOnSquare(GameMove move)
        {
            return PlacePlayerOnSquare(move.x, move.y , move.side);
        }

        public PlayerSide GetPlayerFromSquare (int x , int y)
        {
            if (!IsValidCoord(x, y)) return PlayerSide.None;

            return (PlayerSide)cells[x, y];
        }

        public string Serialize(bool prettyPrint = true)
        {
            string result = string.Empty;
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    char _player = '?';
                    if (cells[x, y] == (int)PlayerSide.Player_X)
                        _player = 'X';
                    else if (cells[x, y] == (int)PlayerSide.Player_O)
                        _player = 'O';

                    result += $"-{_player} ";
                    if (x == cells.GetLength(0) - 1) result += "-";
                }
                result += prettyPrint ? "\n" : "_";
            }
            return result;
        }
        
    }
}