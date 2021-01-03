using System.Collections;
using UnityEngine;

[AddComponentMenu("RogueBall/Ball")]
public class Ball : MonoBehaviour
{
    [Header("Speed Decrease")]
    [Tooltip("Every fixed update, decrease speed")] [SerializeField] float speedDecreaseAtSecond = 0.1f;
    [Tooltip("Every time hit something and bounce, decrease speed")] [SerializeField] float speedDecreaseAtBounce = 0.5f;

    Rigidbody2D rb;

    //movement
    float speed;
    Vector2 direction;
    Coroutine movementCoroutine;

    //damage if speed greater than 0
    public bool Damage => speed > 0;

    #region test

    [Header("Test Throw (direction Y axis)")]
    [SerializeField] bool throwBall = false;
    [SerializeField] float speedThrow = 3;
    [SerializeField] float timer = 1;
    [SerializeField] UnityEngine.UI.Text timerText = default;
    Coroutine testCoroutine;

    private void OnValidate()
    {
        //test throw ball
        if(throwBall)
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
        Throw(speedThrow, transform.up);
    }

    #endregion

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Throw(float speed, Vector2 direction)
    {
        //throw values
        this.speed = speed;
        this.direction = direction;

        //start movement coroutine
        if (movementCoroutine != null)
            StopCoroutine(movementCoroutine);

        movementCoroutine = StartCoroutine(MovementCoroutine());
    }

    IEnumerator MovementCoroutine()
    {
        while (true)
        {
            //do in fixed update
            yield return new WaitForFixedUpdate();

            //if stopped movement, stop coroutine
            if (speed <= 0)
                yield break;

            //move rigidbody then decrease speed
            rb.velocity = direction * speed;
            speed -= speedDecreaseAtSecond * Time.fixedDeltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //decrease speed at bounce
        speed -= speedDecreaseAtBounce;

        //bounce
        direction = Vector2.Reflect(direction, collision.GetContact(0).normal);
    }
}
