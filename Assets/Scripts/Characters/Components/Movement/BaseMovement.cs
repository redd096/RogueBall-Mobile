namespace RogueBall
{
    using UnityEngine;
    using redd096;

    public abstract class BaseMovement : MonoBehaviour
    {
        protected Character character;
        protected Animator anim;

        Waypoint currentWaypoint;
        protected Waypoint CurrentWaypoint
        {
            get
            {
                if (currentWaypoint == null)
                    currentWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(character, transform.position, out currentKey);

                return currentWaypoint;
            }
            set
            {
                currentWaypoint = value;
            }
        }
        protected Vector2Int currentKey;

        protected Coroutine movementCoroutine;

        void OnEnable()
        {
            character = GetComponent<Character>();
            anim = GetComponentInChildren<Animator>();

            //get current waypoint and set to its position
            if (GameManager.instance && GameManager.instance.mapManager)
            {
                CurrentWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(character, transform.position, out currentKey);
                transform.position = CurrentWaypoint.transform.position;
            }
        }

        void Start()
        {
            //get current waypoint and set to its position - cause on enable is called before singleton
            CurrentWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(character, transform.position, out currentKey);
            transform.position = CurrentWaypoint.transform.position;
        }

        public abstract void Move(Vector2Int direction);
    }
}