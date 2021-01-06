using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using redd096;

[AddComponentMenu("RogueBall/Managers/Map Manager")]
public class MapManager : MonoBehaviour
{
    [Header("Refresh")]
    [SerializeField] bool refresh = false;

    [Header("Map")]
    [SerializeField] Waypoint[] waypoints = default;

    Dictionary<Vector2Int, Waypoint> map = new Dictionary<Vector2Int, Waypoint>();

    void Awake()
    {
        //refresh waypoints
        RefreshWaypoints();
    }

    void OnValidate()
    {
        //refresh by inspector
        if(refresh)
        {
            refresh = false;
            RefreshWaypoints();
        }
    }

    void RefreshWaypoints()
    {
        //order on y then x
        Waypoint[] waypointsByOrder = waypoints.OrderBy(waypoint => Mathf.RoundToInt(waypoint.transform.position.y)).ThenBy(waypoint => Mathf.RoundToInt(waypoint.transform.position.x)).ToArray();

        //reset map
        map.Clear();

        //be sure there is something before start
        if (waypointsByOrder == null || waypoints.Length <= 0)
            return;

        int currentY = Mathf.RoundToInt(waypointsByOrder[0].transform.position.y);
        int x = 0;
        int y = 0;
        for (int i = 0; i < waypointsByOrder.Length; i++)
        {
            Waypoint currentWaypoint = waypointsByOrder[i];
        
            //if go to next row, reset x and increase y
            if (Mathf.RoundToInt(currentWaypoint.transform.position.y) > currentY)
            {
                x = 0;
                y++;
                currentY = Mathf.RoundToInt(currentWaypoint.transform.position.y);
            }

            //add to map and increase x
            map.Add(new Vector2Int(x, y), currentWaypoint);        
            x++;
        }
    }

    #region public API

    public Waypoint GetNearestWaypoint(Vector2 position, out Vector2Int waypointKey)
    {
        Vector2Int nearestKey = default;
        float distance = Mathf.Infinity;

        //foreach key in the dictionary
        foreach(Vector2Int key in map.Keys)
        {
            //only if there is a waypoint and is active
            if (map[key] == null || map[key].IsActive == false)
                continue;

            //check distance to find nearest
            float newDistance = Vector3.Distance(map[key].transform.position, position);
            if(newDistance < distance)
            {
                distance = newDistance;
                nearestKey = key;
            }
        }

        waypointKey = nearestKey;
        return map[nearestKey];
    }

    public Waypoint GetWaypointInDirection(Vector2Int currentKey, Vector2Int direction, out Vector2Int waypointKey)
    {
        //get key
        int x = currentKey.x + direction.x;
        int y = currentKey.y + direction.y;
        waypointKey = new Vector2Int(x, y);

        //if there is a waypoint in these coordinates, return it
        if (map.ContainsKey(waypointKey) && map[waypointKey].IsActive)
            return map[waypointKey];

        return null;
    }

    #endregion
}
