﻿namespace RogueBall
{
    using UnityEngine;

    [AddComponentMenu("RogueBall/Characters/Components/Throw Ball")]
    public class ThrowBall : BaseThrowBall
    {
        [Header("Throw")]
        [Tooltip("Force throw")] [SerializeField] float force = 3;
        [SerializeField] float damage = 100;

        public override bool Throw(Ball ball, Vector2 direction)
        {
            if (ball)
            {
                //set ball position and activate
                ball.transform.position = transform.position;
                ball.gameObject.SetActive(true);

                //throw ball
                ball.Throw(force, direction, damage, character);

                return true;
            }

            return false;
        }
    }
}