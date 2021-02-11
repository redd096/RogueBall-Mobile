namespace RogueBall
{
    using UnityEngine;

    [AddComponentMenu("RogueBall/Graphics/Enemy Graphics")]
    public class EnemyGraphics : MonoBehaviour
    {
        [Header("Aim Throw")]
        [Tooltip("Line renderer used to show aim direction")] [SerializeField] LineRenderer aimLine = default;

        [Header("DEBUG")]
        [SerializeField] Transform arrow = default;

        Enemy enemy;

        private void OnEnable()
        {
            //set events
            enemy = GetComponent<Enemy>();
            enemy.onSetMoveDirection += OnSetMoveDirection;
            enemy.onSetThrowDirection += OnSetThrowDirection;
        }

        private void OnDisable()
        {
            //remove events
            if(enemy)
            {
                enemy.onSetMoveDirection -= OnSetMoveDirection;
                enemy.onSetThrowDirection -= OnSetThrowDirection;
            }
        }

        void OnSetMoveDirection(Vector2 startPoint, Vector2 endPoint)
        {
            DebugArrow((endPoint - startPoint).normalized);
        }

        void OnSetThrowDirection(Vector2 startPoint, Vector2 endPoint)
        {
            DebugArrow((endPoint - startPoint).normalized);

            //set line renderer positions
            if(aimLine)
            {
                aimLine.SetPosition(0, new Vector3(startPoint.x, startPoint.y, -1));
                aimLine.SetPosition(1, new Vector3(endPoint.x, endPoint.y, -1));
            }
        }

        /// <summary>
        /// show arrow debug
        /// </summary>
        void DebugArrow(Vector2 direction)
        {
            if (arrow)
            {
                //rotate to direction
                float angle = Vector2.SignedAngle(Vector2.up, direction);
                arrow.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

        }
    }
}