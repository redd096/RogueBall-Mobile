﻿namespace RogueBall
{
    using UnityEngine;

    [AddComponentMenu("RogueBall/Characters/Components/Parry/Timer Parry")]
    public class TimerParry : BaseParry
    {
        [Header("Important")]
        [Tooltip("Timer start on swipe, if hitted in this time can parry")] [SerializeField] float timerParry = 0.05f;

        float timer;

        protected override void OnMove(Waypoint startWaypoint, Waypoint endWaypoint)
        {
            base.OnMove(startWaypoint, endWaypoint);

            //set timer
            timer = Time.time + timerParry;
        }

        protected override bool CheckParry(Waypoint currentWaypoint)
        {
            //if timer is not ended, parry
            if (Time.time < timer)
            {
                character.Parry();
                return true;
            }

            return false;
        }
    }
}