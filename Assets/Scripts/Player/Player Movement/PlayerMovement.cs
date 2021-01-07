using UnityEngine;

public abstract class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Can player move in diagonal or only horizontal and vertical?")] [SerializeField] bool moveDiagonal = false;
    [Tooltip("Release display before this time to get a swipe movement")] [SerializeField] float timeToRelease = 1;
    [Tooltip("Inside this range, is not considered input")] [SerializeField] float deadZone = 100;

    bool isSwiping;
    Vector2 startSwipePosition;
    float timeToSwipe;

    public System.Action<Waypoint, Waypoint> onMove;
    public System.Action onEndMove;

    Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
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
                    CheckSwipe(InputPosition() - startSwipePosition);
                }
            }
        }
    }

    void CheckSwipe(Vector2 delta)
    {
        float absX = Mathf.Abs(delta.x);
        float absY = Mathf.Abs(delta.y);

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
        if(absX > absY)
        {
            direction = delta.x > Mathf.Epsilon ? Vector2Int.right : Vector2Int.left;

            //if move diagonal, add vertical if necessary
            if(moveDiagonal && absY > Mathf.Epsilon)
            {
                direction.y = delta.y > Mathf.Epsilon ? 1 : -1;
            }
        }
        else
        {
            direction = delta.y > Mathf.Epsilon ? Vector2Int.up : Vector2Int.down;

            //if move diagonal, add horizontal if necessary
            if (moveDiagonal && absX > Mathf.Epsilon)
            {
                direction.x = delta.x > Mathf.Epsilon ? 1 : -1;
            }
        }


        //swipe (direction using 1 and -1)
        Move(direction);
        
        //set animator
        anim?.SetInteger("Horizontal", direction.x);
        anim?.SetInteger("Vertical", direction.y);
    }

    protected abstract void Move(Vector2Int direction);

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
