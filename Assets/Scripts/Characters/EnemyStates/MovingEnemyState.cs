namespace RogueBall
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using redd096;

    [System.Serializable]
    public class MovingEnemyState : State
    {
        [Tooltip("Timer between one moves and another")] [SerializeField] float timerMovement = 1;
        [SerializeField] bool goToBallOnlyWhenStopped = true;

        Enemy enemy;
        Coroutine pathCoroutine;

        Waypoint lastWaypoint;
        List<Waypoint> path = new List<Waypoint>();

        public MovingEnemyState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            enemy = stateMachine as Enemy;

            //reset path
            lastWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(enemy, enemy.transform.position);
            pathCoroutine = enemy.StartCoroutine(GetRandomPath());
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

            //check every ball in scene that is really stopped (or just don't do damage - use false on checkOwner, so can't repick self throwed balls before they slow down)
            foreach (Ball ball in Object.FindObjectsOfType<Ball>())
            {
                if(ball.ReallyStopped || (goToBallOnlyWhenStopped == false && ball.CanDamage(enemy) == false))
                {
                    //get its waypoint (check both player and enemy area)
                    Waypoint ballWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(enemy, ball.transform.position, false);
                    if (ballWaypoint.IsPlayerWaypoint                                                                       //be sure is an enemy waypoint
                        || (enemy.MoveOnlyHorizontal && ballWaypoint.PositionInMap.y != lastWaypoint.PositionInMap.y))      //be sure can move horizontal or waypoint is in same row
                        continue;

                    //try create path (only enemy area)
                    List<Waypoint> ballPath = Pathfinding.FindPath(enemy, enemy.MoveDiagonal, lastWaypoint, ballWaypoint);

                    //if there is a path, change state to reach the ball
                    if(ballPath != null && ballPath.Count > 0)
                    {
                        stateMachine.SetState(new MoveToBallEnemyState(stateMachine, timerMovement, ball));
                        break;
                    }
                }
            }
        }

        IEnumerator GetRandomPath()
        {
            path = new List<Waypoint>();

            //while can't use path
            while (path == null || path.Count <= 0 || enemy.CanMove(path[0]) == false)
            {
                //try get path to random point
                Waypoint randomWaypoint = GameManager.instance.mapManager.GetRandomWaypoint(enemy, lastWaypoint);
                path = Pathfinding.FindPath(enemy, enemy.MoveDiagonal, lastWaypoint, randomWaypoint);

                yield return null;
            }

            //start move
            pathCoroutine = enemy.StartCoroutine(Move());
        }

        IEnumerator Move()
        {
            while(path.Count > 0)
            {
                lastWaypoint = path[0];

                //call event
                enemy.onSetMoveDirection?.Invoke(enemy.transform.position, lastWaypoint.transform.position);

                //wait
                yield return new WaitForSeconds(timerMovement);

                //if move, remove waypoint from path
                if(enemy.Move(lastWaypoint))
                {
                    path.RemoveAt(0);
                }
                //if can't move, clear path
                else
                {
                    Debug.LogWarning("Enemy can't reach waypoint");
                    lastWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(enemy, enemy.transform.position);     //can't use lastWaypoint cause can't reach, than use current waypoint
                    path.Clear();
                }
            }

            //get new path
            pathCoroutine = enemy.StartCoroutine(GetRandomPath());
        }
    }
}