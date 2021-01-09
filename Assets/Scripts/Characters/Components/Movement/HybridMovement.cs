namespace RogueBall
{
    using System.Collections;
    using UnityEngine;
    using redd096;

    [AddComponentMenu("RogueBall/Characters/Components/Movement/Hybrid Movement")]
    public class HybridMovement : BaseMovement
    {
        [Header("Hybrid")]
        [Tooltip("Duration movement from one waypoint to another")] [SerializeField] float timeMovement = 0.3f;

        public override void Move(Vector2Int direction)
        {
            //if no coroutine, start movement in direction
            if (movementCoroutine == null)
            {
                movementCoroutine = StartCoroutine(MovementCoroutine(direction));
            }
        }

        IEnumerator MovementCoroutine(Vector2Int direction)
        {
            //get waypoint to move
            Vector2Int newKey;
            Waypoint newWaypoint = GameManager.instance.mapManager.GetWaypointInDirection(character, currentKey, direction, out newKey);

            if (newWaypoint != null)
            {
                //start swipe
                character.onMove?.Invoke(CurrentWaypoint, newWaypoint);
                anim?.SetBool("Move", true);

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
                anim?.SetBool("Move", false);
            }

            movementCoroutine = null;
        }
    }
}