﻿namespace RogueBall
{
    using UnityEngine;
    using redd096;

    public abstract class BaseParry : MonoBehaviour
    {
        protected Character character;

        protected bool isMoving;
        protected Waypoint startWaypoint;
        protected Waypoint endWaypoint;

        void OnEnable()
        {
            character = GetComponent<Character>();

            //set events
            character.onMove += OnMove;
            character.onEndMove += OnEndMove;
        }

        void OnDisable()
        {
            //remove events
            if (character)
            {
                character.onMove -= OnMove;
                character.onEndMove -= OnEndMove;
            }
        }

        protected virtual void OnMove(Waypoint startWaypoint, Waypoint endWaypoint)
        {
            //is moving and set waypoints
            isMoving = true;

            this.startWaypoint = startWaypoint;
            this.endWaypoint = endWaypoint;
        }

        void OnEndMove()
        {
            //stop moving
            isMoving = false;
        }

        public bool TryParry()
        {
            if (isMoving)
            {
                //get current waypoint
                Vector2Int currentKey;
                Waypoint currentWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(character, transform.position, out currentKey);

                //check parry
                return CheckParry(currentWaypoint);
            }

            return false;
        }

        protected abstract bool CheckParry(Waypoint currentWaypoint);
    }
}