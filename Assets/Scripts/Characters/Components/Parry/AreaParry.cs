namespace RogueBall
{
    using UnityEngine;

    [AddComponentMenu("RogueBall/Characters/Components/Parry/Area Parry")]
    public class AreaParry : BaseParry
    {
        [Header("Important")]
        [Tooltip("Parry on starting waypoint")] [SerializeField] bool parryOnStartWaypoint = true;
        [Tooltip("Parry on reached waypoint")] [SerializeField] bool parryOnEndWaypoint = true;

        bool debugLine;
        Vector2 waypointPosition;
        Vector2 currentPosition;

        protected override bool CheckParry(Waypoint currentWaypoint)
        {
            //if can parry (start or end waypoint)
            if ((currentWaypoint == startWaypoint && parryOnStartWaypoint)
                || (currentWaypoint == endWaypoint && parryOnEndWaypoint))
            {
                //draw line and distance for debug
                waypointPosition = currentWaypoint.transform.position;
                currentPosition = transform.position;
                debugLine = true;
                Debug.Log("distance: " + Vector2.Distance(currentWaypoint.transform.position, transform.position));

                //check distance
                if (Vector2.Distance(currentWaypoint.transform.position, transform.position) <= currentWaypoint.AreaParry)
                {
                    return true;
                }
            }

            return false;
        }

        void OnDrawGizmos()
        {
            if (debugLine)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(waypointPosition, currentPosition);
                Gizmos.DrawWireSphere(waypointPosition, 0.1f);
                Gizmos.DrawWireSphere(currentPosition, 0.1f);
            }
        }
    }
}