﻿using System.Collections;
using UnityEngine;
using redd096;

[AddComponentMenu("RogueBall/Player/Movement/Movement Hybrid")]
public class MovementHybrid : PlayerMovement
{
    [Header("Hybrid")]
    [Tooltip("Duration movement from one waypoint to another")] [SerializeField] float timeMovement = 0.3f;

    Transform currentWaypoint;
    Vector2Int currentKey;

    Coroutine movementCoroutine;

    void Start()
    {
        //get current waypoint and set to its position
        currentWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(transform.position, out currentKey);
        transform.position = currentWaypoint.position;
    }

    protected override void Swipe(Vector2 direction)
    {
        //if no coroutine, start movement in direction
        if (movementCoroutine == null)
        {
            TempFunction();

            Vector2Int directionInt = new Vector2Int(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y));

            movementCoroutine = StartCoroutine(MovementCoroutine(directionInt));
        }
    }

    IEnumerator MovementCoroutine(Vector2Int direction)
    {
        //get waypoint to move
        Vector2Int newKey;
        Transform newWaypoint = GameManager.instance.mapManager.GetWaypointInDirection(currentKey, direction, out newKey);

        if (newWaypoint != null)
        {
            //start swipe
            onSwipe?.Invoke(currentWaypoint, newWaypoint);

            //move to new waypoint
            float delta = 0;
            while (delta < 1)
            {
                delta += Time.deltaTime / timeMovement;

                transform.position = Vector2.Lerp(currentWaypoint.position, newWaypoint.position, delta);

                yield return null;
            }

            //save new waypoint
            currentWaypoint = newWaypoint;
            currentKey = newKey;

            //end swipe
            onEndSwipe?.Invoke();
        }
        movementCoroutine = null;
    }

    void TempFunction()
    {
        //be sure to have current waypoint
        if(currentWaypoint == null)
            currentWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(transform.position, out currentKey);
    }
}