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
        [Tooltip("Can enemy move in diagonal or only horizontal and vertical?")] [SerializeField] bool moveDiagonal = false;
        [SerializeField] bool goToBallOnlyWhenStopped = true;

        Character character;
        Coroutine pathCoroutine;

        Waypoint lastWaypoint;
        List<Waypoint> path = new List<Waypoint>();

        public MovingEnemyState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            character = stateMachine as Character;

            //reset path
            lastWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(character, character.transform.position);
            pathCoroutine = character.StartCoroutine(GetRandomPath());
        }

        public override void Exit()
        {
            base.Exit();

            //be sure to stop coroutine
            if (pathCoroutine != null)
                character.StopCoroutine(pathCoroutine);
        }

        public override void Execution()
        {
            base.Execution();

            //check every ball in scene that is really stopped (or just don't do damage - use false on checkOwner, so can't repick self throwed balls before they slow down)
            foreach (Ball ball in Object.FindObjectsOfType<Ball>())
            {
                if(ball.ReallyStopped || (goToBallOnlyWhenStopped == false && ball.CanDamage(character) == false))
                {
                    //get its waypoint (check both player and enemy area), and be sure is an enemy waypoint
                    Waypoint ballWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(character, ball.transform.position, false);
                    if (ballWaypoint.IsPlayerWaypoint)
                        continue;

                    //try create path (only enemy area)
                    List<Waypoint> ballPath = Pathfinding.FindPath(character, moveDiagonal, lastWaypoint, ballWaypoint);

                    //if there is a path, change state to reach the ball
                    if(ballPath != null && ballPath.Count > 0)
                    {
                        stateMachine.SetState(new MoveToBallEnemyState(stateMachine, timerMovement, moveDiagonal, ball));
                        break;
                    }
                }
            }
        }

        IEnumerator GetRandomPath()
        {
            path = new List<Waypoint>();

            //while can't use path
            while (path == null || path.Count <= 0 || character.CanMove(path[0], moveDiagonal) == false)
            {
                //try get path to random point
                Waypoint randomWaypoint = GameManager.instance.mapManager.GetRandomWaypoint(character, lastWaypoint);
                path = Pathfinding.FindPath(character, moveDiagonal, lastWaypoint, randomWaypoint);

                yield return null;
            }

            //start move
            pathCoroutine = character.StartCoroutine(Move());
        }

        IEnumerator Move()
        {
            while(path.Count > 0)
            {
                lastWaypoint = path[0];

                //DEBUG
                Enemy enemy = stateMachine as Enemy;
                enemy.DebugArrow((lastWaypoint.transform.position - character.transform.position).normalized);

                //wait
                yield return new WaitForSeconds(timerMovement);

                //if move, remove waypoint from path
                if(character.Move(lastWaypoint, moveDiagonal))
                {
                    path.RemoveAt(0);
                }
                //if can't move, clear path
                else
                {
                    Debug.LogWarning("Enemy can't reach waypoint");
                    lastWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(character, character.transform.position);     //can't use lastWaypoint cause can't reach, than use current waypoint
                    path.Clear();
                }
            }

            //get new path
            pathCoroutine = character.StartCoroutine(GetRandomPath());
        }
    }
}