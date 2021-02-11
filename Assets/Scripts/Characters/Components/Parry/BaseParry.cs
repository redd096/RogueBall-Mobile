namespace RogueBall
{
    using UnityEngine;
    using redd096;

    public abstract class BaseParry : MonoBehaviour
    {
        [Header("Parry Base")]
        [SerializeField] bool parryOnlyBeforeBounce = true;

        protected Character character;

        protected bool isMoving;
        protected Waypoint startWaypoint;
        protected Waypoint endWaypoint;

        void OnEnable()
        {
            character = GetComponent<Character>();

            //set events
            character.onMove += OnMove;
            character.onEndMove += OnEndMove;
        }

        void OnDisable()
        {
            //remove events
            if (character)
            {
                character.onMove -= OnMove;
                character.onEndMove -= OnEndMove;
            }
        }

        protected virtual void OnMove(Waypoint startWaypoint, Waypoint endWaypoint)
        {
            //is moving and set waypoints
            isMoving = true;

            this.startWaypoint = startWaypoint;
            this.endWaypoint = endWaypoint;
        }

        void OnEndMove()
        {
            //stop moving
            isMoving = false;
        }

        public bool TryParry(Ball ball)
        {
            //if in movement to parry, and ball is parryable
            if (isMoving && ball.IsParryable)
            {
                //check if can parry also after bounce, or if ball didn't bounce
                if (parryOnlyBeforeBounce == false || ball.Bounced == false)
                {
                    //check parry
                    return CheckParry(ball);
                }
            }

            return false;
        }

        protected abstract bool CheckParry(Ball ball);
    }
}