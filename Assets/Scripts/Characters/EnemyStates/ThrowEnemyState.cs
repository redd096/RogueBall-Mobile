namespace RogueBall
{
    using UnityEngine;
    using redd096;

    [System.Serializable]
    public class ThrowEnemyState : State
    {
        [Tooltip("Timer between one throw and another")] [SerializeField] float timerThrow = 1;
        [Tooltip("Aim at player or throw random?")] [SerializeField] bool aimAtPlayer = true;

        Enemy enemy;

        Vector2 aimPoint;
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
            aimPoint = GetAimPoint();

            //call event
            enemy = stateMachine as Enemy;
            enemy.onSetThrowDirection?.Invoke(stateMachine.transform.position, aimPoint);
        }

        public override void Execution()
        {
            base.Execution();

            //continue to aim at player
            if(aimAtPlayer)
            {
                //set direction to throw
                aimPoint = GetAimPoint();

                //call event
                enemy.onSetThrowDirection?.Invoke(stateMachine.transform.position, aimPoint);
            }

            //after timer, throw
            if (Time.time > timer)
            {
                Character character = stateMachine as Character;
                if (character.ThrowBall((aimPoint - new Vector2(stateMachine.transform.position.x, stateMachine.transform.position.y)).normalized))
                {
                    //update timer only if has been succesfull
                    timer = Time.time + timerThrow;

                    //reset throw direction
                    enemy.onSetThrowDirection?.Invoke(Vector2.zero, Vector2.zero);
                }
            }
        }

        Vector2 GetAimPoint()
        {
            //aim at player or random
            if (aimAtPlayer && GameManager.instance.player)
            {
                return GameManager.instance.player.transform.position;
            }
            else
            {
                return Random.insideUnitCircle;
            }
        }
    }
}