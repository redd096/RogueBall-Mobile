namespace RogueBall
{
    using System.Collections;
    using UnityEngine;

    [AddComponentMenu("RogueBall/Ball")]
    public class Ball : MonoBehaviour
    {
        #region variables

        [Header("Important")]
        [SerializeField] float minSpeedToDamage = 0.2f;

        [Header("Use for debug (0 = no use)")]
        [SerializeField] float timerBeforeCanRepickBall = 1;

        Rigidbody2D rb;
        float damage;
        Character owner;
        bool bounced;
        float timerAfterCanHitOwner;

        public bool IsSlow => rb.velocity.magnitude <= minSpeedToDamage;
        public bool ReallyStopped => rb.velocity.magnitude <= 0;
        public float Damage => damage;
        public Character Owner => owner;
        public bool Bounced => bounced;

        #endregion

        #region test throw by inspector

        [Header("Test Throw (direction Y axis)")]
        [SerializeField] bool throwBall = false;
        [SerializeField] float speedThrow = 3;
        [SerializeField] float damageThrow = 100;
        [SerializeField] float timer = 1;
        [SerializeField] UnityEngine.UI.Text timerText = default;
        Coroutine testCoroutine;

        private void OnValidate()
        {
            //test throw ball
            if (throwBall)
            {
                throwBall = false;

                if (testCoroutine != null)
                    StopCoroutine(testCoroutine);

                testCoroutine = StartCoroutine(TestCoroutine());
            }
        }

        IEnumerator TestCoroutine()
        {
            //init timer
            float timeTest = Time.time + timer;
            timerText.text = Mathf.Ceil(timeTest - Time.time).ToString("F0");
            timerText.gameObject.SetActive(true);

            while (timeTest > Time.time)
            {
                //set text
                timerText.text = Mathf.Ceil(timeTest - Time.time).ToString("F0");
                yield return null;
            }

            //remove text and throw
            timerText.gameObject.SetActive(false);
            Throw(speedThrow, transform.up, damageThrow, null);
        }

        #endregion

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            bounced = true;
        }

        public bool CanHit(Character hit)
        {
            return hit != owner                                                             //or hit somebody different from owner
                || bounced                                                                  //or can hit owner after bounce
                || (timerBeforeCanRepickBall > 0 && Time.time > timerAfterCanHitOwner);     //or after a few seconds (just for debug, if enemy throw ball too slow)
        }

        public bool CanDamage(Character hit)
        {
            //check didn't hit owner and ball is not slow
            return hit != owner && IsSlow == false;
        }

        public void Throw(float force, Vector2 direction, float damage, Character owner)
        {
            this.damage = damage;
            this.owner = owner;

            bounced = false;
            timerAfterCanHitOwner = Time.time + timerBeforeCanRepickBall;

            //add force
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}
