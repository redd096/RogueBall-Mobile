﻿using UnityEngine;

[AddComponentMenu("RogueBall/Player/Parry/Area Parry")]
public class AreaParry : PlayerParry
{
    [Header("Important")]
    [Tooltip("Parry on starting waypoint")] [SerializeField] bool parryOnStartWaypoint = true;
    [Tooltip("Parry on reached waypoint")] [SerializeField] bool parryOnEndWaypoint = true;
    [Tooltip("Distance from center of waypoint (red circle)")] [SerializeField] float areaParry = 0.3f;

    protected override void CheckParry(Transform currentWaypoint)
    {
        //if can parry (start or end waypoint)
        if((currentWaypoint == startWaypoint && parryOnStartWaypoint)
            || (currentWaypoint == endWaypoint && parryOnEndWaypoint))
        {
            //check distance
            if (Vector2.Distance(currentWaypoint.position, transform.position) <= areaParry)
            {
                //parry
                Parry();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //draw area parry
        Gizmos.DrawWireSphere(transform.position, areaParry);
    }
}
