namespace RogueBall
{
﻿   using UnityEngine;

    public abstract class BaseThrowBall : MonoBehaviour
    {
        protected Character character;

        void OnEnable()
        {
            character = GetComponent<Character>();
        }

        public abstract void Throw(Ball ball, Vector2 direction);
    }
}