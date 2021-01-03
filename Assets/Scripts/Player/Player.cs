using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("RogueBall/Player/Player")]
public class Player : MonoBehaviour
{
    [Header("Important")]
    [SerializeField] float health = 100;

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

    bool isDead;

    void OnCollisionEnter2D(Collision2D collision)
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
        }
    }

    void GetDamage(float damage)
    {
        //try parry
        if(currentParry && currentParry.TryParry())
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

    void PickBall()
    {
        //can't move, but throw ball

    }
}
