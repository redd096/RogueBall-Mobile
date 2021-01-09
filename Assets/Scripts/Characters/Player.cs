namespace RogueBall
{
    using UnityEngine;

    [AddComponentMenu("RogueBall/Characters/Player")]
    public class Player : Character
    {
        [Header("States")]
        [SerializeField] MovingPlayerState movingState = default;
        [SerializeField] ThrowPlayerState throwState = default;

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
            //disable movement and parry
            SetState(null);

            Debug.Log("dead");
        }
    }
}