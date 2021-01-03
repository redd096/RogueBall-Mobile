using UnityEngine;

[AddComponentMenu("RogueBall/Player/Parry/Area Parry")]
public class AreaParry : PlayerParry
{
    [Header("Important")]
    [Tooltip("Parry on starting waypoint")] [SerializeField] bool parryOnStartWaypoint = true;
    [Tooltip("Parry on reached waypoint")] [SerializeField] bool parryOnEndWaypoint = true;
    [Tooltip("Distance from center of waypoint (red circle)")] [SerializeField] float areaParry = 0.3f;

    bool debugLine;
    Vector2 waypointPosition;
    Vector2 currentPosition;

    protected override bool CheckParry(Transform currentWaypoint)
    {
        //if can parry (start or end waypoint)
        if((currentWaypoint == startWaypoint && parryOnStartWaypoint)
            || (currentWaypoint == endWaypoint && parryOnEndWaypoint))
        {
            //draw line and distance
            waypointPosition = currentWaypoint.position;
            currentPosition = transform.position;
            debugLine = true;

            Debug.Log("distance: " + Vector2.Distance(currentWaypoint.position, transform.position));

            //check distance
            if (Vector2.Distance(currentWaypoint.position, transform.position) <= areaParry)
            {
                //parry
                Parry();
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //draw area parry
        foreach(Transform child in FindObjectOfType<MapManager>().transform)
        {
            Gizmos.DrawWireSphere(child.position, areaParry);
        }

        if(debugLine)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(waypointPosition, currentPosition);
            Gizmos.DrawWireSphere(waypointPosition, 0.1f);
            Gizmos.DrawWireSphere(currentPosition, 0.1f);
        }
    }
}
