namespace RogueBall
{
    using System.Collections.Generic;
    using UnityEngine;
    using System.Linq;

    [AddComponentMenu("RogueBall/Managers/Map Manager")]
    public class MapManager : MonoBehaviour
    {
        [Header("Refresh")]
        [SerializeField] bool refresh = false;

        [Header("Map")]
        [SerializeField] Waypoint[] waypoints = default;

        Dictionary<Vector2Int, Waypoint> mapPlayer = new Dictionary<Vector2Int, Waypoint>();

        void Awake()
        {
            //refresh waypoints
            RefreshWaypoints();
        }

        void OnValidate()
        {
            //refresh by inspector
            if (refresh)
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
            mapPlayer.Clear();

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
                mapPlayer.Add(new Vector2Int(x, y), currentWaypoint);
                x++;
            }
        }

        #region public API

        public Waypoint GetNearestWaypoint(Character character, Vector2 position, out Vector2Int waypointKey)
        {
            bool isPlayer = character is Player;

            Vector2Int nearestKey = default;
            float distance = Mathf.Infinity;

            //foreach key in the dictionary
            foreach (Vector2Int key in mapPlayer.Keys)
            {
                //only if there is a waypoint and is active
                if (mapPlayer[key] == null || mapPlayer[key].IsActive == false
                    || mapPlayer[key].IsPlayerWaypoint != isPlayer)                     //if is playerWaypoint but for enemy, or is enemyWaypoint but for player
                    continue;

                //check distance to find nearest
                float newDistance = Vector3.Distance(mapPlayer[key].transform.position, position);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    nearestKey = key;
                }
            }

            //return nearest (or null if no waypoints)
            waypointKey = nearestKey;
            return mapPlayer.ContainsKey(nearestKey) ? mapPlayer[nearestKey] : null;
        }

        public Waypoint GetWaypointInDirection(Character character, Vector2Int currentKey, Vector2Int direction, out Vector2Int waypointKey)
        {
            bool isPlayer = character is Player;

            //get key
            int x = currentKey.x + direction.x;
            int y = currentKey.y + direction.y;
            waypointKey = new Vector2Int(x, y);

            //if there is a waypoint in these coordinates, return it
            if (mapPlayer.ContainsKey(waypointKey) && mapPlayer[waypointKey] != null && mapPlayer[waypointKey].IsActive
                && mapPlayer[waypointKey].IsPlayerWaypoint == isPlayer)                 //is playerWaypoint for a player, or enemyWaypoint for enemy
            {
                return mapPlayer[waypointKey];
            }

            return null;
        }

        #endregion
    }
}