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
        public void TestVerticalWin()
        {
            string gridMap = "X-O-?-" +
                             "X-O-?-" + 
                             "X-?-X-";

            GameGrid grid = new GameGrid(gridMap);
            Debug.Log(grid.Serialize());
            Assert.IsTrue(grid.IsWinnerVertical(PlayerSide.Player_X));
        }

        [Test]
        public void TestHorizontalWin()
        {
            string gridMap = @"X-O-?-
                               O-O-O-
                               X-?-X-";
            GameGrid grid = new GameGrid(gridMap);
            Debug.Log(grid.Serialize());
            Assert.IsTrue(grid.IsWinnerHorizontal(PlayerSide.Player_O));
        }

        [Test]
        public void TestDiagonalWin()
        {
            string gridmap = @"X-?-O-
                               O-X-?
                               O-?-X";
            GameGrid grid = new GameGrid(gridmap);
            Debug.Log(grid.Serialize());
            Assert.IsTrue(grid.IsWinnerDiagonal(PlayerSide.Player_X));
        }
        
    }
}
