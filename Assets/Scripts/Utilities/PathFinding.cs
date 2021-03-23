namespace RogueBall
{
    using System.Collections.Generic;
    using UnityEngine;
    using redd096;

    public static class Pathfinding
    {
        public static List<Waypoint> FindPath(Character character, bool moveDiagonal, Vector2 startPosition, Vector2 targetPosition)
        {
            //get nodes from world position
            Waypoint startWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(character, startPosition);
            Waypoint targetWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(character, targetPosition);

            //return path
            return FindPath(character, moveDiagonal, startWaypoint, targetWaypoint);
        }

        public static List<Waypoint> FindPath(Character character, bool moveDiagonal, Waypoint startWaypoint, Waypoint targetWaypoint)
        {
            /*
             * OPEN - the set of nodes to be evaluated
             * CLOSE - the set of nodes already evaluated
             * 
             * G cost - distance from start point
             * H cost - distance from end point
             * F cost - sum of G cost and H cost
             */

            //=================================================================

            /*
             * add the start node to OPEN
             * 
             * loop
             *  Current = node in OPEN with the lowest F cost
             *  remove Current from OPEN
             *  add Current to CLOSED
             *  
             * if Current is the target node (path has been found)
             *  return
             *  
             * foreach Neighbour of the Current node
             *  if Neighbour is not traversable or Neighbour is in CLOSED
             *      skip to the next Neighbour
             *      
             *  if new path to Neighbour is shorter OR Neighbour is not in OPEN
             *      set F cost of Neighbour
             *      set parent of Neighbour to Current
             *      if Neighbour is not in OPEN
             *          add Neighbour to OPEN
             */

            List<Waypoint> openList = new List<Waypoint>();     //nodes to be evaluated
            List<Waypoint> closedList = new List<Waypoint>();   //already evaluated

            //add the start node to OPEN
            openList.Add(startWaypoint);

            while (openList.Count > 0)
            {
                //Current = node in OPEN with the lowest F cost
                Waypoint currentWaypoint = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    //if F cost is lower or is the same but H cost is lower
                    if (openList[i].fCost < currentWaypoint.fCost || openList[i].fCost == currentWaypoint.fCost && openList[i].hCost < currentWaypoint.hCost)
                    {
                        currentWaypoint = openList[i];
                    }
                }

                //remove Current from OPEN and add to CLOSED
                openList.Remove(currentWaypoint);
                closedList.Add(currentWaypoint);

                //path has been found, return it
                if (currentWaypoint == targetWaypoint)
                    return CreatePath(startWaypoint, currentWaypoint);

                //foreach Neighbour of the Current node (only walkables)
                foreach (Waypoint neighbour in GameManager.instance.mapManager.GetNeighbours(character, moveDiagonal, currentWaypoint))
                {
                    //if is in CLOSED, skip it
                    if (closedList.Contains(neighbour))
                        continue;

                    //get distance to Neighbour
                    int newCostToNeighbour = currentWaypoint.gCost + GetDistance(currentWaypoint, neighbour);

                    //if new path to Neighbour is shorter or Neighbour is not in OPEN
                    if (newCostToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                    {
                        //set F cost of Neighbour
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetWaypoint);

                        //set parent of Neighbour to Current
                        neighbour.parentWaypoint = currentWaypoint;

                        //if Neighbour is not in OPEN, add it
                        if (!openList.Contains(neighbour))
                            openList.Add(neighbour);
                    }
                }
            }

            //if there is no path, return null
            return null;
        }

        #region private API

        /// <summary>
        /// Calculate distance between 2 points
        /// </summary>
        static int GetDistance(Waypoint waypointA, Waypoint waypointB)
        {
            //get distance on X and Y
            int distanceX = Mathf.Abs(waypointA.PositionInMap.x - waypointB.PositionInMap.x);
            int distanceY = Mathf.Abs(waypointA.PositionInMap.y - waypointB.PositionInMap.y);

            //if distance on X is greater, move oblique (14) to reach Y axis, then move along X axis
            if (distanceX > distanceY)
                return 14 * distanceY + 10 * (distanceX - distanceY);

            //else move oblique (14) to reach X axis, then move along Y axis
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }

        /// <summary>
        /// Retrace path from start to end
        /// </summary>
        static List<Waypoint> CreatePath(Waypoint startWaypoint, Waypoint endWaypoint)
        {
            List<Waypoint> path = new List<Waypoint>();

            //start from end waypoint
            Waypoint currentWaypoint = endWaypoint;

            //while not reached start waypoint
            while (currentWaypoint != startWaypoint)
            {
                //add current waypoint and move to next one
                path.Add(currentWaypoint);
                currentWaypoint = currentWaypoint.parentWaypoint;
            }

            //reverse list to get from start to end
            path.Reverse();

            return path;
        }

        #endregion
    }
}