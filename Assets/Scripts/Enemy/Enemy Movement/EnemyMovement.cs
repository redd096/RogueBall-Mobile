using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Timer between one moves and another")] [SerializeField] float timerMovement = 1;
    [Tooltip("Can enemy move in diagonal or only horizontal and vertical?")] [SerializeField] bool moveDiagonal = false;

    [Header("DEBUG")]
    [SerializeField] Transform arrow = default;

    public System.Action<Waypoint, Waypoint> onMove;
    public System.Action onEndMove;

    protected Animator anim;
    Vector2Int direction;
    float timer;

    void OnEnable()
    {
        anim = GetComponentInChildren<Animator>();

        ResetTimer();

        onMove += OnMove;
        onEndMove += OnEndMove;
        DebugArrow(true);
    }

    void OnDisable()
    {
        onMove -= OnMove;
        onEndMove -= OnEndMove;
    }

    void Update()
    {
        //after timer, do movement
        if(Time.time > timer)
        {
            Move(direction);

            //set animator
            anim?.SetFloat("Horizontal", direction.x);
            anim?.SetFloat("Vertical", direction.y);

            ResetTimer();
        }
    }

    #region debug arrow

    void OnMove(Waypoint startWaypoint, Waypoint endWaypoint)
    {
        DebugArrow(false);
    }

    void OnEndMove()
    {
        DebugArrow(true);
    }

    void DebugArrow(bool activate)
    {
        //activate or deactivate
        arrow.gameObject.SetActive(activate);

        //if activate, rotate to direction
        if(activate)
        {
            //0, 45, 90, 135, 180, -135 (225), -90 (270), -45 (315)
            if (direction.y > 0 && direction.x == 0)
                arrow.localEulerAngles = new Vector3(0, 0, 0);
            else if (direction.y > 0 && direction.x < 0)
                arrow.localEulerAngles = new Vector3(0, 0, 45);
            else if (direction.y == 0 && direction.x < 0)
                arrow.localEulerAngles = new Vector3(0, 0, 90);
            else if (direction.y < 0 && direction.x < 0)
                arrow.localEulerAngles = new Vector3(0, 0, 135);
            else if (direction.y < 0 && direction.x == 0)
                arrow.localEulerAngles = new Vector3(0, 0, 180);
            else if (direction.y < 0 && direction.x > 0)
                arrow.localEulerAngles = new Vector3(0, 0, 255);
            else if (direction.y == 0 && direction.x > 0)
                arrow.localEulerAngles = new Vector3(0, 0, 270);
            else
                arrow.localEulerAngles = new Vector3(0, 0, 315);
        }
    }

    #endregion

    void ResetTimer()
    {
        //update timer
        timer = Time.time + timerMovement;

        direction = GetRandomDirection();
    }

    Vector2Int GetRandomDirection()
    {
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

        return direction;
    }

    protected abstract void Move(Vector2Int direction);
}
