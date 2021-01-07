using UnityEngine;
using redd096;

[AddComponentMenu("RogueBall/Enemy/Enemy Throw Ball")]
public class EnemyThrowBall : MonoBehaviour
{
    [Header("Important")]
    [Tooltip("Timer between one throw and another")] [SerializeField] float timerThrow = 1;
    [Tooltip("Aim at player or throw random?")] [SerializeField] bool aimAtPlayer = true;

    [Header("Throw")]
    [Tooltip("Force throw")] [SerializeField] float force = 3;

    Enemy enemy;
    float timer;

    void Awake()
    {
        enemy = GetComponent<Enemy>();

        timer = Time.time + timerThrow;
    }

    void Update()
    {
        //after timer, do movement
        if (Time.time > timer)
        {
            //update timer
            timer = Time.time + timerThrow;

            Throw(GetDirection());
        }
    }

    Vector2 GetDirection()
    {
        if(aimAtPlayer)
        {
            return GameManager.instance.player.transform.position - transform.position;
        }
        else
        {
            return Random.insideUnitCircle;
        }
    }

    void Throw(Vector2 direction)
    {
        //throw ball
        enemy.ThrowBall(force, direction.normalized);
    }
}
