namespace RogueBall
{
    using UnityEngine;
    using redd096;

    [System.Serializable]
    public class ThrowPlayerState : MovingPlayerState
    {
        public ThrowPlayerState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        protected override bool CheckMoveOnlyHorizontal()
        {
            //do not check "move only horizontal" in CheckSwipe
            return false;
        }

        protected override bool CheckMoveDiagonal()
        {
            //check always if swipe in diagonal
            return true;
        }

        protected override void Swipe(Vector2Int direction)
        {
            //check dead zone - not necessary now, 'cause swipe is called only if X and Y are greater than dead zone
            //if (swipeMovement.magnitude < deadZone)
            //    return;

            //if character can move only horizontal, and swipe only horizontal - do normal movement
            if (character.MoveOnlyHorizontal && direction.y == 0)
            {
                base.Swipe(direction);
            }
            //if swipe is vertical or can move vertical - throw ball
            else
            {
                Character character = stateMachine as Character;
                character.ThrowBall(swipeMovement.normalized);
            }
        }
    }
}