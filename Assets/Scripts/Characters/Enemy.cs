namespace RogueBall
{
    using UnityEngine;

    [AddComponentMenu("RogueBall/Characters/Enemy")]
    public class Enemy : Character
    {
        [Header("States")]
        [SerializeField] MovingEnemyState movingState = default;
        [SerializeField] ThrowEnemyState throwState = default;

        protected override void Start()
        {
            base.Start();

            //set start state
            SetState(movingState);
        }

        protected override void PickBall(Ball ball)
        {
            base.PickBall(ball);

            //set throw state
            SetState(throwState);
        }

        public override void ThrowBall(Vector2 direction)
        {
            base.ThrowBall(direction);

            //set moving state
            SetState(movingState);
        }

        protected override void DeathFunction()
        {
            //destroy
            Destroy(gameObject);
        }
    }
}