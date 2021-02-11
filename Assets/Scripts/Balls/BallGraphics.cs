namespace RogueBall
{
    using System.Collections.Generic;
    using UnityEngine;
    using redd096;

    [AddComponentMenu("RogueBall/Balls/Ball Graphics")]
    public class BallGraphics : MonoBehaviour
    {
        [Header("In Parry Area")]
        [SerializeField] Color colorInParryArea = Color.cyan;

        [Header("Not Parryable")]
        [SerializeField] Color colorNotParryable = Color.red;

        Ball ball;

        Dictionary<Renderer, Color> normalColors = new Dictionary<Renderer, Color>();
        bool inParryArea;
        bool coloredNotParryable;

        void Awake()
        {
            ball = GetComponent<Ball>();

            //set normal colors
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                normalColors.Add(r, r.material.color);
            }
        }

        void Update()
        {
            CheckIsParryable();
            UpdateParryArea();
        }

        void CheckIsParryable()
        {
            //if ball not parryable, color it
            if (ball.IsParryable == false && coloredNotParryable == false)
            {
                ColorBall(colorNotParryable);
                coloredNotParryable = true;
            }
            //else back to normal color
            else if (ball.IsParryable && coloredNotParryable)
            {
                SetNormalColor();
                coloredNotParryable = false;
            }
        }

        void UpdateParryArea()
        {
            //find current waypoint
            Waypoint currentWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(null, transform.position, false);

            //if inside area parry, color it
            if (Vector2.Distance(currentWaypoint.transform.position, transform.position) <= currentWaypoint.AreaParry)
            {
                if (inParryArea == false)
                {
                    inParryArea = true;

                    //only if is parryable
                    if (ball.IsParryable)
                        ColorBall(colorInParryArea);
                }
            }
            //else back to normal color
            else
            {
                if (inParryArea)
                {
                    inParryArea = false;

                    //only if is parryable
                    if (ball.IsParryable)
                        SetNormalColor();
                }
            }
        }

        void ColorBall(Color color)
        {
            //set color
            foreach (Renderer r in normalColors.Keys)
            {
                r.material.color = color;
            }
        }

        void SetNormalColor()
        {
            //set colors to normal
            foreach (Renderer r in normalColors.Keys)
            {
                r.material.color = normalColors[r];
            }
        }
    }
}