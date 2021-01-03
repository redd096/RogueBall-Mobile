using UnityEngine;

[AddComponentMenu("RogueBall/Waypoint")]
public class Waypoint : MonoBehaviour
{
    [Tooltip("Distance from center of waypoint (red circle)")] [SerializeField] float areaParry = 0.3f;

    public float AreaParry => areaParry;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //draw area parry
        Gizmos.DrawWireSphere(transform.position, areaParry);
    }
}
