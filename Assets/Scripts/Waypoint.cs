using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Important")]
    [Tooltip("Is this waypoint active at start?")] [SerializeField] bool isActive = true;

    [Header("Area Parry")]
    [Tooltip("Area used for AreaParry")] [SerializeField] float areaParry = 0.3f;

    public bool IsActive => isActive;
    public float AreaParry => areaParry;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //draw area parry
        Gizmos.DrawWireSphere(transform.position, areaParry);
    }
}
