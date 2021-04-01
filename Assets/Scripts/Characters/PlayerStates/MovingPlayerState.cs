namespace RogueBall
{
    using UnityEngine;
    using redd096;

    [System.Serializable]
    public class MovingPlayerState : State
    {
        [Tooltip("Release display before this time to get a swipe movement")] [SerializeField] float timeToRelease = 1;
        [Tooltip("Inside this range, is not considered input")] [SerializeField] float deadZone = 100;

        protected Character character;
        protected Vector2 swipeMovement;

        bool isSwiping;
        Vector2 startSwipePosition;
        float timeToSwipe;

        public MovingPlayerState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            character = stateMachine as Character;
        }

        public override void Execution()
        {
            base.Execution();

            if (CheckInput() == false)
            {
                //if swiping but no input, stop
                if (isSwiping)
                    isSwiping = false;

                return;
            }

            if (isSwiping == false)
            {
                //start swipe
                if (InputDown())
                {
                    isSwiping = true;

                    //save position and time
                    startSwipePosition = InputPosition();
                    timeToSwipe = Time.time + timeToRelease;
                }
            }
            else
            {
                //stop swipe
                if (InputUp())
                {
                    isSwiping = false;

                    //if in time, check swipe (delta from start position to new position)
                    if (timeToSwipe >= Time.time)
                    {
                        swipeMovement = InputPosition() - startSwipePosition;
                        CheckSwipe();
                    }
                }
            }
        }

        protected virtual void CheckSwipe()
        {
            float absX = Mathf.Abs(swipeMovement.x);
            float absY = Mathf.Abs(swipeMovement.y);

            //move only horizontal
            if (CheckMoveOnlyHorizontal())
                absY = 0;

            //check dead zone
            if (absX < deadZone)
                absX = 0;
            if (absY < deadZone)
                absY = 0;

            //do nothing if everything at 0
            if (absX <= Mathf.Epsilon && absY <= Mathf.Epsilon)
                return;

            //get direction horizontal or vertical (based on greater)
            Vector2Int direction = Vector2Int.zero;
            if (absX > absY)
            {
                direction = swipeMovement.x > Mathf.Epsilon ? Vector2Int.right : Vector2Int.left;

                //if move diagonal, add vertical if necessary
                if (CheckMoveDiagonal() && absY > Mathf.Epsilon)
                {
                    direction.y = swipeMovement.y > Mathf.Epsilon ? 1 : -1;
                }
            }
            else
            {
                direction = swipeMovement.y > Mathf.Epsilon ? Vector2Int.up : Vector2Int.down;

                //if move diagonal, add horizontal if necessary
                if (CheckMoveDiagonal() && absX > Mathf.Epsilon)
                {
                    direction.x = swipeMovement.x > Mathf.Epsilon ? 1 : -1;
                }
            }

            //swipe (direction using 1 and -1)
            Swipe(direction);
        }

        protected virtual bool CheckMoveOnlyHorizontal()
        {
            return character.MoveOnlyHorizontal;
        }

        protected virtual bool CheckMoveDiagonal()
        {
            return character.MoveDiagonal;
        }

        protected virtual void Swipe(Vector2Int direction)
        {
            //get waypoints
            Waypoint currentWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(character, character.transform.position);
            Waypoint targetWaypoint = GameManager.instance.mapManager.GetWaypointInDirection(character, currentWaypoint, direction);

            //swipe (direction using 1 and -1)
            character.Move(targetWaypoint);
        }

        #region inputs

#if UNITY_ANDROID && !UNITY_EDITOR

        bool CheckInput()
        {
            return Input.touchCount > 0;
        }

        bool InputDown()
        {
            return Input.GetTouch(0).phase == TouchPhase.Began;
        }

        bool InputUp()
        {
            return Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled;
        }

        Vector2 InputPosition()
        {
            return Input.GetTouch(0).position;
        }

#else

        bool CheckInput()
        {
            return true;
        }

        bool InputDown()
        {
            return Input.GetKeyDown(KeyCode.Mouse0);
        }

        bool InputUp()
        {
            return Input.GetKeyUp(KeyCode.Mouse0);
        }

        Vector2 InputPosition()
        {
            return Input.mousePosition;
        }

#endif

        #endregion

    }
}