using UnityEngine;

[AddComponentMenu("RogueBall/Player/Parry/Area Parry")]
public class AreaParry : PlayerParry
{
    [Header("Important")]
    [Tooltip("Parry on starting waypoint")] [SerializeField] bool parryOnStartWaypoint = true;
    [Tooltip("Parry on reached waypoint")] [SerializeField] bool parryOnEndWaypoint = true;

    protected override bool CheckParry(Waypoint currentWaypoint)
    {
        //if can parry (start or end waypoint)
        if((currentWaypoint == startWaypoint && parryOnStartWaypoint)
            || (currentWaypoint == endWaypoint && parryOnEndWaypoint))
        {
            //check distance
            if (Vector2.Distance(currentWaypoint.transform.position, transform.position) <= currentWaypoint.AreaParry)
            {
                //parry
                Parry();
                return true;
            }
        }

        return false;
    }
}
