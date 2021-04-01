namespace RogueBall
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using redd096;

    public class MoveToBallEnemyState : State
    {
        float timerMovement = 1;
        bool moveDiagonal = false;
        Ball ballToReach;

        Enemy enemy;
        Coroutine pathCoroutine;

        Waypoint lastWaypoint;
        List<Waypoint> path = new List<Waypoint>();

        public MoveToBallEnemyState(StateMachine stateMachine, float timerMovement, bool moveDiagonal, Ball ballToReach) : base(stateMachine)
        {
            this.timerMovement = timerMovement;
            this.moveDiagonal = moveDiagonal;
            this.ballToReach = ballToReach;
        }

        public override void Enter()
        {
            base.Enter();

            enemy = stateMachine as Enemy;

            //set path
            SetPathToBall();

            //start move
            if (path != null && path.Count > 0)
                pathCoroutine = enemy.StartCoroutine(Move());
        }

        public override void Exit()
        {
            base.Exit();

            //be sure to stop coroutine
            if (pathCoroutine != null)
                enemy.StopCoroutine(pathCoroutine);
        }

        public override void Execution()
        {
            base.Execution();

            //if ball waypoint is not in the path, there is a problem (check also last waypoint, cause maybe we finished the path)
            Waypoint ballWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(enemy, ballToReach.transform.position, false);
            if (path.Contains(ballWaypoint) == false && lastWaypoint != ballWaypoint)
            {
                enemy.StopFollowBall();
            }
        }

        void SetPathToBall()
        {
            lastWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(enemy, enemy.transform.position);

            //get ball waypoint (check both player and enemy area)
            Waypoint ballWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(enemy, ballToReach.transform.position, false);
            if (ballWaypoint.IsPlayerWaypoint                                                                       //be sure is an enemy waypoint
                || (enemy.MoveOnlyHorizontal && ballWaypoint.PositionInMap.y != lastWaypoint.PositionInMap.y))      //be sure can move horizontal or waypoint is in same row
            {
                enemy.StopFollowBall();
                return;
            }

            //try create path (only enemy area)
            path = Pathfinding.FindPath(enemy, moveDiagonal, lastWaypoint, ballWaypoint);

            //if can't reach ball, stop follow it
            if (path == null || path.Count <= 0)
                enemy.StopFollowBall();
        }

        IEnumerator Move()
        {
            while (path.Count > 0)
            {
                lastWaypoint = path[0];

                //call event
                enemy.onSetMoveDirection?.Invoke(enemy.transform.position, lastWaypoint.transform.position);

                //wait
                yield return new WaitForSeconds(timerMovement);

                //if move, remove waypoint from path
                if (enemy.Move(lastWaypoint, moveDiagonal))
                {
                    path.RemoveAt(0);
                }
                //if can't move, stop cause can't reach ball
                else
                {
                    enemy.StopFollowBall();
                }
            }

            //if ballWaypoint != last waypoint there is a problem, cause we finished the path
            Waypoint ballWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(enemy, ballToReach.transform.position, false);
            if (ballWaypoint != lastWaypoint)
            {
                enemy.StopFollowBall();
            }
        }
    }
}