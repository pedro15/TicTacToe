using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TicTactoe.Gameplay;
using UnityEngine;

namespace TicTacToe.Tests
{
    public class BoardTests
    {
        [Test]
        public void BoardDuplicationTest()
        {
            Board board = new Board();
            List<Vector2Intx> positions = board.GetFreePositions();
            PlayerSide side = PlayerSide.Player_O;
            for (int i = 0; i < positions.Count; i++)
            {
                Vector2Intx current = positions[i];
                
                if(board.MakeMove(current , side))
                {
                    side = Board.GetAdversary(side);
                    Debug.Log(board.Serialize());
                    board.UnMakeMove(current);
                }
            }
            Assert.IsTrue(board.Equals(new Board()));
        }

        [Test]
        public void TestWin()
        {
            string _notation = @"O-X-X-
                                X-X-O-
                                X-?-?-";

            /*
                O-X-X-X-X-O-?-?-?
            */
            Board board = new Board(_notation);
            Debug.Log(board.IsTerminal(out PlayerSide _winner));
        }

    }
}