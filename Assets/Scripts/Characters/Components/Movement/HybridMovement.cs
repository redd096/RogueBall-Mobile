namespace RogueBall
{
    using System.Collections;
    using UnityEngine;

    [AddComponentMenu("RogueBall/Characters/Components/Movement/Hybrid Movement")]
    public class HybridMovement : BaseMovement
    {
        [Header("Hybrid")]
        [Tooltip("Duration movement from one waypoint to another")] [SerializeField] float timeMovement = 0.3f;

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
            SetAnimator(CurrentWaypoint, targetWaypoint, true);

            //move to new waypoint
            float delta = 0;
            while (delta < 1)
            {
                delta += Time.deltaTime / timeMovement;

                transform.position = Vector2.Lerp(CurrentWaypoint.transform.position, targetWaypoint.transform.position, delta);

                yield return null;
            }

            //save new waypoint
            CurrentWaypoint = targetWaypoint;

            //end swipe
            character.onEndMove?.Invoke();
            SetAnimator(CurrentWaypoint, targetWaypoint, false);

            movementCoroutine = null;
        }
    }
}