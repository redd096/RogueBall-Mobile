﻿namespace RogueBall
{
    using System.Collections;
    using UnityEngine;
    using redd096;

    [AddComponentMenu("RogueBall/Characters/Components/Movement/Arcade Movement")]
    public class ArcadeMovement : BaseMovement
    {
        [Header("Arcade")]
        [Tooltip("Duration movement from one waypoint to another")] [SerializeField] float timeMovement = 0.3f;
        [Tooltip("Time to stay in new waypoint before come back to start waypoint")] [SerializeField] float timeBeforeComeBack = 0.1f;
        [Tooltip("Duration movement to come back to start waypoint")] [SerializeField] float timeComeBack = 0.3f;

        Coroutine movementCoroutine;

        public override bool Move(Waypoint targetWaypoint, bool moveDiagonal)
        {
            //if no coroutine, start movement in direction
            if (movementCoroutine == null)
            {
                if (CanMove(targetWaypoint, moveDiagonal))
                {
                    movementCoroutine = StartCoroutine(MovementCoroutine(targetWaypoint));
                    return true;
                }
            }

            return false;
        }

        IEnumerator MovementCoroutine(Waypoint targetWaypoint)
        {
            //start swipe
            character.onMove?.Invoke(CurrentWaypoint, targetWaypoint);
            SetAnimator(targetWaypoint.transform.position - CurrentWaypoint.transform.position, true);

            //move to new waypoint
            float delta = 0;
            while (delta < 1)
            {
                delta += Time.deltaTime / timeMovement;

                transform.position = Vector2.Lerp(CurrentWaypoint.transform.position, targetWaypoint.transform.position, delta);
                yield return null;
            }

            SetAnimator(targetWaypoint.transform.position - CurrentWaypoint.transform.position, false);

            //wait before come back
            yield return new WaitForSeconds(timeBeforeComeBack);

            SetAnimator(CurrentWaypoint.transform.position - targetWaypoint.transform.position, true);

            //come back to position
            delta = 0;
            while (delta < 1)
            {
                delta += Time.deltaTime / timeComeBack;

                transform.position = Vector2.Lerp(targetWaypoint.transform.position, CurrentWaypoint.transform.position, delta);
                yield return null;
            }

            //end swipe
            character.onEndMove?.Invoke();
            SetAnimator(CurrentWaypoint.transform.position - targetWaypoint.transform.position, false);

            movementCoroutine = null;
        }
    }
}