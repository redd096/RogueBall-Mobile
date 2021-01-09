namespace RogueBall
{
    using UnityEngine;
    using redd096;

    [System.Serializable]
    public class MovingEnemyState : State
    {
        [Tooltip("Timer between one moves and another")] [SerializeField] float timerMovement = 1;
        [Tooltip("Can enemy move in diagonal or only horizontal and vertical?")] [SerializeField] bool moveDiagonal = false;

        [Header("DEBUG")]
        [SerializeField] Transform arrow = default;

        Vector2Int direction;
        float timer;

        public MovingEnemyState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            //update timer and set direction
            timer = Time.time + timerMovement;
            direction = GetRandomDirection();
            DebugArrow();
        }

        public override void Execution()
        {
            base.Execution();

            //after timer, do movement
            if (Time.time > timer)
            {
                Character character = stateMachine as Character;
                character.Move(direction);

                //update timer and set direction
                timer = Time.time + timerMovement;
                direction = GetRandomDirection();
                DebugArrow();
            }
        }

        Vector2Int GetRandomDirection()
        {
            direction = Vector2Int.zero;

            //move horizontal
            if (Random.value > 0.5f)
            {
                //move right or left
                direction.x = Random.value > 0.5f ? 1 : -1;

                //if move diagonal, random between -1, 0, 1
                if (moveDiagonal)
                    direction.y = Random.Range(-1, 2);
            }
            else
            {
                //move right or left
                direction.y = Random.value > 0.5f ? 1 : -1;

                //if move diagonal, random between -1, 0, 1
                if (moveDiagonal)
                    direction.x = Random.Range(-1, 2);
            }

            return direction;
        }

        void DebugArrow()
        {
            //0, 45, 90, 135, 180, -135 (225), -90 (270), -45 (315)
            if (direction.y > 0 && direction.x == 0)
                arrow.localEulerAngles = new Vector3(0, 0, 0);
            else if (direction.y > 0 && direction.x < 0)
                arrow.localEulerAngles = new Vector3(0, 0, 45);
            else if (direction.y == 0 && direction.x < 0)
                arrow.localEulerAngles = new Vector3(0, 0, 90);
            else if (direction.y < 0 && direction.x < 0)
                arrow.localEulerAngles = new Vector3(0, 0, 135);
            else if (direction.y < 0 && direction.x == 0)
                arrow.localEulerAngles = new Vector3(0, 0, 180);
            else if (direction.y < 0 && direction.x > 0)
                arrow.localEulerAngles = new Vector3(0, 0, 255);
            else if (direction.y == 0 && direction.x > 0)
                arrow.localEulerAngles = new Vector3(0, 0, 270);
            else
                arrow.localEulerAngles = new Vector3(0, 0, 315);
        }
    }
}