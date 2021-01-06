using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("RogueBall/Player/Player")]
public class Player : MonoBehaviour
{
    #region variables

    [Header("Important")]
    [SerializeField] float health = 100;

    Ball currentBall;
    bool isDead;

    #region components

    PlayerMovement currentMovement;
    public PlayerMovement CurrentMovement
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
                foreach (PlayerMovement movement in GetComponents<PlayerMovement>())
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

    PlayerParry currentParry;
    public PlayerParry CurrentParry
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
                foreach (PlayerParry parry in GetComponents<PlayerParry>())
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

    PlayerThrowBall currentThrowBall;
    public PlayerThrowBall CurrentThrowBall
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
                foreach (PlayerThrowBall throwBall in GetComponents<PlayerThrowBall>())
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

    void Start()
    {
        //start without throw ball
        CurrentThrowBall.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //if hit ball
        Ball ball = collision.gameObject.GetComponentInParent<Ball>();
        if(ball)
        {
            if(ball.CanDamage)
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
        if(CurrentParry && CurrentParry.TryParry())
        {
            return;
        }

        //else get damage and check death
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        //disable movement and parry
        //currentMovement.enabled = false;
        //currentParry.enabled = false;

        Debug.Log("dead");
    }

    void PickBall(Ball ball)
    {
        //get ball and deactive it
        currentBall = ball;
        ball.gameObject.SetActive(false);

        //can't move, but throw ball (use privates vars)
        currentMovement.enabled = false;
        currentThrowBall.enabled = true;
    }

    public void ThrowBall(float force, Vector2 direction)
    {
        //set ball position and activate
        currentBall.transform.position = transform.position;
        currentBall.gameObject.SetActive(true);

        //throw ball
        currentBall.Throw(force, direction);

        //can't throw ball, but move (use private vars)
        currentThrowBall.enabled = false;
        currentMovement.enabled = true;
    }
}
