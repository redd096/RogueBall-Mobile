using UnityEngine;

[AddComponentMenu("RogueBall/Player/Movement/Oleoso Movement")]
public class OleosoMovement : PlayerMovement
{
    [Header("Oleoso")]
    [Tooltip("When swing, reset speed or add push to current speed?")] [SerializeField] bool resetSpeed = false;
    [Tooltip("Push force")] [SerializeField] float force = 1;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Move(Vector2Int direction)
    {
        TempFunction();

        //move - reset speed or add force
        if (resetSpeed)
            rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        else
            rb.AddForce(new Vector2(direction.x, direction.y).normalized * force, ForceMode2D.Impulse);
    }

    void TempFunction()
    {
        //be sure to have rigidbody saved
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }
}
