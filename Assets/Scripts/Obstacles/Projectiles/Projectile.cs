using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : FreezeableFunctionality, IProjectile, IObstacle
{
    //fields
    protected Rigidbody2D rb;
    protected Vector2 velocity;
    private float rotation;
    protected bool isMoving;

    void IObstacle.Reset()
    {
        
    }

    void IProjectile.Setup(Vector2 projVelocity)
    {
        Debug.Log("Got to Setup()");
        velocity = projVelocity;
        Debug.Log("Velocity set");
        SetRotation();
        Debug.Log("Rotation Done");
        isMoving = true;
    }

    protected virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        isMoving = false;
    }

    protected virtual void FixedUpdate()
    {
        if (!base.isFrozen && isMoving) rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
    protected void SetRotation()
    {
        Debug.Log("In SetRotation()");
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            Debug.Log("Left/Right");
            if (velocity.x > 0) rotation = 0;
            else rotation = 180;
        }
        else
        {
            Debug.Log("Up/Down");
            if (velocity.y > 0) rotation = 90;
            else rotation = 270;
        }
        Debug.Log("Just before MoveRotation");
        rb.MoveRotation(rotation);
    }
}
