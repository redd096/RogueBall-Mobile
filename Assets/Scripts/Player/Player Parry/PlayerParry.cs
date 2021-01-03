using UnityEngine;

public abstract class PlayerParry : MonoBehaviour
{
    Player player;

    protected bool isMoving;
    protected Waypoint startWaypoint;
    protected Waypoint endWaypoint;

    void OnEnable()
    {
        player = GetComponent<Player>();

        //set events
        player.CurrentMovement.onSwipe += OnSwipe;
        player.CurrentMovement.onEndSwipe += OnEndSwipe;
    }

    void OnDisable()
    {
        //remove events
        player.CurrentMovement.onSwipe -= OnSwipe;
        player.CurrentMovement.onEndSwipe -= OnEndSwipe;
    }

    protected virtual void OnSwipe(Waypoint startWaypoint, Waypoint endWaypoint)
    {
        //is moving and set waypoints
        isMoving = true;

        this.startWaypoint = startWaypoint;
        this.endWaypoint = endWaypoint;
    }

    void OnEndSwipe()
    {
        //stop moving
        isMoving = false;
    }

    public bool TryParry()
    {
        if (isMoving)
        {
            //get current waypoint
            Vector2Int currentKey;
            Waypoint currentWaypoint = redd096.GameManager.instance.mapManager.GetNearestWaypoint(transform.position, out currentKey);

            //check parry
            return CheckParry(currentWaypoint);
        }

        return false;
    }

    protected abstract bool CheckParry(Waypoint currentWaypoint);

    protected void Parry()
    {
        Debug.Log("Miiii un parry!");
    }
}
