using UnityEngine;

public abstract class EnemyParry : MonoBehaviour
{
    Enemy enemy;

    protected bool isMoving;
    protected Waypoint startWaypoint;
    protected Waypoint endWaypoint;

    void OnEnable()
    {
        enemy = GetComponent<Enemy>();

        //set events
        enemy.CurrentMovement.onMove += OnMove;
        enemy.CurrentMovement.onEndMove += OnEndMove;
    }

    void OnDisable()
    {
        //remove events
        if (enemy)
        {
            enemy.CurrentMovement.onMove -= OnMove;
            enemy.CurrentMovement.onEndMove -= OnEndMove;
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

    public bool TryParry()
    {
        if (isMoving)
        {
            //get current waypoint
            Vector2Int currentKey;
            Waypoint currentWaypoint = redd096.GameManager.instance.mapManager.GetNearestWaypoint(transform.position, false, out currentKey);

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
