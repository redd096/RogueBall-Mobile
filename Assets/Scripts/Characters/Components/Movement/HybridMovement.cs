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

        public override bool Move(Vector2Int direction)
        {
            //if no coroutine, start movement in direction
            if (movementCoroutine == null)
            {
                if (CanMove(direction))
                {
                    movementCoroutine = StartCoroutine(MovementCoroutine(direction));
                    return true;
                }
            }

            return false;
        }

        IEnumerator MovementCoroutine(Vector2Int direction)
        {
            //start swipe
            character.onMove?.Invoke(CurrentWaypoint, newWaypoint);
            SetAnimator(direction, true);

            //move to new waypoint
            float delta = 0;
            while (delta < 1)
            {
                delta += Time.deltaTime / timeMovement;

                transform.position = Vector2.Lerp(CurrentWaypoint.transform.position, newWaypoint.transform.position, delta);

                yield return null;
            }

            //save new waypoint
            CurrentWaypoint = newWaypoint;
            currentKey = newKey;

            //end swipe
            character.onEndMove?.Invoke();
            SetAnimator(direction, false);

            movementCoroutine = null;
        }
    }
}