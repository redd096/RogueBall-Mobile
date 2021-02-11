namespace RogueBall
{
    using UnityEngine;

    [AddComponentMenu("RogueBall/Characters/Enemy")]
    [RequireComponent(typeof(EnemyGraphics))]
    public class Enemy : Character
    {
        [Header("States")]
        [SerializeField] MovingEnemyState movingState = default;
        [SerializeField] ThrowEnemyState throwState = default;

        public System.Action<Vector2, Vector2> onSetMoveDirection;
        public System.Action<Vector2, Vector2> onSetThrowDirection;

        protected virtual void Start()
        {
            //set start state
            SetState(movingState);
        }

        protected override void PickBall(Ball ball)
        {
            base.PickBall(ball);

            //set throw state
            SetState(throwState);
        }

        public override bool ThrowBall(Vector2 direction)
        {
            //set moving state
            SetState(movingState);

            return base.ThrowBall(direction);
        }

        protected override void DeathFunction()
        {
            //destroy
            Destroy(gameObject);
        }

        protected override bool TryParry(Ball ball)
        {
            //do parry every time is moving (only if there is parry component)
            if(CurrentMovement && CurrentMovement.IsMoving)
            {
                return CurrentParry;
            }

            return false;
        }

        /// <summary>
        /// called from state when there is a problem to reach a ball
        /// </summary>
        public void StopFollowBall()
        {
            //set moving state
            SetState(movingState);
        }
    }
}