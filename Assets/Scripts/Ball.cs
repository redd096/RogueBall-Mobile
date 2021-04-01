﻿namespace RogueBall
{
    using System.Collections;
    using UnityEngine;
    using redd096;

    [AddComponentMenu("RogueBall/Ball")]
    [RequireComponent(typeof(BallGraphics))]
    public class Ball : MonoBehaviour
    {
        #region variables

        [Header("Important")]
        [SerializeField] bool stopAfterBounces = true;
        [CanShow("stopAfterBounces", NOT = true)] [SerializeField] float minSpeedToDamage = 0.2f;
        [SerializeField] bool damageOnlyBeforeBounce = true;

        [Header("Anchor to Waypoint")]
        [SerializeField] float maxDistanceFromCenter = 0.1f;

        [Header("Use for debug (0 = no use)")]
        [SerializeField] float timerBeforeCanRepickBall = 1;
        [SerializeField] bool showSpeed = false;

        Rigidbody2D rb;
        float damage;
        Character owner;
        bool isParryable = true;
        int numberBouncesBeforeStop;
        int currentBounces;
        float timerAfterCanHitOwner;

        #region events

        public System.Action onBounce;
        public System.Action onThrowed;

        #endregion

        #region to anchor at waypoint

        bool waitBounce;
        Vector2 lastVelocity;
        bool bouncedToAnchorPoint;

        #endregion

        public bool IsSlow => rb.velocity.magnitude <= minSpeedToDamage;
        public bool ReallyStopped => rb.velocity.magnitude <= 0;
        public float Damage => damage;
        public Character Owner => owner;
        public bool IsParryable { get { return isParryable; } set { isParryable = value; } }
        public bool Bounced => currentBounces > 0;

        #endregion

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            currentBounces++;

            //used for anchor point, wait bounce
            if(waitBounce)
            {
                bouncedToAnchorPoint = true;
            }

            onBounce?.Invoke();
        }

        private void FixedUpdate()
        {
            Waypoint currentWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(null, transform.position, false);

            if(showSpeed)
                Debug.Log("ball speed: " + rb.velocity.magnitude.ToString("F2"));

            //if slow
            if(IsSlow)
            {
                //if too much distant from waypoint
                if (Vector3.Distance(transform.position, currentWaypoint.transform.position) > maxDistanceFromCenter)
                {
                    waitBounce = true;

                    //wait bounce
                    if (bouncedToAnchorPoint == false)
                    {
                        rb.velocity = lastVelocity;
                    }
                    //then redirect to waypoint
                    else
                    {
                        Vector3 directionToWaypoint = (currentWaypoint.transform.position - transform.position).normalized;

                        rb.velocity = directionToWaypoint * lastVelocity.magnitude;
                    }
                }
                //else stop movement
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
            //if no slow, save last velocity
            else
            {
                lastVelocity = rb.velocity;
            }
        }

        public bool CanHit(Character hit)
        {
            return hit != owner                                                             //or hit somebody different from owner
                || currentBounces > 0                                                       //or can hit owner after bounce
                || (timerBeforeCanRepickBall > 0 && Time.time > timerAfterCanHitOwner);     //or after a few seconds (just for debug, if enemy throw ball too slow)
        }

        public bool CanDamage(Character hit)
        {
            //check didn't hit owner 
            return owner != null && hit != owner 
                && ( (damageOnlyBeforeBounce == false && IsSlow == false)   //and ball is not slow (if can damage also after bounce)
                || (damageOnlyBeforeBounce && currentBounces <= 0) );       //or ball didn't bounced (if can NOT damage after bounce)
        }

        public void Throw(float force, Vector2 direction, float damage, Character owner, bool isParryable, int numberBouncesBeforeStop)
        {
            this.damage = damage;
            this.owner = owner;
            this.isParryable = isParryable;
            this.numberBouncesBeforeStop = numberBouncesBeforeStop;

            //set no bounce
            currentBounces = 0;
            timerAfterCanHitOwner = Time.time + timerBeforeCanRepickBall;   //for debug

            //reset things for anchor point
            waitBounce = false;
            bouncedToAnchorPoint = false;

            //add force
            rb.AddForce(direction * force, ForceMode2D.Impulse);

            onThrowed?.Invoke();
        }
    }
}
