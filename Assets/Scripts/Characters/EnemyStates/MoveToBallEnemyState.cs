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

        Enemy character;
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

            character = stateMachine as Enemy;

            //set path
            SetPathToBall();

            //start move
            if (path != null && path.Count > 0)
                pathCoroutine = character.StartCoroutine(Move());
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

            //if ball has speed greater than 0, stop follow it
            if(ballToReach.Speed > 0)
            {
                character.StopFollowBall();
            }
        }

        void SetPathToBall()
        {
            //get ball waypoint (check both player and enemy area), and be sure is an enemy waypoint
            Waypoint ballWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(character, ballToReach.transform.position, false);
            if (ballWaypoint.IsPlayerWaypoint)
            {
                character.StopFollowBall();
                return;
            }

            //try create path (only enemy area)
            Waypoint currentWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(character, character.transform.position);
            path = Pathfinding.FindPath(character, moveDiagonal, currentWaypoint, ballWaypoint);

            //if can't reach ball, stop follow it
            if (path == null || path.Count <= 0)
                character.StopFollowBall();
        }

        IEnumerator Move()
        {
            while (path.Count > 0)
            {
                lastWaypoint = path[0];

                //DEBUG
                Enemy enemy = stateMachine as Enemy;
                enemy.DebugArrow((lastWaypoint.transform.position - character.transform.position).normalized);

                //wait
                yield return new WaitForSeconds(timerMovement);

                //if move, remove waypoint from path
                if (character.Move(lastWaypoint, moveDiagonal))
                {
                    path.RemoveAt(0);
                }
                //if can't move, stop cause can't reach ball
                else
                {
                    character.StopFollowBall();
                }
            }

            //reached ball
            //TODO trova il modo di essere certo di aver preso la palla, altrimenti torna in moving enemy state
        }
    }
}