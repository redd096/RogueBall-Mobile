namespace RogueBall
{
    using UnityEngine;
    using redd096;

    [System.Serializable]
    public class ThrowEnemyState : State
    {
        [Tooltip("Timer between one throw and another")] [SerializeField] float timerThrow = 1;
        [Tooltip("Aim at player or throw random?")] [SerializeField] bool aimAtPlayer = true;

        float timer;

        public ThrowEnemyState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            //reset timer
            timer = Time.time + timerThrow;
        }

        public override void Execution()
        {
            base.Execution();

            //after timer, do movement
            if (Time.time > timer)
            {
                //update timer
                timer = Time.time + timerThrow;

                Throw(GetDirection());
            }
        }

        Vector2 GetDirection()
        {
            if (aimAtPlayer)
            {
                return (GameManager.instance.player.transform.position - stateMachine.transform.position).normalized;
            }
            else
            {
                return Random.insideUnitCircle;
            }
        }

        void Throw(Vector2 direction)
        {
            //throw ball
            Character character = stateMachine as Character;
            character.ThrowBall(direction);
        }
    }
}