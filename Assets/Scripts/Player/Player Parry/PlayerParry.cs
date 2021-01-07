﻿using UnityEngine;

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
        player.CurrentMovement.onMove += OnMove;
        player.CurrentMovement.onEndMove += OnEndMove;
    }

    void OnDisable()
    {
        //remove events
        if (player)
        {
            player.CurrentMovement.onMove -= OnMove;
            player.CurrentMovement.onEndMove -= OnEndMove;
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
            Waypoint currentWaypoint = redd096.GameManager.instance.mapManager.GetNearestWaypoint(transform.position, true, out currentKey);

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
