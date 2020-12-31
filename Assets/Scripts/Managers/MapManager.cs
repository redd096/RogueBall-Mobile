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
    [SerializeField] Transform[] waypoints = default;

    Dictionary<Vector2Int, Transform> map = new Dictionary<Vector2Int, Transform>();

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
        Transform[] waypointsByOrder = waypoints.OrderBy(waypoint => Mathf.RoundToInt(waypoint.position.y)).ThenBy(waypoint => Mathf.RoundToInt(waypoint.position.x)).ToArray();

        //reset map
        map.Clear();

        int currentY = Mathf.RoundToInt(waypointsByOrder[0].transform.position.y);
        int x = 0;
        int y = 0;
        for (int i = 0; i < waypointsByOrder.Length; i++)
        {
            Transform currentWaypoint = waypointsByOrder[i];
        
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

    public Transform GetNearestWaypoint(Vector2 position, out Vector2Int waypointKey)
    {
        return map.FindNearest(position, out waypointKey);
    }

    public Transform GetWaypointInDirection(Vector2Int currentKey, Vector2Int direction, out Vector2Int waypointKey)
    {
        //get key
        int x = currentKey.x + direction.x;
        int y = currentKey.y + direction.y;
        waypointKey = new Vector2Int(x, y);

        //if there is a waypoint in these coordinates, return it
        if (map.ContainsKey(waypointKey))
            return map[waypointKey];

        return null;
    }

    #endregion
}
