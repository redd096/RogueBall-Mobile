namespace RogueBall
{
    using UnityEngine;

    [AddComponentMenu("RogueBall/Characters/Enemy")]
    public class Enemy : Character
    {
        [Header("States")]
        [SerializeField] MovingEnemyState movingState = default;
        [SerializeField] ThrowEnemyState throwState = default;

        [Header("DEBUG")]
        [SerializeField] Transform arrow = default;

        public void DebugArrow(Vector2 direction)
        {
            //0, 45, 90, 135, 180, -135 (225), -90 (270), -45 (315), 0 (360)
            float z = 0;

            if(direction.y >= Mathf.Epsilon)
            {
                //-1 = 45
                //0 = 0
                //1 = -45 (315)
                z = redd096.Utility.Remap(direction.x, -1, 1, 90, -90);
            }
            else
            {
                //-1 = 135
                //0 = 180
                //1 = -135 (255)

                z = redd096.Utility.Remap(direction.x, -1, 1, 90, 270);
            }

            arrow.localEulerAngles = new Vector3(0, 0, z);
        }

        void Start()
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