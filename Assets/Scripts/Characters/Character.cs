namespace RogueBall
{
    using UnityEngine;

    public abstract class Character : redd096.StateMachine
    {
        #region variables

        [Header("Important")]
        [SerializeField] float health = 100;

        protected Animator anim;

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

        protected virtual void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            //if hit ball
            Ball ball = collision.gameObject.GetComponentInParent<Ball>();
            if (ball && ball.CanHit(transform))
            {
                if (ball.CanDamage)
                {
                    //get damage
                    GetDamage(ball.Damage);
                }

                //if no current ball, pick ball
                if (currentBall == null)
                    PickBall(ball);
            }
        }

        void GetDamage(float damage)
        {
            //try parry
            if (CurrentParry && CurrentParry.TryParry())
            {
                return;
            }

            //else get damage and check death
            health -= damage;
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

        protected virtual void PickBall(Ball ball)
        {
            //get ball and deactive it
            currentBall = ball;
            ball.gameObject.SetActive(false);
        }

        protected abstract void DeathFunction();

        #region public API

        /// <summary>
        /// from state machine to component
        /// </summary>
        public virtual void ThrowBall(Vector2 direction)
        {
            CurrentThrowBall.Throw(currentBall, direction);
        }

        /// <summary>
        /// from state machine to component
        /// </summary>
        public void Move(Vector2Int direction)
        {
            //set animator
            anim?.SetFloat("Horizontal", direction.x);
            anim?.SetFloat("Vertical", direction.y);

            //do movement
            CurrentMovement.Move(direction);
        }

        /// <summary>
        /// from component, do parry
        /// </summary>
        public void Parry()
        {
            Debug.Log("Miiii un parry!");
        }

        #endregion
    }
}