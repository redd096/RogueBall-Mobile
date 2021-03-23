namespace RogueBall
{
    using UnityEngine;
    using redd096;
    using UnityEngine.UI;

    public abstract class CharacterGraphics : MonoBehaviour
    {
        Character character;

        [Header("Health")]
        [SerializeField] Slider healthSlider = default;

        float maxHealth;

        #region events vars

        [Header("On Move")]
        [SerializeField] ParticleSystem[] particlesOnMove = default;
        [SerializeField] AudioStruct[] soundOnMove = default;
        [SerializeField] bool vibrateOnMove = false;

        [Header("On End Move")]
        [SerializeField] ParticleSystem[] particlesOnEndMove = default;
        [SerializeField] AudioStruct[] soundOnEndMove = default;
        [SerializeField] bool vibrateOnEndMove = false;

        [Header("On Get Damage")]
        [SerializeField] ParticleSystem[] particlesOnGetDamage = default;
        [SerializeField] AudioStruct[] soundOnGetDamage = default;
        [SerializeField] bool vibrateOnGetDamage = false;

        [Header("On Die")]
        [SerializeField] ParticleSystem[] particlesOnDie = default;
        [SerializeField] AudioStruct[] soundOnDie = default;
        [SerializeField] bool vibrateOnDie = false;

        [Header("On Parry")]
        [SerializeField] ParticleSystem[] particlesOnParry = default;
        [SerializeField] AudioStruct[] soundOnParry = default;
        [SerializeField] bool vibrateOnParry = false;

        [Header("On Get Parry Damage")]
        [SerializeField] ParticleSystem[] particlesOnGetParryDamage = default;
        [SerializeField] AudioStruct[] soundOnGetParryDamage = default;
        [SerializeField] bool vibrateOnGetParryDamage = false;

        [Header("On Pick Ball")]
        [SerializeField] ParticleSystem[] particlesOnPickBall = default;
        [SerializeField] AudioStruct[] soundOnPickBall = default;
        [SerializeField] bool vibrateOnPickBall = false;

        [Header("On Throw Ball")]
        [SerializeField] ParticleSystem[] particlesOnThrowBall = default;
        [SerializeField] AudioStruct[] soundOnThrowBall = default;
        [SerializeField] bool vibrateOnThrowBall = false;

        #endregion

        private void Awake()
        {
            //save max health
            maxHealth = GetComponent<Character>().Health;
        }

        protected virtual void OnEnable()
        {
            character = GetComponent<Character>();

            //add events
            character.onMove += OnMove;
            character.onEndMove += OnEndMove;
            character.onGetDamage += OnGetDamage;
            character.onDie += OnDie;
            character.onParry += OnParry;
            character.onGetParryDamage += OnGetParryDamage;
            character.onPickBall += OnPickBall;
            character.onThrowBall += OnThrowBall;
        }

        protected virtual void OnDisable()
        {
            //remove events
            if (character)
            {
                character.onMove -= OnMove;
                character.onEndMove -= OnEndMove;
                character.onGetDamage -= OnGetDamage;
                character.onDie -= OnDie;
                character.onParry -= OnParry;
                character.onGetParryDamage -= OnGetParryDamage;
                character.onPickBall -= OnPickBall;
                character.onThrowBall -= OnThrowBall;
            }
        }

        #region events

        void OnMove(Waypoint startWaypoint, Waypoint endWaypoint)
        {
            //play particles and sound
            ParticlesManager.instance.Play(particlesOnMove, transform.position, transform.rotation);
            SoundManager.instance.Play(soundOnMove, transform.position);

            //vibrate
            if (vibrateOnMove)
                Handheld.Vibrate();
        }

        void OnEndMove()
        {
            //play particles and sound
            ParticlesManager.instance.Play(particlesOnEndMove, transform.position, transform.rotation);
            SoundManager.instance.Play(soundOnEndMove, transform.position);

            //vibrate
            if (vibrateOnEndMove)
                Handheld.Vibrate();
        }

        void OnGetDamage()
        {
            //update health slider
            if (healthSlider)
                healthSlider.value = character.Health / maxHealth;

            //play particles and sound
            ParticlesManager.instance.Play(particlesOnGetDamage, transform.position, transform.rotation);
            SoundManager.instance.Play(soundOnGetDamage, transform.position);

            //vibrate
            if (vibrateOnGetDamage)
                Handheld.Vibrate();
        }

        void OnDie()
        {
            //play particles and sound
            ParticlesManager.instance.Play(particlesOnDie, transform.position, transform.rotation);
            SoundManager.instance.Play(soundOnDie, transform.position);

            //vibrate
            if (vibrateOnDie)
                Handheld.Vibrate();
        }

        void OnParry()
        {
            //play particles and sound
            ParticlesManager.instance.Play(particlesOnParry, transform.position, transform.rotation);
            SoundManager.instance.Play(soundOnParry, transform.position);

            //vibrate
            if (vibrateOnParry)
                Handheld.Vibrate();
        }

        void OnGetParryDamage()
        {
            //update health slider
            if (healthSlider)
                healthSlider.value = character.Health / maxHealth;

            //play particles and sound
            ParticlesManager.instance.Play(particlesOnGetParryDamage, transform.position, transform.rotation);
            SoundManager.instance.Play(soundOnGetParryDamage, transform.position);

            //vibrate
            if (vibrateOnGetParryDamage)
                Handheld.Vibrate();
        }

        void OnPickBall()
        {
            //play particles and sound
            ParticlesManager.instance.Play(particlesOnPickBall, transform.position, transform.rotation);
            SoundManager.instance.Play(soundOnPickBall, transform.position);

            //vibrate
            if (vibrateOnPickBall)
                Handheld.Vibrate();
        }

        void OnThrowBall()
        {
            //play particles and sound
            ParticlesManager.instance.Play(particlesOnThrowBall, transform.position, transform.rotation);
            SoundManager.instance.Play(soundOnThrowBall, transform.position);

            //vibrate
            if (vibrateOnThrowBall)
                Handheld.Vibrate();
        }

        #endregion
    }
}