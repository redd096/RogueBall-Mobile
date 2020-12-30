using UnityEngine;

[AddComponentMenu("RogueBall/Player/Movement/MovementOleoso")]
public class MovementOleoso : PlayerMovement
{
    [Header("Oleoso")]
    [Tooltip("When swing, reset speed or add swing to current speed?")] [SerializeField] bool resetSpeed = false;
    [Tooltip("Push force")] [SerializeField] float force = 1;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Swing(Vector2 direction)
    {
        //move - reset speed or add force
        if (resetSpeed)
            rb.velocity = direction * force;
        else
            rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
