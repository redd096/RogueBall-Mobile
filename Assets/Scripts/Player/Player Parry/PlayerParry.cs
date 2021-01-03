using UnityEngine;

public abstract class PlayerParry : MonoBehaviour
{
    PlayerMovement privateMovement;
    PlayerMovement currentMovement 
    { 
        get 
        {
            //if enabled, return private movement
            if (privateMovement && privateMovement.enabled)
            {
                return privateMovement;
            }
            //else find first enabled
            else
            {
                foreach (PlayerMovement movement in GetComponents<PlayerMovement>())
                {
                    if (movement.enabled)
                    {
                        privateMovement = movement;
                        return movement;
                    }
                }
            }

            return null;
        } 
        set 
        {
            //set private movement
            privateMovement = value; 
        } 
    }

    protected bool isMoving;
    protected Transform startWaypoint;
    protected Transform endWaypoint;

    void OnEnable()
    {  
        currentMovement.onSwipe += OnSwipe;
        currentMovement.onEndSwipe += OnEndSwipe;
    }

    void OnDisable()
    {
        //remove events
        currentMovement.onSwipe -= OnSwipe;
        currentMovement.onEndSwipe -= OnEndSwipe;
    }

    protected virtual void OnSwipe(Transform startWaypoint, Transform endWaypoint)
    {
        //is moving and set waypoints
        isMoving = true;

        this.startWaypoint = startWaypoint;
        this.endWaypoint = endWaypoint;
    }

    void OnEndSwipe()
    {
        //stop moving
        isMoving = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if hit ball and ball is going to do damage
        Ball ball = collision.gameObject.GetComponentInParent<Ball>();
        if (ball && ball.Damage)
        {
            if (isMoving)
            {
                //get current waypoint
                Vector2Int currentKey;
                Transform currentWaypoint = redd096.GameManager.instance.mapManager.GetNearestWaypoint(transform.position, out currentKey);

                //check parry
                CheckParry(currentWaypoint);
            }
        }
    }

    protected abstract void CheckParry(Transform currentWaypoint);

    protected void Parry()
    {
        Debug.Log("Miiii un parry!");
    }
}
