using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : FreezeableFunctionality, IProjectile
{
    //fields
    protected Rigidbody2D rb;
    protected Vector2 velocity;
    private float rotation;
    protected bool isMoving;

    void IProjectile.Setup(Vector2 projVelocity)
    {
        velocity = projVelocity;
        SetRotation();
    }

    protected virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (!base.isFrozen && isMoving) rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
    protected virtual void SetRotation()
    {
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            if (velocity.x > 0) rotation = 0;
            else rotation = 180;
        }
        else
        {
            if (velocity.y > 0) rotation = 90;
            else rotation = 270;
        }
        rb.MoveRotation(rotation);
    }
}
