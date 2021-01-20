namespace RogueBall
{
    using UnityEngine;
    using redd096;

    public abstract class BaseMovement : MonoBehaviour
    {
        public bool IsMoving => movementCoroutine != null;
        protected Coroutine movementCoroutine;

        protected Character character;
        Animator anim;

        Waypoint currentWaypoint;
        protected Waypoint CurrentWaypoint
        {
            get
            {
                if (currentWaypoint == null)
                    currentWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(character, transform.position);

                return currentWaypoint;
            }
            set
            {
                currentWaypoint = value;
            }
        }

        void OnEnable()
        {
            character = GetComponent<Character>();
            anim = GetComponentInChildren<Animator>();

            //get current waypoint and set to its position
            if (GameManager.instance && GameManager.instance.mapManager && CurrentWaypoint)
            {
                transform.position = CurrentWaypoint.transform.position;
            }
        }

        void Start()
        {
            //get current waypoint and set to its position - cause on enable is called before singleton
            if (CurrentWaypoint)
            {
                transform.position = CurrentWaypoint.transform.position;
            }
        }

        protected void SetAnimator(Waypoint startWaypoint, Waypoint endWaypoint, bool move)
        {
            //if move, set direction
            if (move)
            {
                Vector2Int direction = endWaypoint.PositionInMap - startWaypoint.PositionInMap;

                anim?.SetFloat("Horizontal", direction.x);
                anim?.SetFloat("Vertical", direction.y);
            }

            //set if move
            anim?.SetBool("Move", move);
        }

        public abstract bool Move(Waypoint targetWaypoint, bool moveDiagonal);

        public bool CanMove(Waypoint targetWaypoint, bool moveDiagonal)
        {
            //if not current waypoint and is neighbour
            return targetWaypoint != CurrentWaypoint && GameManager.instance.mapManager.GetNeighbours(character, moveDiagonal, CurrentWaypoint).Contains(targetWaypoint);
        }
    }
}