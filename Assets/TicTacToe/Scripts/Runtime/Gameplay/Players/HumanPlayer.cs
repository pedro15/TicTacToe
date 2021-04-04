namespace TicTactoe.Gameplay.Players
{
    public class HumanPlayer : GamePlayer
    {
        private bool IsDone;
        private Vector2Intx selectedMove; 

        public override bool GetMove(out Vector2Intx _movePosition, Board _board)
        {
            _movePosition = selectedMove;
            return IsDone;
        }

        public override void OnEnterTurn()
        {
            IsDone = false;
            selectedMove = new Vector2Intx(-1,-1);
        }

        public override void OnBoardButtonTap(Vector2Intx _position)
        {
            selectedMove = _position;
            IsDone = true;
        }
    }
}