using UnityEngine;

[AddComponentMenu("RogueBall/Player/Parry/Timer Parry")]
public class TimerParry : PlayerParry
{
    [Header("Important")]
    [Tooltip("Timer start on swipe, if hitted in this time can parry")] [SerializeField] float timerParry = 0.05f;

    float timer;

    protected override void OnSwipe(Transform startWaypoint, Transform endWaypoint)
    {
        base.OnSwipe(startWaypoint, endWaypoint);

        //set timer
        timer = Time.time + timerParry;
    }

    protected override bool CheckParry(Transform currentWaypoint)
    {
        //if timer is not ended, parry
        if(Time.time < timer)
        {
            Parry();
            return true;
        }

        return false;
    }
}
