﻿using UnityEngine;

[AddComponentMenu("RogueBall/Player/Movement/Movement Oleoso")]
public class MovementOleoso : PlayerMovement
{
    [Header("Oleoso")]
    [Tooltip("When swing, reset speed or add push to current speed?")] [SerializeField] bool resetSpeed = false;
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
            rb.velocity = direction.normalized * force;
        else
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }
}
