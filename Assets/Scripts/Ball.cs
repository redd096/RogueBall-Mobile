namespace RogueBall
{
    using System.Collections;
    using UnityEngine;

    [AddComponentMenu("RogueBall/Ball")]
    public class Ball : MonoBehaviour
    {
        [Header("Important")]
        [SerializeField] float minSpeedToDamage = 0.2f;

        Rigidbody2D rb;

        public bool Stopped => rb.velocity.magnitude <= 0;
        public float Damage { get; private set; }
        public Character Owner { get; private set; }
        public bool Bounced { get; private set; }

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
            Bounced = true;
        }

        public bool CanHit(Character hit)
        {
            //check if can hit owner (when owner throw ball doesn't have to repick immediatly, so wait first bounce)
            return hit != Owner || Bounced;
        }

        public bool CanDamage(Character hit)
        {
            //check speed (so can damage) and didn't hit owner
            return hit != Owner && rb.velocity.magnitude > minSpeedToDamage;
        }

        public void Throw(float force, Vector2 direction, float damage, Character owner)
        {
            Damage = damage;
            Owner = owner;

            Bounced = false;

            //add force
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}
