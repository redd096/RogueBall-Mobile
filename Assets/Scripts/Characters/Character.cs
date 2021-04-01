namespace RogueBall
{
    using UnityEngine;
    using redd096;

    public abstract class Character : StateMachine
    {
        #region variables

        [Header("Important")]
        [SerializeField] float health = 100;
        [Tooltip("Damage when receive parry")] [SerializeField] float parryDamage = 100;

        [Header("Limit Movement")]
        [SerializeField] bool moveOnlyHorizontal = true;
        [CanShow("moveOnlyHorizontal", NOT = true)] [Tooltip("Can player move in diagonal or only horizontal and vertical?")] [SerializeField] bool moveDiagonal = false;

        protected Ball currentBall;
        bool isDead;

        public float Health => health;
        public bool MoveOnlyHorizontal => moveOnlyHorizontal;
        public bool MoveDiagonal => moveDiagonal;

        #region events

        public System.Action<Waypoint, Waypoint> onMove;
        public System.Action onEndMove;
        public System.Action onGetDamage;
        public System.Action onDie;
        public System.Action onParry;
        public System.Action onGetParryDamage;
        public System.Action onPickBall;
        public System.Action onThrowBall;

        #endregion

        #region components

        BaseMovement currentMovement;
        public BaseMovement CurrentMovement
        {
            get
            {
                //if enabled, return it
                if (currentMovement && currentMovement.enabled)
                {
                    return currentMovement;
                }
                //else find first enabled
                else
                {
                    foreach (BaseMovement movement in GetComponents<BaseMovement>())
                    {
                        if (movement.enabled)
                        {
                            currentMovement = movement;
                            return movement;
                        }
                    }
                }

                return null;
            }
        }

        BaseParry currentParry;
        public BaseParry CurrentParry
        {
            get
            {
                //if enabled, return it
                if (currentParry && currentParry.enabled)
                {
                    return currentParry;
                }
                //else find first enabled
                else
                {
                    foreach (BaseParry parry in GetComponents<BaseParry>())
                    {
                        if (parry.enabled)
                        {
                            currentParry = parry;
                            return parry;
                        }
                    }
                }

                return null;
            }
        }

        ThrowBall currentThrowBall;
        public ThrowBall CurrentThrowBall
        {
            get
            {
                //if enabled, return it
                if (currentThrowBall && currentThrowBall.enabled)
                {
                    return currentThrowBall;
                }
                //else find first enabled
                else
                {
                    foreach (ThrowBall throwBall in GetComponents<ThrowBall>())
                    {
                        if (throwBall.enabled)
                        {
                            currentThrowBall = throwBall;
                            return throwBall;
                        }
                    }
                }

                return null;
            }
        }

        #endregion

        #endregion

        void OnTriggerEnter2D(Collider2D collision)
        {
            //if hit ball
            Ball ball = collision.gameObject.GetComponentInParent<Ball>();
            if (ball && ball.CanHit(this))
            {
                if (ball.CanDamage(this))
                {
                    //get damage
                    GetDamage(ball);
                }

                //if no current ball, pick ball
                if (currentBall == null)
                    PickBall(ball);
            }
        }

        void GetDamage(Ball ball)
        {
            //try parry
            if (TryParry(ball))
            {
                GameManager.instance.uiManager.ActiveParryUI();
                ball.Owner.GetParryDamage();

                onParry?.Invoke();
                return;
            }

            //else get damage and check death
            health -= ball.Damage;
            onGetDamage?.Invoke();

            if (health <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            if (isDead)
                return;

            isDead = true;
            onDie?.Invoke();

            //call death function
            DeathFunction();
        }

        protected abstract void DeathFunction();

        protected virtual void PickBall(Ball ball)
        {
            //get ball and deactive it
            currentBall = ball;
            ball.gameObject.SetActive(false);

            onPickBall?.Invoke();
        }

        public void GetParryDamage()
        {
            //else get parry damage and check death
            health -= parryDamage;
            onGetParryDamage?.Invoke();

            if (health <= 0)
            {
                Die();
            }
        }

        #region components API

        /// <summary>
        /// from state machine to component
        /// </summary>
        public virtual bool ThrowBall(Vector2 direction)
        {
            //remove ball
            Ball ball = currentBall;
            currentBall = null;

            return CurrentThrowBall && CurrentThrowBall.Throw(ball, direction);
        }

        /// <summary>
        /// from state machine to component
        /// </summary>
        public bool Move(Waypoint targetWaypoint)
        {
            return CurrentMovement && CurrentMovement.Move(targetWaypoint);
        }

        /// <summary>
        /// from state machine to component
        /// </summary>
        public bool CanMove(Waypoint targetWaypoint)
        {
            return CurrentMovement && CurrentMovement.CanMove(targetWaypoint);
        }

        /// <summary>
        /// from get damage to component
        /// </summary>
        protected virtual bool TryParry(Ball ball)
        {
            return CurrentParry && CurrentParry.TryParry(ball);
        }

        #endregion
    }
}