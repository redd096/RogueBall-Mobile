using UnityEngine;

[AddComponentMenu("RogueBall/Player/Throw Ball")]
public class PlayerThrowBall : MonoBehaviour
{
    [Header("Swipe")]
    [Tooltip("Release display before this time to get a swipe movement")] [SerializeField] float timeToRelease = 1;
    [Tooltip("Inside this range, is not considered input")] [SerializeField] float deadZone = 100;

    [Header("Throw")]
    [Tooltip("Force throw")] [SerializeField] float force = 3;

    bool isSwiping;
    Vector2 startPosition;
    float timeToSwipe;

    Player player;

    void Awake()
    {
        player = GetComponent<Player>();
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
                startPosition = InputPosition();
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
                    Swipe(InputPosition() - startPosition);
                }
            }
        }
    }

    void Swipe(Vector2 movement)
    {
        //check dead zone
        if (movement.magnitude < deadZone)
            return;

        //throw ball
        player.ThrowBall(force, movement.normalized);
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
