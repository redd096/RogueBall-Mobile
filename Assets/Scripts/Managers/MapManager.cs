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
                currentWaypoint.PositionInMap = new Vector2Int(x, y);
                mapPlayer.Add(currentWaypoint.PositionInMap, currentWaypoint);
                x++;
            }
        }

        #region public API

        public Waypoint GetNearestWaypoint(Character character, Vector2 position, bool checkCharacter = true)
        {
            bool isPlayer = character is Player;

            Vector2Int nearestKey = default;
            float distance = Mathf.Infinity;

            //foreach key in the dictionary
            foreach (Vector2Int key in mapPlayer.Keys)
            {
                //only if there is a waypoint and is active
                if (mapPlayer[key] == null || mapPlayer[key].IsActive == false
                    || (mapPlayer[key].IsPlayerWaypoint != isPlayer && checkCharacter))                     //if is playerWaypoint but for enemy, or is enemyWaypoint but for player (and is check character)
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
            return mapPlayer.ContainsKey(nearestKey) ? mapPlayer[nearestKey] : null;
        }

        public Waypoint GetWaypointInDirection(Character character, Waypoint currentWaypoint, Vector2Int direction, bool checkCharacter = true)
        {
            bool isPlayer = character is Player;

            //get key
            int x = currentWaypoint.PositionInMap.x + direction.x;
            int y = currentWaypoint.PositionInMap.y + direction.y;
            Vector2Int waypointKey = new Vector2Int(x, y);

            //if there is a waypoint in these coordinates, return it
            if (mapPlayer.ContainsKey(waypointKey) && mapPlayer[waypointKey] != null && mapPlayer[waypointKey].IsActive
                && (mapPlayer[waypointKey].IsPlayerWaypoint == isPlayer || checkCharacter == false))        //is playerWaypoint for a player, or enemyWaypoint for enemy (or not check character)
            {
                return mapPlayer[waypointKey];
            }

            return null;
        }

        public List<Waypoint> GetNeighbours(Character character, Waypoint currentWaypoint, bool canDiagonal, bool checkCharacter = true)
        {
            List<Waypoint> neighbours = new List<Waypoint>();

            //get every direction
            List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };
            if(canDiagonal)
            {
                //up right, down right, up left, down left
                directions.Add(new Vector2Int(1, 1));
                directions.Add(new Vector2Int(1, -1));
                directions.Add(new Vector2Int(-1, 1));
                directions.Add(new Vector2Int(-1, -1));
            }

            //foreach direction, check if there is a waypoint to add
            foreach(Vector2Int direction in directions)
            {
                Waypoint waypoint = GetWaypointInDirection(character, currentWaypoint, direction, checkCharacter);
                if (waypoint)
                    neighbours.Add(waypoint);
            }

            return neighbours;
        }

        #endregion
    }
}