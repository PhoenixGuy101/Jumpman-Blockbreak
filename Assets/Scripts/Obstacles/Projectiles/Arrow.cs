using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile, IProjectile, IObstacle
{
    //fields
    private Vector3 lastPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isMoving = false;
            collision.gameObject.TryGetComponent(out IDamageable pInterface);
            if (pInterface != null) pInterface.Die();
        }
        else
        {
            Disappear();
        }
    }

    protected override void Start()
    {
        base.Start();
        lastPos = rb.position + (velocity * 3000);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Vector2.Equals(rb.position, lastPos))
        {
            Disappear();
        }
    }

    private void Disappear()
    {
        isMoving = false;
        Destroy(gameObject); //will need to play a fading animation or something similar
    }
}