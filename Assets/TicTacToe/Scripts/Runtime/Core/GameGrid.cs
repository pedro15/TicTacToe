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
        private int[,] cells;
        public GameGrid()
        {
            cells = new int[3, 3];
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
                }

                if (x >= cells.GetLength(0))
                {
                    y++;
                    x = 0;
                }

                if (y >= cells.GetLength(1)) break;
            }
        }

        // ==================== [ PUBLIC API ] ==================== 


        public bool IsPlayerWinner(PlayerSide side)
        {
            int playerval = (int)side;

            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    if (IsWinnerDiagonal(playerval, x, y) || IsWinnerHorizontal(playerval, x, y) || IsWinnerVertical(playerval, x, y))
                        return true;
                }
            }

            return false;
        }

        public bool IsWinnerDiagonal(int player , int _x , int _y)
        {
            if (!IsOnCorner(_x, _y)) return false;

            int x = _x;
            int y = _y;
            bool result = cells[x, y] == player;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                x = (_x == cells.GetLength(0) - 1) ? x - i : x + i;
                y = (_y == cells.GetLength(1) - 1) ? y - i : y + i;

                if (IsValidCoord(x, y))
                    result &= cells[x, y] == player;
                else
                    break;
            }
            return result;
        }

        public bool IsWinnerVertical(int player , int _x , int _y)
        {
            if (IsOnCorner(_x, _y)) return false;

            int y = _y;

            int count = 0;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                if (IsValidCoord(_x, y))
                {
                    if (cells[_x, y] == player)
                        count++;
                    else
                        return false;

                }
                else break;

                y = (_y == cells.GetLength(1) - 1) ? y - i : y + i;
            }

            return count == cells.GetLength(1);
        }

        public bool IsWinnerHorizontal (int player , int _x , int _y)
        {
            if (IsOnCorner(_x, _y)) return false;

            int x = _x;

            int count = 0;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                if (IsValidCoord(x, _y))
                {
                    if (cells[x, _y] == player)
                        count++;
                    else return false;
                }
                else break;
                x = (_x == cells.GetLength(0) - 1) ? x - i : x + i;
            }

            return count == cells.GetLength(1);
        }

        public bool IsOnCorner(int x , int y )
        {
            return (x == cells.GetLength(0) - 1 || x == 0) && (y == cells.GetLength(1) - 1 || y == 0);
        }

        public bool IsValidCoord(int x , int y)
        {
            return (x >= 0 && x < cells.GetLength(0)) && (y >= 0 && y < cells.GetLength(1));
        }

        public void PlacePlayerOnSquare(int x , int y , PlayerSide side)
        {
            if (IsValidCoord(x,y))
                cells[x, y] = (int)side;
        }
        public PlayerSide GetPlayerFromSquare (int x , int y)
        {
            if (!IsValidCoord(x, y)) return PlayerSide.None;

            return (PlayerSide)cells[x, y];
        }

        public string Serialize()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append("GameGrid: \n");
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    char _player = '?';
                    if (cells[x, y] == (int)PlayerSide.Player_X)
                        _player = 'X';
                    else if (cells[x, y] == (int)PlayerSide.Player_O)
                        _player = 'O';

                    builder.Append($"-{_player} ");
                    if (x == cells.GetLength(0) - 1) builder.Append("-");
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }

    }
}