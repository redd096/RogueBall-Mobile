namespace RogueBall
{
    using UnityEngine;
    using redd096;

    [System.Serializable]
    public class MovingEnemyState : State
    {
        [Tooltip("Timer between one moves and another")] [SerializeField] float timerMovement = 1;
        [Tooltip("Can enemy move in diagonal or only horizontal and vertical?")] [SerializeField] bool moveDiagonal = false;

        Character character;

        Vector2Int direction;
        float timer;

        public MovingEnemyState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            character = stateMachine as Character;

            //reset timer
            timer = Time.time + timerMovement;
        }

        public override void Execution()
        {
            base.Execution();

            //set direction until find one correct
            if(character.CanMove(direction) == false)
            {
                GetRandomDirection();

                //DEBUG
                Enemy enemy = stateMachine as Enemy;
                enemy.DebugArrow(direction);
            }

            //after timer, do movement
            if (Time.time > timer)
            {
                if (character.Move(direction))
                {
                    //update timer only if has been succesfull
                    timer = Time.time + timerMovement;
                }
            }
        }

        void GetRandomDirection()
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
        }
    }
}