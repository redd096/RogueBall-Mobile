namespace RogueBall
{
    using System.Collections.Generic;
    using UnityEngine;
    using redd096;

    [AddComponentMenu("RogueBall/Graphics/Ball Graphics")]
    public class BallGraphics : MonoBehaviour
    {
        Ball ball;

        #region events vars

        [Header("On Bounce")]
        [SerializeField] ParticleSystem[] particlesOnBounce = default;
        [SerializeField] AudioStruct[] soundOnBounce = default;
        [SerializeField] bool vibrateOnBounce = false;
        [SerializeField] bool cameraShakeOnBounce = false;

        [Header("On Throwed")]
        [SerializeField] ParticleSystem[] particlesOnThrowed = default;
        [SerializeField] AudioStruct[] soundOnThrowed = default;
        [SerializeField] bool vibrateOnThrowed = false;
        [SerializeField] bool cameraShakeOnThrowed = false;

        #endregion

        #region parry vars

        [Header("In Parry Area")]
        [SerializeField] Color colorInParryArea = Color.cyan;

        [Header("Not Parryable")]
        [SerializeField] Color colorNotParryable = Color.red;

        Dictionary<Renderer, Color> normalColors = new Dictionary<Renderer, Color>();
        bool inParryArea;
        bool coloredNotParryable;

        #endregion

        void Awake()
        {
            //set normal colors
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                normalColors.Add(r, r.material.color);
            }
        }

        void OnEnable()
        {
            ball = GetComponent<Ball>();

            //add events
            ball.onBounce += OnBounce;
            ball.onThrowed += OnThrowed;
        }

        void OnDisable()
        {
            //remove events
            if(ball)
            {
                ball.onBounce -= OnBounce;
                ball.onThrowed -= OnThrowed;
            }
        }

        void Update()
        {
            //parry checks
            CheckIsParryable();
            UpdateParryArea();
        }

        #region parry

        void CheckIsParryable()
        {
            //if ball not parryable, color it
            if (ball.IsParryable == false && coloredNotParryable == false)
            {
                ColorBall(colorNotParryable);
                coloredNotParryable = true;
            }
            //else back to normal color
            else if (ball.IsParryable && coloredNotParryable)
            {
                SetNormalColor();
                coloredNotParryable = false;
            }
        }

        void UpdateParryArea()
        {
            //find current waypoint
            Waypoint currentWaypoint = GameManager.instance.mapManager.GetNearestWaypoint(null, transform.position, false);

            //if inside area parry, color it
            if (Vector2.Distance(currentWaypoint.transform.position, transform.position) <= currentWaypoint.AreaParry)
            {
                if (inParryArea == false)
                {
                    inParryArea = true;

                    //only if is parryable
                    if (ball.IsParryable)
                        ColorBall(colorInParryArea);
                }
            }
            //else back to normal color
            else
            {
                if (inParryArea)
                {
                    inParryArea = false;

                    //only if is parryable
                    if (ball.IsParryable)
                        SetNormalColor();
                }
            }
        }

        void ColorBall(Color color)
        {
            //set color
            foreach (Renderer r in normalColors.Keys)
            {
                r.material.color = color;
            }
        }

        void SetNormalColor()
        {
            //set colors to normal
            foreach (Renderer r in normalColors.Keys)
            {
                r.material.color = normalColors[r];
            }
        }

        #endregion

        #region events

        void OnBounce()
        {
            //play particles and sound
            ParticlesManager.instance.Play(particlesOnBounce, transform.position, transform.rotation);
            SoundManager.instance.Play(soundOnBounce, transform.position);

            //vibrate
            if (vibrateOnBounce)
                Handheld.Vibrate();

            //camera shake
            if (cameraShakeOnBounce)
                GameManager.instance.cameraShake.StartShake();
        }

        void OnThrowed()
        {
            //play particles and sound
            ParticlesManager.instance.Play(particlesOnThrowed, transform.position, transform.rotation);
            SoundManager.instance.Play(soundOnThrowed, transform.position);

            //vibrate
            if (vibrateOnThrowed)
                Handheld.Vibrate();

            //camera shake
            if (cameraShakeOnThrowed)
                GameManager.instance.cameraShake.StartShake();
        }

        #endregion
    }
}