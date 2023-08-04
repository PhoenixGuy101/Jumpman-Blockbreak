using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile, IProjectile, IObstacle, IFreezeable
{
    //fields

    protected override void OnEnable()
    {
        base.OnEnable();
        StageEnd.OnPlayerReachingEnd += Disappear;
    }

    private void OnDisable()
    {
        StageEnd.OnPlayerReachingEnd -= Disappear;
    }

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

    protected override void Disappear()
    {
        base.Disappear();
        Debug.Log("Arrow Disappear");
    }
}