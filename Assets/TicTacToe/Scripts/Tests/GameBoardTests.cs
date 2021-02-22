using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TicTacToe.Core;

namespace TicTacToe.Tests
{
    public class GameBoardTests
    {
       

        [Test]
        public void WinnerDiagonal()
        {
            string grid_str =  "1-0-0" +
                               "0-1-0" +
                               "0-0-1";

            GameGrid grid = new GameGrid(grid_str);

            Debug.Log(grid.Serialize());

            Assert.IsTrue(grid.IsWinnerDiagonal((int)PlayerSide.Player_X, 0, 0) && grid.IsWinnerDiagonal((int)PlayerSide.Player_X , 2,2));
        }

        [Test]
        public void WinnerHorizontal()
        {
            string grid_str = "0-0-0" +
                              "1-1-1" +
                              "0-0-0";

            GameGrid grid = new GameGrid(grid_str);

            Debug.Log(grid.Serialize());

            int player = (int)PlayerSide.Player_X;

            Assert.IsTrue(grid.IsWinnerHorizontal(player, 0, 1) && grid.IsWinnerHorizontal(player , 2 , 1));
        }

        [Test]
        public void WinnerVertical()
        {
            string grid_str = "0-1-0" +
                              "0-1-0" +
                              "0-1-0";

            GameGrid grid = new GameGrid(grid_str);

            Debug.Log(grid.Serialize());

            int player = (int)PlayerSide.Player_X;

            Assert.IsTrue(grid.IsWinnerVertical(player, 1, 0) && grid.IsWinnerVertical(player , 1,2));
        }

    }
}
