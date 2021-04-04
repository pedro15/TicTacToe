using System.Collections;
using System.Collections.Generic;

namespace TicTactoe.Gameplay.Players
{
    public abstract class GamePlayer
    {
        protected PlayerSide currentSide  {get; private set; }

        public void Init(PlayerSide _currentSide)
        {
            currentSide = _currentSide;
        }
        
        public abstract bool GetMove(out Vector2Intx _movePosition , Board _board);
        
        public virtual void OnBoardButtonTap(Vector2Intx _position) { }

        public virtual void OnEnterTurn() { }
    }
}