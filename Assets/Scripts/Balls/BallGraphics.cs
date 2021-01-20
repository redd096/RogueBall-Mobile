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

        Dictionary<Renderer, Color> normalColors = new Dictionary<Renderer, Color>();
        bool inParryArea;

        void Awake()
        {
            //set normal colors
            foreach(Renderer r in GetComponentsInChildren<Renderer>())
            {
                normalColors.Add(r, r.material.color);
            }
        }

        void Update()
        {
            //find current waypoint
            Waypoint currentWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(null, transform.position, false);

            //if inside area parry, color it
            if (Vector2.Distance(currentWaypoint.transform.position, transform.position) <= currentWaypoint.AreaParry)
            {
                if (inParryArea == false)
                {
                    ColorInParryArea();
                    inParryArea = true;
                }
            }
            //else back to normal color
            else
            {
                if (inParryArea)
                {
                    NormalColorOutOfParryArea();
                    inParryArea = false;
                }
            }
        }

        void ColorInParryArea()
        {
            //set colors in parry area
            foreach (Renderer r in normalColors.Keys)
            {
                r.material.color = colorInParryArea;
            }
        }

        void NormalColorOutOfParryArea()
        {
            //set colors to normal
            foreach (Renderer r in normalColors.Keys)
            {
                r.material.color = normalColors[r];
            }
        }
    }
}