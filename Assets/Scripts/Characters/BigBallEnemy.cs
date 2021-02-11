namespace RogueBall
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("RogueBall/Characters/Big Ball Enemy")]
    public class BigBallEnemy : Enemy
    {
        [Header("BigBall")]
        [Tooltip("Size of big ball")] [SerializeField] Vector2 bigSize = Vector2.one * 2;
        [Tooltip("Set if is parryable big ball (override throw component)")] [SerializeField] bool parryable = false;
        [Tooltip("Reset at bounce? Reset size to normal and parryable to throw component")] [SerializeField] bool resetAtBounce = true;

        Ball throwedBall;
        Vector2 previousSize;
        bool previousParryable;
        bool mustUpdateBall;

        protected override void Update()
        {
            base.Update();

            //do only if throwed ball
            if (throwedBall == null)
                return;

            //start by updating ball
            if(mustUpdateBall)
            {
                throwedBall.IsParryable = parryable;
                throwedBall.transform.localScale = bigSize;

                mustUpdateBall = false;
                return; //wait next frame
            }

            //on deactivate (picked by someone) or bounced if resetAtBounce is true - reset ball
            if(throwedBall.gameObject.activeInHierarchy == false || (resetAtBounce && throwedBall.Bounced))
            {
                throwedBall.transform.localScale = previousSize;
                throwedBall.IsParryable = previousParryable;

                throwedBall = null;
            }
        }

        public override bool ThrowBall(Vector2 direction)
        {
            //if current ball, save it
            if(currentBall)
            {
                throwedBall = currentBall;

                //save previous size and parryable
                previousSize = currentBall.transform.localScale;
                previousParryable = CurrentThrowBall.IsParryable;

                //set we need to update ball
                mustUpdateBall = true;
            }

            return base.ThrowBall(direction);
        }
    }
}