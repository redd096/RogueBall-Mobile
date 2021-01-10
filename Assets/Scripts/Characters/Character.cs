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

        Ball currentBall;
        bool isDead;

        #region events

        public System.Action<Waypoint, Waypoint> onMove;
        public System.Action onEndMove;

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

        BaseThrowBall currentThrowBall;
        public BaseThrowBall CurrentThrowBall
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
                    foreach (BaseThrowBall throwBall in GetComponents<BaseThrowBall>())
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
            if (TryParry())
            {
                ball.Owner.GetParryDamage();
                return;
            }

            //else get damage and check death
            health -= ball.Damage;
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

            //call death function
            DeathFunction();
        }

        protected abstract void DeathFunction();

        protected virtual void PickBall(Ball ball)
        {
            //get ball and deactive it
            currentBall = ball;
            ball.gameObject.SetActive(false);
        }

        public void GetParryDamage()
        {
            //else get parry damage and check death
            health -= parryDamage;
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
            return CurrentThrowBall && CurrentThrowBall.Throw(currentBall, direction);
        }

        /// <summary>
        /// from state machine to component
        /// </summary>
        public bool Move(Vector2Int direction)
        {
            return CurrentMovement && CurrentMovement.Move(direction);
        }

        /// <summary>
        /// from state machine to component
        /// </summary>
        public bool CanMove(Vector2Int direction)
        {
            return CurrentMovement && CurrentMovement.CanMove(direction);
        }

        /// <summary>
        /// from get damage to component
        /// </summary>
        bool TryParry()
        {
            return CurrentParry && CurrentParry.TryParry();
        }

        #endregion
    }
}