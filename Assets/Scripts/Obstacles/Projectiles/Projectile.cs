using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : FreezeableFunctionality, IProjectile, IObstacle, IFreezeable
{
    //fields
    protected Rigidbody2D rb;
    protected Vector2 velocity;
    protected float rotation;
    protected float lifeSpan;
    protected bool isMoving;

    void IObstacle.Reset()
    {
        
    }

    void IProjectile.Setup(Vector2 projVelocity, float projLifespan)
    {
        velocity = projVelocity;
        lifeSpan = projLifespan;
        SetRotation();
        isMoving = true;
    }

    protected virtual void OnEnable()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        //Debug.Log("rigidbody: " + rb);
        //Debug.Log("Arrow Position: " + transform.position);
        isMoving = false;
    }
    
    protected override void Start()
    {
        base.Start();
    }

    protected virtual void FixedUpdate()
    {
        if (!base.isFrozen && isMoving)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            if (lifeSpan >= 0) lifeSpan -= Time.fixedDeltaTime;
            else Disappear();
        }
    }
    private void SetRotation()
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

    protected virtual void Disappear()
    {
        isMoving = false;
        Destroy(gameObject); //will need to play a fading animation or something similar
    }
}
