namespace RogueBall
{
    using UnityEngine;
    using redd096;

    [System.Serializable]
    public class ThrowEnemyState : State
    {
        [Tooltip("Timer between one throw and another")] [SerializeField] float timerThrow = 1;
        [Tooltip("Aim at player or throw random?")] [SerializeField] bool aimAtPlayer = true;

        Vector2 direction;
        float timer;

        public ThrowEnemyState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            //reset timer
            timer = Time.time + timerThrow;

            //set direction to throw
            direction = GetDirection();

            //DEBUG
            Enemy enemy = stateMachine as Enemy;
            enemy.DebugArrow(direction);
        }

        public override void Execution()
        {
            base.Execution();

            //after timer, throw
            if (Time.time > timer)
            {
                Character character = stateMachine as Character;
                if (character.ThrowBall(direction))
                {
                    //update timer only if has been succesfull
                    timer = Time.time + timerThrow;
                }
            }
        }

        Vector2 GetDirection()
        {
            //aim at player or random
            if (aimAtPlayer)
            {
                return (GameManager.instance.player.transform.position - stateMachine.transform.position).normalized;
            }
            else
            {
                return Random.insideUnitCircle;
            }
        }
    }
}