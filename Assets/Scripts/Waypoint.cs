using System.Collections;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Important")]
    [Tooltip("Is this waypoint active at start?")] [SerializeField] bool isActive = true;

    [Header("Area Parry")]
    [Tooltip("Area used for AreaParry")] [SerializeField] float areaParry = 0.3f;

    public bool IsActive => isActive;
    public float AreaParry => areaParry;

    Coroutine reactiveCoroutine;

    void OnDrawGizmos()
    {
        //do only if active
        if (IsActive)
        {
            Gizmos.color = Color.red;

            //draw area parry
            Gizmos.DrawWireSphere(transform.position, areaParry);
        }
    }

    public void Deactive(float timeToReactive)
    {
        //deactive
        isActive = false;

        //start coroutine to reactive
        if (reactiveCoroutine != null)
            StopCoroutine(reactiveCoroutine);

        reactiveCoroutine = StartCoroutine(ReactiveCoroutine(timeToReactive));
    }

    IEnumerator ReactiveCoroutine(float timeToReactive)
    {
        //wait
        yield return new WaitForSeconds(timeToReactive);

        //reactive
        isActive = true;
    }
}
