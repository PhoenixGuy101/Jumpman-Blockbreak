using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    //fields
    private Rigidbody2D rb;
    private Vector2 velocity;
    protected bool isMoving;

    void IProjectile.Setup(Vector2 projVelocity)
    {
        velocity = projVelocity;
        SetRotation();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        
    }

    private void FixedUpdate()
    {
        if (isMoving) rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
    private void SetRotation()
    {
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            if (velocity.x > 0) rb.MoveRotation(0);
            else rb.MoveRotation(180);
        }
        else
        {
            if (velocity.y > 0) rb.MoveRotation(90);
            else rb.MoveRotation(270);
        }
    }
}
